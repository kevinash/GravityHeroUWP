using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace GravityHero
{
    [DataContract]
    public class SensorReading : ViewModel
    {
        DateTimeOffset _timestamp;

        [DataMember]
        public DateTimeOffset Timestamp
        {
            get { return _timestamp; }
            set
            {
                SetValue(ref _timestamp, value, "Timestamp");
            }
        }

        double _x;

        [DataMember]
        public double X
        {
            get { return _x; }
            set
            {
                SetValue(ref _x, value, "X", "Value");
            }
        }

        double _y;

        [DataMember]
        public double Y
        {
            get { return _y; }
            set
            {
                SetValue(ref _y, value, "Y", "Value");
            }
        }

        double _z;

        [DataMember]
        public double Z
        {
            get { return _z; }
            set
            {
                SetValue(ref _z, value, "Z", "Value");
            }
        }


        public double Value
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }
    }
}
