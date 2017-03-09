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
using Android.Util;

namespace TimeGear.Pomodoro
{
    public class Pomodoro
    {
        public enum State
        {
            BEFORE_WORK,
            WORK,
            CONTINUATION_WORK,
            BEFORE_SHORT_BREAK,
            SHORT_BREAK,
            CONTINUATION_SHORT_BREAK,
            BEFORE_LONG_BREAK,
            LONG_BREAK,
            CONTINUATION_LONG_BREAK
        }

        private const int FIRST_INTERVAL = 1;

        public State CurrentState { get; private set; }
        public PomodoroTimer Timer { get; private set; }

        private int mWorkTime;
        private int mShortBreakTime;
        private int mLongBreakTime;
        private int mNumberIntervals;

        public int CurrentInterval { get; private set; }
        public SimpleTime StateTime { get; private set; }
        private int mAddedTime;
        
        public Pomodoro()
        {
            CurrentState = State.BEFORE_WORK;
            CurrentInterval = FIRST_INTERVAL;
            Timer = new PomodoroTimer();
        }

        public Pomodoro(PomodoroParam param)
        {
            SetPomodoroParams(param);
        }

        public void SetPomodoroParams(PomodoroParam param)
        {
            mWorkTime = param.WorkTime;
            mShortBreakTime = param.ShortBreakTime;
            mLongBreakTime = param.LongBreakTime;
            mNumberIntervals = param.NumberIntervals;
            //tymczasowe rozwi¹zanie
            if(!Timer.Running)
            {
                Timer.SetTime(GetStateTime(CurrentState));
            }
        }

        public StateResult FinishState()
        {
            Timer.Stop();
            State finishedState = CurrentState;
            //StateTime += GetStateTime(CurrentState); // to raczej jest niepotrzebne
            CurrentState = GetNextState(CurrentState);
            SetInterval();

            Log.Debug("Pomodoro", "FinishState" + CurrentState);
            return PrepareResult(finishedState);
        }

        //TODO zorientowaæ siê czy jest to potrzebne
        public void StartState()
        {
            int time = GetStateTime(CurrentState);
            //StateTime = time;
            //StateTime.SetTime(time);
            Timer.SetTime(time);
            if (CurrentState != State.BEFORE_WORK && CurrentState != State.BEFORE_SHORT_BREAK && CurrentState != State.BEFORE_LONG_BREAK)
            {
                
                Timer.Start();
            }
        }
        
        private StateResult PrepareResult(State state)
        {
            StateResult result = null;
            if(state != State.BEFORE_WORK && state != State.BEFORE_SHORT_BREAK && state != State.BEFORE_LONG_BREAK)
            {
                //TODO zwróciæ odpowiedni stan
                result = new StateResult()
                {
                    Time = GetStateDuration(state),
                    State = state,
                    Date = DateTime.Now
                };
            }
            return result;
        }

        private State GetNextState(State state)
        {
            switch(state)
            {
                case State.BEFORE_WORK:
                    return State.WORK;
                case State.WORK:
                    return GetBreakState();
                case State.CONTINUATION_WORK:
                    return GetBreakState();
                case State.BEFORE_SHORT_BREAK:
                    return State.SHORT_BREAK;
                case State.SHORT_BREAK:
                case State.CONTINUATION_SHORT_BREAK:
                    return State.BEFORE_WORK;
                case State.BEFORE_LONG_BREAK:
                    return State.LONG_BREAK;
                case State.LONG_BREAK:
                case State.CONTINUATION_LONG_BREAK:
                    return State.BEFORE_WORK;
            }
            return State.BEFORE_WORK;
        }

        private State GetBreakState()
        {
            if (CurrentInterval == mNumberIntervals)
            {
                return State.BEFORE_LONG_BREAK;
            }
            else
            {
                return State.BEFORE_SHORT_BREAK;
            }
        }

        private void SetInterval()
        {
            if (CurrentState == State.BEFORE_WORK)
            {
                if(CurrentInterval >= mNumberIntervals)
                {
                    CurrentInterval = FIRST_INTERVAL;
                }
                else
                {
                    CurrentInterval++;
                }
            }
        }

        private int GetStateDuration(State state)
        {
            switch(state)
            {
                case State.WORK:
                    return mWorkTime;
                case State.SHORT_BREAK:
                    return mShortBreakTime;
                case State.LONG_BREAK:
                    return mLongBreakTime;
                case State.CONTINUATION_WORK:
                case State.CONTINUATION_SHORT_BREAK:
                case State.CONTINUATION_LONG_BREAK:
                    return Timer.Minutes;
                default:
                    return 0;
            }
        }    

        private bool IsTimedState(State state)
        {
            switch(state)
            {
                case State.WORK:
                case State.CONTINUATION_WORK:
                case State.SHORT_BREAK:
                case State.CONTINUATION_SHORT_BREAK:
                case State.LONG_BREAK:
                case State.CONTINUATION_LONG_BREAK:
                    return true;
                default:
                    return true;
            }
        }

        //TODO pomyœleæ nad nazw¹
        private int GetStateTime(State state)
        {
            switch(state)
            {
                case State.BEFORE_WORK:
                case State.WORK:
                    return mWorkTime;
                case State.BEFORE_SHORT_BREAK:
                case State.SHORT_BREAK:
                    return mShortBreakTime;
                case State.BEFORE_LONG_BREAK:
                case State.LONG_BREAK:
                    return mLongBreakTime;
                default:
                    return 0;

            }
        }

        public void SkipState()
        {
            Timer.Stop();
            State skipedState = CurrentState;
            CurrentState = GetNextStateAfterSkip(CurrentState);
            SetInterval();
        }

        public void CancelState()
        {
            Timer.Stop();
        }

        private State GetNextStateAfterSkip(State state)
        {
            switch(state)
            {
                case State.BEFORE_WORK:
                case State.WORK:
                case State.CONTINUATION_WORK:
                    return GetBreakState();
                default:
                    return State.BEFORE_WORK;
            }
        }
    }
}