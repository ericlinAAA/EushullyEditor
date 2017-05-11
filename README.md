## EushullyEditor v3
EushullyEditor Library is a tool to make your own Eushully Script Editor.

### Features
- **Overwrite**: Rewrite all Kamidori strings.
- **Append**: Move recognized strings to the end of the script to avoid bad configurations.
- **AutoDetect**: Rewrite strings without changing unknown offsets.

### YOU NEED TO CONFIGURE IT!
I configured it to work with Kamidori. If you don't know how to configure it, you can try
contacting me and asking for help.

Good luck.

#### Release Resources:
- Hide Eushully Error: https://github.com/guquabc/eushullyNoMsgBox


### Usage:

```
    //To Load
    using VNX.EushullyEditor;
    //...
    FormatOptions Config = new FormatOptions(); //Start with default config
    EushullyEditor Editor = new EushullyEditor(File.ReadAllBytes("C:\\Script.bin"));
    string[] AllStrings = Editor.Import(); //Create Variable with all entries
    
    //...
    
    //To Save	
    byte[] OutScript = Editor.Export(AllStrings); //Get script with new strings
    System.IO.File.WriteAllBytes(@"C:\sample-out.bin", OutScript); //save the script
```

####Tested With: Kamidori and Kami no Rhapsody
