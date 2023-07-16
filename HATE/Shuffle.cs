using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UndertaleModLib;
using System.Collections;
using UndertaleModLib.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UndertaleModLib.Util;
using System.Drawing;
using System.Reflection;

namespace HATE;

static partial class Shuffle
{
    public static bool ShuffleChunk(UndertaleChunk chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream,
                                    bool friskMode, Func<UndertaleChunk, UndertaleData, Random, float, StreamWriter, bool, bool> shuffleFunc)
    {
        if (random == null)
            throw new ArgumentNullException(nameof(random));
        try
        {
            if (!shuffleFunc(chunk, data, random, shuffleChance, logStream, friskMode))
            {
                MsgBoxHelpers.ShowError($"Error occured while modifying chuck {chunk.Name}.");
                logStream.WriteLine($"Error occured while modifying chuck {chunk.Name}.");
                return false;
            }
        }
        catch (Exception e)
        {
            logStream.Write($"Caught exception during modification of chuck {chunk.Name}. -> {e}");
            throw;
        }
        return true;
    }
    public static bool ShuffleChunk(UndertaleChunk chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool friskMode)
    {
        return ShuffleChunk(chunk, data, random, shuffleChance, logStream, friskMode, SimpleShuffle);
    }

    enum ComplexShuffleStep : byte { Shuffling, SecondLog }

    public static Func<UndertaleChunk, UndertaleData, Random, float, StreamWriter, bool, bool> ComplexShuffle(
        Func<UndertaleChunk, UndertaleData, Random, float, StreamWriter, bool, bool> shuffler)
    {
        return (UndertaleChunk chunk, UndertaleData data, Random random, float chance, StreamWriter logStream, bool friskMode) =>
        {
            ComplexShuffleStep step = ComplexShuffleStep.Shuffling;
            try
            {
                shuffler(chunk, data, random, chance, logStream, friskMode);
                step = ComplexShuffleStep.SecondLog;
                logStream.WriteLine($"Shuffled chunk {chunk.Name}.");
            }
            catch (Exception ex)
            {
                logStream.WriteLine($"Caught exception [{ex}] while modifying chunk, during step {step}.");
                throw;
            }

            return true;
        };
    }

    public static bool SimpleShuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool _friskMode)
    {
        var chunk = (_chunk as IUndertaleListChunk)?.GetList();
        List<int> ints = new List<int>();
        for (int i = 0; i < chunk.Count; i++)
        {
            ints.Add(i);
        }
        ints.SelectSome(shuffleChance, random);
        chunk.ShuffleOnlySelected(ints, GetSubfunction(chunk, _chunk), random);
        return true;
    }

    public static bool ShuffleAudio_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool friskMode)
    {
        IList<UndertaleSound> _pointerlist = (_chunk as UndertaleChunkSOND)?.List;
        IList<int> soundList = new List<int>();

        for (int i = 0; i < _pointerlist.Count; i++)
        {
            if (!SoundBlacklist.Contains(_pointerlist[i].Name.Content))
                soundList.Add(i);
        }
        soundList.SelectSome(shuffleChance, random);
        logStream.WriteLine($"Added {soundList.Count} pointers to sound list.");

        _pointerlist.ShuffleOnlySelected(soundList, GetSubfunction(_pointerlist, _chunk), random);

        return true;
    }

    private static IEnumerable<UndertaleString> ShuffleAudio2_Strings(UndertaleData data, IList<UndertaleString> list)
    {
        List<UndertaleString> soundStrings = new();
        foreach (var sound in data.Sounds)
        {
            soundStrings.Add(sound.Name);
            soundStrings.Add(sound.Type);
            soundStrings.Add(sound.File);
        }
        foreach (var str in list)
        {
            string s = str.Content;
            if ((s.EndsWith(".ogg") || s.EndsWith(".wav") || s.EndsWith(".mp3"))
                && !s.StartsWith("music/") /* UNDERTALE */
                && !SoundBlacklist.Contains(s))
                yield return str;
        }
    }
    public static bool ShuffleAudio2_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool friskMode)
    {
        IList<UndertaleString> _pointerlist = (_chunk as UndertaleChunkSTRG)?.List;
        IList<int> stringList = new List<int>();
        foreach (var str in ShuffleAudio2_Strings(data, _pointerlist))
        {
            var i = _pointerlist.IndexOf(str);
            stringList.Add(i);
        }

        stringList.SelectSome(shuffleChance, random);
        logStream.WriteLine($"Added {stringList.Count} string pointers to music file references list.");

        _pointerlist.ShuffleOnlySelected(stringList, GetSubfunction(_pointerlist, _chunk), random);

        return true;
    }

    private static IEnumerable<UndertaleSprite> ShuffleBG2_Sprites(IList<UndertaleSprite> list)
    {
        foreach (var i in list)
        {
            if (i.Name.Content.Trim().StartsWith("bg_"))
                yield return i;
        }
    }
    public static bool ShuffleBG2_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool friskMode)
    {
        IList<UndertaleSprite> chunk = (_chunk as UndertaleChunkSPRT)?.List;
        List<int> sprites = new List<int>();
        foreach (var i in ShuffleBG2_Sprites(chunk))
        {
            sprites.Add(chunk.IndexOf(i));
        }

        sprites.SelectSome(shuffleChance, random);
        chunk.ShuffleOnlySelected(sprites, GetSubfunction(chunk, _chunk), random);
        logStream.WriteLine($"Shuffled {sprites.Count} assumed backgrounds out of {chunk.Count} sprites.");

        return true;
    }
    private static IEnumerable<UndertaleSprite> ShuffleGFX_Sprites(IList<UndertaleSprite> list, bool friskMode)
    {
        foreach (var i in list)
        {
            if (!i.Name.Content.Trim().StartsWith("bg_") &&
                (!FriskSpriteHandles.Contains(i.Name.Content.Trim()) || friskMode))
                yield return i;
        }
    }
    public static bool ShuffleGFX_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool friskMode)
    {
        IList<UndertaleSprite> chunk = (_chunk as UndertaleChunkSPRT)?.List;
        List<int> sprites = new List<int>();
        foreach (var i in ShuffleGFX_Sprites(chunk, friskMode))
        {
            sprites.Add(chunk.IndexOf(i));
        }

        sprites.SelectSome(shuffleChance, random);
        chunk.ShuffleOnlySelected(sprites, GetSubfunction(chunk, _chunk), random);
        logStream.WriteLine($"Shuffled {sprites.Count} out of {chunk.Count} sprite pointers.");

        return true;
    }

    public static bool HitboxFix_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool friskMode)
    {
        IList<UndertaleSprite> pointerList = (_chunk as UndertaleChunkSPRT)?.List;
        TextureWorker worker = new TextureWorker();
        foreach (UndertaleSprite sprite in pointerList)
        {
            // based on ImportGraphics.csx
            sprite.CollisionMasks.Clear();
            var texPageItem = sprite.Textures[0].Texture;
            // for Undertale BNP
            if (texPageItem == null)
            {
                logStream.WriteLine($"Texture 0 of sprite {sprite.Name.Content} is null.");
                continue;
            }
            var texPage = texPageItem.TexturePage;
            var remainingSizeW = Math.Max(0, texPage.TextureData.Width - texPageItem.SourceX);
            var remainingSizeH = Math.Max(0, texPage.TextureData.Height - texPageItem.SourceY);
            texPageItem.SourceWidth = (ushort)Math.Min(texPageItem.SourceWidth, remainingSizeW);
            texPageItem.SourceHeight = (ushort)Math.Min(texPageItem.SourceHeight, remainingSizeH);
            // spr_hotlandmissle
            if (texPageItem.BoundingWidth != sprite.Width || texPageItem.BoundingHeight != sprite.Height)
            {
                texPageItem.BoundingWidth = (ushort)sprite.Width;
                texPageItem.BoundingHeight = (ushort)sprite.Height;
            }
            if (texPageItem.BoundingWidth < texPageItem.TargetWidth || texPageItem.BoundingHeight < texPageItem.TargetHeight)
            {
                texPageItem.BoundingWidth = texPageItem.TargetWidth;
                texPageItem.BoundingHeight = texPageItem.TargetHeight;
                sprite.Width = texPageItem.BoundingWidth;
                sprite.Height = texPageItem.BoundingHeight;
            }
            Bitmap tex;
            try
            {
                tex = worker.GetTextureFor(texPageItem, sprite.Name.Content, true);
            }
            catch (InvalidDataException ex)
            {
                logStream.WriteLine($"Caught InvalidDataException while modifying the hitbox of {sprite.Name.Content}. {ex.Message}");
                continue;
            }
            catch (OutOfMemoryException ex)
            {
                logStream.WriteLine($"Caught OutOfMemoryException while modifying the hitbox of {sprite.Name.Content}. {ex.Message}");
                continue;
            }
            int width = (((int)sprite.Width + 7) / 8) * 8;
            BitArray maskingBitArray = new BitArray(width * (int)sprite.Height);
            try
            {
                for (int y = 0; y < sprite.Height; y++)
                {
                    for (int x = 0; x < sprite.Width; x++)
                    {
                        Color pixelColor = tex.GetPixel(x, y);
                        maskingBitArray[y * width + x] = (pixelColor.A > 0);
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException($"{ex.Message.TrimEnd()} ({sprite.Name.Content})");
            }
            BitArray tempBitArray = new BitArray(maskingBitArray.Length);
            for (int i = 0; i < maskingBitArray.Length; i += 8)
            {
                for (int j = 0; j < 8; j++)
                {
                    tempBitArray[j + i] = maskingBitArray[-(j - 7) + i];
                }
            }
            UndertaleSprite.MaskEntry newEntry = new UndertaleSprite.MaskEntry();
            int numBytes = maskingBitArray.Length / 8;
            byte[] bytes = new byte[numBytes];
            tempBitArray.CopyTo(bytes, 0);
            newEntry.Data = bytes;
            sprite.CollisionMasks.Add(newEntry);
        }
        logStream.WriteLine($"Wrote {pointerList.Count} collision boxes.");

        return true;
    }

    public static bool ShuffleText_Shuffler(UndertaleChunk _chunk, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream, bool friskMode)
    {
        IList<UndertaleString> _pointerlist = (_chunk as UndertaleChunkSTRG)?.List;

        IList<UndertaleString> pl_test = CleanseStringList(_pointerlist, data);

        Dictionary<string, List<int>> stringDict = new Dictionary<string, List<int>>();
        foreach (string chr in DRChoicerControlChars)
        {
            stringDict[chr] = new List<int>();
            stringDict[chr + "_ja"] = new List<int>();
        }
        foreach (string chr in FormatChars)
        {
            stringDict[chr] = new List<int>();
            stringDict[chr + "_ja"] = new List<int>();
        }
        foreach (string chr in CategorizableForcedSubstring)
        {
            stringDict[chr] = new List<int>();
            stringDict[chr + "_ja"] = new List<int>();
        }
        stringDict["_FORCE"] = new List<int>();
        stringDict["_FORCE_ja"] = new List<int>();

        for (var i = 0; i < pl_test.Count; i++)
        {
            var s = _pointerlist[i];
            var convertedString = s.Content;

            if (convertedString.Length < 3 || IsStringBanned(convertedString))
                continue;

            string ch = "";
            foreach (string chr in DRChoicerControlChars.Where(chr => convertedString.Contains(chr)))
            {
                ch = chr;
                break;
            }
            if (ch != "")
            {
                if (convertedString.Any(ix => ix > 127))
                    ch += "_ja";
                stringDict[ch].Add(i);
            }
            else
            {
                string ending = "";
                foreach (string ed in FormatChars.Where(ed => convertedString.EndsWith(ed)))
                {
                    ending = ed;
                    if (ending.StartsWith("/%%"))
                        ending = "/%%";
                    break;
                }
                if (ending == "")
                {
                    string chu = "";
                    foreach (string chr in CategorizableForcedSubstring.Where(chr => convertedString.Contains(chr)))
                    {
                        chu = chr;
                        break;
                    }
                    if (chu != "")
                        ending = chu;
                    else if (ForceShuffleReferenceChars.Any(convertedString.Contains))
                        ending = "_FORCE";
                    else
                        continue;
                }
                if (convertedString.Any(ix => ix > 127))
                    ending += "_ja";

                stringDict[ending].Add(i);
            }

            if (!data.IsGameMaker2())
                continue;
            // Do string_hash_to_newline for good measure
            s.Content = s.Content.Replace("#", "\n");
            convertedString = s.Content;
        }

        foreach (string ending in stringDict.Keys)
        {
            stringDict[ending].SelectSome(shuffleChance, random);
            logStream.WriteLine($"Added {stringDict[ending].Count} string pointers of ending {ending} to dialogue string List.");

            _pointerlist.ShuffleOnlySelected(stringDict[ending], GetSubfunction(_pointerlist, _chunk), random);
        }

        return true;
    }

    private static IList<UndertaleString> CleanseStringList(IList<UndertaleString> pointerlist, UndertaleData data)
    {
        var pl_test = new List<UndertaleString>(pointerlist);
        pl_test.Remove(data.GeneralInfo.FileName);
        pl_test.Remove(data.GeneralInfo.Name);
        //pl_test.Remove(data.GeneralInfo.DisplayName);
        pl_test.Remove(data.GeneralInfo.Config);
        foreach (var func in data.Options.Constants)
        {
            pl_test.Remove(func.Name);
            pl_test.Remove(func.Value);
        }
        foreach (var func in data.Language.EntryIDs)
        {
            pl_test.Remove(func);
        }
        foreach (var func in data.Language.Languages)
        {
            pl_test.Remove(func.Name);
            pl_test.Remove(func.Region);
            foreach (var j in func.Entries)
            {
                pl_test.Remove(j);
            }
        }
        foreach (var func in data.Variables)
        {
            pl_test.Remove(func.Name);
        }
        foreach (var func in data.Functions)
        {
            pl_test.Remove(func.Name);
        }
        foreach (var func in data.CodeLocals)
        {
            pl_test.Remove(func.Name);
            foreach (var i in func.Locals)
                pl_test.Remove(i.Name);
        }
        foreach (var func in data.Code)
        {
            pl_test.Remove(func.Name);
        }
        foreach (var func in data.Rooms)
        {
            pl_test.Remove(func.Name);
            pl_test.Remove(func.Caption);
            if (!data.IsGameMaker2())
                continue;
            foreach (var layer in func.Layers)
            {
                pl_test.Remove(layer.LayerName);
                if (layer.EffectType != null)
                    pl_test.Remove(layer.EffectType);
                if (layer.EffectData != null)
                {
                    pl_test.Remove(layer.EffectData.EffectType);
                    foreach (var prop in layer.EffectData.Properties)
                    {
                        pl_test.Remove(prop.Name);
                        pl_test.Remove(prop.Value);
                    }
                }
                if (layer.AssetsData == null)
                    continue;
                if (layer.AssetsData.Sprites != null)
                    foreach (var asset in layer.AssetsData.Sprites)
                        pl_test.Remove(asset.Name);
                if (layer.AssetsData.Sequences == null)
                    continue;
                foreach (var asset in layer.AssetsData.Sequences)
                    pl_test.Remove(asset.Name);
            }
        }
        foreach (var func in data.GameObjects)
        {
            pl_test.Remove(func.Name);
            foreach (var i in func.Events)
            foreach (var ii in i)
            foreach (var j in ii.Actions)
                pl_test.Remove(j.ActionName);
        }
        if (data.Shaders is not null)
            foreach (var func in data.Shaders)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.GLSL_ES_Vertex);
                pl_test.Remove(func.GLSL_ES_Fragment);
                pl_test.Remove(func.GLSL_Vertex);
                pl_test.Remove(func.GLSL_Fragment);
                pl_test.Remove(func.HLSL9_Vertex);
                pl_test.Remove(func.HLSL9_Fragment);
                foreach (UndertaleShader.VertexShaderAttribute attr in func.VertexShaderAttributes)
                    pl_test.Remove(attr.Name);
            }
        if (data.Timelines is not null)
            foreach (var func in data.Timelines)
            {
                pl_test.Remove(func.Name);
            }
        if (data.AnimationCurves is not null)
            foreach (var func in data.AnimationCurves)
            {
                pl_test.Remove(func.Name);
                foreach (var ch in func.Channels)
                    pl_test.Remove(ch.Name);
            }
        if (data.Sequences is not null)
            foreach (var func in data.Sequences)
            {
                pl_test.Remove(func.Name);

                foreach (KeyValuePair<int, UndertaleString> kvp in func.FunctionIDs)
                    pl_test.Remove(kvp.Value);
                foreach (UndertaleSequence.Keyframe<UndertaleSequence.Moment> moment in func.Moments)
                foreach (KeyValuePair<int, UndertaleSequence.Moment> kvp in moment.Channels)
                    pl_test.Remove(kvp.Value.Event);
                foreach (UndertaleSequence.Keyframe<UndertaleSequence.BroadcastMessage> bm in func.BroadcastMessages)
                foreach (KeyValuePair<int, UndertaleSequence.BroadcastMessage> kvp in bm.Channels)
                foreach (UndertaleString msg in kvp.Value.Messages)
                    pl_test.Remove(msg);

                foreach (var track in func.Tracks)
                {
                    void loop(UndertaleSequence.Track track)
                    {
                        pl_test.Remove(track.Name);
                        pl_test.Remove(track.ModelName);
                        pl_test.Remove(track.GMAnimCurveString);
                        if (track.ModelName.Content == "GMStringTrack")
                            foreach (var i in (track.Keyframes as UndertaleSequence.StringKeyframes).List)
                                foreach (KeyValuePair<int, UndertaleSequence.StringKeyframes.Data> kvp in i.Channels)
                                    pl_test.Remove(kvp.Value.Value);
                        if (track.Tracks != null)
                            foreach (var subtrack in track.Tracks)
                                loop(subtrack);
                    }
                    loop(track);
                }
            }
        foreach (var func in data.Fonts)
        {
            pl_test.Remove(func.Name);
            pl_test.Remove(func.DisplayName);
        }
        foreach (var func in data.Extensions)
        {
            pl_test.Remove(func.Name);
            pl_test.Remove(func.FolderName);
            pl_test.Remove(func.ClassName);
            foreach (var file in func.Files)
            {
                pl_test.Remove(file.Filename);
                pl_test.Remove(file.InitScript);
                pl_test.Remove(file.CleanupScript);
                foreach (var function in file.Functions)
                {
                    pl_test.Remove(function.Name);
                    pl_test.Remove(function.ExtName);
                }
            }
            foreach (var function in func.Options)
            {
                pl_test.Remove(function.Name);
                pl_test.Remove(function.Value);
            }
        }
        foreach (var func in data.Scripts)
        {
            pl_test.Remove(func.Name);
        }
        foreach (var func in data.Sprites)
        {
            pl_test.Remove(func.Name);
        }
        if (data.Backgrounds is not null)
            foreach (var func in data.Backgrounds)
            {
                pl_test.Remove(func.Name);
            }
        foreach (var func in data.Paths)
        {
            pl_test.Remove(func.Name);
        }
        foreach (var func in data.Sounds)
        {
            pl_test.Remove(func.Name);
            pl_test.Remove(func.Type);
            pl_test.Remove(func.File);
        }
        if (data.AudioGroups is not null)
            foreach (var func in data.AudioGroups)
            {
                pl_test.Remove(func.Name);
            }
        if (data.TextureGroupInfo is not null)
            foreach (var func in data.TextureGroupInfo)
            {
                pl_test.Remove(func.Name);
            }
        if (data.EmbeddedImages is not null)
            foreach (var func in data.EmbeddedImages)
            {
                pl_test.Remove(func.Name);
            }
        var feds = (data.FORM.Chunks.GetValueOrDefault("FEDS") as UndertaleChunkFEDS)?.List;
        if (feds is not null)
            foreach (var func in feds)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.Value);
            }
        return pl_test;
    }

    private static void SwapUndertaleSprite(UndertaleSprite kv, UndertaleSprite nv)
    {
        (kv.Name, nv.Name) = (nv.Name, kv.Name);
        (kv.Width, nv.Width) = (nv.Width, kv.Width);
        (kv.Height, nv.Height) = (nv.Height, kv.Height);
        (kv.MarginLeft, nv.MarginLeft) = (nv.MarginLeft, kv.MarginLeft);
        (kv.MarginRight, nv.MarginRight) = (nv.MarginRight, kv.MarginRight);
        (kv.MarginTop, nv.MarginTop) = (nv.MarginTop, kv.MarginTop);
        (kv.MarginBottom, nv.MarginBottom) = (nv.MarginBottom, kv.MarginBottom);
        (kv.V2Sequence, nv.V2Sequence) = (nv.V2Sequence, kv.V2Sequence);
        (kv.OriginX, nv.OriginX) = (nv.OriginX, kv.OriginX);
        (kv.OriginY, nv.OriginY) = (nv.OriginY, kv.OriginY);
        (kv.GMS2PlaybackSpeed, nv.GMS2PlaybackSpeed) = (nv.GMS2PlaybackSpeed, kv.GMS2PlaybackSpeed);
        (kv.GMS2PlaybackSpeedType, nv.GMS2PlaybackSpeedType) = (nv.GMS2PlaybackSpeedType, kv.GMS2PlaybackSpeedType);
        (kv.SSpriteType, nv.SSpriteType) = (nv.SSpriteType, kv.SSpriteType);
        (kv.SpineJSON, nv.SpineJSON) = (nv.SpineJSON, kv.SpineJSON);
        (kv.SpineAtlas, nv.SpineAtlas) = (nv.SpineAtlas, kv.SpineAtlas);
        (kv.SpineTextures, nv.SpineTextures) = (nv.SpineTextures, kv.SpineTextures);
        (kv.YYSWF, nv.YYSWF) = (nv.YYSWF, kv.YYSWF);
        (kv.V3NineSlice, nv.V3NineSlice) = (nv.V3NineSlice, kv.V3NineSlice);
        List<UndertaleSprite.TextureEntry> texturesValue
            = new List<UndertaleSprite.TextureEntry>(kv.Textures);
        List<UndertaleSprite.TextureEntry> texturesValueb
            = new List<UndertaleSprite.TextureEntry>(nv.Textures);
        kv.Textures.Clear();
        nv.Textures.Clear();
        foreach (var t in texturesValueb)
        {
            var te = new UndertaleSprite.TextureEntry();
            te.Texture = t.Texture;
            kv.Textures.Add(te);
        }
        foreach (var t in texturesValue)
        {
            var te = new UndertaleSprite.TextureEntry();
            te.Texture = t.Texture;
            nv.Textures.Add(te);
        }
        List<UndertaleSprite.MaskEntry> CollisionMasks_value
            = new List<UndertaleSprite.MaskEntry>(kv.CollisionMasks);
        List<UndertaleSprite.MaskEntry> CollisionMasks_valueb
            = new List<UndertaleSprite.MaskEntry>(nv.CollisionMasks);
        kv.CollisionMasks.Clear();
        nv.CollisionMasks.Clear();
        foreach (var t in CollisionMasks_valueb)
            kv.CollisionMasks.Add(t);
        foreach (var t in CollisionMasks_value)
            nv.CollisionMasks.Add(t);
        (kv.Transparent, nv.Transparent) = (nv.Transparent, kv.Transparent);
        (kv.Smooth, nv.Smooth) = (nv.Smooth, kv.Smooth);
        (kv.BBoxMode, nv.BBoxMode) = (nv.BBoxMode, kv.BBoxMode);
        (kv.SepMasks, nv.SepMasks) = (nv.SepMasks, kv.SepMasks);
    }
    private static void SwapUndertaleBackground(UndertaleBackground kv, UndertaleBackground nv)
    {
        (kv.Name, nv.Name) = (nv.Name, kv.Name);
        (kv.Transparent, nv.Transparent) = (nv.Transparent, kv.Transparent);
        (kv.Smooth, nv.Smooth) = (nv.Smooth, kv.Smooth);
        (kv.Texture, nv.Texture) = (nv.Texture, kv.Texture);
        (kv.GMS2TileWidth, nv.GMS2TileWidth) = (nv.GMS2TileWidth, kv.GMS2TileWidth);
        (kv.GMS2TileHeight, nv.GMS2TileHeight) = (nv.GMS2TileHeight, kv.GMS2TileHeight);
        (kv.GMS2OutputBorderX, nv.GMS2OutputBorderX) = (nv.GMS2OutputBorderX, kv.GMS2OutputBorderX);
        (kv.GMS2OutputBorderY, nv.GMS2OutputBorderY) = (nv.GMS2OutputBorderY, kv.GMS2OutputBorderY);
        (kv.GMS2TileColumns, nv.GMS2TileColumns) = (nv.GMS2TileColumns, kv.GMS2TileColumns);
        (kv.GMS2FrameLength, nv.GMS2FrameLength) = (nv.GMS2FrameLength, kv.GMS2FrameLength);
    }
    public static Action<int, int> GetSubfunction<T>(IList<T> list, UndertaleChunk chunk)
    {
        return (n, k) =>
        {
            switch (list[n])
            {
                case UndertaleString:
                {
                    var kv = list[k] as UndertaleString;
                    var nv = list[n] as UndertaleString;
                    var _value = kv.Content;
                    kv.Content = nv.Content;
                    nv.Content = _value;
                    return;
                }
                case UndertaleSprite:
                {
                    var kv = list[k] as UndertaleSprite;
                    var nv = list[n] as UndertaleSprite;
                    SwapUndertaleSprite(kv, nv);
                    return;
                }
                case UndertaleBackground:
                {
                    var kv = list[k] as UndertaleBackground;
                    var nv = list[n] as UndertaleBackground;
                    SwapUndertaleBackground(kv, nv);
                    return;
                }
                default: (list[k], list[n]) = (list[n], list[k]);
                    break;
            }
        };
    }
    public static Action<int, int> GetSubfunction(IList list, UndertaleChunk chunk)
    {
        return (n, k) =>
        {
            if (list[n] is UndertaleString)
            {
                var kv = list[k] as UndertaleString;
                var nv = list[n] as UndertaleString;
                var _value = kv.Content;
                kv.Content = nv.Content;
                nv.Content = _value;
                return;
            }
            if (list[n] is UndertaleSprite)
            {
                var kv = list[k] as UndertaleSprite;
                var nv = list[n] as UndertaleSprite;
                SwapUndertaleSprite(kv, nv);
                return;
            }
            if (list[n] is UndertaleBackground)
            {
                var kv = list[k] as UndertaleBackground;
                var nv = list[n] as UndertaleBackground;
                SwapUndertaleBackground(kv, nv);
                return;
            }
            (list[k], list[n]) = (list[n], list[k]);
        };
    }

    public static Func<UndertaleChunk, UndertaleData, Random, float, StreamWriter, bool, bool> SimpleShuffle = ComplexShuffle(SimpleShuffler);

    public static bool JSONStringShuffle(string resource_file, string target_file, UndertaleData data, Random random, float shuffleChance, StreamWriter logStream)
    {
        if (random == null)
            throw new ArgumentNullException(nameof(random));

        JObject langObject = null;

        using (StreamReader stream = File.OpenText(resource_file))
        {
            logStream.WriteLine($"Opened {resource_file}.");
            var desSuccessful = true;
            using (JsonTextReader jsonReader = new JsonTextReader(stream))
            {
                var serializer = new JsonSerializer();
                try
                {
                    langObject = serializer.Deserialize<JObject>(jsonReader);
                } catch (JsonSerializationException exc)
                {
                    logStream.WriteLine($"Error while deserializing file: {exc.Message}");
                    desSuccessful = false;
                }
            }
            logStream.WriteLine($"Closed {resource_file}.");
            if (!desSuccessful)
            {
                return false;
            }
        }

        if (langObject == null) return false;
        if (langObject.First.Type is not JTokenType.Property || langObject.First.Value<JToken>().Type is not JTokenType.String)
        {
            logStream.WriteLine($"File {resource_file} malformed, skipping.");
            return true;
        }

        logStream.WriteLine($"Gathered {langObject.Count} JSON String Entries. ");

        string[] bannedKeys = { "date" };

        Dictionary<string, string> good_strings = new Dictionary<string, string>();
        Dictionary<string, string> final_list = new Dictionary<string, string>();

        foreach (KeyValuePair<string, JToken> entry in langObject)
        {
            string value = (string)entry.Value;
            if (data.IsGameMaker2())
            {
                // Do string_hash_to_newline for good measure
                value = value.Replace("#", "\n");
            }
            if (!bannedKeys.Contains(entry.Key) && value.Length >= 3 && !IsStringBanned(value))
            {
                good_strings.Add(entry.Key, value);
            }
            else
            {
                final_list.Add(entry.Key, value);
            }
        }

        logStream.WriteLine($"Kept {good_strings.Count} good JSON string entries.");
        logStream.WriteLine($"Fastforwarded {final_list.Count} JSON string entries to the final phase.");

        Dictionary<string, Dictionary<string, string>> stringDict
            = new Dictionary<string, Dictionary<string, string>>();

        foreach (KeyValuePair<string, string> s in good_strings)
        {
            string ch = "";
            foreach (string chr in DRChoicerControlChars.Where(chr => s.Value.Contains(chr)))
            {
                ch = chr;
                break;
            }
            if (ch != "")
            {
                if (!stringDict.ContainsKey(ch))
                    stringDict[ch] = new Dictionary<string, string>();
                stringDict[ch].Add(s.Key, s.Value);
            }
            else
            {
                string ending = "";
                foreach (string ed in FormatChars.Where(ed => s.Value.EndsWith(ed)))
                {
                    ending = ed;
                    break;
                }
                if (!stringDict.ContainsKey(ending))
                    stringDict[ending] = new Dictionary<string, string>();

                stringDict[ending].Add(s.Key, s.Value);
            }
        }

        foreach (string ending in stringDict.Keys)
        {
            logStream.WriteLine($"Added {stringDict[ending].Count} JSON string entries of ending <{ending}> to dialogue string List.");

            List<string> ints = new List<string>();
            foreach (string i in stringDict[ending].Keys)
                ints.Add(i);
            ints.SelectSome(shuffleChance, random);
            stringDict[ending].ShuffleOnlySelected(ints, (n, k) => {
                (stringDict[ending][n], stringDict[ending][k]) = (stringDict[ending][k], stringDict[ending][n]);
            }, random);

            // i love crumb sharp!!!!1111
            final_list = final_list.Concat(stringDict[ending]).ToDictionary(x => x.Key, x => x.Value);
        }

        if (!SafeMethods.DeleteFile(target_file)) return false;
        logStream.WriteLine($"Deleted {target_file}.");
        using (FileStream out_writer = File.OpenWrite(target_file))
        {
            using (StreamWriter out_stream = new StreamWriter(out_writer))
            {
                logStream.WriteLine($"Opened {target_file}.");
                using (JsonTextWriter jsonReader = new JsonTextWriter(out_stream))
                {
                    var serializer = new JsonSerializer();
                    jsonReader.IndentChar = ' ';
                    jsonReader.Indentation = 2;
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(jsonReader, final_list);
                }
            }
        }

        logStream.WriteLine($"Closed {target_file}.");

        return true;
    }

    public static bool IsStringBanned(string str)
    {
        bool bannedEX = BannedStringsEX.Any(str.Contains) || BannedStringsSame.Any(str.Equals) || VaporDataStrings.Any(str.Equals);
        return ((BannedStrings.Any(str.Contains) || bannedEX)
                && !(
                    (ForceShuffleReferenceChars.Any(str.Contains) && !bannedEX)
                    || DRChoicerControlChars.Any(str.Contains)
                )
            );
    }
}
