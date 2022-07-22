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
            return Shuffle.ShuffleChunk("SOND", data, random, chance, logstream, _friskMode)
                    && Shuffle.ShuffleChunk("STRG", data, random, chance, logstream, _friskMode, ShuffleAudio2_Shuffler);
            //&& Shuffle.ShuffleChunk(data.EmbeddedAudio, data, random, chance, logstream);               
        }

        private static bool ShuffleAudio2_Shuffler(string _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleString> _pointerlist = (data.FORM.Chunks[_chunk] as UndertaleChunkSTRG)?.List;
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
                string i = strgClone[_i].Content;
                if (i.EndsWith(".ogg") || i.EndsWith(".wav") || i.EndsWith(".mp3"))
                    stringList.Add(_i);
            }

            stringList.SelectSome(shufflechance, random);
            logstream.WriteLine($"Added {stringList.Count} string pointers to music file references list.");

            _pointerlist.ShuffleOnlySelected(stringList, Shuffle.GetSubfunction(_pointerlist), random);

            return true;
        }

        public static bool ShuffleBG_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk("BGND", data, random, chance, logstream, _friskMode) &&
                (!data.IsGameMaker2() || Shuffle.ShuffleChunk("SPRT", data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(ShuffleBG2_Shuffler)));              
        }

        public static bool ShuffleFont_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk("FONT", data, random, chance, logstream, _friskMode);
        }

        public static bool HitboxFix_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk("SPRT", data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(HitboxFix_Shuffler));
        }

        public static bool ShuffleGFX_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk("SPRT", data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(ShuffleGFX_Shuffler));
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
            return success && Shuffle.ShuffleChunk("STRG", data, random, chance, logstream, _friskMode, Shuffle.ComplexShuffle(ShuffleText_Shuffler));
        }

        private static bool ShuffleBG2_Shuffler(string _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleSprite> chunk = (data.FORM.Chunks[_chunk] as UndertaleChunkSPRT)?.List;
            List<int> sprites = new List<int>();

            for (int i = 0; i < chunk.Count; i++)
            {
                UndertaleSprite pointer = chunk[i];

                if (pointer.Name.Content.Trim().StartsWith("bg_"))
                    sprites.Add(i);
            }

            sprites.SelectSome(shufflechance, random);
            chunk.ShuffleOnlySelected(sprites, random);
            logstream.WriteLine($"Shuffled {sprites.Count} assumed backgrounds out of {chunk.Count} sprites.");

            return true;
        }
        public static bool ShuffleGFX_Shuffler(string _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleSprite> chunk = (data.FORM.Chunks[_chunk] as UndertaleChunkSPRT)?.List;
            List<int> sprites = new List<int>();

            for (int i = 0; i < chunk.Count; i++)
            {
                UndertaleSprite pointer = chunk[i];

                if (!pointer.Name.Content.Trim().StartsWith("bg_") &&
                    (!Shuffle.FriskSpriteHandles.Contains(pointer.Name.Content.Trim()) || _friskMode))
                    sprites.Add(i);
            }

            sprites.SelectSome(shufflechance, random);
            chunk.ShuffleOnlySelected(sprites, random);
            logstream.WriteLine($"Shuffled {sprites.Count} out of {chunk.Count} sprite pointers.");

            return true;
        }

        public static bool HitboxFix_Shuffler(string _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleSprite> pointerlist = (data.FORM.Chunks[_chunk] as UndertaleChunkSPRT)?.List;
            TextureWorker worker = new TextureWorker();
            foreach (UndertaleSprite sprite in pointerlist)
            {
                // based on ImportGraphics.csx
                sprite.CollisionMasks.Clear();
                var texPageItem = sprite.Textures[0].Texture;
                // for Undertale BNP
                if (texPageItem == null)
                {
                    logstream.WriteLine($"Texture 0 of sprite {sprite.Name.Content} is null.");
                    continue;
                }
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
                    logstream.WriteLine($"Caught InvalidDataException while modifying the hitbox of {sprite.Name.Content}. {ex.Message}");
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
                } catch (ArgumentOutOfRangeException ex)
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
                int numBytes;
                numBytes = maskingBitArray.Length / 8;
                byte[] bytes = new byte[numBytes];
                tempBitArray.CopyTo(bytes, 0);
                newEntry.Data = bytes;
                sprite.CollisionMasks.Add(newEntry);
            }
            logstream.WriteLine($"Wrote {pointerlist.Count} collision boxes.");

            return true;
        }

        // TODO: clean this
        public static bool ShuffleText_Shuffler(string _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleString> _pointerlist = (data.FORM.Chunks[_chunk] as UndertaleChunkSTRG)?.List;

            IList<UndertaleString> pl_test = CleanseStringList(_pointerlist, data);

            Dictionary<string, List<int>> stringDict = new Dictionary<string, List<int>>();

            for (int _i = 0; _i < pl_test.Count; _i++)
            {
                var i = _pointerlist.IndexOf(pl_test[_i]);

                var s = _pointerlist[i];
                var convertedString = s.Content;

                if (convertedString.Length < 3)
                    continue;
                if (Shuffle.IsStringBanned(convertedString))
                    continue;

                string ch = "";
                foreach (string chr in Shuffle.DRChoicerControlChars)
                {
                    if (convertedString.Contains(chr))
                    {
                        ch = chr;
                        break;
                    }
                }
                if (ch != "")
                {
                    if (convertedString.Any(x => x > 127))
                        ch += "_ja";
                    if (!stringDict.ContainsKey(ch))
                        stringDict[ch] = new List<int>();
                    stringDict[ch].Add(i);
                }
                else
                {
                    string ending = "";
                    foreach (string ed in Shuffle.FormatChars)
                    {
                        if (convertedString.EndsWith(ed))
                        {
                            ending = ed;
                            break;
                        }
                    }
                    if (ending == "")
                    {
                        if (Shuffle.ForceShuffleReferenceChars.Any(convertedString.Contains))
                            ending = "_FORCE";
                        else
                            continue;
                    }
                    if (convertedString.Any(x => x > 127))
                        ending += "_ja";

                    if (!stringDict.ContainsKey(ending))
                        stringDict[ending] = new List<int>();

                    stringDict[ending].Add(i);
                }
            }

            foreach (string ending in stringDict.Keys)
            {
                stringDict[ending].SelectSome(shufflechance, random);
                logstream.WriteLine($"Added {stringDict[ending].Count} string pointers of ending {ending} to dialogue string List.");

                _pointerlist.ShuffleOnlySelected(stringDict[ending], Shuffle.GetSubfunction(_pointerlist), random);
            }

            return true;
        }

        private static IList<UndertaleString> CleanseStringList(IList<UndertaleString> pointerlist, UndertaleData data)
        {
            var pl_test = new List<UndertaleString>(pointerlist);
            pl_test.Remove(data.GeneralInfo.FileName);
            pl_test.Remove(data.GeneralInfo.Name);
            pl_test.Remove(data.GeneralInfo.DisplayName);
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
                if (data.IsGameMaker2())
                {
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
                        if (layer.AssetsData != null)
                        {
                            foreach (var asset in layer.AssetsData.Sprites)
                                pl_test.Remove(asset.Name);
                            foreach (var asset in layer.AssetsData.Sequences)
                                pl_test.Remove(asset.Name);
                        }
                    }
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
                                    foreach (KeyValuePair<int, UndertaleSequence.StringData> kvp in i.Channels)
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
            if (data.FORM.Chunks.ContainsKey("FEDS"))
            {
                var feds = (data.FORM.Chunks["FEDS"] as UndertaleChunkFEDS)?.List;
                if (feds is not null)
                    foreach (var func in feds)
                    {
                        pl_test.Remove(func.Name);
                        pl_test.Remove(func.Value);
                    }
            }
            return pl_test;
        }
    }
}
