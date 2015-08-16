using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Band.Sensors;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Core;
using System.Diagnostics;
using Windows.ApplicationModel.Core;

namespace GravityHero
{
    /// <summary>
    /// Kevin Ashley, Microsoft
    /// </summary>
    public class AccelerometerModel : ViewModel
    {
        public delegate void ChangedHandler(double force);
        public event ChangedHandler Changed;

        DateTimeOffset _startedTime = DateTimeOffset.MinValue;
        double totalTime = 0.0;
        double lastTime = 0.0;
        SensorReading _prev;
        SensorReading _last;
        double MIN = 0.4;
      
        public void Init()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                BandModel.BandClient.SensorManager.Accelerometer.ReportingInterval = TimeSpan.FromMilliseconds(16.0);
                BandModel.BandClient.SensorManager.Accelerometer.StartReadingsAsync(new CancellationToken());
                totalTime = 0.0;
            }
        }

        void Accelerometer_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> e)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
           {
               SensorReading reading = new SensorReading { X = e.SensorReading.AccelerationX, Y = e.SensorReading.AccelerationY, Z = e.SensorReading.AccelerationZ };
               _prev = _last;
               _last = reading;
               Recalculate();
            });
        }


        void Recalculate()
        {
            if (_last.Value <= MIN)
            {
                if (_startedTime > DateTimeOffset.MinValue)
                    lastTime = (DateTimeOffset.Now - _startedTime).TotalSeconds;
                else
                    _startedTime = DateTimeOffset.Now;
            }
            else
            {
                if (_startedTime > DateTimeOffset.MinValue)
                {
                    lastTime = (DateTimeOffset.Now - _startedTime).TotalSeconds;
                    totalTime += lastTime;
                    lastTime = 0.0;
                    _startedTime = DateTimeOffset.MinValue;
                    if (Changed != null)
                        Changed(_last.Value);
                }
            }
        }

        
    }
}
