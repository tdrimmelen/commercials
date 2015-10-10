using System;
using System.Collections.Generic;
using System.Linq;
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
using vMixInterop;

namespace Commercials
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl, vMixWPFUserControl
    {
        Scheduler theScheduler;
        public UserControl1()
        {
            InitializeComponent();

            theScheduler = new Scheduler(this.Dispatcher, Update);

            theScheduler.AddEvent(Scheduler.EventType.Show, new TimeSpan(0, 0, 5), @"C:\Visual Studio Projects\Commercials\Commercials\bin\Debug\quoratio.PNG");
            theScheduler.AddEvent(Scheduler.EventType.Hide, new TimeSpan(0, 0, 15), @"");
            theScheduler.AddEvent(Scheduler.EventType.Show, new TimeSpan(0, 0, 20), @"C:\Visual Studio Projects\Commercials\Commercials\bin\Debug\kales.jpg");
            theScheduler.AddEvent(Scheduler.EventType.Hide, new TimeSpan(0, 0, 30), @"");
            theScheduler.AddEvent(Scheduler.EventType.Show, new TimeSpan(0, 0, 35), @"C:\Visual Studio Projects\Commercials\Commercials\bin\Debug\qfc.PNG");
            theScheduler.AddEvent(Scheduler.EventType.Hide, new TimeSpan(0, 0, 45), @"");
        }

        private void Update(string aCommercial)
        {
            if (aCommercial != "")
            {
                BitmapImage mySource = new BitmapImage();

                mySource.BeginInit();
                mySource.UriSource = new Uri(aCommercial);
                mySource.EndInit();

                Image.Source = mySource;
                Image.Visibility = System.Windows.Visibility.Visible;

            }
            else
            {
                Image.Visibility = System.Windows.Visibility.Hidden;
            }
 
        }

        public void Close()
        {
            theScheduler.Stop();
        }
    

        public TimeSpan GetDuration()
        {
            return theScheduler.Duration();
        }

        public TimeSpan GetPosition()
        {
            return theScheduler.Position();
        }

        public void Load(int width, int height)
        {
 	        
        }

        public void Pause()
        {
            theScheduler.Stop();	        
        }

        public void Play()
        {
            theScheduler.Start();
           
        }

        public void SetPosition(TimeSpan position)
        {
            theScheduler.SetPosition(position);
        }

        public void ShowProperties()
        {
 	       
        }
}
}
