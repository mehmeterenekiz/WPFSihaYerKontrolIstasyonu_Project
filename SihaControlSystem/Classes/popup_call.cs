using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace SihaControlSystem.Classes
{
    public class popup_call
    {
        private static DispatcherTimer timer;
        private static Popup currentPopup;
        public static void PopupShow(Popup popup)
        {
            popup.IsOpen = true;
            currentPopup = popup;

            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (currentPopup != null && currentPopup.IsOpen)
            {
                currentPopup.IsOpen = false;
            }

        }
    }
}
