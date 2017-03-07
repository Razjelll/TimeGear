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
    public class PomodoroTexts
    {
        public static string GetButtonText(PomodoroManager.State state, Context context)
        {
             switch(state)
             {
                 case PomodoroManager.State.BEFORE_WORK:
                     return context.GetString(Resource.String.StartWork);
                 case PomodoroManager.State.BEFORE_SHORT_BREAK:
                     return context.GetString(Resource.String.StartShortBreak);
                 case PomodoroManager.State.BEFORE_LONG_BREAK:
                     return context.GetString(Resource.String.StartLongBreak);
                 default:
                     return context.GetString(Resource.String.Cancel);
             }
        }

        public static string GetStateName(PomodoroManager.State state, Context context)
        {
            switch(state)
            {
                case PomodoroManager.State.BEFORE_WORK:
                case PomodoroManager.State.WORK:
                    return context.GetString(Resource.String.Work);
                case PomodoroManager.State.BEFORE_SHORT_BREAK:
                case PomodoroManager.State.SHORT_BREAK:
                    return context.GetString(Resource.String.ShortBreak);
                case PomodoroManager.State.BEFORE_LONG_BREAK:
                case PomodoroManager.State.LONG_BREAK:
                    return context.GetString(Resource.String.LongBreak);
            }
            return context.GetString(Resource.String.Lack);
        }
    }
}