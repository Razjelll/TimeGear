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

namespace TimeGear.Pomodoro.AndroidUtils
{
    public class PomodoroParamCreator
    {
        public static PomodoroParam Create(Context context)
        {
            int WorkTime = Preferences.GetWorkTime(context);
            return new PomodoroParam
            {
                WorkTime = Preferences.GetWorkTime(context),
                ShortBreakTime = Preferences.GetShortBreakTime(context),
                LongBreakTime = Preferences.GetLongBreakTime(context),
                NumberIntervals = Preferences.GetNumberIntervals(context)
            };
        }
    }
}