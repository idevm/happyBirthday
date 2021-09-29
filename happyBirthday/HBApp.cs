using System;
using System.Collections.Generic;
using System.Text;

namespace happyBirthday
{
    public enum TimeMode
    {
        today,
        thisMonth,
        thisYear
    }


    public class HBApp
    {
        private List<string> lines;
        private List<Dictionary<string, string>> people = new();
        private string text;

        public string DayNow { get; set; } = DateTime.Now.Day.ToString().Length == 2
                    ? DateTime.Now.Day.ToString()
                    : "0" + DateTime.Now.Day.ToString();
        public string MonthNow { get; set; } = DateTime.Now.Month.ToString().Length == 2
            ? DateTime.Now.Month.ToString()
            : "0" + DateTime.Now.Month.ToString();

        public int YearNow { get; set; } = DateTime.Now.Year;

        public List<string> Lines { get => lines; set => lines = value; }

        public List<Dictionary<string, string>> People { get => people; set => people = value; }

        public string Text { get => text; set => text = value; }


        public void ShowText(string txt) => Console.WriteLine(txt);


        public static string GetTextFromUser() => Console.ReadLine();


        public List<string> ReadFile(string path)
        {
            try
            {
                List<string> res = new();
                res.AddRange(System.IO.File.ReadAllLines(path));
                return res;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.IO.FileNotFoundException noFile = new("Не найден файл с данными, а именно: " + ex.Message);
                throw noFile;
            }
        }


        public void WriteFile(string path, string txt) => System.IO.File.AppendAllText(path, txt);


        public List<Dictionary<string, string>> GetPeopleList(List<string> lines, TimeMode tm = TimeMode.today)
        {
            if (lines == null || lines.Count == 0)
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
                        throw new FormatException($"Неправильный формат таблицы: ошибка в строке  {lines.IndexOf(line) + 1}");
                    }
                    string name = values[2];
                    if (name.Length <= 0)
                    {
                        throw new FormatException($"Отсутствует имя в строке {lines.IndexOf(line) + 1}");
                    }
                    string day = values[4].Split(".")[0];
                    if (!int.TryParse(day, out int num) || day.Length != 2 || num > 31)
                    {
                        throw new FormatException($"Неправильный формат дня в строке {lines.IndexOf(line) + 1}");
                    }
                    string month = values[4].Split(".")[1];
                    if (!int.TryParse(month, out num) || month.Length != 2 || num > 12)
                    {
                        throw new FormatException($"Неправильный формат месяца в строке {lines.IndexOf(line) + 1}");
                    }
                    string year = values[4].Split(".")[2];
                    if (!int.TryParse(year, out num) || year.Length != 4 || num > YearNow)
                    {
                        throw new FormatException($"Неправильный формат года в строке {lines.IndexOf(line) + 1}");
                    }
                    switch (tm)
                    {
                        case TimeMode.today:
                            if (month == MonthNow && day == DayNow)
                            {
                                Dictionary<string, string> manD = new()
                                {
                                    ["name"] = name,
                                    ["day"] = day,
                                    ["month"] = month,
                                    ["year"] = year
                                };
                                people.Add(manD);
                            }
                            break;
                        case TimeMode.thisMonth:
                            if (month == MonthNow)
                            {
                                Dictionary<string, string> manM = new()
                                {
                                    ["name"] = name,
                                    ["day"] = day,
                                    ["month"] = month,
                                    ["year"] = year
                                };
                                people.Add(manM);
                            }
                            break;
                        case TimeMode.thisYear:
                            Dictionary<string, string> manY = new()
                            {
                                ["name"] = name,
                                ["day"] = day,
                                ["month"] = month,
                                ["year"] = year
                            };
                            people.Add(manY);
                            break;
                    }
                }
                catch (FormatException ex)
                {
                    FormatException badFormat = new("База данных повреждена или формат данных не соответствует установленному!\n" + ex.Message);
                    throw badFormat;
                }
            }
            return people;
        }


        public string GetText(List<Dictionary<string, string>> people, TimeMode tm = TimeMode.today)
        {
            StringBuilder text = new();
            if (people.Count > 1)
            {
                switch (tm)
                {
                    case TimeMode.today:
                        text.Append($"Сегодня {DayNow}.{MonthNow} отмечают день рождения:\n\n");
                        foreach (Dictionary<string, string> man in people)
                        {
                            text.Append($"\t{man["name"]} ({ GetAge(man["year"])})\n");
                        }
                        break;
                    case TimeMode.thisMonth:
                        text.Append($"В этом месяце отмечают день рождения:\n\n");
                        foreach (Dictionary<string, string> man in people)
                        {
                            text.Append($"\t{man["name"]} ({man["day"]}.{man["month"]})\n");
                        }
                        break;
                    case TimeMode.thisYear:
                        text.Append($"В этом году отмечают день рождения:\n\n");
                        foreach (Dictionary<string, string> man in people)
                        {
                            text.Append($"\t{man["name"]} ({man["day"]}.{man["month"]})\n");
                        }
                        break;
                }
            }
            else
            {
                switch (tm)
                {
                    case TimeMode.today:
                        text.Append(people.Count == 1
                            ? $"Сегодня {DayNow}.{MonthNow} отмечает день рождения\n\n\t{ people[0]["name"]} ({GetAge(people[0]["year"])})\n"
                            : $"Сегодня {DayNow}.{MonthNow} никто не отмечает день рождения");
                        break;
                    case TimeMode.thisMonth:
                        text.Append(people.Count == 1
                            ? $"В этом месяце отмечает день рождения\n\n\t{people[0]["name"]} ({people[0]["day"]}.{people[0]["month"]})\n"
                            : $"В этом месяце никто не отмечает день рождения");
                        break;
                    case TimeMode.thisYear:
                        text.Append(people.Count == 1
                            ? $"В этом году отмечает день рождения\n\n\t{people[0]["name"]} ({people[0]["day"]}.{people[0]["month"]})\n"
                            : $"В этом году никто не отмечает день рождения");
                        break;
                }
            }
            return text.ToString();
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


        public string AddText(List<string> lines, string name, string birthday)
        {
            if (lines == null)
            {
                lines = new();
            }
            string num = "1";
            if (lines.Count != 0)
            {
                num = (int.Parse(lines[lines.Count - 1].Split(";")[0]) + 1).ToString();
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


        public string AddText(List<string> lines)
        {
            if (lines == null)
            {
                lines = new();
            }
            string num = "1";
            if (lines.Count != 0)
            {
                num = (int.Parse(lines[lines.Count - 1].Split(";")[0]) + 1).ToString();
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