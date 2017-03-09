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
using Android.Util;

namespace TimeGear
{
    public class NumberPreference : Preference, NumberPickerDialog.Callback
    {
        private const int LAYOUT = Resource.Layout.NumberEditTextPreference;
        private const int MIN_VALUE = 2;
        private const int MAX_VALUE = 60;

        //TODO dodaæ sprawdzanie poprawnoœci danych
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int Value { get; set; }

        private ISharedPreferences mSharedPreferences;
        private TextView mValueTextView;

        public NumberPreference(Context context) : base(context)
        {
            Init(context);
        }

        public NumberPreference(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        public NumberPreference(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            MinValue = MIN_VALUE;
            MaxValue = MAX_VALUE;
            mSharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context);
            Value = mSharedPreferences.GetInt(Key, MinValue);
            if(Value <= MinValue)
            {
                Value = MinValue;
            }
            LayoutResource = Resource.Layout.NumberEditTextPreference;
        }

        protected override void OnClick()
        {
            Value = mSharedPreferences.GetInt(Key, MinValue);
            NumberPickerDialog dialog = new NumberPickerDialog(MinValue, MaxValue, Value, Context);
            dialog.SetNumberPickerValues(MinValue, MaxValue, Value);
            dialog.SetCallback(this);
            dialog.Show();
        }

        protected override void OnBindView(View view)
        {
            base.OnBindView(view);
            mValueTextView = view.FindViewById<TextView>(Resource.Id.preference_value);
            if (mValueTextView != null)
            {
                mValueTextView.Text = Value.ToString();
            }
        }

        public void OnValueChange(int value)
        {
            Value = value;
            SavePreference(value);
            mValueTextView.Text = value.ToString();
        }

        private void SavePreference(int value)
        {
            ISharedPreferencesEditor editor = mSharedPreferences.Edit();
            editor.PutInt(Key, value);
            editor.Apply();
        }

        
    }
    internal class NumberPickerDialog : Dialog
    {
        private LinearLayout mLayout;
        private NumberPicker mNumberPicker;
        private Button mDoneButton;

        private Callback mCallback;

        public interface Callback
        {
            void OnValueChange(int value);
        }

        public void SetCallback(Callback callback)
        {
            mCallback = callback;
        }

        public NumberPickerDialog(int minValue, int maxValue, int value, Context context) : base(context)
        {
            /*mNumberPicker.MinValue = minValue;
            mNumberPicker.MaxValue = maxValue;
            mNumberPicker.Value = value;*/
            InitDialog(minValue, maxValue, value, context);
        }

        private void InitDialog(int minValue, int maxValue, int value, Context context)
        {
            View view = CreateDialogView(context);
            //SetNumberPickerValues(minValue, maxValue, value);
            SetContentView(view);
            SetListeners();
        }
        public View CreateDialogView(Context context)
        {
            mLayout = new LinearLayout(context);
            mLayout.Orientation = Orientation.Vertical;

            mNumberPicker = CreateNumberPicker(context);
            mDoneButton = CreateDoneButton(context);

            mLayout.AddView(mNumberPicker);
            mLayout.AddView(mDoneButton);

            return mLayout;
        }


        private NumberPicker CreateNumberPicker(Context context)
        {
            NumberPicker numberPicker = new NumberPicker(context);
            return numberPicker;
        }

        private Button CreateDoneButton(Context context)
        {
            Button doneButton = new Button(context, null, Android.Resource.Attribute.BorderlessButtonStyle);
            doneButton.SetWidth(WindowManagerLayoutParams.MatchParent);
            doneButton.Gravity = GravityFlags.CenterHorizontal;
            doneButton.Text = context.GetString(Android.Resource.String.Ok);
            return doneButton;
        }

        public void SetNumberPickerValues(int minValue, int maxValue, int value)
        {
            mNumberPicker.MinValue = minValue;
            mNumberPicker.MaxValue = maxValue;
            mNumberPicker.Value = value;
        }
        
        public void SetListeners()
        {
            mDoneButton.Click += delegate
            {
                if (mCallback != null)
                {
                    mCallback.OnValueChange(mNumberPicker.Value);
                }
                Dismiss();
            };
        }

        private void OnButtonClick()
        {
            if(mCallback != null)
                {
                mCallback.OnValueChange(mNumberPicker.Value);
            }
            Dismiss();
        }
    }
}