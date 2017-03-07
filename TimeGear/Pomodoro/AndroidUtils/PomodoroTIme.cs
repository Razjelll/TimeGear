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
    public class PomodoroTime
    {
        public static int GetTime(PomodoroManager.State state, Context context)
        {
            switch(state)
            {
                case PomodoroManager.State.WORK:
                    return Preferences.GetWorkTime(context);
                case PomodoroManager.State.SHORT_BREAK:
                    return Preferences.GetShortBreakTime(context);
                case PomodoroManager.State.LONG_BREAK:
                    return Preferences.GetLongBreakTime(context);
                default:
                    return 0;
            }
        }
    }
}