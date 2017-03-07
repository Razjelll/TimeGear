using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TimeGearTests
{
    /// <summary>
    /// Summary description for PomodoroManager
    /// </summary>
    [TestClass]
    public class PomodoroManagerTest
    {
        private const int NUMBER_INTERVALS = 3;
        private const int WORK_TIME = 25;
        private const int WORK_TIME_2 = 20;
        private const int SHORT_BREAK_TIME = 5;
        private const int SHORT_BREAK_TIME_2 = 10;
        private const int LONG_BREAK_TIME = 15;
        private const int LONG_BREAK_TIME_2 = 20;
        public PomodoroManagerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Test sprawdzający czy początkowy stan i interwał są odpowiedznie oraz sprawdza, czy po zakończeniu
        /// stanu NONE zostanie otrzymany stan WORK
        /// </summary>
        [TestMethod]
        public void FinishBeforeWorkStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.WORK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
           
        }

        /// <summary>
        /// Test sprawdzający czy po zakończeniu dwóch stanów otrzymamy prawidłowy stan
        /// </summary>
        [TestMethod]
        public void FinishFirstWorkStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_SHORT_BREAK, pomodoro.CurrentState);// powiniśmy otrzymać stan krótkiej przerwy
            Assert.AreEqual(1, pomodoro.CurrentInterval); //nadal powinin być pierwszy interwał
            Assert.AreEqual(pomodoro.SessionWorkTime, WORK_TIME);
        }

        [TestMethod]
        public void FinishBeforeShortBreakStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.FinishState(); //kończymy stan BEFORE_SHORT_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.SHORT_BREAK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
        }

        [TestMethod]
        public void FinishShortBreakStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.ShortBreakTime = SHORT_BREAK_TIME;
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.FinishState(); //kończymy stan BEFORE_SHORT_BREAK
            pomodoro.FinishState(); //kończymy stan SHORT_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
            Assert.AreEqual(2, pomodoro.CurrentInterval);
            Assert.AreEqual(pomodoro.SessionShortBreakTime, SHORT_BREAK_TIME);
            Assert.AreEqual(pomodoro.SessionTime, WORK_TIME + SHORT_BREAK_TIME);
        }


        [TestMethod]
        public void FinishStateAndStarBeforetLongBreakTest()
        {
            //ustawiamy interwał długiej przerwy więc powinna zostać uruchomiona zaraz po zakończeniu pierwszej pracy
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(1);
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_LONG_BREAK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
        }

        [TestMethod]
        public void FinishBeforeLongBreakStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(1);
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.FinishState(); //kończymy stan BEFORE_LONG_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.LONG_BREAK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
        }

        [TestMethod]
        public void FinishLongBreakStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(1);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.LongBreakTime = LONG_BREAK_TIME;
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.FinishState(); //kończymy stan BEFORE_LONG_BREAK
            pomodoro.FinishState(); //kończymy stan LONG_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
            Assert.AreEqual(2, pomodoro.SessionInterval);
            Assert.AreEqual(pomodoro.SessionLongBreakTime, LONG_BREAK_TIME);
            Assert.AreEqual(pomodoro.SessionTime, WORK_TIME + LONG_BREAK_TIME);
        }

        [TestMethod]
        public void FinishCycleTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(2);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.ShortBreakTime = SHORT_BREAK_TIME;
            pomodoro.LongBreakTime = LONG_BREAK_TIME;
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_SHORT_BREAK, pomodoro.CurrentState);
            pomodoro.FinishState();// kończymy stan BEFORE_SHORT_BREAk
            Assert.AreEqual(TimeGear.PomodoroManager.State.SHORT_BREAK, pomodoro.CurrentState);
            pomodoro.FinishState();//kończymy stan SHORT_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.WorkTime = WORK_TIME_2; //zmieniamy czas pracy
            Assert.AreEqual(TimeGear.PomodoroManager.State.WORK, pomodoro.CurrentState);
            pomodoro.FinishState(); //kończymy stan WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_LONG_BREAK, pomodoro.CurrentState);
            pomodoro.FinishState(); //kończymy stan BEFORE_LONG_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.LONG_BREAK, pomodoro.CurrentState);
            pomodoro.FinishState(); //kończymy stan LONG_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
            Assert.AreEqual(3, pomodoro.SessionInterval);
            Assert.AreEqual(WORK_TIME + WORK_TIME_2, pomodoro.SessionWorkTime);
            Assert.AreEqual(SHORT_BREAK_TIME, pomodoro.SessionShortBreakTime);
            Assert.AreEqual(LONG_BREAK_TIME, pomodoro.SessionLongBreakTime);
            Assert.AreEqual(WORK_TIME +WORK_TIME_2 + SHORT_BREAK_TIME + LONG_BREAK_TIME, pomodoro.SessionTime);
        }

        [TestMethod]
        public void FinishStateTimeCounterTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(2);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.ShortBreakTime = SHORT_BREAK_TIME;
            pomodoro.LongBreakTime = LONG_BREAK_TIME;
            FinishEntireCycle(pomodoro);
            FinishEntireCycle(pomodoro);
            Assert.AreEqual(WORK_TIME*4, pomodoro.SessionWorkTime);
            Assert.AreEqual(SHORT_BREAK_TIME*2, pomodoro.SessionShortBreakTime);
            Assert.AreEqual(LONG_BREAK_TIME*2, pomodoro.SessionLongBreakTime);
            int sessionTime = WORK_TIME * 4 + SHORT_BREAK_TIME * 2 + LONG_BREAK_TIME * 2;
            Assert.AreEqual(sessionTime, pomodoro.SessionTime);
            pomodoro.WorkTime = WORK_TIME_2;
            pomodoro.ShortBreakTime = SHORT_BREAK_TIME_2;
            pomodoro.LongBreakTime = LONG_BREAK_TIME_2;
            FinishEntireCycle(pomodoro);
            Assert.AreEqual(WORK_TIME * 4 + WORK_TIME_2 *2, pomodoro.SessionWorkTime);
            Assert.AreEqual(SHORT_BREAK_TIME * 2 + SHORT_BREAK_TIME_2, pomodoro.SessionShortBreakTime);
            Assert.AreEqual(LONG_BREAK_TIME * 2 + LONG_BREAK_TIME_2, pomodoro.SessionLongBreakTime);
            sessionTime += WORK_TIME_2 * 2 + SHORT_BREAK_TIME_2 + LONG_BREAK_TIME_2;
            Assert.AreEqual(sessionTime, pomodoro.SessionTime);

            Assert.AreEqual(1, pomodoro.CurrentInterval);
            Assert.AreEqual(7, pomodoro.SessionInterval);
        }

        private void FinishEntireCycle(TimeGear.PomodoroManager pomodoro)
        {
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.FinishState(); //kończymy stan BEFORE_SHORT_BREAK
            pomodoro.FinishState(); //kończymy stan SHORT_BREAK
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.FinishState(); //kończymy stan BEFORE_LONG_BREAK
            pomodoro.FinishState(); //kończymy stan LONG_BREAK
        }
        /// <summary>
        /// Test sprawdzający zachowanie podczas anulowania stanu pracy. W takim przypadku stan nie powinien się zmienić.
        /// </summary>
        [TestMethod]
        public void CancelWorkStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.WorkTime = 25;
            pomodoro.ShortBreakTime = 5;
            pomodoro.FinishState(); //kończymy stan NONE
            pomodoro.CancelState(); //anulujemy stan WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.WORK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
            Assert.AreEqual(0, pomodoro.SessionWorkTime);
        }

        /// <summary>
        /// Test sprawdzające pomijanie stanu BEFORE_WORK. Pomijanie tego stanu powinno spowodować przejście do stanu BEFORE_SHORT_BREAK lub BEFORE_LONG_BREAK
        /// </summary>
        [TestMethod]
        public void SkipBeforeWorkStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.SkipState(); //pominięcie stanu BEFORE_WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_SHORT_BREAK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
            Assert.AreEqual(1, pomodoro.SessionInterval);
            Assert.AreEqual(0, pomodoro.SessionWorkTime);
        }

        [TestMethod]
        public void SkipWorkStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.FinishState(); //pkończymy stan BEFORE_WORK
            pomodoro.SkipState(); //pominięcie stanu WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_SHORT_BREAK, pomodoro.CurrentState);
            Assert.AreEqual(1, pomodoro.CurrentInterval);
            Assert.AreEqual(1, pomodoro.SessionInterval);
            Assert.AreEqual(0, pomodoro.SessionWorkTime);
        }

        [TestMethod]
        public void SkipBeforeShortBreakStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.SkipState(); //pomijamy stan BEFORE_SHORT_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
            Assert.AreEqual(2, pomodoro.CurrentInterval);
            Assert.AreEqual(1, pomodoro.SessionInterval);
        }

        [TestMethod]
        public void SkipShortBreakStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.ShortBreakTime = SHORT_BREAK_TIME;
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.FinishState(); //kończymy stan BEFORE_SHORT_BREAK
            pomodoro.SkipState(); //pomijamy stan SHORT_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
            Assert.AreEqual(WORK_TIME, pomodoro.SessionWorkTime);
            Assert.AreEqual(0, pomodoro.SessionShortBreakTime);
            Assert.AreEqual(WORK_TIME, pomodoro.SessionTime);
        }

        [TestMethod]
        public void SkipBeforeLongBreakStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(1);
            pomodoro.FinishState(); //konczymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_LONG_BREAK, pomodoro.CurrentState);
            pomodoro.SkipState(); //pomijamy stan BEFORE_LONG_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
        }

        [TestMethod]
        public void SkipLongBreakStateTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(1);
            pomodoro.FinishState(); //konczymy stan BEFORE_WORK
            pomodoro.FinishState(); //kończymy stan WORK
            pomodoro.FinishState(); //kończymy stan BEFORE_LONG_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.LONG_BREAK, pomodoro.CurrentState);
            pomodoro.SkipState(); //pomijamy stan LONG_BREAK
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_WORK, pomodoro.CurrentState);
        }

        [TestMethod]
        public void AddSessionTimeTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(NUMBER_INTERVALS);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK i rozpoczynamy prace
            Assert.AreEqual(TimeGear.PomodoroManager.State.WORK, pomodoro.CurrentState);
            pomodoro.AddSessionTime(15);
            pomodoro.SkipState();
            
            Assert.AreEqual(15, pomodoro.SessionWorkTime);
            pomodoro.SkipState(); //pomijamy stan BEFORE_SHORT_BREAK
            pomodoro.FinishState(); //kończymy stan BEFORE_WORK
            Assert.AreEqual(TimeGear.PomodoroManager.State.WORK, pomodoro.CurrentState);
            pomodoro.FinishState(); //kończymy stan WORK
            Assert.AreEqual(WORK_TIME + 15, pomodoro.SessionWorkTime);
        }

        [TestMethod]
        public void StartLongBreakTest()
        {
            TimeGear.PomodoroManager pomodoro = new TimeGear.PomodoroManager(2);
            pomodoro.WorkTime = WORK_TIME;
            pomodoro.ShortBreakTime = SHORT_BREAK_TIME;
            pomodoro.LongBreakTime = LONG_BREAK_TIME;
            pomodoro.StartLongBreak(); //przechodzimy do długiej przerwy zaraz na początku cyklu
            Assert.AreEqual(TimeGear.PomodoroManager.State.BEFORE_LONG_BREAK, pomodoro.CurrentState);
            Assert.AreEqual(2, pomodoro.CurrentInterval);
        }
        
    }
}
