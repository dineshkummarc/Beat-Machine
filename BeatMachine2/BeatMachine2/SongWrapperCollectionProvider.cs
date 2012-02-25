using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace BeatMachine2
{
    public class SoundEffectsProvider
    {
        private static Dictionary<int, SoundEffectInstance> _soundEffects;
        private static Dictionary<int, double> _bmpData;

        public static ContentManager ContentManager
        {
            get;
            set;
        }

        public static Dictionary<int, SoundEffectInstance> SoundEffects
        {
            get
            {
                if (ContentManager == null)
                {
                    throw new InvalidOperationException("ContentManager should be set before calling this");
                }
                if (_soundEffects == null)
                {
                    _soundEffects = new Dictionary<int, SoundEffectInstance>();
                    _bmpData = new Dictionary<int, double>();
                    int i = 0;
                    foreach (var kvpair in _nameBpmMap)
                    {
                        SoundEffectInstance instance = ContentManager.Load<SoundEffect>(kvpair.Key).CreateInstance();
                        instance.Volume = 1;
                        _soundEffects.Add(i, instance);
                        _bmpData.Add(i, kvpair.Value);
                        i++;
                    }
                }
                return _soundEffects;
            }
        }

        public static Dictionary<int, SoundEffectInstance> GetACustomPlayList()
        {
            Dictionary<int, SoundEffectInstance> _returnList = new Dictionary<int,SoundEffectInstance>();
            var random = new Random(DateTime.Now.Millisecond);
            int currentTrack = 0;
            int sourceIndex = 0;
            for (double lowerBpmBound = 90; lowerBpmBound < 170; lowerBpmBound += 10) {
                int bandCount = 3;
                if (lowerBpmBound == 120) {
                    bandCount = 6;
                }
                else if (lowerBpmBound == 170) {
                    bandCount = 2;
                }

                int rand = random.Next(0, bandCount-1);
                _returnList.Add(currentTrack++, SoundEffects[sourceIndex + rand]);
                sourceIndex += bandCount;
            }
            return _returnList;
        }

        //This is a static hardcoded list of songs names and their Beats-per-minute values.
        //Currently the songs are not included yet - so things won't work. 
        private static Dictionary<string, double> _nameBpmMap = new Dictionary<string, double>()
        {
            {"50 Cent - In da Club",90 },
            {"02 Lift Off",90.14 },
            {"08 Weapon of Choice",97.99 },
            {"Are You Gonna Be My Girl (Album Version) (dc1861805)",105.02 },
            {"01 Adele - Rolling In The Deep",105.06 },
            {"Sun_Glitters_-_They_Dont_Want_To_Let_You_Know_Sumsun_Remix_-_Portugal",109.99 },
            {"American Boy",118.17 },
            {"05 Ya Mama",118.7 },
            {"02 Adele - Rumor Has It",119.99 },
            {"When The Night Knows (Whitney Houston x Chromeo)",122.02 },
            {"Spleen_United_-_Bright_Cities_Keep_Me_Awake_-_Denmark",123.02 },
            {"04 Future Lovers",124.02 },
            {"01 Right Here, Right Now",124.69 },
            {"Off_The_Wall_(Brodinski_Remix)",126.02 },
            {"07 Movin' Too Fast",127.09 },
            {"labrinth-last-time-knife-party-remix-3",131.03 },
            {"01 Familiar Feeling",132.04 },
            {"twenty love-maxtauker elektro  mix",138.01 },
            {"01 Track 1",145.84 },
            {"02 Track 2",145.89 },
            {"06 Track 6",146.04 },
            {"02 The Rockafeller Skank",152.67 },
            {"08 B O B",153.87 },
            {"Outkast Hey Ya",158.98 },
            {"Kman - When Will You Come Home",161.5 },
            {"Josh G's mix1",168.5 },
        };
    }
}
