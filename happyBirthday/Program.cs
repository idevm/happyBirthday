using System;
using System.Collections.Generic;

namespace happyBirthday
{
    public class Program
    {
        public static string DayNow { get; } = DateTime.Now.Day.ToString().Length == 2
            ? DateTime.Now.Day.ToString()
            : "0" + DateTime.Now.Day.ToString();

        public static string MonthNow { get; } = DateTime.Now.Month.ToString().Length == 2
            ? DateTime.Now.Month.ToString()
            : "0" + DateTime.Now.Month.ToString();

        public static int YearNow { get; set; } = DateTime.Now.Year;

        public static string[] Lines { get; set; }

        public static List<Dictionary<string, string>> People { get; set; } = new();

        public static string Text { get; set; }

        private static void Main(string[] args)
        {
            try
            {
                Lines = ReadFile("db.csv");
                People = GetPeopleList(Lines);
                Text = GetText(People);
                ShowText(Text);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                ShowText(ex.Message);
            }
            catch (FormatException ex)
            {
                ShowText(ex.Message);
            }
        }

        public static string[] ReadFile(string path)
        {
            try
            {
                return System.IO.File.ReadAllLines(path);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.IO.FileNotFoundException noFile = new("Не найден файл с данными, а именно: " + ex.Message);
                throw noFile;
            }
        }

        public static List<Dictionary<string, string>> GetPeopleList(string[] lines)
        {
            if (lines == null || lines.Length == 0)
            {
                throw new FormatException("Ошибка: отсутсвуют данные");
            }
            List<Dictionary<string, string>> people = new();
            foreach (string line in lines)
            {
                try
                {
                    string[] values = line.Split(";");
                    if (values.Length != 6)
                    {
                        throw new FormatException($"Неправильный формат таблицы: ошибка в строке  {Array.IndexOf(lines, line) + 1}");
                    }
                    string name = values[2];
                    if (name.Length <= 0)
                    {
                        throw new FormatException($"Отсутствует имя в строке {Array.IndexOf(lines, line) + 1}");
                    }
                    string day = values[4].Split(".")[0];
                    if (!int.TryParse(day, out int num) || day.Length != 2 || num > 31)
                    {
                        throw new FormatException($"Неправильный формат дня в строке {Array.IndexOf(lines, line) + 1}");
                    }
                    string month = values[4].Split(".")[1];
                    if (!int.TryParse(month, out num) || month.Length != 2 || num > 12)
                    {
                        throw new FormatException($"Неправильный формат месяца в строке {Array.IndexOf(lines, line) + 1}");
                    }
                    string year = values[4].Split(".")[2];
                    if (!int.TryParse(year, out num) || year.Length != 4 || num > YearNow)
                    {
                        throw new FormatException($"Неправильный формат года в строке {Array.IndexOf(lines, line) + 1}");
                    }
                    if (month == MonthNow && day == DayNow)
                    {
                        Dictionary<string, string> man = new();
                        man.Add("name", name);
                        man.Add("day", day);
                        man.Add("month", month);
                        man.Add("year", year);
                        people.Add(man);
                    }
                }
                catch(FormatException ex)
                {
                    FormatException badFormat = new("База данных повреждена или формат данных не соответствует установленному!\n" + ex.Message);
                    throw badFormat;
                }
            }
            return people;
        }

        public static string GetText(List<Dictionary<string, string>> people)
        {
            string text;
            if (people.Count > 1)
            {
                text = $"Сегодня {DayNow}.{MonthNow} отмечают день рождения:\n\n";
                foreach (Dictionary<string, string> man in people)
                {
                    text += "\t" + man["name"] + " (" + GetAge(man["year"]) + ")\n";
                }
            }
            else
            {
                text = people.Count == 1
                    ? $"Сегодня {DayNow}.{MonthNow} отмечает день рождения\n\n" + "\t" + people[0]["name"] + " (" + GetAge(people[0]["year"]) + ")\n"
                    : $"Сегодня {DayNow}.{MonthNow} никто не отмечает день рождения";
            }
            return text;
        }

        public static string GetAge(string year)
        {
            int age = YearNow - int.Parse(year);
            string str = age.ToString();
            if (age % 5 == 0 && age >= 50)
            {
                str = "юбилей: " + str;
            }
            if ((str.EndsWith('2') || str.EndsWith('3') || str.EndsWith('4')) && (!str.StartsWith('1') || str.Length == 3))
            {
                str += " года";
            }
            else if (str.EndsWith('1') && (!str.StartsWith('1') || str.Length == 1 || str.Length == 3))
            {
                str += " год";
            }
            else
            {
                str += " лет";
            }
            return str;
        }

        public static void ShowText(string txt)
        {
            Console.WriteLine(txt);
        }
    }
}