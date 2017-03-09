using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TimeGear.Pomodoro
{
    public class PomodoroTimer
    {
        private SimpleTime mTime;
        public int Minutes
        {
            get
            {
                return mTime.Minutes;
            }
        }

        public string FormatedTime {
            get
            {
                return String.Format(mTime.Minutes.ToString("D2") +":" +mTime.Seconds.ToString("D2"));
            }
        }
        public bool CountDownMode { private get; set; }
        public bool Running { get; private set; }

        public PomodoroTimer()
        {
            mTime = new SimpleTime(0);
            Running = false;
            CountDownMode = true;
        }

        public void SetTime(int minutes)
        {
            mTime.Minutes = minutes;
            mTime.Seconds = 0;
        }

        public void Tick()
        {
            if(CountDownMode)
            {
                DecreaseTime();
                if(mTime.Minutes <= 0 && mTime.Seconds <= 0)
                {
                    Stop();
                }
            } 
            else
            {
                IncreaseTime();
            }
        }

        private void DecreaseTime()
        {
            mTime.Seconds--;
            if(mTime.Seconds < 0)
            {
                mTime.Seconds = 59;
                mTime.Minutes--;
            }
        }

        private void IncreaseTime()
        {
            mTime.Seconds++;
            if(mTime.Seconds >- 60)
            {
                mTime.Seconds=0;
                mTime.Minutes++;
            }
        }

        public void Start()
        {
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }
    }
}