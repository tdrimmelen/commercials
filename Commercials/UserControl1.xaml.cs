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
// using vMixInterop;
using System.Reflection;

namespace Commercials
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 

    // vMixUserControl disabled as it gives extra CPU load - No idea of root cause
    public partial class UserControl1 : UserControl // , vMixWPFUserControl
    {
        Scheduler theScheduler;
        public UserControl1()
        {
            InitializeComponent();

            try
            {
                string myConfigFileName = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\commercials.config";
                Configuration myConfig = new Configuration(myConfigFileName);

                string myTempFileName = myConfig["TimelineFilename"];
                string myTimelineFilename = Environment.ExpandEnvironmentVariables(myTempFileName);

                theScheduler = new Scheduler(this.Dispatcher, Update);
                JSONConfig.ReadFromFile(myTimelineFilename, theScheduler);

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                throw e;
            }

            // Initialise source
            BitmapImage mySource = new BitmapImage();

            mySource.BeginInit();
            mySource.UriSource = new Uri(theScheduler.GetFirstSlotCommercial());
            mySource.EndInit();

            Image.Source = mySource;
            Image.Visibility = System.Windows.Visibility.Hidden;

            // Start the scheduler
            theScheduler.Start();

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
            this.Height = height;
            this.Width = width;

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
