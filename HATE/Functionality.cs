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
    public class StringPointer
    {
        public UndertaleString Base;
        public string Ending;
        public string Str;

        public StringPointer(UndertaleString ptr)
        {
            Base = ptr;
            Str = ptr.Content;
            char[] FormatChars = { '%', '/', 'C' };
            List<char> Ending = new List<char>();

            for (int i = 1; i < Str.Length; i++)
            {
                char C = Str[Str.Length - i];

                if (FormatChars.Contains(C))
                    Ending.Add(C);
                else
                    break;
            }

            Ending.Reverse();
            this.Ending = new string(Ending.ToArray());
        }
    }

    public class ResourcePointer
    {
        public int Address;
        public int Location;

        public ResourcePointer(int ptr, int loc) { Address = ptr; Location = loc; }
        public ResourcePointer(byte[] ptr, int loc) { Address = BitConverter.ToInt32(ptr, 0); Location = loc; }
    }



    partial class MainForm
    {

        public bool ShuffleAudio_Func(Random random, float chance, StreamWriter logstream)
        {
            return Shuffle.LoadDataAndFind(Data.Sounds, random, chance, logstream, Shuffle.SimpleShuffle) && 
                   Shuffle.LoadDataAndFind(Data.EmbeddedAudio, random, chance, logstream, Shuffle.SimpleShuffle);               
        }

        public bool ShuffleBG_Func(Random random, float chance, StreamWriter logstream)
        {
            return Shuffle.LoadDataAndFind(Data.Backgrounds, random, chance, logstream, Shuffle.SimpleShuffle);              
        }

        public bool ShuffleFont_Func(Random random, float chance, StreamWriter logstream)
        {
            return Shuffle.LoadDataAndFind(Data.Fonts, random, chance, logstream, Shuffle.SimpleShuffle);
        }

        public bool HitboxFix_Func(Random random_, float chance, StreamWriter logstream_)
        {
            return Shuffle.LoadDataAndFind(Data.Sprites, random_, chance, logstream_, Shuffle.ComplexShuffle(HitboxFix_Shuffler));
        }

        public bool ShuffleGFX_Func(Random random_, float chance, StreamWriter logstream_)
        {
            return Shuffle.LoadDataAndFind(Data.Sprites, random_, chance, logstream_, Shuffle.ComplexShuffle(ShuffleGFX_Shuffler));
        }

        public bool ShuffleText_Func(Random random_, float chance, StreamWriter logstream_)
        {
            bool success = true;
            if (Directory.Exists("./lang") && SafeMethods.GetFiles("./lang").Count > 0)
            {
                foreach (string path in SafeMethods.GetFiles("./lang"))
                {
                    success = success && Shuffle.JSONStringShuffle(path, path, random_, chance, logstream_);
                }
            }
            return success && Shuffle.LoadDataAndFind(Data.Strings, random_, chance, logstream_, Shuffle.ComplexShuffle(ShuffleText_Shuffler));
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

            chunk.ShuffleOnlySelected(sprites, random);
            logstream.WriteLine($"Shuffled {sprites.Count} out of {chunk.Count} sprite pointers.");

            return chunk;
        }

        public IList<UndertaleObject> HitboxFix_Shuffler(IList<UndertaleObject> pointerlist, Random random, float shufflechance, StreamWriter logstream)
        {
            TextureWorker worker = new TextureWorker();
            foreach (UndertaleObject _spriteptr in pointerlist)
            {
                UndertaleSprite spriteptr = (UndertaleSprite)_spriteptr;
                spriteptr.CollisionMasks.Clear();
                spriteptr.CollisionMasks.Add(spriteptr.NewMaskEntry());
                Rectangle bmpRect = new Rectangle(0, 0, (int)spriteptr.Width, (int)spriteptr.Height);
                Bitmap tex = worker.GetTextureFor(spriteptr.Textures[0].Texture, spriteptr.Name.Content, true);
                Bitmap cloneBitmap = tex.Clone(bmpRect, tex.PixelFormat);
                int width = (((int)spriteptr.Width + 7) / 8) * 8;
                BitArray maskingBitArray = new BitArray(width * (int)spriteptr.Width);
                for (int y = 0; y < (int)spriteptr.Height; y++)
                {
                    for (int x = 0; x < (int)spriteptr.Width; x++)
                    {
                        Color pixelColor = cloneBitmap.GetPixel(x, y);
                        maskingBitArray[y * width + x] = (pixelColor.A > 0);
                    }
                }
                BitArray tempBitArray = new BitArray(width * (int)spriteptr.Height);
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

                _pointerlist.ShuffleOnlySelected(stringDict[ending], random);
            }

            return _pointerlist;
        }
    }
}
