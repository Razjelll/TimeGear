using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Runtime;
using System;
using TimeGear.Pomodoro.AndroidUtils;
using TimeGear.Pomodoro;
using Android.Util;

namespace TimeGear
{
    [Activity(Label = "TimeGear", MainLauncher = true, Icon = "@drawable/icon")]
    public class TimerActivity : Activity, TimerService.Callback, PomodoroCancelAlertDialog.Callback
    {
        private const int PREFERENCES_REQUEST = 9892;

        private const int ENTIRE_PROGRESS_LAYOUT = Resource.Layout.EntireProgressBar;
        private const int STAGE_PROGRESS_LAYOUT = Resource.Layout.StageProgress;
        private const int STAGE_NAME_LAYOUT = Resource.Layout.StageNameText;
        private const int CLOCK_TEXT_LAYOUT = Resource.Layout.ClockText;
        private const int START_BUTTON_LAYOUT = Resource.Layout.StartButton;

        private const int ENTIRE_PROGRESS_RESOURCE_ID = Resource.Id.entire_progress;
        private const int STAGE_PROGRESS_RESOURCE_ID = Resource.Id.stage_progress;
        private const int STAGE_NAME_RESOURCE_ID = Resource.Id.stage_name_text;
        private const int CLOCK_TEXT_RESOURCE_ID = Resource.Id.clock;
        private const int START_BUTTON_RESOURCE_ID = Resource.Id.start_button;

        private LinearLayout mLayout;
        private ProgressBar mEntireProgressBar;
        private ProgressBar mStageProgressBar;
        private TextView mStageNameTextView;
        private TextView mClockTextView;
        private Button mStartButton;

        private PomodoroManager mPomodoroManager;
        private TimerServiceConnection mServiceConnection;

        private bool mIsConfigurationChange = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TimerActivity);
            Init();

            mServiceConnection = LastNonConfigurationInstance as TimerServiceConnection;
           /* if(mServiceConnection != null)
            {
                mBinder = mServiceConnection.Binder;
            } */
            if(mPomodoroManager == null)
            {
                int numberIntervals = Preferences.GetNumberIntervals(BaseContext);
                mPomodoroManager = new PomodoroManager(numberIntervals);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if(mServiceConnection == null)
            {
                BindTimerService();
            }
            else
            {
                mServiceConnection.Service.SetCallback(this);
            }
            UpdateStateUI();
        }

        private void UpdateStateUI()
        {
            PomodoroManager.State state = mPomodoroManager.CurrentState;
            Log.Debug("TimerActivity", "State = " + state);
            if(mStartButton != null)
            {
                mStartButton.Text = PomodoroTexts.GetButtonText(state, BaseContext);
            }
            if(mStageNameTextView != null)
            {
                mStageNameTextView.Text = PomodoroTexts.GetStateName(state, BaseContext);
            }
        }

        //TODO obejrzeć
        protected override void OnPause()
        {
            base.OnPause();
            if(mServiceConnection.Service != null)
            {
                //mBinder.GetService().SetCallback(null);
                mServiceConnection.Service.SetCallback(null);
                if(mIsConfigurationChange)
                {
                    UnbindService(mServiceConnection);
                }
            }
            //mServiceConnection.SetCallback(null);
        }

        private void BindTimerService()
        {
            if(mServiceConnection == null || mServiceConnection.Service == null)
            {
                Intent intent = new Intent(this, typeof(TimerService));
                mServiceConnection = new TimerServiceConnection(this);
                BindService(intent, mServiceConnection, Bind.AutoCreate);
            }
            else
            {
                mServiceConnection.Service.SetCallback(this);
            }
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            base.OnRetainNonConfigurationInstance();
            mIsConfigurationChange = true;
            return mServiceConnection;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(requestCode == PREFERENCES_REQUEST)
            {
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            mLayout.RemoveAllViews();
            InitControls();
        }

        private void Init()
        {
            if (Preferences.IsShowToolbar(BaseContext))
            {
                SetupToolbar();
            }

            InitControls();
        }

        private void SetupToolbar() 
        {
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetCustomView(Resource.Layout.Toolbar);
            ActionBar.SetDisplayShowCustomEnabled(true);
            ImageView settingsButton = ActionBar.CustomView.FindViewById<ImageView>(Resource.Id.toolbar_settings);
            settingsButton.Click += delegate
            {
                StartPreferencesActivity();
            };
        }

        private void StartPreferencesActivity()
        {
            Intent intent = new Intent(this, typeof(PreferencesActivity));
            StartActivityForResult(intent, PREFERENCES_REQUEST);
        }


        private void InitControls()
        {
            mLayout = FindViewById<LinearLayout>(Resource.Id.timer_activity_layout);

            LayoutInflater inflater = (LayoutInflater)this.GetSystemService(Context.LayoutInflaterService);
            if (Preferences.IsShowEntireProgress(BaseContext))
            {
                InitEntireProgressBar(inflater, mLayout);
            } 
            if (Preferences.IsShowStageProgress(BaseContext))
            {
                InitStageProgressBar(inflater, mLayout);
            }
            if (Preferences.IsShowStageName(BaseContext))
            {
                InitStageNameTextView(inflater, mLayout);
            }
            InitClockTextView(inflater, mLayout);
            if (Preferences.IsShowStartButton(BaseContext))
            {
                InitStartButton(inflater, mLayout);
            }
        }

        private void InitEntireProgressBar(LayoutInflater inflater, ViewGroup viewGroup)
        {
            var view = inflater.Inflate(ENTIRE_PROGRESS_LAYOUT, viewGroup);
            mEntireProgressBar = view.FindViewById<ProgressBar>(ENTIRE_PROGRESS_RESOURCE_ID);
        }

        private void InitStageProgressBar(LayoutInflater inflater, ViewGroup viewGroup)
        {
            var view = inflater.Inflate(STAGE_PROGRESS_LAYOUT, viewGroup);
            mStageProgressBar = view.FindViewById<ProgressBar>(STAGE_PROGRESS_RESOURCE_ID);
        }

        private void InitStageNameTextView(LayoutInflater inflater, ViewGroup viewGroup)
        {
            var view = inflater.Inflate(STAGE_NAME_LAYOUT, viewGroup);
            mStageNameTextView = view.FindViewById<TextView>(STAGE_NAME_RESOURCE_ID);
        }

        private void InitClockTextView(LayoutInflater inflater, ViewGroup viewGroup)
        {
            var view = inflater.Inflate(CLOCK_TEXT_LAYOUT, viewGroup);
            mClockTextView = view.FindViewById<TextView>(CLOCK_TEXT_RESOURCE_ID);
            mClockTextView.Text = "00:00";
        }

        private void InitStartButton(LayoutInflater inflater, ViewGroup viewGroup)
        {
            var view = inflater.Inflate(START_BUTTON_LAYOUT, viewGroup);
            mStartButton = view.FindViewById<Button>(START_BUTTON_RESOURCE_ID);
            mStartButton.Click += delegate
            {
                StartTimer();
            };
        }

        private void StartTimer()
        {
            mPomodoroManager.FinishState();
            PomodoroManager.State state = mPomodoroManager.CurrentState;
            UpdateStateUI();
            int time = PomodoroTime.GetTime(state, BaseContext);
            mServiceConnection.Service.StartTimer(time);
        }
    
        //TimerService
        public void OnTimerTick(string time)
        {
            mClockTextView.Text = time;
        }

        public void OnTimerComplete()
        {
            //TODO 
        }

        public void OnTimerCancel()
        {
            //TODO
        } 

        public void OnSkipWork()
        {
            //mServiceConnection.Service.StopTimer();
            
        }

        public void OnCancelWork()
        {
            //mServiceConnection.Service.StopTimer();
        }

        public void OnServiceConnected(TimerServiceBinder binder)
        {
           /* mBinder = binder;
            if(mServiceConnection.Service != null)
            {
                mServiceConnection.Service.SetCallback(this);
            }*/

        }

        public void OnSkipBreak()
        {
            throw new NotImplementedException();
        }

        
    }

    internal class TimerServiceConnection : Java.Lang.Object, IServiceConnection
    {

        private TimerActivity mActivity;
        public TimerServiceBinder Binder { get; private set; }
        public TimerService Service
        {
            get
            {
                if (Binder != null)
                {
                    return Binder.GetService();
                }
                return null;
            }
        }

        public TimerServiceConnection(TimerActivity activity)
        {
            mActivity = activity;
        }
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            Binder = service as TimerServiceBinder;
            if (Binder != null || Service != null)
            {
                Service.SetCallback(mActivity);
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            Service.SetCallback(null);
            Binder = null;
            
        }
    }

    internal class PomodoroCancelAlertDialog : AlertDialog
    {
        private PomodoroManager.State mCurrentState;
        private Context mContext;
        private Callback mCallback;

        public interface Callback
        {
            void OnSkipWork();
            void OnCancelWork();
            void OnSkipBreak();
        }

        public PomodoroCancelAlertDialog(Context context, PomodoroManager.State state) : base(context)
        {
            mCurrentState = state;
            mContext = context;
            if(mCurrentState == PomodoroManager.State.WORK || mCurrentState == PomodoroManager.State.BEFORE_WORK)
            {
                PrepareWorkAlertDialog();
            }
            else
            {
                PrepareBreakAlertDialog();
            }
        }

        public void SetCallback(Callback callback)
        {
            mCallback = callback;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        private void PrepareWorkAlertDialog()
        {

        }

        private void PrepareBreakAlertDialog()
        {
            
        }


    }
}

