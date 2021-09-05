using System;
using System.Collections.Generic;

namespace happyBirthday
{
    public class Program
    {
        public static string Path { get; set; } = "db.csv";
        public static string[] Lines { get; set; }
        public static List<Dictionary<string, string>> People { get; set; } = new();
        public static string DayNow { get; set; }
        public static string MonthNow { get; set; }
        public static int YearNow { get; set; }

        private static void Main(string[] args)
        {
            GetDateNow();
            try
            {
                ReadFile(Path);
                GetPeopleList();
                Console.WriteLine(GetText());
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void GetDateNow()
        {
            DayNow = DateTime.Now.Day.ToString();
            if (DayNow.Length < 2)
            {
                DayNow = "0" + DayNow;
            }
            MonthNow = DateTime.Now.Month.ToString();
            if (MonthNow.Length < 2)
            {
                MonthNow = "0" + MonthNow;
            }
            YearNow = DateTime.Now.Year;
        }

        public static void ReadFile(string path)
        {
            try
            {
                Lines = System.IO.File.ReadAllLines(path);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.IO.FileNotFoundException noFile = new("Не найден файл с данными, а именно: " + ex.Message);
                throw noFile;
            }
        }

        public static void GetPeopleList()
        {
            if (Lines.Length == 0)
            {
                throw new FormatException("Ошибка: отсутсвуют данные");
            }
            foreach (string line in Lines)
            {
                try
                {
                    string[] values = line.Split(";");
                    if (values.Length != 6)
                    {
                        throw new FormatException($"Неправильный формат таблицы: ошибка в строке  {Array.IndexOf(Lines, line) + 1}");
                    }
                    string name = values[2];
                    if (name.Length <= 0)
                    {
                        throw new FormatException($"Отсутствует имя в строке {Array.IndexOf(Lines, line) + 1}");
                    }
                    string day = values[4].Split(".")[0];
                    if (!int.TryParse(day, out int num) || day.Length != 2 || num > 31)
                    {
                        throw new FormatException($"Неправильный формат дня в строке {Array.IndexOf(Lines, line) + 1}");
                    }
                    string month = values[4].Split(".")[1];
                    if (!int.TryParse(month, out num) || month.Length != 2 || num > 12)
                    {
                        throw new FormatException($"Неправильный формат месяца в строке {Array.IndexOf(Lines, line) + 1}");
                    }
                    string year = values[4].Split(".")[2];
                    if (!int.TryParse(year, out num) || year.Length != 4 || num > YearNow)
                    {
                        throw new FormatException($"Неправильный формат года в строке {Array.IndexOf(Lines, line) + 1}");
                    }
                    if (month == MonthNow && day == DayNow)
                    {
                        Dictionary<string, string> man = new();
                        man.Add("name", name);
                        man.Add("day", day);
                        man.Add("month", month);
                        man.Add("year", year);
                        People.Add(man);
                    }
                }
                catch(FormatException ex)
                {
                    FormatException badFormat = new("База данных повреждена или формат данных не соответствует установленному!\n" + ex.Message);
                    throw badFormat;
                }
            }
        }

        public static string GetText()
        {
            string text;
            if (People.Count > 1)
            {
                text = $"Сегодня {DayNow}.{MonthNow} отмечают день рождения:\n\n";
                foreach (Dictionary<string, string> man in People)
                {
                    text += "\t" + man["name"] + " (" + GetAge(man["year"]) + ")\n";
                }
            }
            else
            {
                text = People.Count == 1
                    ? $"Сегодня {DayNow}.{MonthNow} отмечает день рождения\n\n" + "\t" + People[0]["name"] + " (" + GetAge(People[0]["year"]) + ")\n"
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
    }
}