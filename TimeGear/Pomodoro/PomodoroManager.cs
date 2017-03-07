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
    public class PomodoroManager
    {
        private const int FIRST_INTERVAL = 1;
        /// <summary>
        /// Enumeracja okre�laj�ca mo�liwe stany.
        /// </summary>
        public enum State
        {
            BEFORE_WORK,
            WORK, 
            BEFORE_SHORT_BREAK,
            SHORT_BREAK,
            BEFORE_LONG_BREAK,
            LONG_BREAK
        }

        /// <summary>
        /// Liczba interwa��w, po kt�rych rozpocznie si� nowy cykl. Interwa� jest dodawany po sko�czeniu przerwy. 
        /// </summary>
        public int IntervalsToLongBreak { private get; set; }
        /// <summary>
        /// Numer obecnego interwa�u. Obecny interwa� jest liczb� pomi�dzy 0 a LongBreakIntervals. Po przekroczeniu tego zakresu zostaje resetowany
        /// </summary>
        public int CurrentInterval { get; private set; }
        /// <summary>
        /// Liczba interwa��w liczona od momentu uruchomienia aplikacji
        /// </summary>
        public int SessionInterval { get; private set; }
        /// <summary>
        /// Czas w minutach, kt�ry up�yn�� od momentu uruchomienia aplikacji
        /// </summary>
        public int SessionTime { get; private set; }
        /// <summary>
        /// Czas pracy. Je�li zostanie ustawiony b�dzie dodawany do czasu pracy w sesji po zako�czeniu stany pracy
        /// </summary>
        public int WorkTime {get; set; }
        /// <summary>
        /// Czas d�ugiej przerwy. Je�li zostanie ustawiony b�dzie dodawany do czasu d�ugiej przerwy po zako�czeniu stanu d�ugiej przerwy
        /// </summary>
        public int LongBreakTime {get; set; }
        /// <summary>
        /// Czas kr�tkiej przerwy. Po zako�czeniu stanu kr�tkiej przerwy zostanie dodany do czasu kr�tkiej przerwy w sessji.
        /// </summary>
        public int ShortBreakTime {get; set; }

        public int SessionWorkTime { get; private set; }
        public int SessionLongBreakTime { get; private set; }
        public int SessionShortBreakTime { get; private set; }
        
        /// <summary>
        /// Obecny stan pomodoro. 
        /// </summary>
        public State CurrentState { get; private set; }
        /// <summary>
        /// Stan kt�ry nast�pi po zako�czeniu obecnego stanu.
        /// </summary>
        //public State NextState { get; private set; }

        public PomodoroManager(int cycleIntervals)
        {
            Initialize(cycleIntervals);
        }

        private void Initialize(int cycleIntervals)
        {
            CurrentState = State.BEFORE_WORK;
            //NextState = State.WORK;
            CurrentInterval = FIRST_INTERVAL;
            SessionInterval = FIRST_INTERVAL;
            IntervalsToLongBreak = cycleIntervals;
        }

        public void FinishState()
        {
            AddTime(CurrentState);
            CurrentState = GetNextState(CurrentState);
            if(CheckNewInterval())
            {
                SetIntervals(true);
            }
        }

        private void SetIntervals(bool increaseSessionInterval)
        {
           
            if (increaseSessionInterval)
            {
                SessionInterval++;
            }
                
            if (CurrentInterval == IntervalsToLongBreak)
            {
                CurrentInterval = FIRST_INTERVAL;
            }
            else
            {
                CurrentInterval++;
            }
         
        }

        private void AddTime(State state)
        {
            switch(state)
            {
                case State.WORK:
                    AddWorkTime(WorkTime);
                    break;
                case State.SHORT_BREAK:
                    AddShortBreakTime(ShortBreakTime);
                    break;
                case State.LONG_BREAK:
                    AddLongBreakTime(LongBreakTime);
                    break;
            }
        }

        private State GetNextState(State currentState)
        {
            switch (currentState)
            {
                case State.BEFORE_WORK:
                    return State.WORK;
                case State.WORK:
                    return GetStateAfterWork();
                case State.BEFORE_SHORT_BREAK:
                    return State.SHORT_BREAK;
                case State.SHORT_BREAK:
                    return State.BEFORE_WORK;
                case State.BEFORE_LONG_BREAK:
                    return State.LONG_BREAK;
                case State.LONG_BREAK:
                    return State.BEFORE_WORK;
                default:
                    return State.BEFORE_WORK;
            }
        }

        private void AddWorkTime(int time)
        {
            SessionWorkTime += time;
            SessionTime += time;
        }

        private void AddShortBreakTime(int time)
        {
            SessionShortBreakTime += time;
            SessionTime += time;
        }

        private void AddLongBreakTime(int time)
        {
            SessionLongBreakTime += time;
            SessionTime += time;
        }

        public void CancelState()
        {

        }

        public void SkipState()
        {
            CurrentState = GetNextStateAfterSkip(CurrentState);
            if(CheckNewInterval())
            {
                SetIntervals(false);
            }
        }

        private bool CheckNewInterval()
        {
            return CurrentState == State.BEFORE_WORK;
        }

        public void AddSessionTime(int time)
        {
            switch (CurrentState)
            {
                case State.WORK:
                    AddWorkTime(time);
                    break;
                case State.SHORT_BREAK:
                    AddShortBreakTime(time);
                    break;
                case State.LONG_BREAK:
                    AddLongBreakTime(time);
                    break;
            }
        }

        public void StartLongBreak()
        {
            CurrentState = State.BEFORE_LONG_BREAK;
            CurrentInterval = IntervalsToLongBreak;
        }

        private State GetNextStateAfterSkip(State currentState)
        {
            switch (currentState)
            {
                case State.BEFORE_WORK:
                    return GetStateAfterWork();
                case State.WORK:
                    return GetStateAfterWork();
                case State.BEFORE_SHORT_BREAK:
                    return State.BEFORE_WORK;
                case State.SHORT_BREAK:
                    return State.BEFORE_WORK;
                case State.BEFORE_LONG_BREAK:
                    return State.BEFORE_WORK;
                case State.LONG_BREAK:
                    return State.BEFORE_WORK;
                default:
                    return State.BEFORE_WORK;
            }
        }

        

        private State GetStateAfterWork()
        {
            //sprawdzamy czy obecny interwa� jest o jeden mniejszy od interwa�u w kt�rym powinna wyst�pi� d�uga przerwa
            //robimy tak, poniewa� to sprawdzanie odbedzie si� przed zmian� stanu na przerw�,
            //poniewa� pobieramy stan kt�ry nast�pi po kolejnej zmianie
            if(CurrentInterval == IntervalsToLongBreak)
            {
                return State.BEFORE_LONG_BREAK;
            }
            return State.BEFORE_SHORT_BREAK;
        }

    }
}