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
        public static List<string> DRChoicerControlChars = new List<string>{
            "\\\\C1",
            "\\\\C2",
            "\\\\C3",
            "\\\\C4",
            "\\C1",
            "\\C2",
            "\\C3",
            "\\C4",
            "\\C"
        };
        public static List<string> FormatChars = new List<string>{
            "/%%",
            "/%%.", // SCR_TEXT_1935 in UNDERTALE
            "/%%\"", // item_desc_23_0 in UNDERTALE
            "/^",
            "/%",
            "/",
            "%%",
            "%",
            "-",
        };
        // hack
        // my dumb ass going to throw all strings with these substrings into ""
        public static List<string> ForceShuffleReferenceChars = new List<string>{
            "#",
            "&",
            "^",
            "||",
            "\\[1]",
            "\\[2]",
            "\\[C]",
            "\\[G]",
            "\\[I]",
            "\\*Z",
            "\\*X",
            "\\*C",
            "\\*D",
            "\\X",
            "\\W",
            "* ",
            "...",
            "~"
        };

        public static bool ShuffleChunk(IList<UndertaleObject> chuck, Random random, float shufflechance, StreamWriter logstream,
            bool friskmode, Func<IList<UndertaleObject>, Random, float, StreamWriter, bool, bool> shufflefunc)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            try
            {
                if (!shufflefunc(chuck, random, shufflechance, logstream, friskmode))
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
        public static bool ShuffleChunk(IList<UndertaleObject> chuck, Random random, float shufflechance, StreamWriter logstream, bool friskmode)
        {
            return ShuffleChunk(chuck, random, shufflechance, logstream, friskmode, SimpleShuffle);
        }

        enum ComplexShuffleStep : byte { Shuffling, SecondLog }

        public static Func<IList<UndertaleObject>, Random, float, StreamWriter, bool, bool> ComplexShuffle(
            Func<IList<UndertaleObject>, Random, float, StreamWriter, bool, bool> shuffler)
        {
            return (IList<UndertaleObject> chuck, Random random, float chance, StreamWriter logstream, bool friskmode) =>
            {
                ComplexShuffleStep step = ComplexShuffleStep.Shuffling;
                try
                {
                    shuffler(chuck, random, chance, logstream, friskmode);
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

        public static bool SimpleShuffler(IList<UndertaleObject> chuck, Random random, float shuffleChance, StreamWriter logStream, bool _friskMode)
        {
            List<int> ints = new List<int>();
            for (int i = 0; i < chuck.Count; i++)
            {
                ints.Add(i);
            }
            ints.SelectSome(shuffleChance, random);
            chuck.ShuffleOnlySelected(ints, random);
            return true;
        }

        public static Func<IList<UndertaleObject>, Random, float, StreamWriter, bool, bool> SimpleShuffle = ComplexShuffle(SimpleShuffler);

        public static readonly Regex jsonLineRegex = new Regex("\\s*\"(.+)\":\\s*\"(.+)\",", RegexOptions.ECMAScript);

        public class JSONStringEntry
        {
            public string Key;
            public string Str;

            public JSONStringEntry(string key, string str)
            {
                Key = key;
                Str = str;
            }
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
                string ch = "";
                foreach (string chr in DRChoicerControlChars)
                {
                    if (s.Str.Contains(chr))
                    {
                        ch = chr;
                        break;
                    }
                }
                if (ch != "")
                {
                    if (!stringDict.ContainsKey(ch))
                        stringDict[ch] = new List<JSONStringEntry>();
                    stringDict[ch].Add(s);
                }
                else
                {
                    string ending = "";
                    foreach (string ed in FormatChars)
                    {
                        if (s.Str.EndsWith(ed))
                        {
                            ending = ed;
                            break;
                        }
                    }
                    if (!stringDict.ContainsKey(ending))
                        stringDict[ending] = new List<JSONStringEntry>();

                    stringDict[ending].Add(s);
                }
                totalStrings++;
            }

            foreach (string ending in stringDict.Keys)
            {
                logstream.WriteLine($"Added {stringDict[ending].Count} JSON string entries of ending <{ending}> to dialogue string List.");

                List<int> ints = new List<int>();
                for (int i = 0; i < stringDict[ending].Count; i++)
                    ints.Add(i);
                ints.SelectSome(shufflechance, random);
                stringDict[ending].ShuffleOnlySelected(ints, (n, k) => {
                    string tmp = stringDict[ending][n].Key;
                    stringDict[ending][n].Key = stringDict[ending][k].Key;
                    stringDict[ending][k].Key = tmp;
                }, random);

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
