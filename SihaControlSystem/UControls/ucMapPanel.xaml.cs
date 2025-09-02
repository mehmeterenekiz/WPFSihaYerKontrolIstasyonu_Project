using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using SihaControlSystem.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SihaControlSystem.UControls
{
    /// <summary>
    /// Interaction logic for ucMapPanel.xaml
    /// </summary>
    public partial class ucMapPanel : UserControl
    {
        public ucMapPanel()
        {
            InitializeComponent();
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
            gmapControl.Zoom = 5.5;
            gmapControl.ShowCenter = false;

            gmapControl.MapProvider = GMapProviders.GoogleSatelliteMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            // Başlangıç konumu (örnek: İstanbul)
            gmapControl.Position = new PointLatLng(41.0082, 28.9784);

            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;

            DataTable tumGorevlerTablosu = DBislemci.TumGorevKoordinatlariniGetir();

            if (tumGorevlerTablosu != null)
            {
                double? minLat = null, maxLat = null, minLng = null, maxLng = null;


                foreach (DataRow row in tumGorevlerTablosu.Rows)
                {
                    if (double.TryParse(row["KoordinatLat"].ToString(), out double lat) &&
                        double.TryParse(row["KoordinatLng"].ToString(), out double lng))
                    {
                        PointLatLng konum = new PointLatLng(lat, lng);

                        // Marker Shape (Ellipse) tanımla
                        Ellipse markerShape = new Ellipse
                        {
                            Width = 12,
                            Height = 12,
                            Fill = Brushes.Red,
                            Stroke = Brushes.Black,
                            StrokeThickness = 1,
                            RenderTransformOrigin = new Point(0.5, 0.5),
                            Cursor = Cursors.Hand,
                        };

                        // Popup oluştur
                        Popup markerPopup = new Popup
                        {
                            PlacementTarget = markerShape,
                            Placement = PlacementMode.Right,
                            AllowsTransparency = true,
                            PopupAnimation = PopupAnimation.Fade,
                            StaysOpen = true // 👈 Popup açık kalmalı, biz yöneteceğiz
                        };

                        Border popupBorder = new Border
                        {
                            Background = Brushes.White,
                            BorderBrush = Brushes.DarkGray,
                            BorderThickness = new Thickness(1),
                            CornerRadius = new CornerRadius(1, 4, 4, 4),
                            Padding = new Thickness(5),
                            Child = new StackPanel
                            {
                                Children =
                                {
                                    new TextBlock
                                    {
                                        Text = $"Görev Adı: {row["GorevAd"]}\nLat: {lat:F5}\nLng: {lng:F5}",
                                        Foreground = Brushes.Black,
                                        Margin = new Thickness(0, 0, 0, 5)
                                    },
                                    new Button
                                    {
                                        Content = "İncele",
                                        Width = 80,
                                        HorizontalAlignment = HorizontalAlignment.Left,
                                        Margin = new Thickness(0),
                                        Tag = row,
                                        Style = (Style)Application.Current.Resources["ucButtons"],
                                        FontSize = 12,
                                    }
                                }
                            }
                        };

                        markerPopup.HorizontalOffset = 0;
                        markerPopup.VerticalOffset = 10;

                        markerPopup.Child = popupBorder;

                        // Durum bayrakları
                        bool isMouseOverMarker = false;
                        bool isMouseOverPopup = false;

                        DispatcherTimer popupTimer = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromSeconds(1)
                        };

                        popupTimer.Tick += (s, e) =>
                        {
                            popupTimer.Stop();
                            if (!isMouseOverMarker && !isMouseOverPopup)
                            {
                                markerPopup.IsOpen = false;
                            }
                        };

                        markerShape.MouseEnter += (s, e) =>
                        {
                            markerShape.Cursor = Cursors.Hand;
                            isMouseOverMarker = true;
                            markerPopup.IsOpen = true;
                            popupTimer.Stop();
                        };

                        markerShape.MouseLeave += (s, e) =>
                        {
                            isMouseOverMarker = false;
                            popupTimer.Start();
                        };

                        popupBorder.MouseEnter += (s, e) =>
                        {
                            popupBorder.Cursor = Cursors.Hand;
                            isMouseOverPopup = true;
                            markerPopup.IsOpen = true;
                            popupTimer.Stop();
                        };

                        popupBorder.MouseLeave += (s, e) =>
                        {
                            isMouseOverPopup = false;
                            popupTimer.Start();
                        };

                        // Marker tanımla
                        GMapMarker marker = new GMapMarker(konum)
                        {
                            Shape = markerShape,
                            Offset = new Point(-6, -6)
                        };

                        // Harita üzerine ekle
                        gmapControl.Markers.Add(marker);
                        mainGrid.Children.Add(markerPopup);

                        // Koordinat sınırlarını güncelle
                        minLat = minLat.HasValue ? Math.Min(minLat.Value, lat) : lat;
                        maxLat = maxLat.HasValue ? Math.Max(maxLat.Value, lat) : lat;
                        minLng = minLng.HasValue ? Math.Min(minLng.Value, lng) : lng;
                        maxLng = maxLng.HasValue ? Math.Max(maxLng.Value, lng) : lng;
                    }
                }

                if (minLat.HasValue && maxLat.HasValue && minLng.HasValue && maxLng.HasValue)
                {
                    double ortLat = (minLat.Value + maxLat.Value) / 2;
                    double ortLng = (minLng.Value + maxLng.Value) / 2;
                    gmapControl.Position = new PointLatLng(ortLat, ortLng);

                }
            }
        }
    }
}
