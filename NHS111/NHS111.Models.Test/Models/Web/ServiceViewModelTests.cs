using System;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using DayOfWeek = System.DayOfWeek;

namespace NHS111.Models.Test.Models.Web
{
    [TestFixture]
    public class ServiceViewModelTests
    {
        private readonly ServiceCareItemRotaSession MONDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession TUESDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession WEDNESDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Wednesday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Wednesday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession THURSDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession FRIDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 30},
            EndTime = new TimeOfDay() {Hours = 17, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession SATURDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
            StartTime = new TimeOfDay() {Hours = 9, Minutes = 0},
            EndTime = new TimeOfDay() {Hours = 18, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession SUNDAY_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
            StartTime = new TimeOfDay() {Hours = 10, Minutes = 0},
            EndTime = new TimeOfDay() {Hours = 16, Minutes = 0}
        };

        private readonly ServiceCareItemRotaSession THURSDAY_MORNING_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            StartTime = new TimeOfDay() { Hours = 8, Minutes = 30 },
            EndTime = new TimeOfDay() { Hours = 11, Minutes = 0 }
        };

        private readonly ServiceCareItemRotaSession THURSDAY_AFTERNOON_SESSION = new ServiceCareItemRotaSession()
        {
            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
            StartTime = new TimeOfDay() { Hours = 13, Minutes = 00 },
            EndTime = new TimeOfDay() { Hours = 18, Minutes = 0 }
        };

        #region IsOpen

        [Test]
        public void IsOpen_Returns_True_When_Service_Open_All_Hours()
        {
            var service = new ServiceViewModel()
            {
                OpenAllHours = true,
            };

            Assert.IsTrue(service.IsOpen);
        }
        
        [Test]
        public void IsOpen_Returns_True_When_Service_Is_Open()
        {
            var clock = new StaticClock(DayOfWeek.Monday, 10, 37);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.IsTrue(service.IsOpen);
        }

        [Test]
        public void IsOpen_Returns_False_When_Service_Is_Closed()
        {
            var clock = new StaticClock(DayOfWeek.Monday, 8, 2);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.IsFalse(service.IsOpen);
        }

        [Test]
        public void IsOpen_Returns_False_When_Service_Has_No_Rota_Sessions()
        {
            var clock = new StaticClock(DayOfWeek.Sunday, 16, 2);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
            };

            Assert.IsFalse(service.IsOpen);
        }

        [Test]
        public void IsOpen_Returns_False_When_Service_Has_No_Rota_Session_For_Today()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 35);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION
                }
            };

            Assert.IsFalse(service.IsOpen);
        }

        #endregion

        #region CurrentStatus

        [Test]
        public void CurrentStatus_Returns_24Hours_When_Service_Open_All_Hours()
        {
            var service = new ServiceViewModel()
            {
                OpenAllHours = true,
            };

            Assert.AreEqual("Open today: 24 hours", service.CurrentStatus);
            Assert.AreEqual("Open today: 24 hours", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void CurrentStatus_Returns_Open_Today_Midnight()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 6, 40);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,            
                    new ServiceCareItemRotaSession()
                    {
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        StartTime = new TimeOfDay() { Hours = 0, Minutes = 0 },
                        EndTime = new TimeOfDay() { Hours = 7, Minutes = 30 }
                    },
                    new ServiceCareItemRotaSession()
                    {
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        StartTime = new TimeOfDay() { Hours = 22, Minutes = 0 },
                        EndTime = new TimeOfDay() { Hours = 23, Minutes = 59 }
                    },
                    SUNDAY_SESSION
                }
            };

            Assert.IsTrue(service.IsOpen);
            Assert.AreEqual("Open today: 00:00 until 07:30", service.CurrentStatus);
            Assert.AreEqual("Open today: 00:00 until 07:30", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void CurrentStatus_Returns_Correct_Open_Today_Times()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 35);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Open today: 09:00 until 18:00", service.CurrentStatus);
            Assert.AreEqual("Open today: 09:00 until 18:00", service.ServiceOpeningTimesMessage);
        }

 

        [Test]
        public void CurrentStatus_Returns_Correct_Open_Today_Times_When_Time_Before_Todays_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 7, 30);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Opens today: 09:30 until 17:00", service.CurrentStatus);
            Assert.AreEqual("Opens today: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void CurrentStatus_Returns_Closed_When_Time_After_Todays_Closing()
        {
            var clock = new StaticClock(DayOfWeek.Wednesday, 19, 05);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Closed", service.CurrentStatus);
       }

        [Test]
        public void CurrentStatus_Returns_Closed_When_No_Rota_Session_For_Today()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 05);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION
                }
            };

            Assert.AreEqual("Closed", service.CurrentStatus);
        }

        #endregion

        #region OpeningTimes

        [Test]
        public void OpeningTimes_Returns_List_Of_Opening_Times()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 05);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION
                }
            };

            var openingTimes = service.OpeningTimes;
            Assert.AreEqual(7, openingTimes.Count);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Monday]);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Tuesday]);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Wednesday]);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Thursday]);
            Assert.AreEqual("9:30am - 5:00pm", openingTimes[DayOfWeek.Friday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Saturday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Sunday]);
        }

        [Test]
        public void OpeningTimes_For_Open_All_Hours_Returns_Empty_List()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 05);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = true,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION
                }
            };

            var openingTimes = service.OpeningTimes;
            Assert.AreEqual(0, openingTimes.Count);
        }

        [Test]
        public void OpeningTimes_Null_Rota_Sessions_Returns_Closed()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 12, 05);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false
            };

            var openingTimes = service.OpeningTimes;
            Assert.AreEqual(7, openingTimes.Count);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Monday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Tuesday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Wednesday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Thursday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Friday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Saturday]);
            Assert.AreEqual("Closed", openingTimes[DayOfWeek.Sunday]);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_After_All_Returns_Closed()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 18, 00);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    THURSDAY_MORNING_SESSION,
                    THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Closed", service.CurrentStatus);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_Before_Morning_Returns_Morning_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 4, 00);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    THURSDAY_MORNING_SESSION,
                    THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Opens today: 08:30 until 11:00", service.CurrentStatus);
            Assert.AreEqual("Opens today: 08:30 until 11:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_At_End_Of_First_Session_Returns_Morning()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 10, 59);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    THURSDAY_MORNING_SESSION,
                    THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Open today: 08:30 until 11:00", service.CurrentStatus);
            Assert.AreEqual("Open today: 08:30 until 11:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_Between_Sessions_Returns_Afternoon_Next_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 11, 00);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    THURSDAY_MORNING_SESSION,
                    THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Opens today: 13:00 until 18:00", service.CurrentStatus);
            Assert.AreEqual("Opens today: 13:00 until 18:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_InBetween_Morning_Session_Returns_Morning_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 10, 00);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    THURSDAY_MORNING_SESSION,
                    THURSDAY_AFTERNOON_SESSION
                },
            };

            Assert.AreEqual("Open today: 08:30 until 11:00", service.ServiceOpeningTimesMessage);
            Assert.AreEqual("Open today: 08:30 until 11:00", service.CurrentStatus);
        }

        [Test]
        public void OpeningTimes_Multiple_Rota_Sessions_Time_InBetween_Afternoon_Session_Returns_Afternoon_Opening()
        {
            var clock = new StaticClock(DayOfWeek.Thursday, 15, 00);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    THURSDAY_MORNING_SESSION,
                    THURSDAY_AFTERNOON_SESSION
                },
            };
            
            Assert.AreEqual("Open today: 13:00 until 18:00", service.CurrentStatus);
            Assert.AreEqual("Open today: 13:00 until 18:00", service.ServiceOpeningTimesMessage);
        }

        #endregion

        #region ServiceOpeningTimes

        [Test]
        public void ServiceOpeningTimesMessage_Returns_Tomorrow_Closed_Today()
        {
            var clock = new StaticClock(DayOfWeek.Wednesday, 12, 30);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,            
                    SATURDAY_SESSION,
                    SUNDAY_SESSION
                }
            };

            Assert.AreEqual("Opens tomorrow: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }


        [Test]
        public void ServiceOpeningTimesMessage_Returns_Next_Day_Closed_Today()
        {
            var clock = new StaticClock(DayOfWeek.Wednesday, 12, 30);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    TUESDAY_SESSION
                }
            };

            Assert.AreEqual("Opens Tuesday: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void ServiceOpeningTimesMessage_Returns_First_Session_Of_Two_On_Same_Day_Closed_Today()
        {
            var clock = new StaticClock(DayOfWeek.Friday, 12, 30);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    THURSDAY_AFTERNOON_SESSION,
                    THURSDAY_MORNING_SESSION
                }
            };

            Assert.AreEqual("Opens Thursday: 08:30 until 11:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void ServiceOpeningTimesMessage_Returns_Correct_Open_Tomorrow_Times()
        {
            var clock = new StaticClock(DayOfWeek.Sunday, 22, 35);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                }
            };

            Assert.AreEqual("Opens tomorrow: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void ServiceOpeningTimesMessage_Returns_Correct_Open_Day_After_Tomorrow_Times()
        {
            var clock = new StaticClock(DayOfWeek.Saturday, 22, 35);
            var service = new ServiceViewModel(clock)
            {
                OpenAllHours = false,
                RotaSessions = new[]
                {
                    MONDAY_SESSION,
                    TUESDAY_SESSION,
                    WEDNESDAY_SESSION,
                    THURSDAY_SESSION,
                    FRIDAY_SESSION,
                }
            };

            Assert.AreEqual("Opens Monday: 09:30 until 17:00", service.ServiceOpeningTimesMessage);
        }

        [Test]
        public void ServiceOpeningTimesMessage_Returns_24Hours_When_Service_Open_All_Hours()
        {
            var service = new ServiceViewModel()
            {
                OpenAllHours = true,
            };

            Assert.AreEqual("Open today: 24 hours", service.CurrentStatus);
            Assert.AreEqual("Open today: 24 hours", service.ServiceOpeningTimesMessage);
        }
        [Test]
        public void ServiceOpeningTimesMessage_Returns_Closed_When_Not_Open_All_Hours_And_No_Rotasessions()
        {
            var service = new ServiceViewModel()
            {
                OpenAllHours = false,
            };

            Assert.AreEqual("Closed", service.CurrentStatus);
            Assert.AreEqual("Closed", service.ServiceOpeningTimesMessage);
        }

        #endregion
    }
    
    public class StaticClock : IClock
    {
        private readonly DayOfWeek _day;
        private readonly int _hour;
        private readonly int _minute;
        private readonly DateTime _date;

        public StaticClock(DayOfWeek day, int hour, int minute)
        {
            _day = day;
            _hour = hour;
            _minute = minute;

            //get the next date that the given day of week falls on
            var start = (int)DateTime.Now.DayOfWeek;
            var target = (int)_day;
            if (target <= start)
                target += 7;
            _date = DateTime.Now.AddDays(target - start);
        }

        public DateTime Now
        {
            get
            {
                return new DateTime(_date.Year, _date.Month, _date.Day, _hour, _minute, 0);
            }
        }
    }
}
