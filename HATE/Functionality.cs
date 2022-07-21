using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using UndertaleModLib;
using UndertaleModLib.Models;
using System.Drawing;
using UndertaleModLib.Util;
using System.Collections;

namespace HATE
{
    public static class Functionality
    {
        private static readonly string[] _friskSpriteHandles = {
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

        public static bool ShuffleAudio_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)data.Sounds, random, chance, logstream, _friskMode); 
                   //&& Shuffle.ShuffleChunk(data.EmbeddedAudio, random, chance, logstream);               
        }

        public static bool ShuffleBG_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)data.Backgrounds, random, chance, logstream, _friskMode);              
        }

        public static bool ShuffleFont_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)data.Fonts, random, chance, logstream, _friskMode);
        }

        public static bool HitboxFix_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)data.Sprites, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(HitboxFix_Shuffler));
        }

        public static bool ShuffleGFX_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)data.Sprites, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(ShuffleGFX_Shuffler));
        }

        public static bool ShuffleText_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            bool success = true;
            if (Directory.Exists("./lang") && SafeMethods.GetFiles("./lang").Count > 0)
            {
                foreach (string path in SafeMethods.GetFiles("./lang"))
                {
                    success = success && Shuffle.JSONStringShuffle(path, path, random, chance, logstream);
                }
            }
            return success && Shuffle.ShuffleChunk((IList<UndertaleObject>)data.Strings, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(ShuffleText_Shuffler));
        }

        public static bool ShuffleGFX_Shuffler(IList<UndertaleObject> chunk, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            List<int> sprites = new List<int>();

            for (int i = 0; i < chunk.Count; i++)
            {
                UndertaleSprite pointer = (UndertaleSprite)chunk[i];

                if (!_friskSpriteHandles.Contains(pointer.Name.Content.Trim()) || _friskMode)
                    sprites.Add(i);
            }

            chunk.ShuffleOnlySelected(sprites, random, shufflechance);
            logstream.WriteLine($"Shuffled {sprites.Count} out of {chunk.Count} sprite pointers.");

            return true;
        }

        public static bool HitboxFix_Shuffler(IList<UndertaleObject> pointerlist, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            TextureWorker worker = new TextureWorker();
            foreach (UndertaleObject _spriteptr in pointerlist)
            {
                // based on ImportGraphics.csx
                UndertaleSprite spriteptr = (UndertaleSprite)_spriteptr;
                spriteptr.CollisionMasks.Clear();
                spriteptr.CollisionMasks.Add(spriteptr.NewMaskEntry());
                Bitmap tex = worker.GetTextureFor(spriteptr.Textures[0].Texture, spriteptr.Name.Content, true);
                Rectangle bmpRect = new Rectangle(0, 0, tex.Width, tex.Height);
                Bitmap cloneBitmap = tex.Clone(bmpRect, tex.PixelFormat);
                int width = (((int)cloneBitmap.Width + 7) / 8) * 8;
                BitArray maskingBitArray = new BitArray(width * (int)cloneBitmap.Height);
                for (int y = 0; y < (int)spriteptr.Height; y++)
                {
                    for (int x = 0; x < (int)spriteptr.Width; x++)
                    {
                        Color pixelColor = cloneBitmap.GetPixel(x, y);
                        maskingBitArray[y * width + x] = (pixelColor.A > 0);
                    }
                }
                BitArray tempBitArray = new BitArray(width * (int)cloneBitmap.Height);
                for (int i = 0; i < maskingBitArray.Length; i += 8)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tempBitArray[j + i] = maskingBitArray[-(j - 7) + i];
                    }
                }
                int numBytes;
                numBytes = maskingBitArray.Length / 8;
                byte[] bytes = new byte[numBytes];
                tempBitArray.CopyTo(bytes, 0);
                for (int i = 0; i < bytes.Length; i++)
                    spriteptr.CollisionMasks[0].Data[i] = bytes[i];
            }
            logstream.WriteLine($"Wrote {pointerlist.Count} collision boxes.");

            return true;
        }

        // TODO: clean this
        public static bool ShuffleText_Shuffler(IList<UndertaleObject> _pointerlist, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            string[] bannedStrings = { "_" };

            Dictionary<string, List<int>> stringDict = new Dictionary<string, List<int>>();

            for (int i = 0; i < _pointerlist.Count; i++)
            {
                var s = (UndertaleString)_pointerlist[i];
                var convertedString = s.Content;

                if (convertedString.Length >= 3 && !bannedStrings.Any(convertedString.Contains) && !(convertedString.Any(x => x > 127)))
                {
                    List<char> Ending = new List<char>();

                    for (int i2 = 1; i2 < convertedString.Length; i2++)
                    {
                        char C = convertedString[convertedString.Length - i2];

                        if (Shuffle.FormatChars.Contains(C))
                            Ending.Add(C);
                        else
                            break;
                    }

                    Ending.Reverse();
                    string ending = new string(Ending.ToArray());

                    if (!stringDict.ContainsKey(ending))
                        stringDict[ending] = new List<int>();

                    stringDict[ending].Add(i);
                }
            }

            foreach (string ending in stringDict.Keys)
            {
                logstream.WriteLine($"Added {stringDict[ending].Count} string pointers of ending {ending} to dialogue string List.");

                _pointerlist.ShuffleOnlySelected(stringDict[ending], random, shufflechance);
            }

            return true;
        }
    }
}
