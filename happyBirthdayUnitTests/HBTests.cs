using System;
using System.Collections.Generic;
using happyBirthday;
using NUnit.Framework;

namespace happyBirthdayUnitTests
{
    [TestFixture]
    public class Tests
    {
        public static string ThisDay { get; } = DateTime.Now.Day.ToString().Length == 2
            ? DateTime.Now.Day.ToString()
            : "0" + DateTime.Now.Day.ToString();

        public static string ThisMonth { get; } = DateTime.Now.Month.ToString().Length == 2
            ? DateTime.Now.Month.ToString()
            : "0" + DateTime.Now.Month.ToString();


        [TestCase("db")][TestCase("db.")][TestCase("d.csv")]
        [TestCase("db.xls")][TestCase("db.txt")][TestCase("../db.csv")]
        public void ReadFile_BadFiles_ThrowException(string path)
        {
            Exception ex = Assert.Catch(() => Program.ReadFile(path));
            StringAssert.Contains("Не найден файл с данными, а именно: ", ex.Message);
        }


        [Test]
        public void GetPeopleList_OneMan_ReturnsPeopleList()
        {
            string[] lines = new string[1] { $"1;;Ivan;;{ThisDay}.{ThisMonth}.2000;" };
            List<Dictionary<string, string>> exp = new();
            Dictionary<string, string> man = new();
            man.Add("name", "Ivan");
            man.Add("day", ThisDay);
            man.Add("month", ThisMonth);
            man.Add("year", "2000");
            exp.Add(man);
            List<Dictionary<string, string>> res = Program.GetPeopleList(lines);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetPeopleList_MoreMen_ReturnsPeopleList()
        {
            string[] lines = new string[2] { $"1;;Ivan;;{ThisDay}.{ThisMonth}.1990;", $"2;;Iva;;{ThisDay}.{ThisMonth}.1991;" };
            List<Dictionary<string, string>> exp = new();
            foreach (string line in lines)
            {
                string[] values = line.Split(";");
                Dictionary<string, string> man = new();
                man.Add("name", values[2]);
                man.Add("day", values[4].Split(".")[0]);
                man.Add("month", values[4].Split(".")[1]);
                man.Add("year", values[4].Split(".")[2]);
                exp.Add(man);
            }
            List<Dictionary<string, string>> res = Program.GetPeopleList(lines);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetPeopleList_ValidDataButNoBirthdays_ReturnsEmpty()
        {
            string tomorrow = (int.Parse(ThisDay) + 1).ToString().Length == 2
            ? (int.Parse(ThisDay) + 1).ToString()
            : "0" + (int.Parse(ThisDay) + 1).ToString();
            string[] lines = new string[2] { $"1;;Ivan;;{tomorrow}.{ThisMonth}.1990;", $"2;;Iva;;{tomorrow}.{ThisMonth}.1991;" };
            List<Dictionary<string, string>> exp = new();
            List<Dictionary<string, string>> res = Program.GetPeopleList(lines);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetPeopleList_NoData_ThrowsExceptiion()
        {
            string[] lines = Array.Empty<string>();
            Exception ex = Assert.Catch(() => Program.GetPeopleList(lines));
            StringAssert.Contains("Ошибка: отсутсвуют данные", ex.Message);
        }


        [Test]
        public void GetPeopleList_NullData_ThrowsExceptiion()
        {
            string[] lines = null;
            Exception ex = Assert.Catch(() => Program.GetPeopleList(lines));
            StringAssert.Contains("Ошибка: отсутсвуют данные", ex.Message);
        }


        [TestCase("1;;Ivan;;01.01.1990")][TestCase("1;;Ivan;;")]
        [TestCase("1;;Ivan;")][TestCase("1;;Ivan")][TestCase("1;;")]
        [TestCase("1;")][TestCase("1")][TestCase("")]
        public void GetPeopleList_BadFormatting_ThrowsExceptiion(string line)
        {
            string[] lines = new string[1] { line };
            Exception ex = Assert.Catch(() => Program.GetPeopleList(lines));
            StringAssert.Contains("Неправильный формат таблицы", ex.Message);
        }


        [Test]
        public void GetPeopleList_NoName_ThrowsExceptiion()
        {
            string[] lines = new string[1] { "1;;;;01.01.1990;" };
            Exception ex = Assert.Catch(() => Program.GetPeopleList(lines));
            StringAssert.Contains("Отсутствует имя в строке 1", ex.Message);
        }


        [TestCase("1;;Ivan;;.01.1990;")][TestCase("1;;Ivan;;001.01.1990;")]
        [TestCase("1;;Ivan;;f.01.1990;")][TestCase("1;;Ivan;;33.01.1990;")]
        public void GetPeopleList_BadDay_ThrowsExceptiion(string line)
        {
            string[] lines = new string[1] { line };
            Exception ex = Assert.Catch(() => Program.GetPeopleList(lines));
            StringAssert.Contains("Неправильный формат дня в строке 1", ex.Message);
        }


        [TestCase("1;;Ivan;;01..1990;")][TestCase("1;;Ivan;;01.010.1990;")]
        [TestCase("1;;Ivan;;01.h.1990;")][TestCase("1;;Ivan;;01.22.1990;")]
        public void GetPeopleList_BadMonth_ThrowsExceptiion(string line)
        {
            string[] lines = new string[1] { line };
            Exception ex = Assert.Catch(() => Program.GetPeopleList(lines));
            StringAssert.Contains("Неправильный формат месяца в строке 1", ex.Message);
        }


        [TestCase("1;;Ivan;;01.01.199;")][TestCase("1;;Ivan;;01.01.19902;")]
        [TestCase("1;;Ivan;;01.01.d;")][TestCase("1;;Ivan;;01.01.;")]
        public void GetPeopleList_BadYear_ThrowsExceptiion(string line)
        {
            string[] lines = new string[1] { line };
            Exception ex = Assert.Catch(() => Program.GetPeopleList(lines));
            StringAssert.Contains("Неправильный формат года в строке 1", ex.Message);
        }


        [Test]
        public void GetText_NoPeople_ReturnsText()
        {
            List<Dictionary<string, string>> people = new();
            string exp = $"Сегодня {ThisDay}.{ThisMonth} никто не отмечает день рождения";
            string res = Program.GetText(people);
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_OnePeople_ReturnsText()
        {
            List<Dictionary<string, string>> people = new();
            Dictionary<string, string> man = new();
            man.Add("name", "Ivan");
            man.Add("day", ThisDay);
            man.Add("month", ThisMonth);
            man.Add("year", "1990");
            people.Add(man);
            Program.YearNow = 2020;
            string exp = $"Сегодня {ThisDay}.{ThisMonth} отмечает день рождения\n\n" + "\t" + "Ivan" + " (" + "30 лет" + ")\n";
            string res = Program.GetText(people);
            Program.YearNow = DateTime.Now.Year;
            Assert.AreEqual(exp, res);
        }


        [Test]
        public void GetText_MorePeople_ReturnsText()
        {
            List<Dictionary<string, string>> people = new();
            Dictionary<string, string> man1 = new();
            Dictionary<string, string> man2 = new();
            man1.Add("name", "Ivan");
            man1.Add("day", ThisDay);
            man1.Add("month", ThisMonth);
            man1.Add("year", "1990");
            people.Add(man1);
            man2.Add("name", "Iva");
            man2.Add("day", ThisDay);
            man2.Add("month", ThisMonth);
            man2.Add("year", "1991");
            people.Add(man2);
            Program.YearNow = 2020;
            string exp = $"Сегодня {ThisDay}.{ThisMonth} отмечают день рождения:\n\n\tIvan (" + "30 лет" + ")\n\tIva (" + "29 лет" + ")\n";
            string res = Program.GetText(people);
            Program.YearNow = DateTime.Now.Year;
            Assert.AreEqual(exp, res);
        }


        [TestCase("0", "0 лет")][TestCase("1", "1 год")][TestCase("2", "2 года")]
        [TestCase("3", "3 года")][TestCase("4", "4 года")][TestCase("5", "5 лет")]
        [TestCase("6", "6 лет")][TestCase("7", "7 лет")][TestCase("8", "8 лет")]
        [TestCase("9", "9 лет")][TestCase("10", "10 лет")][TestCase("11", "11 лет")]
        [TestCase("12", "12 лет")][TestCase("13", "13 лет")][TestCase("14", "14 лет")]
        [TestCase("15", "15 лет")][TestCase("16", "16 лет")][TestCase("17", "17 лет")]
        [TestCase("18", "18 лет")][TestCase("19", "19 лет")][TestCase("20", "20 лет")]
        [TestCase("21", "21 год")][TestCase("22", "22 года")][TestCase("23", "23 года")]
        [TestCase("24", "24 года")][TestCase("25", "25 лет")][TestCase("30", "30 лет")]
        [TestCase("33", "33 года")][TestCase("35", "35 лет")][TestCase("49", "49 лет")]
        [TestCase("50", "юбилей: 50 лет")][TestCase("51", "51 год")][TestCase("55", "юбилей: 55 лет")]
        [TestCase("80", "юбилей: 80 лет")][TestCase("81", "81 год")][TestCase("100", "юбилей: 100 лет")]
        [TestCase("101", "101 год")][TestCase("102", "102 года")][TestCase("103", "103 года")]
        [TestCase("105", "юбилей: 105 лет")][TestCase("205", "юбилей: 205 лет")][TestCase("301", "301 год")]
        public void GetAge_SomeYears_ReturnsText(string year, string exp)
        {
            Program.YearNow = int.Parse(year) * 2;
            string res = Program.GetAge(year);
            Program.YearNow = DateTime.Now.Year;
            Assert.AreEqual(exp, res);
        }


        [TestCase("Ivan", "01.01.1990", "1;;IVAN;;01.01.1990;\n")]
        public void AddText_NoDataOneValidInput_ReturnsText(string name, string birthday, string exp)
        {
            string[] lines = new string[0];
            string res = Program.AddText(lines, name, birthday);
            Assert.AreEqual(exp, res);
        }


        [TestCase("Ivan", "01.01.1990", "1;;IVAN;;01.01.1990;\n")]
        public void AddText_NullDataOneValidInput_ReturnsText(string name, string birthday, string exp)
        {
            string[] lines = null;
            string res = Program.AddText(lines, name, birthday);
            Assert.AreEqual(exp, res);
        }


        [TestCase("Ivan", "01.01.1990", "3;;IVAN;;01.01.1990;\n")]
        public void AddText_SomeDataOneValidInput_ReturnsText(string name, string birthday, string exp)
        {
            string[] lines = new string[2] { "1;;Boris;;01.01.1990;\n", "2;;Vlad;;01.02.1980;\n" };
            string res = Program.AddText(lines, name, birthday);
            Assert.AreEqual(exp, res);
        }


        [TestCase("", "01.01.1990")]
        public void AddText_BadNameInput_ThrowsException(string name, string birthday)
        {
            string[] lines = new string[0];
            Exception ex = Assert.Catch(() => Program.AddText(lines, name, birthday));
            StringAssert.Contains("Введены некорректные данные\nВведите корректные ФИО (например Иванов Иван Иванович)", ex.Message);
        }


        [TestCase("Ivan", "0.01.1990")][TestCase("Ivan", "01.1.1990")][TestCase("Ivan", "01.01.199")]
        [TestCase("Ivan", "01.01")][TestCase("Ivan", "01.")][TestCase("Ivan", ".01.1990")]
        [TestCase("Ivan", "01.f.1990")][TestCase("Ivan", "01..1990")][TestCase("Ivan", "01.01.")]
        [TestCase("Ivan", "")][TestCase("Ivan", "01011990")][TestCase("Ivan", "01,01,1990")]
        [TestCase("Ivan", "1.1.1990")][TestCase("Ivan", "f")][TestCase("Ivan","..")]
        public void AddText_BadBirthdayInputs_ThrowsException(string name, string birthday)
        {
            string[] lines = new string[0];
            Exception ex = Assert.Catch(() => Program.AddText(lines, name, birthday));
            StringAssert.Contains("Введены некорректные данные\nВведите корректную дату рождения (например 02.08.1999)", ex.Message);
        }
    }
}
