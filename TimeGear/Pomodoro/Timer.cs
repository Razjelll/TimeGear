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
    public class Timer
    {
        private const int ONE_SECONDS = 1000;
        System.Timers.Timer mTimer;
        private TimerClock Time;
        private bool mIsStarted;
        private TimerClock mClock;

        public int Minutes
        {
            get
            {
                return mClock.Minutes;
            }
        }

        public int Seconds
        {
            get
            {
                return mClock.Seconds;
            }
        }
        
        public interface Callback
        {
            void OnTick(int minutes, int seconds);
            void OnFinish();
        }

        public Timer(int minutes, int seconds)
        {
            mClock = new TimerClock();
            mClock.SetTime(minutes, seconds);
        }

        private void InitTimer()
        {
            mTimer = new System.Timers.Timer();
            mTimer.Interval = ONE_SECONDS;
            mTimer.Elapsed += OnTimeEvent;
        }

        private void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {

        }

        
    }
}