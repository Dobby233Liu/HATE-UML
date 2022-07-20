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
    partial class MainForm
    {
        public bool ShuffleAudio_Func(Random random, float chance, StreamWriter logstream)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)Data.Sounds, random, chance, logstream); 
                   //&& Shuffle.ShuffleChunk(Data.EmbeddedAudio, random, chance, logstream);               
        }

        public bool ShuffleBG_Func(Random random, float chance, StreamWriter logstream)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)Data.Backgrounds, random, chance, logstream);              
        }

        public bool ShuffleFont_Func(Random random, float chance, StreamWriter logstream)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)Data.Fonts, random, chance, logstream);
        }

        public bool HitboxFix_Func(Random random, float chance, StreamWriter logstream)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)Data.Sprites, random, chance, logstream, Shuffle.ComplexShuffle(HitboxFix_Shuffler));
        }

        public bool ShuffleGFX_Func(Random random, float chance, StreamWriter logstream)
        {
            return Shuffle.ShuffleChunk((IList<UndertaleObject>)Data.Sprites, random, chance, logstream, Shuffle.ComplexShuffle(ShuffleGFX_Shuffler));
        }

        public bool ShuffleText_Func(Random random, float chance, StreamWriter logstream)
        {
            bool success = true;
            if (Directory.Exists("./lang") && SafeMethods.GetFiles("./lang").Count > 0)
            {
                foreach (string path in SafeMethods.GetFiles("./lang"))
                {
                    success = success && Shuffle.JSONStringShuffle(path, path, random, chance, logstream);
                }
            }
            return success && Shuffle.ShuffleChunk((IList<UndertaleObject>)Data.Strings, random, chance, logstream, Shuffle.ComplexShuffle(ShuffleText_Shuffler));
        }

        public IList<UndertaleObject> ShuffleGFX_Shuffler(IList<UndertaleObject> chunk, Random random, float shufflechance, StreamWriter logstream)
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

            return chunk;
        }

        public IList<UndertaleObject> HitboxFix_Shuffler(IList<UndertaleObject> pointerlist, Random random, float shufflechance, StreamWriter logstream)
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

            return pointerlist;
        }

        // TODO: clean this
        public IList<UndertaleObject> ShuffleText_Shuffler(IList<UndertaleObject> _pointerlist, Random random, float shufflechance, StreamWriter logstream)
        {
            string[] bannedStrings = { "_" };

            Dictionary<string, List<int>> stringDict = new Dictionary<string, List<int>>();

            for (int i = 0; i < _pointerlist.Count; i++)
            {
                var s = (UndertaleString)_pointerlist[i];
                var convertedString = s.Content;

                if (convertedString.Length >= 3 && !bannedStrings.Any(convertedString.Contains) && !(convertedString.Any(x => x > 127)))
                {
                    char[] FormatChars = { '%', '/', 'C' };
                    List<char> Ending = new List<char>();

                    for (int i2 = 1; i2 < convertedString.Length; i2++)
                    {
                        char C = convertedString[convertedString.Length - i2];

                        if (FormatChars.Contains(C))
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

            return _pointerlist;
        }
    }
}
