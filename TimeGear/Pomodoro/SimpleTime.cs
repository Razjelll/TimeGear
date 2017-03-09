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
    public class SimpleTime
    {
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public SimpleTime(int minutes, int seconds)
        {
            SetTime(minutes, seconds);
        }

        public SimpleTime(int minutes)
        {
            SetTime(minutes);
        }

        public void SetTime(int minutes, int seconds)
        {
            if(minutes >= 0)
            {
                Minutes = minutes;
            }
            if(seconds >=0 && seconds < 60)
            {
                Seconds = seconds;
            }
        }

        public void SetTime(int minutes)
        {
            SetTime(minutes, 0);
        }

        public override string ToString()
        {
            string seconds = Seconds.ToString();
            if(Seconds < 10)
            {
                seconds = "0" + seconds;
            }
            return Minutes + ":" + seconds;
        }

        public  bool Equals(SimpleTime obj)
        {
            return Minutes == obj.Minutes && Seconds == obj.Seconds;
        }
    }
}