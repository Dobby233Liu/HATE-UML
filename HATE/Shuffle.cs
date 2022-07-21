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
            "~",
            "(",
            ")",
            "?",
            "'",
            " ",
            ";",
            "-",
            "Greetings.",
            "Understood."
        };
        public static string[] BannedStrings = {
            "%%", "@@",
            "Z}", "`}", "0000000000000000", "~~~",
            "DO NOT EDIT IN GAMEMAKER",
            "gml_", "scr_", "SCR_", "room_", "obj_", "blt_", "spr_",
            "audiogroup_", "macros",
            "abc_1", "show_debug_message", "string_",
            "battle_", "item_", "recover_hp",
            "music/", ".ogg", "external/", "bg_", "Compatibility_", "path_",
            "attention_hackerz_no_2", "snd_", "__",
            "castroll_", "credits_", "mettnews_",
            "shop1_", "shop2_", "shop3_", "shop4_", "shop5_",
            "mett_opera", "item_name_", "item_use_",
            "the_end", "mett_ratings_", "mettquiz_", "trophy_install",
            "hardmode_", "elevator_", "flowey_", "item_drop_", "labtv_",
            "damage_", "joyconfig_", "settings_", "title_", "soundtest_",
            "roomname_", "title_press_", "instructions_",
            "save_menu_", "stat_menu_", "field_", "weekday_", "menu_", "snd_",
            "phonename_", "image_", "papyrus_", "audio_", "steam_", "sprite_",
            "msg", "fnt_",
            "name_entry_",
            "file0", "donate_", "NaN", "system_information_", "ord(",
            "monstername_", "undertale.ini", "dr.ini", "keyconfig_", ".ini", "config.ini", "credits.txt",
            "true_config.ini", ".json",
            "Bscotch", "testlines.txt", "s_key", "_key", "_label", "shop_", "basicmonster_",
            "undertale.exe", "flowey.exe", "undertale.EXE", "Undertale.exe",
            "UNDERTALE.exe", "file1", "file2", "file3", "file4", "file5",
            "file6", "file7", "file8", "file9", "Action1", "Action2", "Action3",
            "000010000", "000001000", "000100000", "000000010", "000000001",
            "Alter", "Met1", "alter2", "FloweyExplain1", "Mett", "EndMet",
            "MeetLv1", "Pass", "MeetLv2", "MeetLv", "KillYou", "Gameover", ".sav", "graphic_", "GMSpriteFramesTrack",
            "in_Position", "in_Colour", "in_TextureCoord", "_shot.png", ".gif", "special_name_check_",
            "_ch1.json", "u_pixelSize", "u_Uvs", "u_paletteId", "u_palTexture",
            "DEVICE_", "global.flag[",
            "zurasuon", "zurasuoff", "emote", "speaker", "instancecreate",
            "var", "in", "globalvar", "autowalk", "autofacing", "depthobject",
            "stick", "mus", "music", "free_all", "resume", "initplay", "initloop",
            "volume", "pitchtime", "loopsfxpitch", "loopsfxpitchtime", "loopsfxstop",
            "loopsfxvolume", "panspeed", "pan", "panobj", "pannable", "jump", "jumpinplace",
            "addxy", "arg_objectxy", "actortoobject", "actortokris", "actortocaterpillar",
            "saveload", "save", "load", "waitcustom", "waitdialoguer", "waitbox", "terminate",
            "terminatekillactors", "krislight", "krisdark", "susielight", "susielighteyes", "susiedark",
            "susiedarkeyes", "ralseihat", "ralseinohat", "noellelight", "noelledark", "berdlylight", "berdlydark",
            "susieunhappy", "berdlyunhappy", "ralseiunhappy", "queenchair",
            "temp_save_failed", "filech",
            "noone", "enter_button_assign",
            "mainbig", "comicsans", "tinynoelle", "dotumche", "noelle_cropped", "leftmid", "rightmid", "bottommid",
            "<hash>", "enemytalk", "enemyattack", "noroom=", "_pocketed=", "_noroominventory=",
            "soundplay", "walkdirect",
            "showdialog", "savepadindex", "slottitle",
            "saveslotsize",
        };
        public static string[] FriskSpriteHandles = {
            // UNDERTALE
            "spr_maincharal", "spr_maincharau", "spr_maincharar", "spr_maincharad",
            "spr_maincharau_stark", "spr_maincharar_stark", "spr_maincharal_stark",
            "spr_maincharad_pranked", "spr_maincharal_pranked",
            "spr_maincharad_umbrellafall", "spr_maincharau_umbrellafall", "spr_maincharar_umbrellafall", "spr_maincharal_umbrellafall",
            "spr_maincharad_umbrella", "spr_maincharau_umbrella", "spr_maincharar_umbrella", "spr_maincharal_umbrella",
            "spr_charad", "spr_charad_fall", "spr_charar", "spr_charar_fall", "spr_charal", "spr_charal_fall", "spr_charau", "spr_charau_fall",
            "spr_maincharar_shadow", "spr_maincharal_shadow", "spr_maincharau_shadow", "spr_maincharad_shadow",
            "spr_maincharal_tomato", "spr_maincharal_burnt", "spr_maincharal_water",
            "spr_maincharar_water", "spr_maincharau_water", "spr_maincharad_water", "spr_mainchara_pourwater",
            "spr_maincharad_b", "spr_maincharau_b", "spr_maincharar_b", "spr_maincharal_b",
            "spr_doorA", "spr_doorB", "spr_doorC", "spr_doorD", "spr_doorX",
            // DELTARUNE
            "spr_krisr", "spr_krisl", "spr_krisd", "spr_krisu", "spr_kris_fall", "spr_krisr_sit",
            "spr_krisd_dark", "spr_krisr_dark", "spr_krisu_dark", "spr_krisl_dark",
            "spr_krisd_slide", "spr_krisd_slide_light",
            "spr_krisd_heart", "spr_krisd_slide_heart", "spr_krisu_heart", "spr_krisl_heart", "spr_krisr_heart",
            "spr_kris_fallen_dark", "spr_krisu_run", "spr_kris_fall_d_white", "spr_kris_fall_turnaround",
            "spr_kris_fall_d_lw", "spr_kris_fall_d_dw", "spr_kris_fall_smear", "spr_kris_dw_landed",
            "spr_kris_fall_ball", "spr_kris_jump_ball", "spr_kris_dw_land_example_dark", "spr_kris_fall_example_dark",
            "spr_krisu_fall_lw", "spr_kris_pose", "spr_kris_dance",
            "spr_kris_sword_jump", "spr_kris_sword_jump_down", "spr_kris_sword_jump_settle", "spr_kris_sword_jump_up",
            "spr_kris_coaster", "spr_kris_coaster_hurt_front", "spr_kris_coaster_hurt_back",
            "spr_kris_coaster_front", "spr_kris_coaster_empty", "spr_kris_coaster_back",
            "spr_kris_hug_left", "spr_kris_peace", "spr_kris_rude_gesture",
            "spr_kris_sit_wind", "spr_kris_hug", "spr_krisb_pirouette", "spr_krisb_bow",
            "spr_krisb_victory", "spr_krisb_defeat", "spr_krisb_attackready",
            "spr_krisb_act", "spr_krisb_actready", "spr_krisb_itemready", "spr_krisb_item",
            "spr_krisb_attack", "spr_krisb_hurt", "spr_krisb_intro", "spr_krisb_idle", "spr_krisb_defend",
            "spr_krisb_virokun", "spr_krisb_virokun_doctor", "spr_krisb_virokun_nurse", "spr_krisb_wan",
            "spr_krisb_wan_tail", "spr_krisb_wiggle",
            "spr_krisb_ready_throw_torso", "spr_krisb_ready_throw_full", "spr_krisb_throw",
            "spr_krisd_bright", "spr_krisl_bright", "spr_krisr_bright", "spr_krisu_bright",
            "spr_kris_fell",
            "spr_teacup_kris", "spr_teacup_kris_tea", "spr_teacup_kris_tea2", "spr_kris_tea",
            "spr_kris_hug_ch1",
            "spr_krisb_pirouette_ch1", "spr_krisb_bow_ch1", "spr_krisb_victory_ch1",
            "spr_krisb_defeat_ch1", "spr_krisb_attackready_ch1", "spr_krisb_act_ch1",
            "spr_krisb_actready_ch1", "spr_krisb_itemready_ch1", "spr_krisb_item_ch1",
            "spr_krisb_attack_ch1", "spr_krisb_attack_old_ch1", "spr_krisb_hurt_ch1",
            "spr_krisb_intro_ch1", "spr_krisb_idle_ch1", "spr_krisb_defend_ch1",
            "spr_kris_drop_ch1", "spr_kris_fell_ch1",
            "spr_krisr_kneel_ch1", "spr_krisd_bright_ch1", "spr_krisl_bright_ch1",
            "spr_krisr_bright_ch1", "spr_krisu_bright_ch1", "spr_krisd_heart_ch1",
            "spr_krisd_slide_heart_ch1", "spr_krisu_heart_ch1", "spr_krisl_heart_ch1",
            "spr_krisr_heart_ch1", "spr_kris_fallen_dark_ch1",
            "spr_krisd_dark_ch1", "spr_krisr_dark_ch1", "spr_krisu_dark_ch1", "spr_krisl_dark_ch1",
            "spr_krisd_slide_ch1", "spr_krisd_slide_light_ch1",
            "spr_krisr_ch1", "spr_krisl_ch1", "spr_krisd_ch1", "spr_krisu_ch1",
            "spr_krisr_sit_ch1", "spr_kris_fall_ch1",
            "spr_doorAny", "spr_doorE", "spr_doorF", "spr_doorW",
            "spr_doorE_ch1", "spr_doorF_ch1", "spr_doorW_ch1"
        };

        public static bool ShuffleChunk(IList<UndertaleObject> chuck, UndertaleData data, Random random, float shufflechance, StreamWriter logstream,
            bool friskmode, Func<IList<UndertaleObject>, UndertaleData, Random, float, StreamWriter, bool, bool> shufflefunc)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            try
            {
                if (!shufflefunc(chuck, data, random, shufflechance, logstream, friskmode))
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
        public static bool ShuffleChunk(IList<UndertaleObject> chuck, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool friskmode)
        {
            return ShuffleChunk(chuck, data, random, shufflechance, logstream, friskmode, SimpleShuffle);
        }

        enum ComplexShuffleStep : byte { Shuffling, SecondLog }

        public static Func<IList<UndertaleObject>, UndertaleData, Random, float, StreamWriter, bool, bool> ComplexShuffle(
            Func<IList<UndertaleObject>, UndertaleData, Random, float, StreamWriter, bool, bool> shuffler)
        {
            return (IList<UndertaleObject> chuck, UndertaleData data, Random random, float chance, StreamWriter logstream, bool friskmode) =>
            {
                ComplexShuffleStep step = ComplexShuffleStep.Shuffling;
                try
                {
                    shuffler(chuck, data, random, chance, logstream, friskmode);
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

        public static bool SimpleShuffler(IList<UndertaleObject> chuck, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool _friskMode)
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

        public static Func<IList<UndertaleObject>, UndertaleData, Random, float, StreamWriter, bool, bool> SimpleShuffle = ComplexShuffle(SimpleShuffler);

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
