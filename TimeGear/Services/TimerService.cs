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

namespace TimeGear
{
    [Service]
    public class TimerService : Service
    {
        public Callback mCallback;
        private TimerServiceBinder mBinder;
        private TimerHandler mTimerHandler = new TimerHandler();

           
        public bool TimerRunning { get; private set; }

        public TimerService()
        {
            mTimerHandler.SetServiceCallback(this);
            TimerRunning = false;
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

        public void StartTimer(int timeInMinutes)
        {
            StartTimer(timeInMinutes, 0);
        }

        public void StartTimer(int minutes, int seconds)
        {
            if(mTimerHandler != null)
            {
                mTimerHandler.SetTime(minutes, seconds);
                mTimerHandler.SendEmptyMessage(TimerHandler.START_TIMER);
            }
            TimerRunning = true;
        }

        public void StopTimer()
        {
            if(mTimerHandler != null)
            {
                mTimerHandler.SendEmptyMessage(TimerHandler.STOP_TIMER);
            }
            TimerRunning = false;
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
        TimerService mService;

        public TimerServiceBinder(TimerService service)
        {
            mService = service;
        }

        public TimerService GetService()
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

        public bool TimerRunning;
        private TimerClock mClock;
        private TimerService mTimerService;

        public TimerHandler()
        {
            mClock = new TimerClock();
        }

        public void SetTime(int minutes, int seconds)
        {
            mClock.SetTime(minutes, seconds);
        }

        internal void SetServiceCallback(TimerService service)
        {
            mTimerService = service;
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
                    StopTimer();
                    break;
                case TICK_TIMER:
                    OnTickTimer();
                    break;
            }
        }

        private void StartTimer()
        {
            TimerRunning = true;
            SendEmptyMessageDelayed(TICK_TIMER, ONE_SECONDS);
        }

        private void OnTickTimer()
        {
            if (TimerRunning)
            {
                
                if(!mClock.IsZero())
                {
                    mClock.TickClock();
                    mTimerService.OnTimerTick(mClock.GetFormatedTime());
                    SendEmptyMessageDelayed(TICK_TIMER, ONE_SECONDS);
                }
                else
                {
                    StopTimer();
                    //mTimerService.OnTimerComplete();
                }
            }
        }

        private void StopTimer()
        { 
            TimerRunning = false;
        }

    }
}