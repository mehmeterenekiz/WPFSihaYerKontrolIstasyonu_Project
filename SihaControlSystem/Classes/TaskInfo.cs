using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SihaControlSystem.Classes
{
    public class TaskInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vehicle { get; set; }
        public string OperatorAdi { get; set; }
        public string Date { get; set; }
        public double Time { get; set; }
        public double Maxft { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }


    }

}
