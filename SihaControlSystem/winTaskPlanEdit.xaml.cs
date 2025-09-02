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
    /// Interaction logic for winTaskPlanEdit.xaml
    /// </summary>
    public partial class winTaskPlanEdit : Window
    {
        public winTaskPlanEdit()
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

                winTaskPlanEdit gk = Application.Current.Windows.OfType<winTaskPlanEdit>().SingleOrDefault(x => x.IsActive);

                if (gk != null)
                {
                    winTaskPlanEdit2 winTaskPlanEdit2 = new winTaskPlanEdit2
                    {
                        Owner = gk,
                        WindowStartupLocation = WindowStartupLocation.Manual
                    };

                    winTaskPlanEdit2.Left = gk.Left + (gk.Width - winTaskPlanEdit2.Width) / 2;
                    winTaskPlanEdit2.Top = gk.Top + (gk.Height - winTaskPlanEdit2.Height) / 2;

                    gk.Opacity = 0;
                    winTaskPlanEdit2.ShowDialog();
                    gk.Opacity = 1.0;
                }

            }
            else
            {
                MessageBox.Show("Lütfen bir görev seçiniz.");
            }
        }

        private void btnTaskDelete_Click(object sender, RoutedEventArgs e)
        {

            if (dtg_TasksList.SelectedItem is DataRowView row)
            {
                SecilenGorev = new TaskInfo
                {
                    Id = Convert.ToInt32(row["ID"]),
                    Name = row["GorevAd"]?.ToString() ?? "",
                };

                if (DBislemci.TaskDelete(SecilenGorev.Id))
                {
                    this.Close();
                    MessageBox.Show($"{SecilenGorev.Name} silme işlemi başarılı");

                    MainWindow gk = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault(x => x.IsActive);
                    winTaskPlanEdit winTaskPlanEdit = new winTaskPlanEdit();
                    winTaskPlanEdit.Owner = gk;
                    winTaskPlanEdit.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Silme işlemi başarısız");
                }

            }
            else
            {
                MessageBox.Show("Lütfen bir görev seçiniz.");
            }

        }
    }
}
