using SihaControlSystem.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SihaControlSystem.Services
{
    public class TelemetrySimulator
    {
        private Random rnd = new Random();

        public TelemetryData Generate()
        {
            return new TelemetryData
            {
                Altitude = rnd.NextDouble() * 100,
                Speed = rnd.NextDouble() * 50,
                Battery = 100 - rnd.NextDouble() * 10
            };
        }
    }

}
