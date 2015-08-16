using GravityHero;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GravityHeroUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int maxCount = 10;
        int bandCount = 0;
        double maxForce = 0;
        bool isAchievementUnlocked = false;
        bool isSecondAchievementUnlocked = false;
        SpeechSynthesizer synth;

        AccelerometerModel _accelerometerModel = new AccelerometerModel();

        public MainPage()
        {
            this.InitializeComponent();
            UpdateCount();
        }

        private async void start_Click(object sender, RoutedEventArgs e)
        {
            await BandModel.InitAsync();
            Reset();
            _accelerometerModel.Init();
            _accelerometerModel.Changed += _accelerometerModel_Changed;
            synth = new SpeechSynthesizer();
            Speak("Let's go!");
            isAchievementUnlocked = false;

        }

        async void Speak(string message)
        {
            var stream = await synth.SynthesizeTextToStreamAsync(message);
            media.SetSource(stream, stream.ContentType);
            media.Play();
        }

        void _accelerometerModel_Changed(double force)
        {

            bandCount++;
            UpdateCount();
            if (force > maxForce)
            {
                maxForce = force;
                heroText.Text = String.Format("Intensity {0:F2}G", maxForce);
            }
            if (!isAchievementUnlocked && bandCount >= maxCount * 0.2)
            {
                Speak("Just a few more!");
                isAchievementUnlocked = true;
            }
            if (!isSecondAchievementUnlocked && isAchievementUnlocked && bandCount >= maxCount * 0.8)
            {
                Speak("Almost there!");
                isAchievementUnlocked = true;
            }
            BandModel.BandClient.NotificationManager.VibrateAsync(Microsoft.Band.Notifications.VibrationType.NotificationAlarm);
            //Speak(bandCount.ToString()+"!");
        }

        private void Reset()
        {
            if (BandModel.BandClient != null)
            {
                Random random = new Random();
                maxCount = random.Next(10, 20);
                UpdateCount();
            }
        }

        void UpdateCount()
        {
            countText.Text = String.Format("{0}", bandCount);
            togoText.Text = String.Format("jumps to go: {0}", maxCount);
        }
    }
}
