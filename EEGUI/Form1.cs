﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using EushullyEditor;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;

namespace EEGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Kamidori Configuration
            Resources.RemoveBreakLine = false;
            Resources.Monospaced = true;
            Resources.MonospacedLengthLimit = 63;
            var jpTxt = File.ReadAllText("jp.txt");
            JPText = JsonConvert.DeserializeObject<List<string>>(jpTxt);

            InitializeComponent();

        }
        BinEditor Editor;
        List<string> JPText;
        string FileName;
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "All Bin files | *.bin";
            DialogResult dr = fd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                //EE = new EushullyEditor(System.IO.File.ReadAllBytes(fd.FileName), new FormatOptions()); //Initializate with default configuration
                FileName = fd.FileName;
                LoadFile(FileName);
            }
        }
        public int index;
        private void LoadFile(string fileName)
        {
            Editor = new BinEditor(System.IO.File.ReadAllBytes(fileName), new FormatOptions()
            {
                ClearOldStrings = true,
                BruteValidator = true
            });

            Text = "Eusshuly Script - v" + Editor.ScriptVersion;

            listBox1.Items.Clear();
            foreach (string str in Editor.Import())
                listBox1.Items.Add(str);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                index = listBox1.SelectedIndex;
                //GET TEXT WITH FAKE BREAK LINE
                textBox1.Text = Resources.GetFakedBreakLineText(listBox1.Items[index].ToString().Replace("\\n", "\n")).Replace("\n", "\\n");
                textBox1 = Resources.AutoLigth(textBox1);
            }
            catch { }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\n' || e.KeyChar == '\r')
            {
                string toChineseTranditional = string.Empty;

                foreach (var c in textBox1.Text)
                {
                    var tranditionalString = ChineseConverter.Convert(c.ToString(), ChineseConversionDirection.SimplifiedToTraditional);

                    //matched
                    if (c.ToString() == "么" || tranditionalString == "么")
                    {
                        toChineseTranditional += "麼";
                    }
                    else if (c.ToString() == "啊" || tranditionalString == "啊")
                    {
                        toChineseTranditional += "阿";
                    }
                    else if (c.ToString() == "擊" || tranditionalString == "擊")
                    {
                        toChineseTranditional += "撃";
                    }
                    else if (c.ToString() == "嗯" || tranditionalString == "嗯")
                    {
                        toChineseTranditional += "恩";
                    }
                    else if (c.ToString() == "你" || tranditionalString == "你" || c.ToString() == "妳" || tranditionalString == "妳")
                    {
                        toChineseTranditional += "匿";
                    }
                    else if (c.ToString() == "," || c.ToString() == "、")
                    {
                        toChineseTranditional += "，";
                    }
                    else if (JPText.Contains(tranditionalString))
                    {
                        toChineseTranditional += tranditionalString;
                    }
                    else
                    {
                        toChineseTranditional += c.ToString();
                    }
                }

                var encodeToJP = Encoding.GetEncoding(932).GetBytes(toChineseTranditional);
                var JPBytesToStr = Encoding.GetEncoding(932).GetString(encodeToJP);

                //SAVE TEXT WITH FAKE BREAK LINE
                Editor.StringsInfo[index].Content = (Resources.FakeBreakLine(JPBytesToStr.Replace("\\n", "\n")));
                listBox1.Items[index] = JPBytesToStr;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create backup file
            File.Copy(FileName, $"{FileName}.bak", true);

            //save file
            System.IO.File.WriteAllBytes(FileName, Editor.Export());

            //reload file
            LoadFile(FileName);
        }

        private void openReadOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "All Bin files | *.bin";
            DialogResult dr = fd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                MessageBox.Show("You are using Read-Only Mode", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Editor = new BinEditor(System.IO.File.ReadAllBytes(fd.FileName), new FormatOptions()); //Initializate with default configuration
                Editor.Import();
                listBox1.Items.Clear();
                Editor.StringsInfo = Resources.MergeStrings(ref Editor, true);
                foreach (EushullyEditor.String str in Editor.StringsInfo)
                    listBox1.Items.Add(str.Content);

            }
        }
    }
}