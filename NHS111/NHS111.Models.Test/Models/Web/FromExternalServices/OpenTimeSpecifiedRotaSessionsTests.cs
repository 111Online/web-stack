using Moq;
using NHS111.Models.Models.Web.Clock;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using System;

namespace NHS111.Models.Test.Models.Web.FromExternalServices
{
    [TestFixture]
    public class RotaSessionsAndSpecifiedSessionsTests
    {
        private Mock<IClock> _IClock;
        private const string _status = "Open";

        [SetUp]
        public void SetUp()
        {
            _IClock = new Mock<IClock>();

            _IClock.Setup(c => c.Now).Returns(new DateTime(2018, 01, 25, 13, 35, 1, 1));
        }

        [Test]
        public void OpenTimeSpecifiedSessions_RotaSessionAcrossTwodays_OverridenBySpecifiedSessionOnStartDay()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "27-01-2018-18:05-23:30"
                },
                RotaSessions = new[]
                {
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=0,Minutes=1},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndTime = new TimeOfDay{Hours=23,Minutes=59},
                        Status = "Open"
                    },
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=9,Minutes=3},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
                        EndTime = new TimeOfDay{Hours=21,Minutes=5},
                        Status = "Open"
                    }
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(2, result.Length);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(0, result[0].StartTime.Hours);
            Assert.AreEqual(1, result[0].StartTime.Minutes);
            Assert.AreEqual(23, result[0].EndTime.Hours);
            Assert.AreEqual(59, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[1].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[1].EndDayOfWeek));
            Assert.AreEqual(18, result[1].StartTime.Hours);
            Assert.AreEqual(5, result[1].StartTime.Minutes);
            Assert.AreEqual(23, result[1].EndTime.Hours);
            Assert.AreEqual(30, result[1].EndTime.Minutes);
            Assert.AreEqual(_status, result[1].Status);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_WrongSpecifiedSessionTimeFormat_DoesNotOverrideRotaSessions()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "26-01-2018-10:07-22:10",
                    "27-01-2018-*8:05-13:30"
                },
                RotaSessions = new[]
                {
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=0,Minutes=1},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndTime = new TimeOfDay{Hours=23,Minutes=59},
                        Status = "Open"
                    },
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=9,Minutes=3},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndTime = new TimeOfDay{Hours=21,Minutes=5},
                        Status = "Open"
                    }
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(2, result.Length);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(9, result[0].StartTime.Hours);
            Assert.AreEqual(3, result[0].StartTime.Minutes);
            Assert.AreEqual(21, result[0].EndTime.Hours);
            Assert.AreEqual(5, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[1].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[1].EndDayOfWeek));
            Assert.AreEqual(10, result[1].StartTime.Hours);
            Assert.AreEqual(7, result[1].StartTime.Minutes);
            Assert.AreEqual(22, result[1].EndTime.Hours);
            Assert.AreEqual(10, result[1].EndTime.Minutes);
            Assert.AreEqual(_status, result[1].Status);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_WrongSpecifiedSessionDateFormat_DoesNotOverrideRotaSessions()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "2018-01-26-08:05-13:30"
                },
                RotaSessions = new[] { new ServiceCareItemRotaSession{
                        StartTime = new TimeOfDay{Hours=0,Minutes=1},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndTime = new TimeOfDay{Hours=23,Minutes=59},
                        Status = "Open"
                    }
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(0, result[0].StartTime.Hours);
            Assert.AreEqual(1, result[0].StartTime.Minutes);
            Assert.AreEqual(23, result[0].EndTime.Hours);
            Assert.AreEqual(59, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_MultipleSpecifiedSessionsOnSameDay_OverrideExistingAndShowsAllSpecifiedSessionsInList()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "26-01-2018-21:10-22:02",
                    "26-01-2018-14:18-21:07",
                    "26-01-2018-08:05-13:30",
                },
                RotaSessions = new[]
                {
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=0,Minutes=0},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndTime = new TimeOfDay{Hours=10,Minutes=55},
                        Status = "Open"
                    }
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(3, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(21, result[0].StartTime.Hours);
            Assert.AreEqual(10, result[0].StartTime.Minutes);
            Assert.AreEqual(22, result[0].EndTime.Hours);
            Assert.AreEqual(2, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[1].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[1].EndDayOfWeek));
            Assert.AreEqual(14, result[1].StartTime.Hours);
            Assert.AreEqual(18, result[1].StartTime.Minutes);
            Assert.AreEqual(21, result[1].EndTime.Hours);
            Assert.AreEqual(7, result[1].EndTime.Minutes);
            Assert.AreEqual(_status, result[1].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[2].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[2].EndDayOfWeek));
            Assert.AreEqual(8, result[2].StartTime.Hours);
            Assert.AreEqual(5, result[2].StartTime.Minutes);
            Assert.AreEqual(13, result[2].EndTime.Hours);
            Assert.AreEqual(30, result[2].EndTime.Minutes);
            Assert.AreEqual(_status, result[2].Status);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_MultipleRotaSessionsOnSameDay_AllRotaSessionsForThatDayOverridden()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "26-01-2018-08:05-13:30"
                },
                RotaSessions = new[]
                {
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=0,Minutes=0},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndTime = new TimeOfDay{Hours=10,Minutes=55},
                        Status = "Open"
                    },
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=12,Minutes=2},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndTime = new TimeOfDay{Hours=23,Minutes=59},
                        Status = "Open"
                    }
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(8, result[0].StartTime.Hours);
            Assert.AreEqual(5, result[0].StartTime.Minutes);
            Assert.AreEqual(13, result[0].EndTime.Hours);
            Assert.AreEqual(30, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_MultipleSpecifiedSessionsInNextWeek_OverridesExistingSessionsForThoseDays()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "26-01-2018-08:05-13:30",
                    "27-01-2018-09:07-10:20",
                    "29-01-2018-11:15-14:45",
                    "30-01-2018-22:25-23:35",
                },
                RotaSessions = new[]
                {
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=0,Minutes=1},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndTime = new TimeOfDay{Hours=23,Minutes=59},
                        Status = "Open"
                    },
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=3,Minutes=40},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday,
                        EndTime = new TimeOfDay{Hours=4,Minutes=55},
                        Status = "Open"
                    },
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=7,Minutes=53},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday,
                        EndTime = new TimeOfDay{Hours=9,Minutes=2},
                        Status = "Open"
                    },
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=6,Minutes=41},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday,
                        EndTime = new TimeOfDay{Hours=20,Minutes=14},
                        Status = "Open"
                    },
                    new ServiceCareItemRotaSession
                    {
                        StartTime = new TimeOfDay{Hours=19,Minutes=8},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
                        EndTime = new TimeOfDay{Hours=21,Minutes=57},
                        Status = "Open"
                    },
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(5, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[1].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[1].EndDayOfWeek));
            Assert.AreEqual(8, result[1].StartTime.Hours);
            Assert.AreEqual(5, result[1].StartTime.Minutes);
            Assert.AreEqual(13, result[1].EndTime.Hours);
            Assert.AreEqual(30, result[1].EndTime.Minutes);
            Assert.AreEqual(_status, result[1].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[2].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[2].EndDayOfWeek));
            Assert.AreEqual(9, result[2].StartTime.Hours);
            Assert.AreEqual(7, result[2].StartTime.Minutes);
            Assert.AreEqual(10, result[2].EndTime.Hours);
            Assert.AreEqual(20, result[2].EndTime.Minutes);
            Assert.AreEqual(_status, result[2].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Sunday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(7, result[0].StartTime.Hours);
            Assert.AreEqual(53, result[0].StartTime.Minutes);
            Assert.AreEqual(9, result[0].EndTime.Hours);
            Assert.AreEqual(2, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday.Equals(result[3].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Monday.Equals(result[3].EndDayOfWeek));
            Assert.AreEqual(11, result[3].StartTime.Hours);
            Assert.AreEqual(15, result[3].StartTime.Minutes);
            Assert.AreEqual(14, result[3].EndTime.Hours);
            Assert.AreEqual(45, result[3].EndTime.Minutes);
            Assert.AreEqual(_status, result[3].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday.Equals(result[4].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday.Equals(result[4].EndDayOfWeek));
            Assert.AreEqual(22, result[4].StartTime.Hours);
            Assert.AreEqual(25, result[4].StartTime.Minutes);
            Assert.AreEqual(23, result[4].EndTime.Hours);
            Assert.AreEqual(35, result[4].EndTime.Minutes);
            Assert.AreEqual(_status, result[4].Status);


        }

        [Test]
        public void OpenTimeSpecifiedSessions_NoRotaSessionsOrOpenTimeSpecified_ReturnsEmptyList()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {

            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_NoRotaSessions_UsesOpenTimeSpecified()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "26-01-2018-08:05-13:30",
                    "27-01-2018-07:15-20:09"
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(2, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(8, result[0].StartTime.Hours);
            Assert.AreEqual(5, result[0].StartTime.Minutes);
            Assert.AreEqual(13, result[0].EndTime.Hours);
            Assert.AreEqual(30, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[1].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Saturday.Equals(result[1].EndDayOfWeek));
            Assert.AreEqual(7, result[1].StartTime.Hours);
            Assert.AreEqual(15, result[1].StartTime.Minutes);
            Assert.AreEqual(20, result[1].EndTime.Hours);
            Assert.AreEqual(9, result[1].EndTime.Minutes);
            Assert.AreEqual(_status, result[1].Status);

        }

        [Test]
        public void OpenTimeSpecifiedSessions_NoSpecifiedSessions_UsesRotaSessions()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                RotaSessions = new[]
                    {
                        new ServiceCareItemRotaSession
                        {
                            StartTime = new TimeOfDay{ Hours=0, Minutes=1},
                            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                            EndTime = new TimeOfDay{ Hours=23, Minutes=59},
                            Status = "Open"
                        },
                        new ServiceCareItemRotaSession
                        {
                            StartTime = new TimeOfDay { Hours = 2, Minutes = 3 },
                            StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
                            EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday,
                            EndTime = new TimeOfDay { Hours = 22, Minutes = 5 },
                            Status = "Open"
                        }
                    }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(2, result.Length);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(0, result[0].StartTime.Hours);
            Assert.AreEqual(1, result[0].StartTime.Minutes);
            Assert.AreEqual(23, result[0].EndTime.Hours);
            Assert.AreEqual(59, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);

            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday.Equals(result[1].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Tuesday.Equals(result[1].EndDayOfWeek));
            Assert.AreEqual(2, result[1].StartTime.Hours);
            Assert.AreEqual(3, result[1].StartTime.Minutes);
            Assert.AreEqual(22, result[1].EndTime.Hours);
            Assert.AreEqual(5, result[1].EndTime.Minutes);
            Assert.AreEqual(_status, result[1].Status);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_SetInDifferentOrder_OverridesExistingSessionForThatDay()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                RotaSessions = new[] { new ServiceCareItemRotaSession{
                        StartTime = new TimeOfDay{Hours=0,Minutes=0},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                        EndTime = new TimeOfDay{Hours=23,Minutes=59},
                        Status = "Open"
                    }
                },
                OpenTimeSpecifiedSessions = new[]
                {
                    "26-01-2018-08:05-13:30"
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(8, result[0].StartTime.Hours);
            Assert.AreEqual(5, result[0].StartTime.Minutes);
            Assert.AreEqual(13, result[0].EndTime.Hours);
            Assert.AreEqual(30, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);
        }


        [Test]
        public void OpenTimeSpecifiedSessions_SessionInNextWeek_OverridesExistingSessionForThatDay()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "26-01-2018-08:05-13:30"
                },
                RotaSessions = new[] { new ServiceCareItemRotaSession{
                    StartTime = new TimeOfDay{Hours=0,Minutes=0},
                    StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                    EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday,
                    EndTime = new TimeOfDay{Hours=23,Minutes=59},
                    Status = "Open"
                }
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Friday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(8, result[0].StartTime.Hours);
            Assert.AreEqual(5, result[0].StartTime.Minutes);
            Assert.AreEqual(13, result[0].EndTime.Hours);
            Assert.AreEqual(30, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_OnlyRotaWithoutSpecified_ShowsRotaAndOpen()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                RotaSessions = new[] { new ServiceCareItemRotaSession{
                        StartTime = new TimeOfDay{Hours=1,Minutes=0},
                        StartDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
                        EndDayOfWeek = NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday,
                        EndTime = new TimeOfDay{Hours=15,Minutes=30},
                        Status = "Open"
                    }
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.IsTrue(sut.IsOpen);
            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(1, result[0].StartTime.Hours);
            Assert.AreEqual(0, result[0].StartTime.Minutes);
            Assert.AreEqual(15, result[0].EndTime.Hours);
            Assert.AreEqual(30, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);
        }

        [Test]
        public void OpenTimeSpecifiedSessions_OnlySpecifiedWithoutRota_ShowsSpecifiedSessionAndOpen()
        {
            //arrange
            DosService sut = new DosService(_IClock.Object)
            {
                OpenTimeSpecifiedSessions = new[]
                {
                    "25-01-2018-01:00-15:30"
                }
            };

            //act
            var result = sut.RotaSessionsAndSpecifiedSessions;

            Assert.IsTrue(sut.IsOpen);
            Assert.AreEqual(1, result.Length);
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday.Equals(result[0].StartDayOfWeek));
            Assert.IsTrue(NHS111.Models.Models.Web.FromExternalServices.DayOfWeek.Thursday.Equals(result[0].EndDayOfWeek));
            Assert.AreEqual(1, result[0].StartTime.Hours);
            Assert.AreEqual(0, result[0].StartTime.Minutes);
            Assert.AreEqual(15, result[0].EndTime.Hours);
            Assert.AreEqual(30, result[0].EndTime.Minutes);
            Assert.AreEqual(_status, result[0].Status);
        }
    }
}
