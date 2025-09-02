using SihaControlSystem.Classes;
using SihaControlSystem.Classes.Parametreler;
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
using System.Windows.Shapes;

namespace SihaControlSystem
{
    /// <summary>
    /// Interaction logic for winTaskPlanAdd.xaml
    /// </summary>
    public partial class winTaskPlanAdd : Window
    {
        public winTaskPlanAdd()
        {
            InitializeComponent();
        }

        private void btn_closewin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dragPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btn_AddCoord_Click(object sender, RoutedEventArgs e)
        {
            warning_text.Text = "Lütfen İHA'yı hareket ettirerek ulaşmak istediğiniz hedef koordinatı seçiniz!";
            popupBilgi.IsOpen = true;
            popup_call.PopupShow(popupBilgi);

            winTaskPlanAdd gk = Application.Current.Windows.OfType<winTaskPlanAdd>().SingleOrDefault(x => x.IsActive);

            if (gk != null)
            {
                winCoordSelection coordWindow = new winCoordSelection
                {
                    Owner = gk,
                    WindowStartupLocation = WindowStartupLocation.Manual
                };

                coordWindow.Left = gk.Left + (gk.Width - coordWindow.Width) / 2;
                coordWindow.Top = gk.Top + (gk.Height - coordWindow.Height) / 2;

                gk.Opacity = 0;
                coordWindow.ShowDialog();
                gk.Opacity = 1.0;
            }
        }

        private void txtDuration_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length-1))
            {
                e.Handled = true;
            }
        }

        private void txtIrtifa_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            // Boş alan kontrolü
            bool isEmpty = string.IsNullOrWhiteSpace(txtMissionName.Text) ||
                           txtMissionDate.SelectedDate == null ||
                           string.IsNullOrWhiteSpace(txtMissionTime.Text) ||
                           string.IsNullOrWhiteSpace(txtTargetLat.Text) ||
                           string.IsNullOrWhiteSpace(txtTargetLng.Text) ||
                           string.IsNullOrWhiteSpace(txtDuration.Text) ||
                           string.IsNullOrWhiteSpace(txtIrtifa.Text);

            if (isEmpty)
            {
                warning_text.Text = "Lütfen boş alanları doldurunuz!";
                popupBilgi.IsOpen = true;
                popup_call.PopupShow(popupBilgi);
            }
            else
            {
                Prm veri = new Prm();
                veri.GorevAd = txtMissionName.Text;
                veri.Tarih = txtMissionDate.Text;
                veri.Saat = txtMissionTime.Text;
                veri.KoordinatLat = txtTargetLat.Text;
                veri.KoordinatLng = txtTargetLng.Text;
                veri.SureDk = txtDuration.Text;
                veri.HedefMaxIrtifa = txtIrtifa.Text;

                ComboBoxItem selectedItemOperator = txtOperator.SelectedItem as ComboBoxItem;
                if (selectedItemOperator != null && selectedItemOperator.Tag != null)
                {
                    veri.OperatorID = Convert.ToInt32(selectedItemOperator.Tag);
                }

                ComboBoxItem selectedItemSiha = txtVehicle.SelectedItem as ComboBoxItem;
                if (selectedItemSiha != null && selectedItemSiha.Tag != null)
                {
                    veri.AracID = Convert.ToInt32(selectedItemSiha.Tag);
                }


                if (DBislemci.TaskInsert(veri))
                {
                    this.Close();
                    MessageBox.Show($"{veri.GorevAd} ekleme işlemi başarılı");
                }
                else
                {
                    warning_text.Text = "Görev ekleme işlemi başarısız";
                    popupBilgi.IsOpen = true;
                    popup_call.PopupShow(popupBilgi);
                }
            }
        }

    }
}
