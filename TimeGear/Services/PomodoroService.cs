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
using TimeGear.Pomodoro.AndroidUtils;

namespace TimeGear
{
    [Service]
    public class PomodoroService : Service
    {
        public Callback mCallback;
        private TimerServiceBinder mBinder;
        private TimerHandler mTimerHandler = new TimerHandler();

        internal Pomodoro.Pomodoro mPomodoro;

        public bool TimerRunning
        {
            get
            {
                return mPomodoro.Timer.Running;
            }
        }
        public string TimerTime
        {
            get
            {
                return mPomodoro.Timer.FormatedTime;
            }
        }

        public Pomodoro.Pomodoro.State State
        {
            get
            {
               return mPomodoro.CurrentState;
            }
        }

        public PomodoroService() :base()
        {
            mPomodoro = new Pomodoro.Pomodoro();
            mTimerHandler.SetServiceCallback(this);
            Context context = BaseContext;
        }

        public void UpdatePomodoroParams()
        {
            mPomodoro.SetPomodoroParams(PomodoroParamCreator.Create(this));
        }

        public override IBinder OnBind(Intent intent)
        {
            mBinder = new TimerServiceBinder(this);
            return mBinder;
        }

        public void SetCallback(Callback callback)
        {
            mCallback = callback;
            mTimerHandler.SendEmptyMessage(TimerHandler.SET_CALLBACK);
        }

        public void StartTimer()
        {
            if(mPomodoro == null)
            {
                mPomodoro = new Pomodoro.Pomodoro();
                UpdatePomodoroParams();
            }
            mPomodoro.FinishState(); //koñczymy poprzedni stan
            mPomodoro.StartState(); //zaczynamy nowy stan
            if(mTimerHandler != null)
            {
                mTimerHandler.SendEmptyMessage(TimerHandler.START_TIMER);
            }
        }

        /*public void StartTimer(int minutes, int seconds)
        {
            if(mTimerHandler != null)
            {
                mTimerHandler.SetTime(minutes, seconds);
                mTimerHandler.SendEmptyMessage(TimerHandler.START_TIMER);
            }
            TimerRunning = true;
        }*/

        public void StopTimer()
        {
            mPomodoro.FinishState();
            if (mTimerHandler != null)
            {
                mTimerHandler.SendEmptyMessage(TimerHandler.STOP_TIMER);
            }
        }

        public void Skip()
        {
            mPomodoro.SkipState();
            if(mTimerHandler != null)
            {
                mTimerHandler.SendEmptyMessage(TimerHandler.STOP_TIMER);
            }
        }

        public void Cancel()
        {
            mPomodoro.CancelState();
            if(mTimerHandler != null)
            {
                mTimerHandler.SendEmptyMessage(TimerHandler.STOP_TIMER);
            }
        }

        internal void OnTimerTick(string time)
        {
            if (mCallback != null)
            {
                mCallback.OnTimerTick(time);
            }
        }

        internal void OnTimerComplete()
        {
            if (mCallback != null)
            {
                mCallback.OnTimerComplete();
            }
        }

        public interface Callback
        {
            void OnTimerTick(string time);
            void OnTimerComplete();

            void OnTimerCancel();
            void OnServiceConnected(TimerServiceBinder binder);
        }

    }


    public class TimerServiceBinder : Binder
    {
        PomodoroService mService;

        public TimerServiceBinder(PomodoroService service)
        {
            mService = service;
        }

        public PomodoroService GetService()
        {
            return mService;
        }
    }

    /*public class TimerServiceConnection : Java.Lang.Object, IServiceConnection
    {
        private TimerService.Callback mCallack;
        public TimerServiceBinder Binder { get; private set; }

        public bool IsBind { get; private set; }

        //TODO przyjrzeæ siê temu
        public TimerService Service
        {
            get
            {
                if(Binder != null)
                {
                    return Binder.GetService();
                }
                else
                {
                    return null;
                }
            }
        }

        /*public void SetCallback(TimerService.Callback callback)
        {
            mCallack = callback;
        }*/

        /*public void OnServiceConnected(ComponentName name, IBinder binder)
        {
            Binder = binder as TimerServiceBinder;
            
            if(Binder != null)
            {
                if(Service.mCallback != null)
                {
                    Service.mCallback.OnServiceConnected(Binder);
                }
            }
            IsBind = true;
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            IsBind = false;
        }
        
    }*/

    internal class TimerHandler : Handler
    {
        private const int ONE_SECONDS = 1000;

        internal const int START_TIMER = 0;
        internal const int STOP_TIMER = 1;
        internal const int TICK_TIMER = 2;
        internal const int SET_CALLBACK = 3;

        private PomodoroService mPomodoroService;
        private int mSemaphore; //pilnuje ¿eby w tym samym czasie móg³ byæ odpalony tylko jeden zegar

        public TimerHandler()
        {
            mSemaphore = 1;
        }

        internal void SetServiceCallback(PomodoroService service)
        {
            mPomodoroService = service;
        }

        public override void HandleMessage(Message msg)
        {
            base.HandleMessage(msg);
            switch (msg.What)
            {
                case START_TIMER:
                    StartTimer();
                    break;
                case STOP_TIMER:

                case TICK_TIMER:
                    OnTickTimer();
                    break;
            }
        }

        private void StartTimer()
        {
            if(mSemaphore==1)
            {
                SendEmptyMessageDelayed(TICK_TIMER, ONE_SECONDS);
                mSemaphore--;
            }
        }

        private void StopTimer()
        {
            mSemaphore++;
        }

        private void OnTickTimer()
        {
            if (mPomodoroService.mPomodoro.Timer.Running)
            {
                mPomodoroService.mPomodoro.Timer.Tick();
                mPomodoroService.OnTimerTick(mPomodoroService.TimerTime);
                SendEmptyMessageDelayed(TICK_TIMER, ONE_SECONDS);
            }
            else
            {
                mSemaphore=1;
            }
            //w przeciwnym wypadku nast¹pi zatrzymanie zegara
        }



    }
}