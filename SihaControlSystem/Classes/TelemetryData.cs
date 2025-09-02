using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SihaControlSystem.Classes
{
    public class TelemetryData
    {
        public double Altitude { get; set; }
        public double Speed { get; set; }
        public double Battery { get; set; }

        public override string ToString()
        {
            return $"İrtifa: {Altitude:F1} m\nHız: {Speed:F1} m/s\nPil: %{Battery:F1}";
        }
    }

}
