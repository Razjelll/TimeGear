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
using Android.Preferences;

namespace TimeGear
{
    class Preferences
    {
        //PREFERENCJE ZADANIA
        private static string WORK_TIME = "pref_work_time";
        private static string SHORT_BREAK_TIME = "pref_short_time";
        private static string LONG_BREAK_TIME = "pref_long_time";
        private static string NUMBER_INTERVALS = "pref_number_intervals";

        private static int WORK_TIME_DEFAULT = 1;
        private static int SHORT_BREAK_TIME_DEFAULT = 1;
        private static int LONG_BREAK_TIME_DEFAULT = 1;
        private static int NUMBER_INTERVALS_DEFAULT = 2;

        //PREFERENCJE INTERFEJSU
        private static string ENTIRE_PROGRESS = "pref_entire_progress";
        private static string STAGE_PROGRESS = "pref_stage_progress";
        private static string STAGE_NAME = "pref_stage_name";
        private static string START_BUTTON = "pref_start_button";
        private static string TOOLBAR = "pref_toolbar";
        public static int GetWorkTime(Context context)
        {
            return GetSharedPreferences(context).GetInt(WORK_TIME, WORK_TIME_DEFAULT);
        }

        public static void SetWorkTime(int value, Context context)
        {
            SetIntPreference(WORK_TIME, value, context);
        }

        public static int GetShortBreakTime(Context context)
        {
            return GetSharedPreferences(context).GetInt(SHORT_BREAK_TIME, SHORT_BREAK_TIME_DEFAULT);
        }

        public static void SetShortBreakTIme(int value, Context context)
        {
            SetIntPreference(SHORT_BREAK_TIME, value, context);
        }

        public static int GetLongBreakTime(Context context)
        {
            return GetSharedPreferences(context).GetInt(LONG_BREAK_TIME, LONG_BREAK_TIME_DEFAULT);
        }

        public static void SetLongBreakTime(int value, Context context)
        {
            SetIntPreference(LONG_BREAK_TIME, value, context);
        }

        public static int GetNumberIntervals(Context context)
        {
            return GetSharedPreferences(context).GetInt(NUMBER_INTERVALS, NUMBER_INTERVALS_DEFAULT);
        }

        public static void SetNumberIntervals(int value, Context context)
        {
            SetIntPreference(NUMBER_INTERVALS, value, context);
        }

        public static bool IsShowEntireProgress(Context context)
        {
            return GetSharedPreferences(context).GetBoolean(ENTIRE_PROGRESS, true);
        }

        public static void SetShowEntireProgress(bool value, Context context)
        {
            SetBoolPreference(ENTIRE_PROGRESS, value, context);
        }

        public static bool IsShowStageProgress(Context context)
        {
            return GetSharedPreferences(context).GetBoolean(STAGE_PROGRESS, true);
        }

        public static void SetShowStageProgress(bool value, Context context)
        {
            SetBoolPreference(STAGE_PROGRESS, value, context);
        }

        public static bool IsShowStageName(Context context)
        {
            return GetSharedPreferences(context).GetBoolean(STAGE_NAME, true);
        }

        public static void SetShowStageName(bool value, Context context)
        {
            SetBoolPreference(STAGE_NAME, value, context);
        }

        public static bool IsShowStartButton(Context context)
        {
            return GetSharedPreferences(context).GetBoolean(START_BUTTON, true);
        }

        public static void SetShowStartButton(bool value, Context context)
        {
            SetBoolPreference(START_BUTTON, value, context);
        }

        public static bool IsShowToolbar(Context context)
        {
            return GetSharedPreferences(context).GetBoolean(TOOLBAR, true);
        }

        public static void SetShowToolbar(bool value, Context context)
        {
            SetBoolPreference(TOOLBAR, value, context);
        }

        private static ISharedPreferences GetSharedPreferences(Context context)
        {
            return PreferenceManager.GetDefaultSharedPreferences(context);
        }

        private static void SetIntPreference(string preference, int value, Context context)
        {
            ISharedPreferencesEditor editor = GetSharedPreferences(context).Edit();
            editor.PutInt(preference, value);
            editor.Apply();
        }

        private static void SetBoolPreference(string preference, bool value, Context context)
        {
            ISharedPreferencesEditor editor = GetSharedPreferences(context).Edit();
            editor.PutBoolean(preference, value);
            editor.Apply();
        }
    }
}