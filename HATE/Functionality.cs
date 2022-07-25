using System;
using System.IO;
using UndertaleModLib;
using UndertaleModLib.Util;

namespace HATE;

public static class Functionality
{
    public static bool ShuffleAudio_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
    {
        return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("SOND"), data, random, chance, logstream, _friskMode)
               && Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("STRG"), data, random, chance, logstream, _friskMode,
                                       Shuffle.ComplexShuffle(Shuffle.ShuffleAudio2_Shuffler));
        //&& Shuffle.ShuffleChunk(data.EmbeddedAudio, data, random, chance, logStream);               
    }

    public static bool ShuffleBG_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
    {
        return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("BGND"), data, random, chance, logstream, _friskMode) &&
               (!data.IsGameMaker2() || Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("SPRT"), data, random, chance, logstream, _friskMode,
                                                             Shuffle.ComplexShuffle(Shuffle.ShuffleBG2_Shuffler)));
    }

    public static bool ShuffleFont_Func(UndertaleData data, Random random, float chance, StreamWriter logStream, bool friskMode)
    {
        return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("FONT"), data, random, chance, logStream, friskMode);
    }

    public static bool HitboxFix_Func(UndertaleData data, Random random, float chance, StreamWriter logStream, bool friskMode)
    {
        return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("SPRT"), data, random, chance, logStream, friskMode, Shuffle.ComplexShuffle(Shuffle.HitboxFix_Shuffler));
    }

    public static bool ShuffleGFX_Func(UndertaleData data, Random random, float chance, StreamWriter logStream, bool friskMode)
    {
        return Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("SPRT"), data, random, chance, logStream, friskMode, Shuffle.ComplexShuffle(Shuffle.ShuffleGFX_Shuffler));
    }

    public static bool ShuffleText_Func(UndertaleData data, Random random, float chance, StreamWriter logStream, bool friskMode)
    {
        bool success = true;
        if (Directory.Exists("./lang") && SafeMethods.GetFiles("lang").Count > 0)
        {
            foreach (string path in SafeMethods.GetFiles("lang"))
                success = success && Shuffle.JSONStringShuffle(path, path, data, random, chance, logStream);
        }
        return success && Shuffle.ShuffleChunk(data.FORM.Chunks.GetValueOrDefault("STRG"), data, random, chance, logStream, friskMode, Shuffle.ComplexShuffle(Shuffle.ShuffleText_Shuffler));
    }
}