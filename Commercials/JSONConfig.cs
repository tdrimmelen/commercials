using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Commercials
{

    class JSONConfig
    {
        public static void ReadFromFile(string aFileName, Scheduler aScheduler)
        {

            FileStream myFileStream = new FileStream(aFileName, FileMode.Open);
            Timeline myTimeline;
            string myFolderName = Path.GetDirectoryName(aFileName);

            try
            {
                DataContractJsonSerializer myJsonSerializer = new DataContractJsonSerializer(typeof(Timeline));
                object myObjResponse = myJsonSerializer.ReadObject(myFileStream);
                myTimeline = myObjResponse as Timeline;

                TimeSpan myLastSlotTime = TimeSpan.Zero;

                foreach (Slot mySlot in myTimeline.slots)
                {
                    TimeSpan mySlotStart = TimeSpan.Parse("00:" + mySlot.start);
                    String myPath = myFolderName + @"\" + mySlot.commercial;

                    if (mySlotStart <= myLastSlotTime)
                    {
                        throw new FormatException(@"Slot time '" + mySlotStart + @"' overlapping or not in right order.");
                    }

                    if (!File.Exists(myPath))
                    {
                        throw new System.IO.FileNotFoundException("File " + mySlot.commercial + " does not exists");
                    }

                    try
                    {
                        BitmapImage mySource = new BitmapImage();

                        mySource.BeginInit();
                        mySource.UriSource = new Uri(myPath);
                        mySource.EndInit();

                    }
                    catch (Exception)
                    {
                        throw new FormatException("File " + mySlot.commercial + " cannot be loaded. Wrong format?");
                    }

                    aScheduler.AddEvent(Scheduler.EventType.Show, mySlotStart, myFolderName + @"\" + mySlot.commercial);

                    int myDuration = 0;

                    if (mySlot.duration > 0)
                    {
                        myDuration = mySlot.duration;
                    }
                    else
                    {
                        myDuration = myTimeline.defaultDuration;
                    }

                    TimeSpan mySlotEnd =  mySlotStart.Add(TimeSpan.FromSeconds(myDuration));
                    aScheduler.AddEvent(Scheduler.EventType.Hide, mySlotEnd, "");

                    myLastSlotTime = mySlotEnd;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
