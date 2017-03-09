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
using static TimeGear.Pomodoro.Pomodoro;

namespace TimeGear.Logic
{
    public class PomodoroTexts
    {
        public static string GetButtonText(Pomodoro.Pomodoro.State state, Context context)
        {
             switch(state)
             {
                 case State.BEFORE_WORK:
                     return context.GetString(Resource.String.StartWork);
                 case State.BEFORE_SHORT_BREAK:
                     return context.GetString(Resource.String.StartShortBreak);
                 case State.BEFORE_LONG_BREAK:
                     return context.GetString(Resource.String.StartLongBreak);
                 default:
                     return context.GetString(Resource.String.Cancel);
             }
        }

        public static string GetStateName(Pomodoro.Pomodoro.State state, Context context)
        {
            switch(state)
            {
                case State.BEFORE_WORK:
                case State.WORK:
                    return context.GetString(Resource.String.Work);
                case State.BEFORE_SHORT_BREAK:
                case State.SHORT_BREAK:
                    return context.GetString(Resource.String.ShortBreak);
                case State.BEFORE_LONG_BREAK:
                case State.LONG_BREAK:
                    return context.GetString(Resource.String.LongBreak);
            }
            return context.GetString(Resource.String.Lack);
        }
    }
}