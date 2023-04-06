using Microsoft.VisualBasic.FileIO;
using System;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
#nullable disable

bool running = true;
if (!File.Exists(SpecialDirectories.MyDocuments + "\\DOOMModsFolderPathForBC3DDSApp.txt")) // Checks if storage folder for DOOM Mods folder path exists - Bol
    File.Create(SpecialDirectories.MyDocuments + "\\DOOMModsFolderPathForBC3DDSApp.txt").Close(); // If not - create and leave until require - Bol
StreamReader DOOMModsPathReader = new StreamReader(SpecialDirectories.MyDocuments + "\\DOOMModsFolderPathForBC3DDSApp.txt");
string DOOMModsPath = DOOMModsPathReader.ReadLine(); // Give DOOMModsPath string the path to doom mods - Bol
DOOMModsPathReader.Close();
string encoding = "BC3";
string encodingFileName = "";

Console.ForegroundColor = ConsoleColor.White;

while (running)
{
    char choice = ' ';
    Console.Title = encoding + " DDS To BImage 2.51"; // Set Console Title - Bol
    Console.Clear(); // For not duplicating or keeping anything - Bol
    Console.Write($"DOOM Snapmap Discord Server, go join! We got a lot of modding here -> https://discord.gg/snapmap\n\n\tMENU:\n\n\t\t1 - BImage Generator\n\n\t\t2 - Change Encoding [{encoding}] RECOMMENDED LIMIT 2048x2048\t|    MAX LIMIT 4096x4096\n\n\t\t3 - Help\n\n\t\t4 - Credits\n\n\t\t5 - Change DOOM Mods folder path [{DOOMModsPath}]\n\n\t\t6 - Close\n\n\t\t7 - Changelog\n\n\t\t\tChoice: "); // Menu visuals
    choice = Console.ReadKey().KeyChar; // Initializing Char Choice, giving it value of a button which was pressed - Bol
    if (choice == '1' || choice == 13)
    {
            Console.Write("\n\nPath to .dds File: "); // Ask for .dds file full path - Bol
            string path = Console.ReadLine();
            if (path == "")
                path = "a";
            FileInfo ddsFile = new FileInfo(path); // Initializing FileInfo for getting file size and other stuff. User gives it a path of .dds file - Bol

            if (!ddsFile.Exists || ddsFile.Extension != ".dds" || ddsFile.Length > 16777363) // If file does not exist, does not have .dds extension or is too big
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File may not exist, may not be .dds, may be larger than 16777363 bytes (16,777362999999998 megabytes) or input is invalid. Instructions on making custom textures can be found in 2 - Help from main menu");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
            else // If file passes filter
            {
                if (!File.Exists(ddsFile.Directory + $"\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.txt")) // Checks if file exists
                    File.Create(ddsFile.Directory + $"\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.txt").Close(); // Creates if its not - Bol

                if (!File.Exists(ddsFile.Directory + $"\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image")) // Same here but for result file - Bol
                    File.Create(ddsFile.Directory + $"\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image").Close();


                Stream stream = File.Open(ddsFile.FullName, FileMode.Open, FileAccess.ReadWrite); // Initialize stream for the DDSFile to read information from DDSFile - Bol
                stream.Position = 150; // Set position of the stream to the 150th byte of DDSFile's stream - Bol
                Stream streamBImageTemp = File.Open(ddsFile.Directory + $"\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.txt", FileMode.Open, FileAccess.ReadWrite);
                Stream streamBImage = File.Open(ddsFile.Directory + $"\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image", FileMode.Open, FileAccess.ReadWrite);
                string BImagePath = ddsFile.Directory + $"\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image";

                if (DOOMModsPath != "") // im writing doom mods auto implementation part and all i want is to get sleep. do not wait for explanations
                {
                    Console.Write($"Implement into {DOOMModsPath} with decl;material? 1 - Yes, 2 - No\n\t\tChoice: ");
                    choice = Console.ReadKey().KeyChar;
                    if (choice == '1')
                    {
                        Console.Write("\nFile inside generated/image that bimage needs to be in: ");
                        string file = Console.ReadLine();
                        string filePath = "";
                        if (file != "")
                        {
                            Directory.CreateDirectory(DOOMModsPath + "\\generated\\image\\" + file);
                            File.Create(DOOMModsPath + $"\\generated\\image\\{file}\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image").Close();
                            streamBImage = File.Open(DOOMModsPath + $"\\generated\\image\\{file}\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image", FileMode.Open, FileAccess.ReadWrite);
                            BImagePath = DOOMModsPath + $"\\generated\\image\\{file}\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image";
                            filePath = $"{file}\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.tga";
                        }
                        else
                        {
                            File.Create(DOOMModsPath + $"\\generated\\image\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image").Close();
                            streamBImage = File.Open(DOOMModsPath + $"\\generated\\image\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image", FileMode.Open, FileAccess.ReadWrite);
                            BImagePath = DOOMModsPath + $"\\generated\\image\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}{encodingFileName}.bimage;image";
                            filePath = $"{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}.tga";
                        }
                        Console.Write("\nFile inside generated/image that decl;material needs to be in: ");
                        string fileMaterial = Console.ReadLine();
                        if (fileMaterial != "")
                        {
                            Directory.CreateDirectory(DOOMModsPath + "\\generated\\decls\\material\\" + fileMaterial);
                            File.Create(DOOMModsPath + $"\\generated\\decls\\material\\{fileMaterial}\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}.decl;material").Close();
                            StreamWriter declWriter = new StreamWriter(DOOMModsPath + $"\\generated\\decls\\material\\{fileMaterial}\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}.decl;material");
                            declWriter.Write("{\r\nambientprogram\tca_sledge\r\nsparediffusemap    " + filePath + "\r\nvirtualmapping\t{ 0.000488, 0.000488, 0.303711, 0.771973 }\r\nusevirtualmapping\t1.000000\r\nvirtualtexturefeedbackfloat\t{ 1.000000, 2048.000000, 245760.000000, 16.000000 }\r\nphysicalfilterparms\tphysicalvmtrfilterparms\r\nminlodmap\t_black\r\npagetablemap\t_vmtrpagetable\r\nphysicalmappingsmap\t_physicalvmtrmappings\r\nphysicalpagesmaparray0\t_physicalvmtrpages0\r\nphysicalpagesmaparray1\t_physicalvmtrpages0\r\nphysicalpagesmap4\t_black\r\nphysicalpagesprtmap4\t_black\r\n}"); ;
                            declWriter.Close();
                        }
                        else
                        {
                            File.Create(DOOMModsPath + $"\\generated\\decls\\material\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}.decl;material").Close();
                            StreamWriter declWriter = new StreamWriter(DOOMModsPath + $"\\generated\\decls\\material\\{ddsFile.Name.Substring(0, ddsFile.Name.Length - 4)}.decl;material");
                            declWriter.Write("{\r\nambientprogram\tca_sledge\r\nsparediffusemap    " + filePath + "\r\nvirtualmapping\t{ 0.000488, 0.000488, 0.303711, 0.771973 }\r\nusevirtualmapping\t1.000000\r\nvirtualtexturefeedbackfloat\t{ 1.000000, 2048.000000, 245760.000000, 16.000000 }\r\nphysicalfilterparms\tphysicalvmtrfilterparms\r\nminlodmap\t_black\r\npagetablemap\t_vmtrpagetable\r\nphysicalmappingsmap\t_physicalvmtrmappings\r\nphysicalpagesmaparray0\t_physicalvmtrpages0\r\nphysicalpagesmaparray1\t_physicalvmtrpages0\r\nphysicalpagesmap4\t_black\r\nphysicalpagesprtmap4\t_black\r\n}");
                            declWriter.Close();
                        }
                    }
                }

                Console.Title = encoding + " DDS To BImage 2.5 (PROCESSING " + ddsFile.Name + ")"; // Set Console Title - Bol
                Console.WriteLine("\nReading " + ddsFile.Name + ", PLEASE DO NOT PRESS ANYTHING WHILE FOCUSED ON THIS WINDOW IF IT TAKES TIME"); // Tell user to not fuck around - Bol

                BinaryReader ddsFileReading = new BinaryReader(stream); // Initialize Binary Reader for reading DDSFile stream - Bol
                BinaryWriter ddsFileWriting = new BinaryWriter(streamBImageTemp); // Initialize Binary Writer for writing into .txt file stream - Bol

                byte[] readText = ddsFileReading.ReadBytes((int)stream.Length); // Allocate byte array with data from DDSFile (ignoring first 149 bytes) - Bol

                Console.WriteLine($"{stream.Position}/{stream.Length}"); // Show stream position and stream length. This was here for other reasons, but now its decoration
                stream.Position = 0;                                     // purposes. Oh, also we set DDSFile stream position to the first byte - Bol

                ddsFileWriting.Write(ddsFileReading.ReadBytes((int)stream.Length)); // Read all ddsFile and print into .txt, this was here for developer purposes.
                ddsFileReading.Close();                                             // Keeping it in case someone with very specific task and conditions needs it
                ddsFileWriting.Close();                                             // We also close binary reader and writers so these files wont be considered as 
                ddsFileWriting = new BinaryWriter(streamBImage);                    // currently being used. We also give BinaryWriter a BImage stream - Bol

                Console.Write("Read .dds, 148 Bytes deleted\n\tInsert Height of your image, please (Can be found in original .png, .dds does not have width/height props): ");
                int height = Int32.Parse(Console.ReadLine());
                Console.Write("\tAnd now we need the width: ");
                int width = Int32.Parse(Console.ReadLine()); // Parsing strings, giving their values to integers, all that. No TryParse cuz lazy lmao - Bol

                // BELOW CODE IS 80% DONE BY SamPT, FIXES BY BOL

                byte temp = 0x56; // 86 in hexademical - SamPT
                ddsFileWriting.Write(temp); // 1 - Bol

                temp = 0xD7; // 215 in hex - SamPT
                ddsFileWriting.Write(temp); // 2 - Bol

                temp = 0x10; // 16 in hex - SamPT
                ddsFileWriting.Write(temp); // 3 - Bol

                temp = 0x53; // 83 in hex - SamPT
                ddsFileWriting.Write(temp); // 4 - Bol

                int tempInt = 1112100103; // 0x07 MIB (int is 4 bytes) - SamPT
                ddsFileWriting.Write(tempInt); // 5-8 - Bol

                tempInt = 0; // 0 0 0 0 (int is 4 bytes) - SamPT
                ddsFileWriting.Write(tempInt); // 9-12 - Bol

                short tempShrt = 0; // 0 0 (short is 2 bytes) - SamPT
                ddsFileWriting.Write(tempShrt); // 13-14 - Bol

                // writer will just read these as int values like you defined them above, and write 4 bytes - SamPT
                temp = 0;
                byte[] tempArray = BigInteger.Parse($"{width}").ToByteArray().Reverse().SkipWhile(item => item == 0).ToArray();
                if (tempArray.Length < 4)
                    while (tempArray.Length != 4)
                        tempArray = tempArray.Append(temp).ToArray();
                ddsFileWriting.Write(tempArray); // 15-18. This thing is super retarded by the way. But it needs to be like that. Won't even explain it - Bol

                tempArray = BigInteger.Parse($"{height}").ToByteArray().Reverse().SkipWhile(item => item == 0).ToArray();
                if (tempArray.Length < 4)
                    while (tempArray.Length != 4)
                        tempArray = tempArray.Append(temp).ToArray();
                ddsFileWriting.Write(tempArray); // 19-22 - Bol

                ddsFileWriting.Write(tempInt); // 23-26 - Bol
                ddsFileWriting.Write(temp); // 27 - Bol

                temp = 1;
                ddsFileWriting.Write(temp); // 28 - Bol
                ddsFileWriting.Write(tempInt); // 29-32 - Bol

                if (encoding == "BC1")
                    temp = 10; // 0A - Bol
                else if (encoding == "BC3")
                    temp = 11; // just copying values from textureFormat_t - Bol
                else
                    temp = 23; // 17 - Bol
                ddsFileWriting.Write(temp); // 33 - Bol

                temp = 0;
                ddsFileWriting.Write(tempShrt); // 34-35 - Bol
                ddsFileWriting.Write(temp); // 36 - Bol

                temp = 5;
                ddsFileWriting.Write(temp); // 37 - Bol

                temp = 0;
                ddsFileWriting.Write(tempInt); // 38-41 - Bol
                ddsFileWriting.Write(tempInt); // 42-45 - Bol
                ddsFileWriting.Write(tempInt); // 46-49 - Bol
                ddsFileWriting.Write(tempShrt); // 50-51 - Bol
                ddsFileWriting.Write(temp); // 52 - Bol

                tempArray = BigInteger.Parse($"{width}").ToByteArray().Reverse().SkipWhile(item => item == 0).ToArray();
                if (tempArray.Length < 4)
                    while (tempArray.Length != 4)
                        tempArray = tempArray.Append(temp).ToArray();
                ddsFileWriting.Write(tempArray); // 53-56 - Bol

                tempArray = BigInteger.Parse($"{height}").ToByteArray().Reverse().SkipWhile(item => item == 0).ToArray();
                if (tempArray.Length < 3)
                    while (tempArray.Length != 3)
                        tempArray = tempArray.Append(temp).ToArray();
                ddsFileWriting.Write(tempArray); // 57-59 - Bol

                tempArray = BigInteger.Parse($"{ddsFile.Length - 148}").ToByteArray().Reverse().ToArray();
                ddsFileWriting.Write(tempArray); // 60-62 (actually 60-63 but this thing is weird), we get dds file size (in bytes) here and substract 148 from it - Bol

                temp = 0xFF;
                ddsFileWriting.Write(temp); // 63 - Bol
                ddsFileWriting.Write(temp); // 64 - Bol
                ddsFileWriting.Write(readText); // Now, after we're done with header we add rest of the file (skipping first 149 bytes) - Bol
                ddsFileWriting.Close(); // Close BinaryWriter - Bol

                Console.ForegroundColor = ConsoleColor.Green; // Make text green
                Console.WriteLine($"Converted to BImage!\t{BImagePath}\n\n");
                Console.ForegroundColor = ConsoleColor.White; // Make it white
                Console.ReadKey();
        }
    }
    else if (choice == '2')
    {
        if (encoding == "BC1")
            encoding = "BC3";
        else if (encoding == "BC3")
        {
            encoding = "BC7";
            encodingFileName = "$bc7";
        }
        else
        {
            encoding = "BC1";
            encodingFileName = "";
        }
    }
    else if (choice == '3') // buncha teeeext here and waiting for player input - Bol
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n\nHow to make proper .DDS file:\n\t\tIf you are not sure if your .dds file has correct compression or is valid in general, visit https://photopea.com/, drag your source image there, then go to File -> Export As -> More -> DDS and choose BC1/BC3/BC7 of compression - SamPT"); // SamPT :)
        Console.WriteLine("\n\nHow to make your texture work:\n\t\tYou should make folder for your BImage inside generated/image folder, or just put it there. Then you gotta make material for it: Go to decls/material. There you can toss\\create your material file. Here's an example:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n{\r\nambientprogram\tca_sledge\r\nsparediffusemap    fish.tga\r\nvirtualmapping\t{ 0.000488, 0.000488, 0.303711, 0.771973 }\r\nusevirtualmapping\t1.000000\r\nvirtualtexturefeedbackfloat\t{ 1.000000, 2048.000000, 245760.000000, 16.000000 }\r\nphysicalfilterparms\tphysicalvmtrfilterparms\r\nminlodmap\t_black\r\npagetablemap\t_vmtrpagetable\r\nphysicalmappingsmap\t_physicalvmtrmappings\r\nphysicalpagesmaparray0\t_physicalvmtrpages0\r\nphysicalpagesmaparray1\t_physicalvmtrpages0\r\nphysicalpagesmap4\t_black\r\nphysicalpagesprtmap4\t_black\r\n}\n");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Instead of the \"fish.tga\" used for \"sparediffusemap\" you use path to your bimage with .tga extension, and if you put it inside folder which is inside image folder, you should include folder in path too - Bol (Improved by TerraTela, information from T0y0ta)");
        Console.WriteLine("\n\nExample on how to insert path correctly:\n\t\tPath to .dds File: E:\\Downloads\\fish.dds\n\t\t\t\t\t\t\t - Bol");
        Console.ReadKey();
        Console.ForegroundColor = ConsoleColor.White;
    }
    else if (choice == '4') // buncha teeext #2 but with cool people included - Bol
    {
        Console.WriteLine("\n\n\t\tCREDITS:");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n\t\t\tBol - Main Developer, wrote most of the code");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n\t\t\tSamPT - Helped a lot with bytes stuff, very helpful and friendly");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n\t\t\tGleb - Helped with using \"using\" in less janky way (Was used in previous builds)");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n\t\t\tTerraTela - Analyzing results, making material;decl instruction more understandable");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n\t\t\ttelesnow - Testing all builds of application");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("\n\t\t\tT0y0ta - Analyzing results, made guide that was the main reason of making the program");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n\t\tThank you all for helping me with this application. Without you, guys, this'd be impossible to make :)");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey();
    }
    else if (choice == '5')
    {
        Console.Write("\n\n\t\tPath to DOOM Mods Folder: ");
        DOOMModsPath = Console.ReadLine();
        StreamWriter DOOMModsPathWriter = new StreamWriter(SpecialDirectories.MyDocuments + "\\DOOMModsFolderPathForBC3DDSApp.txt"); // Make stream writer for doom mods folder path keeper - Bol
        DOOMModsPathWriter.Write(DOOMModsPath);
        DOOMModsPathWriter.Close(); // Must have or wont save - Bol
    }
    else if (choice == '6' || choice == 27)
        running = false; // Set running to false so loop of running app on is broken, results in closing as user chose - Bol
    else if (choice == '7')
    {
        Console.WriteLine("\n\n\t\tCHANGELOG:");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n\t\t\tFrom Bol:\n\t\t\t\tMulti-Image making lazy fix, removed DE encodings, credits spellings fix, bc7 fix, feedback of convertion came back");
        Console.ForegroundColor = ConsoleColor.White;
        Console.ReadKey();
    }
}