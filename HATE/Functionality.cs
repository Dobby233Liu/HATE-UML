﻿using System;
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
        public static bool ShuffleAudio_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk("SOND", data, random, chance, logstream, _friskMode);
            //&& Shuffle.ShuffleChunk(data.EmbeddedAudio, data, random, chance, logstream);               
        }

        public static bool ShuffleBG_Func(UndertaleData data, Random random, float chance, StreamWriter logstream, bool _friskMode)
        {
            return Shuffle.ShuffleChunk("BGND", data, random, chance, logstream, _friskMode);              
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

        public static bool ShuffleGFX_Shuffler(string _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleSprite> chunk = (data.FORM.Chunks[_chunk] as UndertaleChunkSPRT)?.List;
            List<int> sprites = new List<int>();

            for (int i = 0; i < chunk.Count; i++)
            {
                UndertaleSprite pointer = chunk[i];

                if (!Shuffle.FriskSpriteHandles.Contains(pointer.Name.Content.Trim()) || _friskMode)
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
            foreach (UndertaleResource _spriteptr in pointerlist)
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
        public static bool ShuffleText_Shuffler(string _chunk, UndertaleData data, Random random, float shufflechance, StreamWriter logstream, bool _friskMode)
        {
            IList<UndertaleString> _pointerlist = (data.FORM.Chunks[_chunk] as UndertaleChunkSTRG)?.List;
            IList<UndertaleString> pl_test = new List<UndertaleString>(_pointerlist);
            foreach (var func in data.Variables)
            {
                pl_test.Remove(func.Name);
            }
            foreach (var func in data.Functions)
            {
                pl_test.Remove(func.Name);
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
                    }
                }
            }
            foreach (var func in data.GameObjects)
            {
                pl_test.Remove(func.Name);
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
                }
            if (data.Sequences is not null)
                foreach (var func in data.Sequences)
                {
                    /* TODO: BroadcastMessage */
                    pl_test.Remove(func.Name);
                    foreach (KeyValuePair<int, UndertaleString> kvp in func.FunctionIDs)
                    {
                        pl_test.Remove(kvp.Value);
                    }
                    /*foreach (var track in func.Tracks)
                    {

                    }*/
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
            var feds = (data.FORM.Chunks["FEDS"] as UndertaleChunkFEDS)?.List;
            if (feds is not null)
                foreach (var func in feds)
                {
                    pl_test.Remove(func.Name);
                    pl_test.Remove(func.Value);
                }
            pl_test.Remove(data.GeneralInfo.FileName);
            pl_test.Remove(data.GeneralInfo.Name);
            pl_test.Remove(data.GeneralInfo.DisplayName);
            pl_test.Remove(data.GeneralInfo.Config);
            foreach (var func in data.Options.Constants)
            {
                pl_test.Remove(func.Name);
                pl_test.Remove(func.Value);
            }

            Dictionary<string, List<int>> stringDict = new Dictionary<string, List<int>>();

            for (int _i = 0; _i < pl_test.Count; _i++)
            {
                var i = _pointerlist.IndexOf(pl_test[_i]);

                var s = _pointerlist[i];
                var convertedString = s.Content;

                if (convertedString.Length < 3)
                    continue;
                if (Shuffle.BannedStrings.Any(convertedString.Contains)
                    && !Shuffle.ForceShuffleReferenceChars.Any(convertedString.Contains))
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

                _pointerlist.ShuffleOnlySelected(stringDict[ending], (n, k) => {
                    string value = (_pointerlist[k]).Content;
                    _pointerlist[k].Content = _pointerlist[n].Content;
                    _pointerlist[n].Content = value;
                }, random);
            }

            return true;
        }
    }
}
