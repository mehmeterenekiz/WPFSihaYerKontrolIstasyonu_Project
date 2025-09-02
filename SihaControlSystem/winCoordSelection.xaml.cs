using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
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
using System.Windows.Shapes;

namespace SihaControlSystem
{
    /// <summary>
    /// Interaction logic for winCoordSelection.xaml
    /// </summary>
    public partial class winCoordSelection : Window
    {
        public winCoordSelection()
        {
            InitializeComponent();
        }

        private bool isDragging = false;
        private Point startMousePosition;
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

            if (this.Owner is winTaskPlanAdd parentWindow)
            {
                if (double.TryParse(parentWindow.txtTargetLat.Text, out double lat) &&
                    double.TryParse(parentWindow.txtTargetLng.Text, out double lng))
                {
                    baslangicKonumu = new PointLatLng(lat, lng);
                    enlem.Text = lat.ToString("F6");
                    boylam.Text = lng.ToString("F6");
                }
            }

            gmapControl.Position = baslangicKonumu;

            // SİHA için marker oluştur
            var sihaImage = new System.Windows.Controls.Image
            {
                Width = 80,
                Height = 44,
                Source = new BitmapImage(new Uri("pack://application:,,,/images/icons/mapicons/mapsiha.png")),
                RenderTransformOrigin = new Point(0.5, 0.5),
                RenderTransform = new RotateTransform(0)
            };

            // Event handler'ları burada tanımla
            sihaImage.MouseLeftButtonDown += SihaImage_MouseLeftButtonDown;
            sihaImage.MouseMove += SihaImage_MouseMove;
            sihaImage.MouseLeftButtonUp += SihaImage_MouseLeftButtonUp;
            sihaImage.MouseEnter += SihaImage_MouseEnter;
            sihaImage.MouseLeave += SihaImage_MouseLeave;


            // GMapMarker'ı oluştur ve shape olarak bu image'i ver
            sihaMarker = new GMapMarker(baslangicKonumu)
            {
                Shape = sihaImage,
                Offset = new Point(-40, -22)
            };

            gmapControl.Markers.Add(sihaMarker);
        }

        private void SihaImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void SihaImage_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        private void SihaImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            Mouse.OverrideCursor = Cursors.Hand;
            startMousePosition = e.GetPosition(gmapControl);
            gmapControl.CanDragMap = false; // Harita sürüklemeyi geçici kapat
            ((Image)sender).CaptureMouse();
        }

        private void SihaImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentMousePosition = e.GetPosition(gmapControl);
                var localPoint = new PointLatLng();

                // Ekran farkını enlem-boylama dönüştür
                GMapControl map = gmapControl;
                localPoint = map.FromLocalToLatLng((int)currentMousePosition.X, (int)currentMousePosition.Y);

                // Marker pozisyonunu güncelle
                sihaMarker.Position = localPoint;

                // TextBlock'lara konumu yaz
                enlem.Text = localPoint.Lat.ToString("F6");
                boylam.Text = localPoint.Lng.ToString("F6");

                if (this.Owner is winTaskPlanAdd parentWindowAdd)
                {
                    parentWindowAdd.txtTargetLat.Text = localPoint.Lat.ToString("F6");
                    parentWindowAdd.txtTargetLng.Text = localPoint.Lng.ToString("F6");
                }
                if (this.Owner is winTaskPlanEdit2 parentWindowEdit2)
                {
                    parentWindowEdit2.txtTargetLat.Text = localPoint.Lat.ToString("F6");
                    parentWindowEdit2.txtTargetLng.Text = localPoint.Lng.ToString("F6");
                }
            }
        }

        private void SihaImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            Mouse.OverrideCursor = null;
            gmapControl.CanDragMap = true; // Harita sürüklemeyi geri aç
            ((Image)sender).ReleaseMouseCapture();
        }

        private void btn_winclosecoord_Click(object sender, RoutedEventArgs e)
        {
            if (this.Owner is winTaskPlanAdd parentWindow)
            {
                parentWindow.popupBilgi.IsOpen = false;
                parentWindow.txtTargetLat.Text = null;
                parentWindow.txtTargetLng.Text = null;
            }
            this.Close();
        }

        private void dragPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            if (this.Owner is winTaskPlanAdd parentWindow)
            {
                parentWindow.popupBilgi.IsOpen = false;
                parentWindow.txtTargetLat.Text = null;
                parentWindow.txtTargetLng.Text = null;
            }
            this.Close();
        }

        private void btn_winsavecoord_Click(object sender, RoutedEventArgs e)
        {
            if (this.Owner is winTaskPlanAdd parentWindow)
            {
                parentWindow.popupBilgi.IsOpen = false;
            }
            this.Close();
        }
    }
}
