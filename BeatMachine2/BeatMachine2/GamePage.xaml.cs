using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Threading;

namespace BeatMachine2
{
    public partial class GamePage : PhoneApplicationPage
    {
        ContentManager contentManager;
        //GameTimer timer;
        //SpriteBatch spriteBatch;

        // Settable vars
        public TimeSpan playDuration = TimeSpan.FromSeconds(10);
        public TimeSpan fadeDuration = TimeSpan.FromSeconds(3);
        //public TimeSpan seek = TimeSpan.FromSeconds(30); // how far into the song do we start playing
        private Dictionary<int, SoundEffectInstance> tracks;
        Timer t1, t2, t3;
        int currentTrack = 0;
        bool song1Playing = true;
        float fadeIncrement;
        SoundEffectInstance song1;
        SoundEffectInstance song2;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            SoundEffectsProvider.ContentManager = contentManager;
            tracks = SoundEffectsProvider.SoundEffects;

            fadeIncrement = 1f / (float)fadeDuration.Seconds;

            t1 = new Timer(StartNewSong, null, playDuration - fadeDuration, playDuration);
            t2 = new Timer(StopCurrentSong, null, playDuration, playDuration);

            song1 = tracks[0];
            song2 = tracks[1];
            song1.Volume = 1;
            song1.Play();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            //SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: use this.content to load your game content here

            // Start the timer
            //timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StopTimers();
            // Stop the timer
            //timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            //SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: Add your update logic here
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            //SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
        }

        private void StartNewSong(object o)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                t3 = new Timer(Fade, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));

                if (currentTrack == (tracks.Count - 1))
                {
                    currentTrack = -1;
                    //tracks = SoundEffectsProvider.GetACustomPlayList();
                }

                if (song1Playing)
                {
                    song2 = tracks[++currentTrack];
                    //song2.Position = seek;
                    
                    song2.Play();
                    
                }
                else
                {
                    song1 = tracks[++currentTrack];
                    //song1.Position = seek;
                    song1.Play();
                    
                }
            });
        }


        private void StopCurrentSong(object o)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {

                t3.Dispose();

                if (song1Playing)
                {
                    song1.Stop();
                }
                else
                {
                    song2.Stop();
                }

                song1Playing = !song1Playing;
            });
        }

        private float volumeUp(float volume)
        {
            float r = volume + fadeIncrement;
            return (r > 1) ? 1 : r;
        }

        private float volumeDown(float volume)
        {
            float r = volume - fadeIncrement;
            return (r < 0 || r > 1) ? 0 : r;
        }

        private void Fade(object o)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                if (song1Playing)
                {
                    song1.Volume = volumeDown(song1.Volume);
                    song2.Volume = volumeUp(song2.Volume);
                }
                else
                {
                    song1.Volume = volumeUp(song1.Volume);
                    song2.Volume = volumeDown(song2.Volume);
                }
            });
        }

        private void StopTimers()
        {
            t1.Dispose();
            t2.Dispose();
            if (song1Playing)
            {
                song1.Stop();
            }
            else
            {
                song2.Stop();
            }
        }
    }
}