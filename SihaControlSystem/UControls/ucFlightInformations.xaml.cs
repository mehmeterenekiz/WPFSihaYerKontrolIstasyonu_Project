using GMap.NET.WindowsPresentation;
using GMap.NET;
using GMap.NET.MapProviders;
using SihaControlSystem.Classes;
using SihaControlSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Net;

namespace SihaControlSystem.UControls
{
    /// <summary>
    /// Interaction logic for ucFlightInformations.xaml
    /// </summary>
    public partial class ucFlightInformations : UserControl
    {
        private TelemetrySimulator simulator;
        private DispatcherTimer timer;
        private DispatcherTimer returnTimer;
        private TelemetryData currentData;
        private int returnCountdown = 5;
        private PointLatLng? hedefKonum = null;

        public ucFlightInformations()
        {
            InitializeComponent();

            simulator = new TelemetrySimulator();
            currentData = new TelemetryData();
        }

        private double CalculateBearing(PointLatLng from, PointLatLng to)
        {
            double lat1 = DegreesToRadians(from.Lat);
            double lon1 = DegreesToRadians(from.Lng);
            double lat2 = DegreesToRadians(to.Lat);
            double lon2 = DegreesToRadians(to.Lng);

            double dLon = lon2 - lon1;

            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) -
                       Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            double brng = Math.Atan2(y, x);

            return (RadiansToDegrees(brng) + 360) % 360;
        }

        private double DegreesToRadians(double deg) => deg * (Math.PI / 180);
        private double RadiansToDegrees(double rad) => rad * (180 / Math.PI);

        private Random random = new Random(); // Sınıfın üstüne koy
        private void Timer_Tick(object sender, System.EventArgs e)
        {
            currentData = simulator.Generate();
            txtTelemetry.Text = currentData.ToString();

            PointLatLng oncekiKonum = sihaMarker.Position;
            PointLatLng yeniKonum;

            if (hedefKonum != null)
            {
                double maxStep = 0.05; // ne kadar ilerleyecek (ayarla)
                yeniKonum = MoveTowards(oncekiKonum, hedefKonum.Value, maxStep);
            }
            else
            {
                // Eğer hedef yoksa rastgele hareket
                double deltaLat = (random.NextDouble() - 0.2) * 0.03;
                double deltaLng = (random.NextDouble() - 0.2) * 0.01;

                yeniKonum = new PointLatLng(oncekiKonum.Lat + deltaLat, oncekiKonum.Lng + deltaLng);
            }

            sihaMarker.Position = yeniKonum;
            gmapControl.Position = yeniKonum;

            double bearing = CalculateBearing(oncekiKonum, yeniKonum);

            // Dönme açısını marker görüntüsüne uygula
            if (sihaMarker.Shape is Image image && image.RenderTransform is RotateTransform rt)
            {
                rt.Angle = bearing;
            }

            rotaNoktalari.Add(yeniKonum);

            rotaCizgisi.Points.Clear();
            foreach (var nokta in rotaNoktalari)
            {
                rotaCizgisi.Points.Add(nokta);
            }

            if (rotaCizgisi.Shape is Path path)
            {
                path.Stroke = Brushes.White;
                path.StrokeThickness = 2;
                path.Opacity = 0.9;
            }
        }

        private void btnTakeOff_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("İHA havalanıyor...");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void btnLand_Click(object sender, RoutedEventArgs e)
        {
            // Normal veri üretimini durdur
            if (timer!=null)
            {
                MessageBox.Show("İHA iniş yapıyor...");
                timer.Stop();
                returnCountdown = 5;

                // Geri dönüş simülasyon zamanlayıcısı başlat
                returnTimer = new DispatcherTimer();
                returnTimer.Interval = TimeSpan.FromSeconds(1);
                returnTimer.Tick += ReturnTimer_Tick;
                returnTimer.Start();
            }
            else
            {
                MessageBox.Show("İHA uçuşta değil...");
            }
        }

        private void btnReturnHome_Click(object sender, RoutedEventArgs e)
        {
            // Normal veri üretimini durdur
            if (timer != null)
            {
                MessageBox.Show("İHA eve dönüyor...");
                timer.Stop();
                returnCountdown = 5;

                // Geri dönüş simülasyon zamanlayıcısı başlat
                returnTimer = new DispatcherTimer();
                returnTimer.Interval = TimeSpan.FromSeconds(1);
                returnTimer.Tick += ReturnTimer_Tick;
                returnTimer.Start();
            }
            else
            {
                MessageBox.Show("İHA uçuşta değil...");
            }
        }

        private void ReturnTimer_Tick(object sender, EventArgs e)
        {
            if (returnCountdown > 0)
            {
                // Her saniye hızı ve irtifayı azalt (doğrusal azaltım)
                currentData.Altitude = Math.Max(0, currentData.Altitude - (currentData.Altitude / returnCountdown));
                currentData.Speed = Math.Max(0, currentData.Speed - (currentData.Speed / returnCountdown));

                returnCountdown--;
                txtTelemetry.Text = currentData.ToString();
            }
            else
            {
                // 5 saniye sonunda tam sıfırlama
                currentData.Altitude = 0;
                currentData.Speed = 0;
                txtTelemetry.Text = currentData.ToString();

                returnTimer.Stop();
            }
        }

        private GMapMarker sihaMarker;
        private List<PointLatLng> rotaNoktalari = new List<PointLatLng>();
        private GMapRoute rotaCizgisi;

        private void gmapControl_Loaded(object sender, RoutedEventArgs e)
        {
            gmapControl.Bearing = 0;
            gmapControl.CanDragMap = true;
            gmapControl.DragButton = MouseButton.Left; // Sol tuşla sürükleme

            gmapControl.MinZoom = 2;
            gmapControl.MaxZoom = 18;
            gmapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;

            gmapControl.ShowTileGridLines = false;
            gmapControl.Zoom = 10;
            gmapControl.ShowCenter = false;

            gmapControl.MapProvider = GMapProviders.GoogleSatelliteMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            // Başlangıç konumu (örnek: İstanbul)
            gmapControl.Position = new PointLatLng(41.0082, 28.9784);

            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;

            PointLatLng baslangicKonumu = new PointLatLng(41.0082, 28.9784);

            // SİHA için marker oluştur
            sihaMarker = new GMapMarker(baslangicKonumu)
            {
                Shape = new System.Windows.Controls.Image
                {
                    Width = 80,
                    Height = 44,
                    Source = new BitmapImage(new Uri("pack://application:,,,/images/icons/mapicons/mapsiha.png")), // kendi ikonunu kullan
                    RenderTransformOrigin = new Point(0.5, 0.5), // Dönüş merkezi: orta
                    RenderTransform = new RotateTransform(0)     // Başlangıç açısı: 0 derece
                },
                Offset = new System.Windows.Point(-40, -22) // ikon ortalansın
            };

            gmapControl.Markers.Add(sihaMarker);

            rotaNoktalari.Add(baslangicKonumu);
            rotaCizgisi = new GMapRoute(rotaNoktalari);

            //rotaCizgisi.Shape = new Line ()
            //{
            //    Stroke = System.Windows.Media.Brushes.Red,
            //    StrokeThickness = 2,
            //    Opacity = 0.6
            //};

            gmapControl.Markers.Add(rotaCizgisi);
        }

        MainWindow gk = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        private GMapMarker HedefMarker;
        private void btnSelectPlan_Click(object sender, RoutedEventArgs e)
        {
            winTaskPlanPick winTaskPlanPick = new winTaskPlanPick();
            winTaskPlanPick.Owner = gk;
            gk.Opacity = 0.8;
            bool? result = winTaskPlanPick.ShowDialog();
            gk.Opacity = 1;

            if (result == true && winTaskPlanPick.SecilenGorev != null)
            {
                var secilen = winTaskPlanPick.SecilenGorev;
                hedefKonum = new PointLatLng(secilen.lat, secilen.lng);
                taskinfos.Text = $" ID: {secilen.Id} \n Görev Adı: {secilen.Name} \n İHA Türü: {secilen.Vehicle} \n Operatör Adı: {secilen.OperatorAdi} \n Hedef Max İrtifa (ft): {secilen.Maxft} " +
                    $"\n Koordinatlar: {secilen.lat} {secilen.lng}";
            }

            // SİHA için marker oluştur
            if (hedefKonum.HasValue)
            {
                gmapControl.Markers.Remove(HedefMarker);

                HedefMarker = new GMapMarker(hedefKonum.Value)
                {
                    Shape = new System.Windows.Shapes.Ellipse
                    {
                        Width = 20,
                        Height = 20,
                        Fill = Brushes.Red,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    },
                    Offset = new Point(-10, -10)
                };

                gmapControl.Markers.Add(HedefMarker);
            }
        }

        private PointLatLng MoveTowards(PointLatLng current, PointLatLng target, double maxStep)
        {
            double latDiff = target.Lat - current.Lat;
            double lngDiff = target.Lng - current.Lng;

            double distance = Math.Sqrt(latDiff * latDiff + lngDiff * lngDiff);

            if (distance <= maxStep || distance == 0)
            {
                reachedtarget.Text = "Hedef konuma ulaşıldı.";
                return target; // hedefine ulaştı
            }
            else
            {
                double ratio = maxStep / distance;
                double newLat = current.Lat + latDiff * ratio;
                double newLng = current.Lng + lngDiff * ratio;
                return new PointLatLng(newLat, newLng);
            }
        }

    }
}
