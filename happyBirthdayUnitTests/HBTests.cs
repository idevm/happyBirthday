using System;
using System.Collections.Generic;
using happyBirthday;
using NUnit.Framework;

namespace happyBirthdayUnitTests
{
    [TestFixture]
    public class Tests
    {
        private HBApp app;

        private int ThisDay { get; } = DateTime.Now.Day;

        private int ThisMonth { get; } = DateTime.Now.Month;

        [SetUp]
        public void SetUp()
        {
            app = new();
        }


        [TestCase("db")]
        [TestCase("db.")]
        [TestCase("d.csv")]
        [TestCase("db.xls")]
        [TestCase("db.txt")]
        [TestCase("../db.csv")]
        public void ReadFile_BadFiles_ThrowException(string path)
        {
            Exception ex = Assert.Catch(() => app.ReadFile(path));
            StringAssert.Contains("Не найден файл с данными, а именно: ", ex.Message);
        }


        [Test]
        public void GetPeopleList_OneMan_ReturnsPeopleList()
        {
            List<string> lines = new()
            {
                $"1;;Ivan;;{app.ToString(ThisDay)}.{app.ToString(ThisMonth)}.2000;"
            };
            List<Person> exp = new()
            {
                new Person("Ivan")
                {
                    Birthday = ThisDay,
                    Birthmonth = ThisMonth,
                    Birthyear = 2000
                }
            };
            List<Person> res = app.GetPeopleList(lines);
            Assert.AreEqual(exp[0].Birthday, res[0].Birthday);
            Assert.AreEqual(exp[0].Birthmonth, res[0].Birthmonth);
            Assert.AreEqual(exp[0].Birthyear, res[0].Birthyear);
            Assert.AreEqual(exp[0].Name, res[0].Name);
        }


        [Test]
        public void GetPeopleList_MoreMen_ReturnsPeopleList()
        {
            List<string> lines = new()
            {
                $"1;;Ivan;;{app.ToString(ThisDay)}.{app.ToString(ThisMonth)}.1990;",
                $"2;;Iva;;{app.ToString(ThisDay)}.{app.ToString(ThisMonth)}.1991;"
            };
            List<Person> exp = new()
            {
                new Person("Ivan")
                {
                    Birthday = ThisDay,
                    Birthmonth = ThisMonth,
                    Birthyear = 1990
                },
                new Person("Iva")
                {
                    Birthday = ThisDay,
                    Birthmonth = ThisMonth,
                    Birthyear = 1991
                }
            };
            List<Person> res = app.GetPeopleList(lines);
            Assert.AreEqual(exp[0].Birthday, res[0].Birthday);
            Assert.AreEqual(exp[0].Birthmonth, res[0].Birthmonth);
            Assert.AreEqual(exp[0].Birthyear, res[0].Birthyear);
            Assert.AreEqual(exp[0].Name, res[0].Name);
            Assert.AreEqual(exp[1].Birthday, res[1].Birthday);
            Assert.AreEqual(exp[1].Birthmonth, res[1].Birthmonth);
            Assert.AreEqual(exp[1].Birthyear, res[1].Birthyear);
            Assert.AreEqual(exp[1].Name, res[1].Name);
        }


        [Test]
        public void PeopleListFilter_ValidDataButNoBirthdays_ReturnsEmptyPeopleList()
        {
            int tomorrow = ThisDay + 1;
            List<Person> people = new()
            {
                new Person("Ivan")
                {
                    Birthday = tomorrow,
                    Birthmonth = ThisMonth,
                    Birthyear = 1990
                },
                new Person("Iva")
                {
                    Birthday = tomorrow,
                    Birthmonth = ThisMonth,
                    Birthyear = 1991
                }
            };
            List<Person> exp = new();
            List<Person> res = app.PeopleListFilter(people);
            Assert.AreEqual(exp, res);
        }


        [TestCase(Mode.today, 1)]
        [TestCase(Mode.thisMonth, 2)]
        [TestCase(Mode.thisYear, 3)]
        public void PeopleListFilter_SomeTimeModes_ReturnsPeopleList(Mode tm, int CountOfCorrectLines)
        {
            app.DayNow = 1;
            app.MonthNow = 1;
            app.YearNow = 2021;
            List<Person> people = new()
            {
                new Person("Ivan1")
                {
                    Birthday = 1,
                    Birthmonth = 1,
                    Birthyear = 1990
                },
                new Person("Ivan2")
                {
                    Birthday = 2,
                    Birthmonth = 1,
                    Birthyear = 1991
                },
                new Person("Ivan3")
                {
                    Birthday = 2,
                    Birthmonth = 2,
                    Birthyear = 1992
                }
            };
            List<Person> exp = new();
            for (int i = 0; i < CountOfCorrectLines; i++)
            {
                exp.Add(people[i]);
            }
            List<Person> res = app.PeopleListFilter(people, tm);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetPeopleList_NoData_ThrowsExceptiion()
        {
            List<string> lines = new();
            Exception ex = Assert.Catch(() => app.GetPeopleList(lines));
            StringAssert.Contains("Ошибка: отсутсвуют данные", ex.Message);
        }


        [Test]
        public void GetPeopleList_NullData_ThrowsExceptiion()
        {
            List<string> lines = null;
            Exception ex = Assert.Catch(() => app.GetPeopleList(lines));
            StringAssert.Contains("Ошибка: отсутсвуют данные", ex.Message);
        }


        [TestCase("1;;Ivan;;01.01.1990")]
        [TestCase("1;;Ivan;;")]
        [TestCase("1;;Ivan;")]
        [TestCase("1;;Ivan")]
        [TestCase("1;;")]
        [TestCase("1;")]
        [TestCase("1")]
        [TestCase("")]
        public void GetPeopleList_BadFormatting_ThrowsExceptiion(string line)
        {
            List<string> lines = new() { line };
            Exception ex = Assert.Catch(() => app.GetPeopleList(lines));
            StringAssert.Contains("Неправильный формат таблицы", ex.Message);
        }


        [Test]
        public void GetPeopleList_NoName_ThrowsExceptiion()
        {
            List<string> lines = new() { "1;;;;01.01.1990;" };
            Exception ex = Assert.Catch(() => app.GetPeopleList(lines));
            StringAssert.Contains("Отсутствует имя в строке 1", ex.Message);
        }


        [TestCase("1;;Ivan;;.01.1990;")]
        [TestCase("1;;Ivan;;001.01.1990;")]
        [TestCase("1;;Ivan;;f.01.1990;")]
        [TestCase("1;;Ivan;;33.01.1990;")]
        public void GetPeopleList_BadDay_ThrowsExceptiion(string line)
        {
            List<string> lines = new() { line };
            Exception ex = Assert.Catch(() => app.GetPeopleList(lines));
            StringAssert.Contains("Неправильный формат дня в строке 1", ex.Message);
        }


        [TestCase("1;;Ivan;;01..1990;")]
        [TestCase("1;;Ivan;;01.010.1990;")]
        [TestCase("1;;Ivan;;01.h.1990;")]
        [TestCase("1;;Ivan;;01.22.1990;")]
        public void GetPeopleList_BadMonth_ThrowsExceptiion(string line)
        {
            List<string> lines = new() { line };
            Exception ex = Assert.Catch(() => app.GetPeopleList(lines));
            StringAssert.Contains("Неправильный формат месяца в строке 1", ex.Message);
        }


        [TestCase("1;;Ivan;;01.01.199;")]
        [TestCase("1;;Ivan;;01.01.19902;")]
        [TestCase("1;;Ivan;;01.01.d;")]
        [TestCase("1;;Ivan;;01.01.;")]
        public void GetPeopleList_BadYear_ThrowsExceptiion(string line)
        {
            List<string> lines = new() { line };
            Exception ex = Assert.Catch(() => app.GetPeopleList(lines));
            StringAssert.Contains("Неправильный формат года в строке 1", ex.Message);
        }


        [Test]
        public void GetText_NoPeopleToday_ReturnsText()
        {
            List<Person> people = new();
            string exp = $"Сегодня {app.ToString(ThisDay)}.{app.ToString(ThisMonth)} никто не отмечает день рождения";
            string res = app.GetText(people, Mode.today);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_OneManToday_ReturnsText()
        {
            List<Person> people = new()
            {
                new Person("Ivan")
                {
                    Birthday = ThisDay,
                    Birthmonth = ThisMonth,
                    Birthyear = 1990
                }
            };
            app.YearNow = 2020;
            string exp = $"Сегодня {app.ToString(ThisDay)}.{app.ToString(ThisMonth)} отмечает день рождения\n\n\tIvan (30 лет)\n";
            string res = app.GetText(people, Mode.today);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_MorePeopleToday_ReturnsText()
        {
            List<Person> people = new()
            {
                new Person("Ivan")
                {
                    Birthday = ThisDay,
                    Birthmonth = ThisMonth,
                    Birthyear = 1990
                },
                new Person("Iva")
                {
                    Birthday = ThisDay,
                    Birthmonth = ThisMonth,
                    Birthyear = 1991
                }
            };
            app.YearNow = 2020;
            string exp = $"Сегодня {app.ToString(ThisDay)}.{app.ToString(ThisMonth)} отмечают день рождения:\n\n\tIvan (30 лет)\n\tIva (29 лет)\n";
            string res = app.GetText(people, Mode.today);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_NoPeopleThisMonth_ReturnsText()
        {
            List<Person> people = new();
            string exp = "В этом месяце никто не отмечает день рождения";
            string res = app.GetText(people, Mode.thisMonth);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_OneManThisMonth_ReturnsText()
        {
            List<Person> people = new()
            {
                new Person("Ivan")
                {
                    Birthday = 31,
                    Birthmonth = ThisMonth,
                    Birthyear = 1990
                }
            };
            app.YearNow = 2020;
            string exp = $"В этом месяце отмечает день рождения\n\n\tIvan (31.{app.ToString(ThisMonth)})\n";
            string res = app.GetText(people, Mode.thisMonth);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_MorePeopleThisMonth_ReturnsText()
        {
            List<Person> people = new()
            {
                new Person("Ivan")
                {
                    Birthday = 31,
                    Birthmonth = ThisMonth,
                    Birthyear = 1990
                },
                new Person("Iva")
                {
                    Birthday = 13,
                    Birthmonth = ThisMonth,
                    Birthyear = 1991
                }
            };
            app.YearNow = 2020;
            string exp = $"В этом месяце отмечают день рождения:\n\n\tIvan (31.{app.ToString(ThisMonth)})\n\tIva (13.{app.ToString(ThisMonth)})\n";
            string res = app.GetText(people, Mode.thisMonth);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_NoPeopleThisYear_ReturnsText()
        {
            List<Person> people = new();
            string exp = "В этом году никто не отмечает день рождения";
            string res = app.GetText(people, Mode.thisYear);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_OneManThisYear_ReturnsText()
        {
            List<Person> people = new()
            {
                new Person("Ivan")
                {
                    Birthday = 31,
                    Birthmonth = 01,
                    Birthyear = 1990
                }
            };
            app.YearNow = 2020;
            string exp = $"В этом году отмечает день рождения\n\n\tIvan (31.01)\n";
            string res = app.GetText(people, Mode.thisYear);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_MorePeopleThisYear_ReturnsText()
        {
            List<Person> people = new()
            {
                new Person("Ivan")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990
                },
                new Person("Iva")
                {
                    Birthday = 01,
                    Birthmonth = 02,
                    Birthyear = 1991
                }
            };
            app.YearNow = 2020;
            string exp = $"В этом году отмечают день рождения:\n\n\tIvan (01.01)\n\tIva (01.02)\n";
            string res = app.GetText(people, Mode.thisYear);
            Assert.AreEqual(exp, res);
        }


        [TestCase(0, "0 лет")]
        [TestCase(1, "1 год")]
        [TestCase(2, "2 года")]
        [TestCase(3, "3 года")]
        [TestCase(4, "4 года")]
        [TestCase(5, "5 лет")]
        [TestCase(6, "6 лет")]
        [TestCase(7, "7 лет")]
        [TestCase(8, "8 лет")]
        [TestCase(9, "9 лет")]
        [TestCase(10, "10 лет")]
        [TestCase(11, "11 лет")]
        [TestCase(12, "12 лет")]
        [TestCase(13, "13 лет")]
        [TestCase(14, "14 лет")]
        [TestCase(15, "15 лет")]
        [TestCase(16, "16 лет")]
        [TestCase(17, "17 лет")]
        [TestCase(18, "18 лет")]
        [TestCase(19, "19 лет")]
        [TestCase(20, "20 лет")]
        [TestCase(21, "21 год")]
        [TestCase(22, "22 года")]
        [TestCase(23, "23 года")]
        [TestCase(24, "24 года")]
        [TestCase(25, "25 лет")]
        [TestCase(30, "30 лет")]
        [TestCase(33, "33 года")]
        [TestCase(35, "35 лет")]
        [TestCase(49, "49 лет")]
        [TestCase(50, "юбилей: 50 лет")]
        [TestCase(51, "51 год")]
        [TestCase(55, "юбилей: 55 лет")]
        [TestCase(80, "юбилей: 80 лет")]
        [TestCase(81, "81 год")]
        [TestCase(100, "юбилей: 100 лет")]
        [TestCase(101, "101 год")]
        [TestCase(102, "102 года")]
        [TestCase(103, "103 года")]
        [TestCase(105, "юбилей: 105 лет")]
        [TestCase(205, "юбилей: 205 лет")]
        [TestCase(301, "301 год")]
        public void GetAge_SomeYears_ReturnsText(int year, string exp)
        {
            app.YearNow = year * 2;
            Person p = new("");
            string res = p.GetAge(year);
            Assert.AreEqual(exp, res);
        }


        [TestCase("Ivan", "01.01.1990")]
        public void AddPerson_NoDataOneValidInput_ReturnsList(string name, string birthday)
        {
            List<Person> people = new();
            List<Person> exp = new();
            Person p = new("IVAN")
            {
                Birthday = 01,
                Birthmonth = 01,
                Birthyear = 1990,
                number = 1
            };
            exp.Add(p);
            List<Person> res = app.AddPerson(people, name, birthday);
            Assert.AreEqual(exp[0].Birthday, res[0].Birthday);
            Assert.AreEqual(exp[0].Birthmonth, res[0].Birthmonth);
            Assert.AreEqual(exp[0].Birthyear, res[0].Birthyear);
            Assert.AreEqual(exp[0].Name, res[0].Name);
        }


        [TestCase("Ivan", "01.01.1990")]
        public void AddPerson_NullDataOneValidInput_ReturnsList(string name, string birthday)
        {
            List<Person> people = null;
            List<Person> exp = new();
            Person p = new("IVAN")
            {
                Birthday = 01,
                Birthmonth = 01,
                Birthyear = 1990,
                number = 1
            };
            exp.Add(p);
            List<Person> res = app.AddPerson(people, name, birthday);
            Assert.AreEqual(exp[0].Birthday, res[0].Birthday);
            Assert.AreEqual(exp[0].Birthmonth, res[0].Birthmonth);
            Assert.AreEqual(exp[0].Birthyear, res[0].Birthyear);
            Assert.AreEqual(exp[0].Name, res[0].Name);
        }


        [TestCase("Ivan", "01.01.1990")]
        public void AddPerson_SomeDataOneValidInput_ReturnsList(string name, string birthday)
        {
            List<Person> people = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                },
                new Person("Vlad")
                {
                    Birthday = 01,
                    Birthmonth = 02,
                    Birthyear = 1980,
                    number = 2
                }
            };
            List<Person> exp = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                },
                new Person("Vlad")
                {
                    Birthday = 01,
                    Birthmonth = 02,
                    Birthyear = 1980,
                    number = 2
                },
                new Person("IVAN")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 3
                }
            };
            List<Person> res = app.AddPerson(people, name, birthday);
            Assert.AreEqual(exp[0].Birthday, res[0].Birthday);
            Assert.AreEqual(exp[0].Birthmonth, res[0].Birthmonth);
            Assert.AreEqual(exp[0].Birthyear, res[0].Birthyear);
            Assert.AreEqual(exp[0].Name, res[0].Name);
            Assert.AreEqual(exp[1].Birthday, res[1].Birthday);
            Assert.AreEqual(exp[1].Birthmonth, res[1].Birthmonth);
            Assert.AreEqual(exp[1].Birthyear, res[1].Birthyear);
            Assert.AreEqual(exp[1].Name, res[1].Name);
            Assert.AreEqual(exp[2].Birthday, res[2].Birthday);
            Assert.AreEqual(exp[2].Birthmonth, res[2].Birthmonth);
            Assert.AreEqual(exp[2].Birthyear, res[2].Birthyear);
            Assert.AreEqual(exp[2].Name, res[2].Name);
        }


        [TestCase("", "01.01.1990")]
        public void AddPerson_BadNameInput_ThrowsException(string name, string birthday)
        {
            List<Person> people = new();
            Exception ex = Assert.Catch(() => app.AddPerson(people, name, birthday));
            StringAssert.Contains("Введены некорректные данные\nВведите корректные ФИО (например Иванов Иван Иванович)", ex.Message);
        }


        [TestCase("Ivan", "0.01.1990")]
        [TestCase("Ivan", "01.1.1990")]
        [TestCase("Ivan", "01.01.199")]
        [TestCase("Ivan", "01.01")]
        [TestCase("Ivan", "01.")]
        [TestCase("Ivan", ".01.1990")]
        [TestCase("Ivan", "01.f.1990")]
        [TestCase("Ivan", "01..1990")]
        [TestCase("Ivan", "01.01.")]
        [TestCase("Ivan", "")]
        [TestCase("Ivan", "01011990")]
        [TestCase("Ivan", "01,01,1990")]
        [TestCase("Ivan", "1.1.1990")]
        [TestCase("Ivan", "f")]
        [TestCase("Ivan", "..")]
        public void AddPerson_BadBirthdayInputs_ThrowsException(string name, string birthday)
        {
            List<Person> people = new();
            Exception ex = Assert.Catch(() => app.AddPerson(people, name, birthday));
            StringAssert.Contains("Введены некорректные данные\nВведите корректную дату рождения (например 02.08.1999)", ex.Message);
        }


        [Test]
        public void RemovePerson_SomeDataNoValidInput_ReturnsList()
        {
            List<Person> people = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                }
            };
            Person pToRemove = null;
            List<Person> exp = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                }
            };
            List<Person> res = app.RemovePerson(people, pToRemove);
            Assert.AreEqual(exp[0].Birthday, res[0].Birthday);
            Assert.AreEqual(exp[0].Birthmonth, res[0].Birthmonth);
            Assert.AreEqual(exp[0].Birthyear, res[0].Birthyear);
            Assert.AreEqual(exp[0].Name, res[0].Name);
        }


        [Test]
        public void RemovePerson_SomeDataOneValidInput_ReturnsList()
        {
            List<Person> people = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                },
                new Person("Vlad")
                {
                    Birthday = 01,
                    Birthmonth = 02,
                    Birthyear = 1980,
                    number = 2
                },
                new Person("IVAN")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 3
                }
            };
            Person pToRemove = people[2];
            List<Person> exp = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                },
                new Person("Vlad")
                {
                    Birthday = 01,
                    Birthmonth = 02,
                    Birthyear = 1980,
                    number = 2
                }
            };
            people = app.RemovePerson(people, pToRemove);
            bool res = people.Exists(x => x.Name == "IVAN");
            Assert.False(res);
        }


        [Test]
        public void FindPersonByName_SomeDataValidInput_ReturnsPerson()
        {
            List<Person> people = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                }
            };
            string name = "Boris";
            List<Person> exp = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                }
            };
            List<Person> res = app.FindPersonByName(people, name);
            Assert.AreEqual(exp[0].Birthday, res[0].Birthday);
            Assert.AreEqual(exp[0].Birthmonth, res[0].Birthmonth);
            Assert.AreEqual(exp[0].Birthyear, res[0].Birthyear);
            Assert.AreEqual(exp[0].Name, res[0].Name);
        }


        [Test]
        public void FindPersonByName_NoDataValidInput_ThrowsEx()
        {
            List<Person> people = new();
            string name = "Boris";
            Exception ex = Assert.Catch(() => app.FindPersonByName(people, name));
            StringAssert.Contains("Ошибка: отсутсвуют данные", ex.Message);
        }


        [Test]
        public void FindPersonByDate_SomeDataNoValidInput_ReturnsPerson()
        {
            List<Person> people = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                }
            };
            List<Person> exp = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                }
            };
            List<Person> res = app.FindPersonByDate(people, 01);
            Assert.AreEqual(exp[0].Birthday, res[0].Birthday);
            Assert.AreEqual(exp[0].Birthmonth, res[0].Birthmonth);
            Assert.AreEqual(exp[0].Birthyear, res[0].Birthyear);
            Assert.AreEqual(exp[0].Name, res[0].Name);
        }


        [TestCase(1, -1, -1)]
        [TestCase(1, 1, -1)]
        [TestCase(1, 1, 1990)]
        [TestCase(-1, 1, -1)]
        [TestCase(-1, -1, 1990)]
        [TestCase(1, -1, 1990)]
        public void FindPersonByDate_SomeDataSomeArguments_ReturnsPerson(int d, int m, int y)
        {
            List<Person> people = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                }
            };
            List<Person> exp = new()
            {
                new Person("Boris")
                {
                    Birthday = 01,
                    Birthmonth = 01,
                    Birthyear = 1990,
                    number = 1
                }
            };
            List<Person> res = app.FindPersonByDate(people, d, m, y);
            Assert.AreEqual(exp[0].Name, res[0].Name);
        }


        [Test]
        public void UpdateText_UnsortedList_ReturnsText()
        {
            List<Person> people = new()
            {
                new Person("BORIS")
                {
                    Birthday = 31,
                    Birthmonth = 12,
                    Birthyear = 1990,
                    number = 1
                },
                new Person("VLAD")
                {
                    Birthday = 01,
                    Birthmonth = 02,
                    Birthyear = 1980,
                    number = 2
                },
                new Person("IVAN")
                {
                    Birthday = 01,
                    Birthmonth = 02,
                    Birthyear = 1990,
                    number = 3
                }
            };
            string exp = $"1;;IVAN;;01.02.1990;\n2;;VLAD;;01.02.1980;\n3;;BORIS;;31.12.1990;\n";
            string res = app.UpdateText(people);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void UpdateText_NullList_ReturnsText()
        {
            List<Person> people = null;
            Exception ex = Assert.Catch(() => app.UpdateText(people));
            StringAssert.Contains("Ошибка: отсутсвуют данные", ex.Message);
        }


        [Test]
        public void UpdateText_NoDataList_ReturnsText()
        {
            List<Person> people = new();
            Exception ex = Assert.Catch(() => app.UpdateText(people));
            StringAssert.Contains("Ошибка: отсутсвуют данные", ex.Message);
        }


        [Test]
        public void ValidInput_SomeValidInput_ReturnsTrue()
        {
            bool res = app.ValidInput("Ivan", "01.01.1990");
            Assert.True(res);
        }


        [Test]
        public void ValidInput_NoInput_ReturnsFalse()
        {
            bool res = app.ValidInput();
            Assert.False(res);
        }


        [Test]
        public void ValidInput_OnlyValidName_ReturnsTrue()
        {
            bool res = app.ValidInput("Ivan");
            Assert.True(res);
        }


        [Test]
        public void ValidInput_OnlyValidBirthDay_ReturnsTrue()
        {
            bool res = app.ValidInput("01.01.1990");
            Assert.True(res);
        }


        [TestCase("Ivan", "0.01.1990")]
        [TestCase("Ivan", "01.1.1990")]
        [TestCase("Ivan", "01.01.199")]
        [TestCase("Ivan", "01.01")]
        [TestCase("Ivan", "01.")]
        [TestCase("Ivan", ".01.1990")]
        [TestCase("Ivan", "01.f.1990")]
        [TestCase("Ivan", "01..1990")]
        [TestCase("Ivan", "01.01.")]
        [TestCase("Ivan", "")]
        [TestCase("Ivan", "01011990")]
        [TestCase("Ivan", "01,01,1990")]
        [TestCase("", "01.01.1990")]
        [TestCase("Ivan", "f")]
        [TestCase("Ivan", "..")]
        [TestCase("", "")]
        public void ValidInput_SomeBadValues_ReturnsFalse(string n, string bd)
        {
            bool res = app.ValidInput(n, bd);
            Assert.False(res);
        }
    }
}
