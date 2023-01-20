using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace McTash.ArkCodeGeneratorRedux
{
    public class ACGR
    {
        private static List<string> includes = new List<string>();

        public static void Main(string[] args)
        {
            var files = Directory.GetFiles(Directory.GetCurrentDirectory());

            if (IsArkModFolder(files))
            {
                var origOutput = MakeSpawnCodes();


                //Change these bools to determine what is included (resource, counsumable, armor)
                GenerateIncludedItems(true, true, false);

                string[] lines = origOutput.Split(Environment.NewLine.ToCharArray());
                string result = GeneratePullList(lines);
                result = "PullResourceAdditions=" + result;
                result = result.TrimEnd(',');
                string newOutput = origOutput + Environment.NewLine + result;
                try
                {
                    File.WriteAllText("Output.txt", newOutput);
                    Console.WriteLine("Pull list created");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to write output file\\r\\n{e.ToString()}");
                }
            }
        }

        //private static bool ValidateFile(string filePath)
        //{
        //    try
        //    {
        //        if (!File.Exists(filePath))
        //        {
        //            return false;
        //        }

        //        string fileText = File.ReadAllText(filePath);
        //        string needle = @"Code generated with ARKMod.net";
        //        if (fileText.Contains(needle))
        //        {
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Failed to validate file{filePath.ToString()} with exception\\r\\n{e.Message}\\r\\n{e.StackTrace}");
        //    }

        //    return false;
        //}

        private static void GenerateIncludedItems(bool resource, bool consumable, bool armor)
        {
            if (resource) { includes.Add(@"PrimalItemResource_"); }
            if (consumable) { includes.Add(@"PrimalItemConsumable_"); }
            if (armor) { includes.Add(@"PrimalItemArmor_"); }
        }

        private static string GeneratePullList(string[] lines)
        {
            StringBuilder pullList = new StringBuilder();

            foreach (var line in lines)
            {
                foreach (var include in includes)
                {
                    if (line.Contains(include))
                    {
                        string result = ParseLine(line);
                        if (!string.IsNullOrEmpty(result))
                        {
                            pullList.Append(result + ",");
                        }
                    }
                }
            }

            return pullList.ToString();
        }

        private static string ParseLine(string line)
        {
            string pattern = @".*Blueprint'(?<juice>.*)'.*";
            Regex reg = new Regex(pattern);

            Match match = reg.Match(line);
            if (match.Success)
            {
                foreach (Group group in match.Groups)
                {
                    if (group.Name.Equals("juice"))
                    {
                        return group.Value;
                    }
                }
            }


            return "";
        }

        #region This code is adapted/lifted from https://www.arkmod.net/sourcefiles
        //I cannot identify the original author but full credit/rights goes to said author whomever/wherever they are from this point, minor adaptations by me.

        private static bool IsArkModFolder(string[] files)
        {
            int fileCount = 0;
            int amtFiles = files.Length;

            foreach (string file in files)
            {
                if (Path.GetFileNameWithoutExtension(file).StartsWith("PrimalGameData"))
                    return true;
                if (fileCount == amtFiles - 1)
                    return false;
                fileCount++;
            }
            return false;
        }

        private static string MakeSpawnCodes()
        {

            StringBuilder output = new StringBuilder();

            string str0 = "\nARK Code Generator REDUX";
            string str1 = "\nCode generated with ARKMod.net's ARK Code Generator. For latest version, visit https://arkmod.net/.\nHappy ARKing!";
            string str2 = "\n---------------------------------------------------------------------------------Engram Names---------------------------------------------------------------------------------\n";
            string str3 = "\n---------------------------------------------------------------------------------Item Spawncodes--------------------------------------------------------------------------------\n";
            string str4 = "\n---------------------------------------------------------------------------------Creature Spawncodes-----------------------------------------------------------------------------\n";
            string str5 = "\n---------------------------------------------------------------------------------Tamed Creature Spawncodes-----------------------------------------------------------------------\n";
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories);


            //File.Delete("Output.txt");
            //File.AppendAllText("Output.txt", str1 + Environment.NewLine);
            //File.AppendAllText("Output.txt", str2 + Environment.NewLine);

            output.Append(str0 + Environment.NewLine);
            output.Append(str1 + Environment.NewLine);
            output.Append(str2);

            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.StartsWith("EngramEntry"))
                {
                    string str6 = withoutExtension + "_C";
                    //File.AppendAllText("Output.txt", str6 + Environment.NewLine);
                    output.Append(str6 + Environment.NewLine);
                    Console.WriteLine(str6);
                }
            }
            //File.AppendAllText("Output.txt", str3 + Environment.NewLine);
            output.Append(str3 + Environment.NewLine);
            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.StartsWith("PrimalItem"))
                {
                    string str7 = "admincheat GiveItem \"Blueprint'" + path.Substring(path.IndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 1 1 0";
                    //File.AppendAllText("Output.txt", str7 + Environment.NewLine);
                    output.Append(str7 + Environment.NewLine);
                    Console.WriteLine(str7);
                }
            }
            //File.AppendAllText("Output.txt", str4 + Environment.NewLine);
            output.Append(str4 + Environment.NewLine);
            foreach (string path in files)
            {
                string withoutExtension = Path.GetFileNameWithoutExtension(path);
                if (withoutExtension.Contains("Character_BP"))
                {
                    string str8 = "admincheat SpawnDino \"Blueprint'" + path.Substring(path.IndexOf("Content")).Replace("Content\\", "\\Game\\").Replace(".uasset", "." + withoutExtension).Replace("\\", "/") + "'\" 500 0 0 120";
                    //File.AppendAllText("Output.txt", str8 + Environment.NewLine);
                    output.Append(str8 + Environment.NewLine);
                    Console.WriteLine(str8);
                }
            }
            //File.AppendAllText("Output.txt", str5 + Environment.NewLine);
            output.Append(str5 + Environment.NewLine);
            foreach (string path in files)
            {
                string str9 = Path.GetFileNameWithoutExtension(path) + "_C";
                if (str9.Contains("Character_BP"))
                {
                    string str10 = "admincheat GMSummon \"" + str9 + "\" 120";
                    //File.AppendAllText("Output.txt", str10 + Environment.NewLine);
                    output.Append(str10 + Environment.NewLine);
                    Console.WriteLine(str10);
                }
            }
            //File.AppendAllText("Output.txt", str1 + Environment.NewLine);
            output.Append(str1 + Environment.NewLine);

            return output.ToString();
        }
        #endregion
    }
}


