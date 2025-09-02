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

namespace SihaControlSystem.UControls
{
    /// <summary>
    /// Interaction logic for ucHeaderFlightInformations.xaml
    /// </summary>
    public partial class ucHeaderFlightInformations : UserControl
    {
        public ucHeaderFlightInformations()
        {
            InitializeComponent();
        }

        public void SetBorderMargin(Thickness newMargin)
        {
            content_header_siha_flight.Margin = newMargin; // myBorder UserControl içindeki Border kontrolünün x:Name'i
        }
    }
}
