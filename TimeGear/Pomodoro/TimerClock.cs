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

namespace TimeGear
{
    public class TimerClock
    {
        private int mTime;
        private int mStopTime;

        public int Minutes
        {
            get
            {
                return mTime / 60;
            }
        }        

        public int Seconds
        {
            get
            {
                return mTime % 60;
            }
        }

        public bool CountDown { get; set; }

        public TimerClock()
        {
            mTime = 0;
            mStopTime = 0;
            CountDown = true;
        }

        public void SetTime(int minutes, int seconds)
        {
            mTime = minutes * 60 + seconds;
        }

        public void SetTime(int minutes)
        {
            SetTime(minutes, 0);
        }

        public void SetStopTime(int minutes, int seconds)
        {
            mStopTime = minutes * 60 + seconds;
        }

        public void SetStopTime(int minutes)
        {
            SetStopTime(minutes, 0);
        }

        public void DecreaseTime()
        {
            if (!IsStopTime())
            {
                mTime--;
            }
        }

        private bool IsStopTime()
        {
            if(CountDown)
            {
                return mTime <= mStopTime;
            }
            else
            {
                if(mStopTime != 0)
                {
                    return mTime >= mStopTime;

                }
            }
            return false;
        }

        public void IncreaseTime()
        {
            if (!IsStopTime())
            {
                mTime++;
            }
        }

        public void TickClock()
        {
            if (CountDown)
            {
                DecreaseTime();
            } 
            else
            {
                IncreaseTime();
            }
        }

        public bool IsZero()
        {
            return Minutes == 0 && Seconds == 0;
        }

        public string GetFormatedTime()
        {
            string seconds = Seconds.ToString();
            if(Seconds < 10)
            {
                seconds = "0" + seconds;
            }
            return Minutes + ":" + seconds;
        }
    }
}