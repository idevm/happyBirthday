using System;
using System.Collections.Generic;
using happyBirthday;
using NUnit.Framework;

namespace happyBirthdayUnitTests
{
    [TestFixture]
    public class Tests
    {
        //[Test]
        //public void GetPeopleList_GoodData_AddDict()
        //{
        //    Program.GetDateNow();
        //    Program.Lines = new string[1] { "1;;Ivan;;01.01.1990;" };
        //    Program.People = new();
        //    Program.GetPeopleList();
        //    List<Dictionary<string, string>> exp = new();
        //    Dictionary<string, string> man = new();
        //    man.Add("name", "Ivan");
        //    man.Add("day", "01");
        //    man.Add("month", "01");
        //    man.Add("year", "1990");
        //    exp.Add(man);
        //    Assert.AreSame(exp, Program.People);
        //}

        [Test]
        public void GetPeopleList_NoData_ThrowsExceptiion()
        {
            Program.Lines = new string[0];
            Exception ex = Assert.Catch(() => Program.GetPeopleList());
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
            Program.Lines = new string[1] { line };
            Exception ex = Assert.Catch(() => Program.GetPeopleList());
            StringAssert.Contains("Неправильный формат таблицы", ex.Message);
        }

        [Test]
        public void GetPeopleList_NoName_ThrowsExceptiion()
        {
            Program.GetDateNow();
            Program.Lines = new string[1] { "1;;;;01.01.1990;" };
            Exception ex = Assert.Catch(() => Program.GetPeopleList());
            StringAssert.Contains("Отсутствует имя в строке", ex.Message);
        }

        [TestCase("1;;Ivan;;.01.1990;")]
        [TestCase("1;;Ivan;;001.01.1990;")]
        [TestCase("1;;Ivan;;f.01.1990;")]
        [TestCase("1;;Ivan;;33.01.1990;")]
        public void GetPeopleList_BadDay_ThrowsExceptiion(string line)
        {
            Program.GetDateNow();
            Program.Lines = new string[1] { line };
            Exception ex = Assert.Catch(() => Program.GetPeopleList());
            StringAssert.Contains("Неправильный формат дня в строке", ex.Message);
        }

        [TestCase("1;;Ivan;;01..1990;")]
        [TestCase("1;;Ivan;;01.010.1990;")]
        [TestCase("1;;Ivan;;01.h.1990;")]
        [TestCase("1;;Ivan;;01.22.1990;")]
        public void GetPeopleList_BadMonth_ThrowsExceptiion(string line)
        {
            Program.GetDateNow();
            Program.Lines = new string[1] { line };
            Exception ex = Assert.Catch(() => Program.GetPeopleList());
            StringAssert.Contains("Неправильный формат месяца в строке", ex.Message);
        }

        [TestCase("1;;Ivan;;01.01.199;")]
        [TestCase("1;;Ivan;;01.01.19902;")]
        [TestCase("1;;Ivan;;01.01.d;")]
        [TestCase("1;;Ivan;;01.01.;")]
        public void GetPeopleList_BadYear_ThrowsExceptiion(string line)
        {
            Program.GetDateNow();
            Program.Lines = new string[1] { line };
            Exception ex = Assert.Catch(() => Program.GetPeopleList());
            StringAssert.Contains("Неправильный формат года в строке", ex.Message);
        }

        [Test]
        public void GetText_NoPeople_ReturnsText()
        {
            Program.GetDateNow();
            Program.People = new List<Dictionary<string, string>>();
            string exp = $"Сегодня {Program.DayNow}.{Program.MonthNow} никто не отмечает день рождения";
            string res = Program.GetText();
            Assert.AreEqual(exp, res);
        }

        [Test]
        public void GetText_OnePeople_ReturnsText()
        {
            Program.GetDateNow();
            Program.People = new List<Dictionary<string, string>>();
            Dictionary<string, string> man = new();
            man.Add("name", "Ivan");
            man.Add("day", Program.DayNow);
            man.Add("month", Program.MonthNow);
            man.Add("year", "1990");
            Program.People.Add(man);
            string exp = $"Сегодня {Program.DayNow}.{Program.MonthNow} отмечает день рождения\n\n" + "\t" + "Ivan" + " (" + Program.GetAge("1990") + ")\n";
            string res = Program.GetText();
            Assert.AreEqual(exp, res);
        }

        [Test]
        public void GetText_MorePeople_ReturnsText()
        {
            Program.GetDateNow();
            Program.People = new List<Dictionary<string, string>>();
            Dictionary<string, string> man1 = new();
            Dictionary<string, string> man2 = new();
            man1.Add("name", "Ivan");
            man1.Add("day", Program.DayNow);
            man1.Add("month", Program.MonthNow);
            man1.Add("year", "1990");
            Program.People.Add(man1);
            man2.Add("name", "Iva");
            man2.Add("day", Program.DayNow);
            man2.Add("month", Program.MonthNow);
            man2.Add("year", "1991");
            Program.People.Add(man2);
            string exp = $"Сегодня {Program.DayNow}.{Program.MonthNow} отмечают день рождения:\n\n\tIvan (" + Program.GetAge("1990") + ")\n\tIva (" + Program.GetAge("1991") + ")\n";
            string res = Program.GetText();
            Assert.AreEqual(exp, res);
        }

        [TestCase("0", "0 лет")]
        [TestCase("1", "1 год")]
        [TestCase("2", "2 года")]
        [TestCase("3", "3 года")]
        [TestCase("4", "4 года")]
        [TestCase("5", "5 лет")]
        [TestCase("6", "6 лет")]
        [TestCase("7", "7 лет")]
        [TestCase("8", "8 лет")]
        [TestCase("9", "9 лет")]
        [TestCase("10", "10 лет")]
        [TestCase("11", "11 лет")]
        [TestCase("12", "12 лет")]
        [TestCase("13", "13 лет")]
        [TestCase("14", "14 лет")]
        [TestCase("15", "15 лет")]
        [TestCase("16", "16 лет")]
        [TestCase("17", "17 лет")]
        [TestCase("18", "18 лет")]
        [TestCase("19", "19 лет")]
        [TestCase("20", "20 лет")]
        [TestCase("21", "21 год")]
        [TestCase("22", "22 года")]
        [TestCase("23", "23 года")]
        [TestCase("24", "24 года")]
        [TestCase("25", "25 лет")]
        [TestCase("30", "30 лет")]
        [TestCase("33", "33 года")]
        [TestCase("35", "35 лет")]
        [TestCase("49", "49 лет")]
        [TestCase("50", "юбилей: 50 лет")]
        [TestCase("51", "51 год")]
        [TestCase("55", "юбилей: 55 лет")]
        [TestCase("80", "юбилей: 80 лет")]
        [TestCase("81", "81 год")]
        [TestCase("100", "юбилей: 100 лет")]
        [TestCase("101", "101 год")]
        [TestCase("102", "102 года")]
        [TestCase("103", "103 года")]
        [TestCase("105", "юбилей: 105 лет")]
        public void GetAge_SomeYears_ReturnsText(string year, string exp)
        {
            Program.YearNow = int.Parse(year)*2;
            string res = Program.GetAge(year);
            Assert.AreEqual(exp, res);
        }
    }
}
