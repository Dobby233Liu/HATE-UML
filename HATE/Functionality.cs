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
using System.Reflection;

namespace HATE
{
    public static class Functionality
    {
        public static bool ShuffleAudio_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("SOND"), data, random, chance, logstream, _friskMode)
                    && Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("STRG"), data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(ShuffleAudio2_Shuffler));
            //&& Shuffle.ShuffleChunk(data.EmbeddedAudio, data, random, chance, logstream);               
        }

        private static bool ShuffleAudio2_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleString> _pointerlist = (_chunk as UndertaleChunkSTRG)?.List;
            IList<UndertaleString> strgClone = new List<UndertaleString>(_pointerlist);
            foreach (var func in data.Sounds)
            {
                strgClone.Remove(func.Name);
                strgClone.Remove(func.Type);
                strgClone.Remove(func.File);
            }

            IList<int> stringList = new List<int>();

            for (int _i = 0; _i < strgClone.Count; _i++)
            {
                var i = _pointerlist.IndexOf(strgClone[_i]);
                string s = strgClone[_i].Content;
                if ((s.EndsWith(".ogg") || s.EndsWith(".wav") || s.EndsWith(".mp3"))
                    && !s.StartsWith("music/") /* UNDERTALE */)
                    stringList.Add(i);
            }

            stringList.SelectSome(shufflechance, random);
            logstream.WriteLine($"Added {stringList.Count} string pointers to music file references list.");

            _pointerlist.ShuffleOnlySelected(stringList, Shuffle.GetSubfunction(_pointerlist), random);

            return true;
        }

        public static bool ShuffleBG_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("BGND"), data, random, chance, logstream, _friskMode) &&
                (!data.IsGameMaker2() || Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("SPRT"), data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(Shuffle.ShuffleBG2_Shuffler)));
        }

        public static bool ShuffleFont_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("FONT"), data, random, chance, logstream, _friskMode);
        }

        public static bool HitboxFix_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("SPRT"), data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(Shuffle.HitboxFix_Shuffler));
        }

        public static bool ShuffleGFX_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("SPRT"), data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(Shuffle.ShuffleGFX_Shuffler));
        }

        public static bool ShuffleText_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            bool success = true;
            if (Directory.Exists("./lang") && SafeMethods.GetFiles("lang").Count > 0)
            {
                foreach (string path in SafeMethods.GetFiles("lang"))
                {
                    success = success && Shuffle.JSONStringShuffle(path, path, data, random, chance, logstream);
                }
            }
            return success && Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("STRG"), data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(Shuffle.ShuffleText_Shuffler));
        }
    }
}
