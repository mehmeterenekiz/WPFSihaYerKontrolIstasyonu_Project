using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SihaControlSystem.Classes.Parametreler
{
    public class Prm
    {

    //Parametreleri insert etme
    
        private string _GorevAd;
        private string _Tarih;
        private string _Saat;
        private string _KoordinatLat;
        private string _KoordinatLng;
        private string _SureDk;
        private string _HedefMaxIrtifa;

        private int _OperatorID;
        private int _AracID;

        public string GorevAd { get => _GorevAd; set => _GorevAd = value; }
        public string Tarih { get => _Tarih; set => _Tarih = value; }
        public string Saat { get => _Saat; set => _Saat = value; }
        public string KoordinatLat { get => _KoordinatLat; set => _KoordinatLat = value; }
        public string KoordinatLng { get => _KoordinatLng; set => _KoordinatLng = value; }
        public string SureDk { get => _SureDk; set => _SureDk = value; }
        public string HedefMaxIrtifa { get => _HedefMaxIrtifa; set => _HedefMaxIrtifa = value; }
        public int OperatorID { get => _OperatorID; set => _OperatorID = value; }
        public int AracID { get => _AracID; set => _AracID = value; }
    }
}
