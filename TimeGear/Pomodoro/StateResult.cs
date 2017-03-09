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
    public class StateResult
    {
        public Pomodoro.State State { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }
    }
}