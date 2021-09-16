using System;
using System.Collections.Generic;

namespace happyBirthday
{
    public class Program
    {
        private string DayNow { get; } = DateTime.Now.Day.ToString().Length == 2
            ? DateTime.Now.Day.ToString()
            : "0" + DateTime.Now.Day.ToString();

        private string MonthNow { get; } = DateTime.Now.Month.ToString().Length == 2
            ? DateTime.Now.Month.ToString()
            : "0" + DateTime.Now.Month.ToString();

        public int YearNow { get; set; } = DateTime.Now.Year;

        private string[] Lines { get; set; }

        private List<Dictionary<string, string>> People { get; set; } = new();

        private string Text { get; set; }


        private static void Main()
        {
            Program p = new();
            try
            {
                p.Lines = ReadFile("db.csv");
                p.People = p.GetPeopleList(p.Lines);
                p.Text = p.GetTextToday(p.People);
                ShowText(p.Text);
                p.People = p.GetPeopleList(p.Lines, "thisMonth");
                p.Text = p.GetTextThisMonth(p.People);
                ShowText(p.Text);
                WriteFile("db.csv", p.AddText(p.Lines));
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


        public static void ShowText(string txt) => Console.WriteLine(txt);


        public static string GetTextFromUser() => Console.ReadLine();


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


        public static void WriteFile(string path, string txt) => System.IO.File.AppendAllText(path, txt);


        public List<Dictionary<string, string>> GetPeopleList(string[] lines, string mode = "today")
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
                    if (mode == "today")
                    {
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
                    else
                    {
                        if (month == MonthNow)
                        {
                            Dictionary<string, string> man = new();
                            man.Add("name", name);
                            man.Add("day", day);
                            man.Add("month", month);
                            man.Add("year", year);
                            people.Add(man);
                        }
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


        public string GetTextToday(List<Dictionary<string, string>> people)
        {
            string text;
            if (people.Count > 1)
            {
                text = $"Сегодня {DayNow}.{MonthNow} отмечают день рождения:\n\n";
                foreach (Dictionary<string, string> man in people)
                {
                    text += $"\t{man["name"]} ({ GetAge(man["year"])})\n";
                }
            }
            else
            {
                text = people.Count == 1
                    ? $"Сегодня {DayNow}.{MonthNow} отмечает день рождения\n\n\t{ people[0]["name"]} ({GetAge(people[0]["year"])})\n"
                    : $"Сегодня {DayNow}.{MonthNow} никто не отмечает день рождения";
            }
            return text;
        }


        public string GetTextThisMonth(List<Dictionary<string, string>> people)
        {
            string text;
            if (people.Count > 1)
            {
                text = $"В этом месяце отмечают день рождения:\n\n";
                foreach (Dictionary<string, string> man in people)
                {
                    text += $"\t{man["name"]} ({man["day"]}.{man["month"]})\n";
                }
            }
            else
            {
                text = people.Count == 1
                    ? $"В этом месяце отмечает день рождения\n\n\t{people[0]["name"]} ({people[0]["day"]}.{people[0]["month"]})\n"
                    : $"В этом месяце никто не отмечает день рождения";
            }
            return text;
        }


        public string GetAge(string year)
        {
            int age = YearNow - int.Parse(year);
            string str = age.ToString();
            if (age % 5 == 0 && age >= 50)
            {
                str = "юбилей: " + str;
            }
            if ((str.EndsWith('2') || str.EndsWith('3') || str.EndsWith('4')) && (!str.StartsWith('1')
                || str.Length == 3))
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


        public string AddText(string[] lines, string name, string birthday)
        {
            if (lines == null)
            {
                lines = Array.Empty<string>();
            }
            string num = "1";
            if (lines.Length != 0)
            {
                num = (int.Parse(lines[lines.Length - 1].Split(";")[0]) + 1).ToString();
            }
            try
            {
                if (!ValidInput(name: name))
                {
                    throw new FormatException("Введите корректные ФИО (например Иванов Иван Иванович)");
                }
                if (!ValidInput(birthday: birthday))
                {
                    throw new FormatException("Введите корректную дату рождения (например 02.08.1999)");
                }
            }
            catch (FormatException ex)
            {
                FormatException badInput = new("Введены некорректные данные\n" + ex.Message);
                throw badInput;
            }
            string result = $"{num};;{name.ToUpper()};;{birthday};\n";
            return result;
        }


        public string AddText(string[] lines)
        {
            if (lines == null)
            {
                lines = Array.Empty<string>();
            }
            string num = "1";
            if (lines.Length != 0)
            {
                num = (int.Parse(lines[lines.Length - 1].Split(";")[0]) + 1).ToString();
            }
            ShowText("Введите ФИО");
            string name = GetTextFromUser();
            while (true)
            {
                if (!ValidInput(name: name))
                {
                    ShowText("Введите корректные ФИО (например Иванов Иван Иванович):");
                    name = GetTextFromUser().ToUpper();
                }
                else
                {
                    break;
                }
            }
            ShowText("Введите дату рождения (в формате дд.мм.гггг):");
            string birthday = GetTextFromUser();
            while (true)
            {
                if (!ValidInput(birthday: birthday))
                {
                    ShowText("Введите корректную дату рождения (например 02.08.1999):");
                    birthday = GetTextFromUser();
                }
                else
                {
                    break;
                }
            }
            string result = $"{num};;{name.ToUpper()};;{birthday};\n";
            return result;
        }


        public bool ValidInput(string name = "nameParam", string birthday = "birthdayParam")
        {
            if (name == "nameParam" && birthday == "birthdayParam") return false;
            if (name != "nameParam")
            {
                if (name.Length < 1) return false;
            }
            if (birthday != "birthdayParam")
            {
                if (birthday.Length < 1 || birthday.Split(".").Length != 3) return false;
                string day = birthday.Split(".")[0];
                string month = birthday.Split(".")[1];
                string year = birthday.Split(".")[2];
                if (!int.TryParse(day, out int d)
                || day.Length != 2
                || d > 31
                || !int.TryParse(month, out int m)
                || month.Length != 2
                || m > 12
                || !int.TryParse(year, out int y)
                || year.Length != 4
                || y > YearNow)
                {
                    return false;
                }
            }
            return true;
        }
    }
}