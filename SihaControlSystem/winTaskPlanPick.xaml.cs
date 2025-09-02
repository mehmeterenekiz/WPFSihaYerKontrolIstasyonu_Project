using SihaControlSystem.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for winTaskPlanPick.xaml
    /// </summary>
    public partial class winTaskPlanPick : Window
    {
        public winTaskPlanPick()
        {
            InitializeComponent();
        }

        private void dragPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void btn_closewin_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TaskPlanPicker_Loaded(object sender, RoutedEventArgs e)
        {
            DBislemci.GridDoldur(dtg_TasksList);
        }

        public TaskInfo? SecilenGorev { get; private set; }
        private void btn_picktask_Click(object sender, RoutedEventArgs e)
        {
            if (dtg_TasksList.SelectedItem is DataRowView row)
            {
                SecilenGorev = new TaskInfo
                {
                    Id = Convert.ToInt32(row["ID"]),
                    Name = row["GorevAd"]?.ToString() ?? "",
                    Vehicle = row["HavaAraciAdi"]?.ToString() ?? "",
                    OperatorAdi = row["AdiSoyadi"]?.ToString() ?? "",
                    Date = row["Tarih"]?.ToString() ?? "",
                    Time = Convert.ToInt32(row["SureDk"]),
                    Maxft = Convert.ToInt32(row["HedefMaxIrtifa"]),
                    lat = Convert.ToDouble(row["KoordinatLat"]),
                    lng = Convert.ToDouble(row["KoordinatLng"]),
                };

                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Lütfen bir görev seçiniz.");
            }
        }
    }
}
