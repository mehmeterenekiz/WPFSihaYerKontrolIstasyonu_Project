using SihaControlSystem.Classes;
using SihaControlSystem.UControls;
using System.Text;
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

namespace SihaControlSystem
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;
        private double _angle = 0;
        private double _speed = 1;
        private bool _isMouseOver = false;

        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RotatingImage_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromMilliseconds(30);
                _timer.Tick += Timer_Tick;
            }

            _isMouseOver = true;
            _speed = 1;
            _timer.Start();
        }

        private void RotatingImage_MouseLeave(object sender, MouseEventArgs e)
        {
            _isMouseOver = false;
            _speed = -_speed; // Ters yönde dönmeye başla
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _angle += _speed;
            rotateTransform.Angle = (_angle % 360 + 360) % 360; // Negatif açıları da pozitif tut

            if (_isMouseOver)
            {
                _speed += 0.3; // Hızlan
            }
            else
            {
                _speed *= 0.95; // Yavaşlat

                if (Math.Abs(_speed) < 0.5)
                {
                    _timer.Stop();

                }
            }
        }

        private void brd_Sagust_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            homepagetoggle.IsChecked = true;
            DBconnection.ConnectionTest();
            content_main_label.Content = DBconnection.ConnectionStatus;
            content_main_label.Content = DBconnection.DBaddress;

        }

        private void btn_iconstate_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_minmax_Click(object sender, RoutedEventArgs e)
        {

            if (RotatingImage.Width != 60)
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                    sidebar.RowDefinitions[1].Height = new GridLength(170, GridUnitType.Pixel);

                    if (content_header != null && content_header.ColumnDefinitions.Count > 1)
                        content_header.ColumnDefinitions[1].Width = new GridLength(400, GridUnitType.Pixel);

                    if (HeaderHomePage != null && HeaderHomePage.content_header != null && HeaderHomePage.content_header.ColumnDefinitions.Count > 1)
                        HeaderHomePage.content_header.ColumnDefinitions[1].Width = new GridLength(400, GridUnitType.Pixel);

                    content_header_siha.Margin = new Thickness(250, -3, 820, 45);
                    if (HeaderControlSiha != null)
                    {
                        HeaderControlSiha.SetBorderMargin(new Thickness(250, -3, 820, 45));
                    }
                    if (HeaderHomePage != null)
                    {
                        HeaderHomePage.content_header_siha.Margin = new Thickness(250, -3, 820, 45);
                    }
                    if (HeaderTaskPlanning != null)
                    {
                        HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(250, -3, 820, 45);
                    }
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    sidebar.RowDefinitions[1].Height = new GridLength(155, GridUnitType.Pixel);

                    if (content_header != null && content_header.ColumnDefinitions.Count > 1)
                        content_header.ColumnDefinitions[1].Width = new GridLength(340, GridUnitType.Pixel);

                    if (HeaderHomePage != null && HeaderHomePage.content_header != null && HeaderHomePage.content_header.ColumnDefinitions.Count > 1)
                        HeaderHomePage.content_header.ColumnDefinitions[1].Width = new GridLength(340, GridUnitType.Pixel);

                    content_header_siha.Margin = new Thickness(165, -3, 272, 42);
                    if (HeaderControlSiha != null)
                    {
                        HeaderControlSiha.SetBorderMargin(new Thickness(165, -3, 272, 42));

                    }
                    if (HeaderHomePage != null)
                    {
                        HeaderHomePage.content_header_siha.Margin = new Thickness(165, -3, 272, 42);
                    }

                    if (HeaderTaskPlanning != null)
                    {
                        HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(165, -3, 272, 42);
                    }
                }
            }
            else
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                    sidebar.RowDefinitions[1].Height = new GridLength(170, GridUnitType.Pixel);

                    if (content_header != null && content_header.ColumnDefinitions.Count > 1)
                        content_header.ColumnDefinitions[1].Width = new GridLength(400, GridUnitType.Pixel);

                    if (HeaderHomePage != null && HeaderHomePage.content_header != null && HeaderHomePage.content_header.ColumnDefinitions.Count > 1)
                        HeaderHomePage.content_header.ColumnDefinitions[1].Width = new GridLength(400, GridUnitType.Pixel);

                    content_header_siha.Margin = new Thickness(400, -3, 820, 45);
                    if (HeaderControlSiha != null)
                    {
                        HeaderControlSiha.SetBorderMargin(new Thickness(400, -3, 820, 45));
                    }
                    if (HeaderHomePage != null)
                    {
                        HeaderHomePage.content_header_siha.Margin = new Thickness(400, -3, 820, 45);
                    }
                    if (HeaderTaskPlanning != null)
                    {
                        HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(400, -3, 820, 45);
                    }
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    sidebar.RowDefinitions[1].Height = new GridLength(155, GridUnitType.Pixel);

                    if (content_header != null && content_header.ColumnDefinitions.Count > 1)
                        content_header.ColumnDefinitions[1].Width = new GridLength(340, GridUnitType.Pixel);

                    if (HeaderHomePage != null && HeaderHomePage.content_header != null && HeaderHomePage.content_header.ColumnDefinitions.Count > 1)
                        HeaderHomePage.content_header.ColumnDefinitions[1].Width = new GridLength(340, GridUnitType.Pixel);

                    content_header_siha.Margin = new Thickness(225, -3, 372, 45);
                    if (HeaderControlSiha != null)
                    {
                        HeaderControlSiha.SetBorderMargin(new Thickness(225, -3, 372, 45));

                    }
                    if (HeaderHomePage != null)
                    {
                        HeaderHomePage.content_header_siha.Margin = new Thickness(225, -3, 372, 45);
                    }
                    if (HeaderTaskPlanning != null)
                    {
                        HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(225, -3, 372, 45);
                    }
                }
            }
        }

        private void hamburgermenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (RotatingImage.Width != 60)
            {
                grd_columnmenu.Width = new GridLength(70, GridUnitType.Pixel);

                lbl_menu1.Visibility = Visibility.Hidden;
                lbl_menu2.Visibility = Visibility.Hidden;
                lbl_menu4.Visibility = Visibility.Hidden;
                lbl_menu5.Visibility = Visibility.Hidden;

                lbl_logoyazi.Visibility = Visibility.Hidden;
                RotatingImage.Width = 60;
                menu_bottomlbl.Visibility = Visibility.Hidden;
                menu_bottom_windowimage.Visibility = Visibility.Hidden;

                if (content_header_siha != null)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        content_header_siha.Margin = new Thickness(400, -3, 820, 45);
                    }
                    else
                    {
                        content_header_siha.Margin = new Thickness(225, -3, 372, 45);
                    }
                }

                if (HeaderHomePage != null)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        HeaderHomePage.content_header_siha.Margin = new Thickness(400, -3, 820, 45);
                    }
                    else
                    {
                        HeaderHomePage.content_header_siha.Margin = new Thickness(225, -3, 372, 45);
                    }
                }
                if (HeaderControlSiha != null)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        HeaderControlSiha.SetBorderMargin(new Thickness(400, -3, 820, 45));
                    }
                    else
                    {
                        HeaderControlSiha.SetBorderMargin(new Thickness(225, -3, 372, 45));
                    }
                }
                if (HeaderTaskPlanning != null)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(400, -3, 820, 45);
                    }
                    else
                    {
                        HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(225, -3, 372, 45);
                    }
                }

            }
            else
            {
                grd_columnmenu.Width = new GridLength(220, GridUnitType.Pixel);

                lbl_menu1.Visibility = Visibility.Visible;
                lbl_menu2.Visibility = Visibility.Visible;
                lbl_menu4.Visibility = Visibility.Visible;
                lbl_menu5.Visibility = Visibility.Visible;

                lbl_logoyazi.Visibility = Visibility.Visible;
                RotatingImage.Width = 130;
                menu_bottomlbl.Visibility = Visibility.Visible;
                menu_bottom_windowimage.Visibility = Visibility.Visible;

                if (content_header_siha != null)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        content_header_siha.Margin = new Thickness(250, -3, 820, 45);
                    }
                    else
                    {
                        content_header_siha.Margin = new Thickness(165, -3, 272, 42);
                    }
                }

                if (HeaderHomePage != null)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        HeaderHomePage.content_header_siha.Margin = new Thickness(250, -3, 820, 45);
                    }
                    else
                    {
                        HeaderHomePage.content_header_siha.Margin = new Thickness(165, -3, 272, 42);
                    }
                }
                if (HeaderControlSiha != null)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        HeaderControlSiha.SetBorderMargin(new Thickness(250, -3, 820, 45));
                    }
                    else
                    {
                        HeaderControlSiha.SetBorderMargin(new Thickness(165, -3, 272, 42));
                    }
                }
                if (HeaderTaskPlanning != null)
                {
                    if (this.WindowState == WindowState.Maximized)
                    {
                        HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(250, -3, 820, 45);
                    }
                    else
                    {
                        HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(165, -3, 272, 42);
                    }
                }
            }

        }

        private ucHeaderHomePage HeaderHomePage;
        private void homepage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            ucHomePage ContentHomePage = new ucHomePage();
            uc_call.Uc_Add(content_main, ContentHomePage);
            HeaderHomePage = new ucHeaderHomePage();
            uc_call.Uc_Add(content_header, HeaderHomePage);
            ContentHomePage.ContentLabel.Content = DBconnection.ConnectionStatus;

            if (RotatingImage.Width != 60)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    HeaderHomePage.content_header_siha.Margin = new Thickness(250, -3, 820, 45);

                    if (HeaderHomePage.content_header != null && HeaderHomePage.content_header.ColumnDefinitions.Count > 1)
                        HeaderHomePage.content_header.ColumnDefinitions[1].Width = new GridLength(400, GridUnitType.Pixel);
                }
                else
                {
                    HeaderHomePage.content_header_siha.Margin = new Thickness(165, -3, 272, 42);

                    if (HeaderHomePage.content_header != null && HeaderHomePage.content_header.ColumnDefinitions.Count > 1)
                        HeaderHomePage.content_header.ColumnDefinitions[1].Width = new GridLength(340, GridUnitType.Pixel);
                }
            }
            else
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    HeaderHomePage.content_header_siha.Margin = new Thickness(400, -3, 820, 45);

                    if (HeaderHomePage.content_header != null && HeaderHomePage.content_header.ColumnDefinitions.Count > 1)
                        HeaderHomePage.content_header.ColumnDefinitions[1].Width = new GridLength(400, GridUnitType.Pixel);
                }
                else
                {
                    HeaderHomePage.content_header_siha.Margin = new Thickness(225, -3, 372, 45);

                    if (HeaderHomePage.content_header != null && HeaderHomePage.content_header.ColumnDefinitions.Count > 1)
                        HeaderHomePage.content_header.ColumnDefinitions[1].Width = new GridLength(340, GridUnitType.Pixel);
                }
            }
        }

        int secimdurumu;
        private void homepagetoggle_Click(object sender, RoutedEventArgs e)
        {
            secimdurumu = 6;
            secilendurum();
        }

        private ucHeaderFlightInformations HeaderControlSiha;
        private void menubutton_flightinformations_Click(object sender, RoutedEventArgs e)
        {
            secimdurumu = 1;
            secilendurum();
            uc_call.Uc_Add(content_main, new ucFlightInformations());
            HeaderControlSiha = new ucHeaderFlightInformations();
            uc_call.Uc_Add(content_header, HeaderControlSiha);


            if (RotatingImage.Width != 60)
            {
                // Pencere durumu kontrolü yapılır ve uygun margin’i UserControl’e gönderilir
                if (this.WindowState == WindowState.Maximized)
                {
                    HeaderControlSiha.SetBorderMargin(new Thickness(250, -3, 820, 45));
                    content_header_siha.Margin = new Thickness(250, -3, 820, 45);
                }
                else
                {
                    HeaderControlSiha.SetBorderMargin(new Thickness(165, -3, 272, 42));
                    content_header_siha.Margin = new Thickness(165, -3, 272, 42);
                }
            }
            else
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    HeaderControlSiha.SetBorderMargin(new Thickness(400, -3, 820, 45));
                    content_header_siha.Margin = new Thickness(250, -3, 820, 45);
                }
                else
                {
                    HeaderControlSiha.SetBorderMargin(new Thickness(225, -3, 372, 45));
                    content_header_siha.Margin = new Thickness(165, -3, 272, 42);
                }
            }
        }

        private void menubutton_mappanel_Click(object sender, RoutedEventArgs e)
        {
            secimdurumu = 2;
            secilendurum();
            uc_call.Uc_Add(content_main, new ucMapPanel());
        }

        private ucHeaderTaskPlanning HeaderTaskPlanning;
        private void menubutton_taskplanning_Click(object sender, RoutedEventArgs e)
        {
            secimdurumu = 4;
            secilendurum();

            uc_call.Uc_Add(content_main, new ucTaskPlanning());
            HeaderTaskPlanning = new ucHeaderTaskPlanning();
            uc_call.Uc_Add(content_header, HeaderTaskPlanning);

            if (RotatingImage.Width != 60)
            {
                // Pencere durumu kontrolü yapılır ve uygun margin’i UserControl’e gönderilir
                if (this.WindowState == WindowState.Maximized)
                {

                    HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(250, -3, 820, 45);
                    content_header_siha.Margin = new Thickness(250, -3, 820, 45);
                }
                else
                {
                    HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(165, -3, 272, 42);
                    content_header_siha.Margin = new Thickness(165, -3, 272, 42);
                }
            }
            else
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(400, -3, 820, 45);
                    content_header_siha.Margin = new Thickness(250, -3, 820, 45);
                }
                else
                {
                    HeaderTaskPlanning.content_header_siha_taskplanning.Margin = new Thickness(225, -3, 372, 45);
                    content_header_siha.Margin = new Thickness(165, -3, 272, 42);
                }
            }
        }

        private void menubutton_systemstate_Click(object sender, RoutedEventArgs e)
        {
            secimdurumu = 5;
            secilendurum();
        }

        //checked durumu
        void secilendurum()
        {
            if (secimdurumu == 1)
            {
                menubutton_flightinformations.IsChecked = true;
            }
            else 
            {
                menubutton_flightinformations.IsChecked = false;
            }

            if (secimdurumu == 2)
            {
                menubutton_mappanel.IsChecked = true;
            }
            else
            {
                menubutton_mappanel.IsChecked = false;
            }

            if (secimdurumu == 4)
            {
                menubutton_taskplanning.IsChecked = true;
            }
            else
            {
                menubutton_taskplanning.IsChecked = false;
            }


            if (secimdurumu == 5)
            {
                menubutton_systemstate.IsChecked = true;
            }
            else
            {
                menubutton_systemstate.IsChecked = false;
            }

            if (secimdurumu == 6)
            {
                homepagetoggle.IsChecked = true;
            }
            else
            {
                homepagetoggle.IsChecked = false;
            }
        }

    }
}