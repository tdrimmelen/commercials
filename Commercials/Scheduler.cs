using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Windows.Threading;

namespace Commercials
{

    class Scheduler
    {
        public enum EventType { Show, Hide};

        private class CommercialEvent
        {
            public EventType type { get; set; }
            public TimeSpan offset { get; set; }
            public string commercial { get; set; }

        }

        Queue<CommercialEvent> theQueue;
        Queue<CommercialEvent>.Enumerator theEnum;

        private Action<string> theDelegate;
        private System.Timers.Timer theTimer;
        private Dispatcher theDispatcher;
        private DateTimeOffset theLastTime; // Time that last event occured
        private TimeSpan theLastRelTime; // Relative time last event occured

        private System.Object theLock;
 

        public Scheduler(Dispatcher aDispatcher, Action<string> aDelegate)
        {
            theDelegate = aDelegate;
            theDispatcher = aDispatcher;
            theTimer = new System.Timers.Timer();

            theQueue = new Queue<CommercialEvent>();
            theLastRelTime = TimeSpan.Zero;

            theTimer.Elapsed += Run;
            theTimer.Enabled = false;
            theTimer.AutoReset = true;

            theLock = new System.Object();


        }

        ~Scheduler()
        {
            theTimer.Enabled = false;
        }

        public void AddEvent(EventType aType, TimeSpan anOffset, string aCommercial)
        {
            CommercialEvent myEvent = new CommercialEvent();

            myEvent.type = aType;
            myEvent.offset = anOffset;
            myEvent.commercial = aCommercial;

            lock(theLock)
            {
                theQueue.Enqueue(myEvent);
                theEnum = theQueue.GetEnumerator();
                theEnum.MoveNext();
            }
        }

        public string GetFirstSlotCommercial()
        {
            return theQueue.First().commercial;
        }

        private void Run(Object source, ElapsedEventArgs ea)
        {
            lock(theLock)
            {
                theTimer.Enabled = false;

                CommercialEvent myEvent = theEnum.Current;
                theEnum.MoveNext();

                if (myEvent.type == EventType.Show)
                {
                    theDispatcher.BeginInvoke(DispatcherPriority.Send, theDelegate, myEvent.commercial);
                }
                else
                {
                    theDispatcher.BeginInvoke(DispatcherPriority.Send, theDelegate, "");
                }

                theLastRelTime = myEvent.offset;
                theLastTime = DateTimeOffset.Now;

                Start();

            }

        }

        public void Start()
        {
            lock(theLock)
            {
                try
                {
                    CommercialEvent myEvent = theEnum.Current;

                    theTimer.Interval = (myEvent.offset - theLastRelTime).TotalMilliseconds;
                    theLastTime = DateTimeOffset.Now;
                    theTimer.Enabled = true;

                }
                catch(InvalidOperationException)
                {
                    // Do nothing
                }
 
            }

        }

        public void Stop()
        {
            lock (theLock)
            {
                theTimer.Enabled = false;
                theLastRelTime = theLastRelTime + (DateTimeOffset.Now - theLastTime);
            }
        
        }

        public TimeSpan Position()
        {
            lock(theLock)
            {
                if (theTimer.Enabled)
                {
                    return theLastRelTime + (DateTimeOffset.Now - theLastTime);
                }
                else
                {
                    return theLastRelTime;
                }
            }
        }

        public TimeSpan Duration()
        {
            lock(theLock)
            {
                return theQueue.Last().offset;
            }
        }

        public void SetPosition(TimeSpan aPosition)
        {
            lock(theLock)
            {
                bool myIsRunning = (theTimer.Enabled);
                bool myIsBeforeFirstEvent = true;

                Stop();

                theEnum = theQueue.GetEnumerator();
                theEnum.MoveNext();

                // Points to event that is just before aPosition
                Queue<CommercialEvent>.Enumerator myCurrentEventEnum = theQueue.GetEnumerator(); 

                // Search next event to process and set theEnum to it
                foreach (CommercialEvent e in theQueue)
                {
                    if (e.offset > aPosition)
                    {
                        break;
                    }
                    theEnum.MoveNext();
                    myCurrentEventEnum.MoveNext();
                    myIsBeforeFirstEvent = false;
                }
                theLastRelTime = aPosition;

                if (!myIsBeforeFirstEvent)
                {
                    CommercialEvent myEvent = myCurrentEventEnum.Current;

                    if (myEvent.type == EventType.Show && !myIsBeforeFirstEvent)
                    {
                        theDispatcher.BeginInvoke(DispatcherPriority.Send, theDelegate, myEvent.commercial);
                    }
                    else
                    {
                        theDispatcher.BeginInvoke(DispatcherPriority.Send, theDelegate, "");
                    }

                }
                else
                {
                    theDispatcher.BeginInvoke(DispatcherPriority.Send, theDelegate, "");
                }

                if (myIsRunning)
                {
                    Start();
                }

            }


        }
    }
}
