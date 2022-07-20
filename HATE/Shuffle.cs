using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UndertaleModLib;
using System.Collections;

namespace HATE
{
    static class Shuffle
    {
        public const int WordSize = 4;

        public static bool ShuffleChunk(IList<UndertaleObject> chuck, Random random, float shufflechance, StreamWriter logstream,
            Func<IList<UndertaleObject>, Random, float, StreamWriter, bool> shufflefunc)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            try
            {
                if (!shufflefunc(chuck, random, shufflechance, logstream))
                {
                    logstream.WriteLine($"Error occured while modifying chuck.");
                    return false;
                }
            }
            catch (Exception e)
            {
                logstream.Write($"Caught exception during modification of the chuck. -> {e}");
                throw;
            }
            return false;
        }
        public static bool ShuffleChunk(IList<UndertaleObject> chuck, Random random, float shufflechance, StreamWriter logstream)
        {
            return ShuffleChunk(chuck, random, shufflechance, logstream, SimpleShuffle);
        }

        enum ComplexShuffleStep : byte { Shuffling, SecondLog }

        public static Func<IList<UndertaleObject>, Random, float, StreamWriter, bool> ComplexShuffle(
            Func<IList<UndertaleObject>, Random, float, StreamWriter, IList<UndertaleObject>> shuffler)
        {
            return (IList<UndertaleObject> chuck, Random random, float chance, StreamWriter logstream) =>
            {
                ComplexShuffleStep step = ComplexShuffleStep.Shuffling;
                try
                {
                    var newChuck = shuffler(chuck, random, chance, logstream);
                    chuck.Clear();
                    foreach (UndertaleObject obj in newChuck)
                        chuck.Add(obj);
                    step = ComplexShuffleStep.SecondLog;
                    logstream.WriteLine($"Shuffled {chuck.Count} pointers.");
                }
                catch (Exception ex)
                {
                    logstream.WriteLine($"Caught exception [{ex}] while modifying chunk, during step {step}.");
                    throw;
                }

                return true;
            };
        }

        public static IList<UndertaleObject> SimpleShuffler(IList<UndertaleObject> chuck, Random random, float shuffleChance, StreamWriter logStream)
        {
            chuck.Shuffle(random);
            return chuck;
        }

        public static Func<IList<UndertaleObject>, Random, float, StreamWriter, bool> SimpleShuffle = ComplexShuffle(SimpleShuffler);

        public static readonly Regex jsonLineRegex = new Regex("\\s*\"(.+)\":\\s*\"(.+)\",", RegexOptions.ECMAScript);

        public class JSONStringEntry
        {
            public string Key;
            public string Ending;
            public string Str;

            public JSONStringEntry(string key, string str)
            {
                Key = key;
                Str = str;
                char[] FormatChars = { '%', '/', 'C' };
                List<char> Ending = new List<char>();

                for (int i = 1; i < str.Length; i++)
                {
                    char C = str[str.Length - i];

                    if (FormatChars.Contains(C))
                        Ending.Add(C);
                    else
                        break;
                }

                Ending.Reverse();
                this.Ending = new string(Ending.ToArray());
            }
        }

        public static void JSONSwapLoc(JSONStringEntry lref, JSONStringEntry rref)
        {
            string tmp = lref.Key;
            lref.Key = rref.Key;
            rref.Key = tmp;
        }

        public static bool JSONStringShuffle(string resource_file, string target_file, Random random, float shufflechance, StreamWriter logstream)
        {
            // actual JSON libraries are complicated, thankfully we can bodge it together with magic and friendship
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            byte[] readBuffer = new byte[WordSize];

            List<JSONStringEntry> strings = new List<JSONStringEntry>();

            using (FileStream stream = new FileStream(resource_file, FileMode.OpenOrCreate))
            {
                StreamReader text_reader = new StreamReader(stream);
                logstream.WriteLine($"Opened {resource_file}.");

                while (stream.Position != stream.Length)
                {
                    string cur_line = text_reader.ReadLine();
                    if (cur_line == "}" || cur_line == "{") { continue; }

                    Match m = jsonLineRegex.Match(cur_line);


                    string[] match = {m.Groups[1].ToString(), m.Groups[2].ToString() };

                    strings.Add(new JSONStringEntry(match[0], match[1]));
                }
            }
            logstream.WriteLine($"Closed {resource_file}.");

            logstream.WriteLine($"Gathered {strings.Count} JSON String Entries. ");

            string[] bannedStrings = { "_", "||" };
            string[] bannedKeys = { "date" };

            List<JSONStringEntry> good_strings = new List<JSONStringEntry>();
            List<JSONStringEntry> final_list = new List<JSONStringEntry>();

            foreach (JSONStringEntry entry in strings)
            {
                if (entry.Str.Length >= 3 && entry.Key.Contains('_'))
                {
                    good_strings.Add(entry);
                }
                else
                {
                    final_list.Add(entry);
                }
            }

            logstream.WriteLine($"Kept {good_strings.Count} good JSON string entries.");
            logstream.WriteLine($"Fastforwarded {final_list.Count} JSON string entries to the final phase.");

            Dictionary<string, List<JSONStringEntry>> stringDict = new Dictionary<string, List<JSONStringEntry>>();
            int totalStrings = 0;

            foreach (JSONStringEntry s in good_strings)
            {
                if (!stringDict.ContainsKey(s.Ending))
                    stringDict[s.Ending] = new List<JSONStringEntry>();

                stringDict[s.Ending].Add(s);
                totalStrings++;
            }

            foreach (string ending in stringDict.Keys)
            {
                logstream.WriteLine($"Added {stringDict[ending].Count} JSON string entries of ending <{ending}> to dialogue string List.");

                stringDict[ending].Shuffle(JSONSwapLoc, random);

                final_list = final_list.Concat(stringDict[ending]).ToList();
            }

            using (FileStream out_stream = new FileStream(target_file, FileMode.Create, FileAccess.ReadWrite))
            {
                logstream.WriteLine($"Opened {target_file}.");
                StreamWriter writer = new StreamWriter(out_stream);
                writer.Write("{\n");

                foreach (JSONStringEntry s in final_list)
                {
                    char comma = (s.Key == final_list[final_list.Count - 1].Key) ? ' ' : ',';
                    writer.WriteLine($"\t\"{s.Key}\": \"{s.Str}\"{comma} ");
                }
                writer.Write("}\n");
                writer.Flush();
            }

            logstream.WriteLine($"Closed {target_file}.");

            return false;
        }
    }
}
