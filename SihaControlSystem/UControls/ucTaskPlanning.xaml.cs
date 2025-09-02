using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using SihaControlSystem.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace SihaControlSystem.UControls
{
    /// <summary>
    /// Interaction logic for ucTaskPlanning.xaml
    /// </summary>
    public partial class ucTaskPlanning : UserControl
    {
        public ucTaskPlanning()
        {
            InitializeComponent();
        }

        MainWindow gk = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        private void btnTaskPlanAdd_Click(object sender, RoutedEventArgs e)
        {
            winTaskPlanAdd winTaskPlanAdd = new winTaskPlanAdd();
            winTaskPlanAdd.Owner = gk;
            gk.Opacity = 0.8;
            winTaskPlanAdd.ShowDialog();
            gk.Opacity = 1.0;
        }

        private GMapMarker sihaMarker;
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
        }

        private void btnTaskPlanArrange_Click(object sender, RoutedEventArgs e)
        {
            winTaskPlanEdit winTaskPlanEdit = new winTaskPlanEdit();
            winTaskPlanEdit.Owner = gk;
            gk.Opacity = 0.8;
            bool? result = winTaskPlanEdit.ShowDialog();
            gk.Opacity = 1.0;

            if (result == true) // Seçim yapılmışsa
            {
                TaskInfo? selectedTask = winTaskPlanEdit.SecilenGorev;

                if (selectedTask != null)
                {
                    int selectedTaskId = selectedTask.Id;

                }
            }

        }
    }
}
