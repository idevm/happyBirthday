using System;
using System.Collections.Generic;
using System.Text;

namespace happyBirthday
{
    public enum Mode
    {
        today,
        thisMonth,
        jan,
        feb,
        mar,
        apr,
        may,
        jun,
        jul,
        aug,
        sep,
        oct,
        nov,
        dec,
        all,
        findResults
    }


    public class HBApp
    {
        private List<Person> persons = new();

        public int DayNow { get; set; } = DateTime.Now.Day;

        public int MonthNow { get; set; } = DateTime.Now.Month;

        public int YearNow { get; set; } = DateTime.Now.Year;

        public List<Person> Persons { get => persons; set => persons = value; }


        public void WriteFile(string path, string txt)
        {
            System.IO.File.WriteAllText(path, txt);
        }


        public List<string> ReadFile(string path)
        {
            try
            {
                List<string> lines = new();
                lines.AddRange(System.IO.File.ReadAllLines(path));
                return lines;
            }
            catch (System.IO.FileNotFoundException ex)
            {
                WriteFile("db.csv", "");
                System.IO.FileNotFoundException noFile = new("Не найден файл с данными, а именно: " + ex.Message);
                throw noFile;
            }
        }


        public List<Person> GetPeopleList(List<string> lines)
        {
            List<Person> persons = new(lines?.Count ?? 0);
            if (lines == null)
            {
                return persons;
            }
            foreach (string line in lines)
            {
                try
                {
                    string[] values = line.Split(";");
                    if (values.Length != 6)
                    {
                        throw new FormatException($"Неправильный формат таблицы: ошибка в строке  {lines.IndexOf(line) + 1}");
                    }
                    if (values[2].Length == 0)
                    {
                        throw new FormatException($"Отсутствует имя в строке {lines.IndexOf(line) + 1}");
                    }
                    if (!ValidateDay(values[4].Split(".")[0]))
                    {
                        throw new FormatException($"Неправильный формат дня в строке {lines.IndexOf(line) + 1}");
                    }
                    if (!ValidateMonth(values[4].Split(".")[1]))
                    {
                        throw new FormatException($"Неправильный формат месяца в строке {lines.IndexOf(line) + 1}");
                    }
                    if (!ValidateYear(values[4].Split(".")[2]))
                    {
                        throw new FormatException($"Неправильный формат года в строке {lines.IndexOf(line) + 1}");
                    }
                    Person p = new(values[2]);
                    p.Birthday = int.Parse(values[4].Split(".")[0]);
                    p.Birthmonth = int.Parse(values[4].Split(".")[1]);
                    p.Birthyear = int.Parse(values[4].Split(".")[2]);
                    p.number = int.Parse(values[0] ?? "0");
                    persons.Add(p);
                }
                catch (FormatException ex)
                {
                    FormatException badFormat = new("База данных повреждена или формат данных не соответствует установленному!\n" + ex.Message);
                    throw badFormat;
                }
            }
            return persons;
        }


        public List<Person> PeopleListFilter(
            List<Person> persons,
            Mode tm = Mode.today)
        {
            List<Person> filteredList = new();
            foreach (Person pers in persons)
            {
                switch (tm)
                {
                    case Mode.today:
                        if (pers.Birthmonth == MonthNow
                            && pers.Birthday == DayNow)
                        {
                            filteredList.Add(pers);
                        }
                        break;
                    case Mode.thisMonth:
                        if (pers.Birthmonth == MonthNow)
                        {
                            filteredList.Add(pers);
                        }
                        break;
                    case Mode.all:
                        return persons;
                    case Mode.jan:
                        if (pers.Birthmonth == 1) filteredList.Add(pers);
                        break;
                    case Mode.feb:
                        if (pers.Birthmonth == 2) filteredList.Add(pers);
                        break;
                    case Mode.mar:
                        if (pers.Birthmonth == 3) filteredList.Add(pers);
                        break;
                    case Mode.apr:
                        if (pers.Birthmonth == 4) filteredList.Add(pers);
                        break;
                    case Mode.may:
                        if (pers.Birthmonth == 5) filteredList.Add(pers);
                        break;
                    case Mode.jun:
                        if (pers.Birthmonth == 6) filteredList.Add(pers);
                        break;
                    case Mode.jul:
                        if (pers.Birthmonth == 7) filteredList.Add(pers);
                        break;
                    case Mode.aug:
                        if (pers.Birthmonth == 8) filteredList.Add(pers);
                        break;
                    case Mode.sep:
                        if (pers.Birthmonth == 9) filteredList.Add(pers);
                        break;
                    case Mode.oct:
                        if (pers.Birthmonth == 10) filteredList.Add(pers);
                        break;
                    case Mode.nov:
                        if (pers.Birthmonth == 11) filteredList.Add(pers);
                        break;
                    case Mode.dec:
                        if (pers.Birthmonth == 12) filteredList.Add(pers);
                        break;
                    case Mode.findResults:
                        break;
                }
            }
            return filteredList;
        }


        public string GetText(List<Person> persons, Mode tm = Mode.today)
        {
            StringBuilder text = new("");
            if (persons.Count > 1)
            {
                switch (tm)
                {
                    case Mode.today:
                        text.Append($"Сегодня {ToString(DayNow)}.{ToString(MonthNow)} отмечают день рождения:\n\n");
                        break;
                    case Mode.thisMonth:
                        text.Append($"В этом месяце отмечают день рождения:\n\n");
                        break;
                    case Mode.all:
                        text.Append($"Все дни рождения:\n\n");
                        break;
                    case Mode.findResults:
                        text.Append($"Результат поиска:\n\n");
                        break;
                    case Mode.jan:
                        text.Append($"В январе отмечают день рождения:\n\n");
                        break;
                    case Mode.feb:
                        text.Append($"В феврале отмечают день рождения:\n\n");
                        break;
                    case Mode.mar:
                        text.Append($"В марте отмечают день рождения:\n\n");
                        break;
                    case Mode.apr:
                        text.Append($"В апреле отмечают день рождения:\n\n");
                        break;
                    case Mode.may:
                        text.Append($"В мае отмечают день рождения:\n\n");
                        break;
                    case Mode.jun:
                        text.Append($"В июне отмечают день рождения:\n\n");
                        break;
                    case Mode.jul:
                        text.Append($"В июле отмечают день рождения:\n\n");
                        break;
                    case Mode.aug:
                        text.Append($"В августе отмечают день рождения:\n\n");
                        break;
                    case Mode.sep:
                        text.Append($"В сентябре отмечают день рождения:\n\n");
                        break;
                    case Mode.oct:
                        text.Append($"В октябре отмечают день рождения:\n\n");
                        break;
                    case Mode.nov:
                        text.Append($"В ноябре отмечают день рождения:\n\n");
                        break;
                    case Mode.dec:
                        text.Append($"В декабре отмечают день рождения:\n\n");
                        break;
                }
                foreach (Person p in persons)
                {
                    switch (tm)
                    {
                        case Mode.today:
                            _ = p.Birthyear != 0
                                ? text.Append($"\t{p.Name} ({p.GetAge(YearNow)})\n")
                                : text.Append($"\t{p.Name}\n");
                            break;
                        case Mode.thisMonth:
                            text.Append($"\t{p.Name} ({ToString(p.Birthday)}.{ToString(p.Birthmonth)})\n");
                            break;
                        case Mode.all:
                            _ = p.Birthyear != 0
                                ? text.Append($"\t{p.Name} ({ToString(p.Birthday)}.{ToString(p.Birthmonth)}.{ToString(p.Birthyear)})\n")
                                : text.Append($"\t{p.Name} ({ToString(p.Birthday)}.{ToString(p.Birthmonth)})\n");
                            break;
                        case Mode.findResults:
                            _ = p.Birthyear != 0
                                ? text.Append($"\t{p.Name} ({ToString(p.Birthday)}.{ToString(p.Birthmonth)}.{ToString(p.Birthyear)})\n")
                                : text.Append($"\t{p.Name} ({ToString(p.Birthday)}.{ToString(p.Birthmonth)})\n");
                            break;
                        case Mode.jan:
                        case Mode.feb:
                        case Mode.mar:
                        case Mode.apr:
                        case Mode.may:
                        case Mode.jun:
                        case Mode.jul:
                        case Mode.aug:
                        case Mode.sep:
                        case Mode.oct:
                        case Mode.nov:
                        case Mode.dec:
                            text.Append($"\t{p.Name} ({ToString(p.Birthday)}.{ToString(p.Birthmonth)})\n");
                            break;
                    }
                }
            }
            else
            {
                switch (tm)
                {
                    case Mode.today:
                        text.Append(persons.Count == 1
                            ? persons[0].Birthyear != 0
                            ? $"Сегодня {ToString(DayNow)}.{ToString(MonthNow)} отмечает день рождения\n\n\t{ persons[0].Name} ({persons[0].GetAge(YearNow)})\n"
                            : $"Сегодня {ToString(DayNow)}.{ToString(MonthNow)} отмечает день рождения\n\n\t{ persons[0].Name} \n"
                            : $"Сегодня {ToString(DayNow)}.{ToString(MonthNow)} никто не отмечает день рождения");
                        break;
                    case Mode.thisMonth:
                        text.Append(persons.Count == 1
                            ? $"В этом месяце отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В этом месяце никто не отмечает день рождения");
                        break;
                    case Mode.all:
                        text.Append(persons.Count == 1
                            ? persons[0].Birthyear != 0
                            ? $"Все дни рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)}.{ToString(persons[0].Birthyear)})\n"
                            : $"Все дни рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"Нет записей о днях рождения");
                        break;
                    case Mode.findResults:
                        text.Append(persons.Count == 1
                            ? persons[0].Birthyear != 0
                            ? $"Результат поиска:\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)}.{ToString(persons[0].Birthyear)})\n"
                            : $"Результат поиска:\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"Совпадений не найдено");
                        break;
                    case Mode.jan:
                        text.Append(persons.Count == 1
                            ? $"В январе отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В январе никто не отмечает день рождения");
                        break;
                    case Mode.feb:
                        text.Append(persons.Count == 1
                            ? $"В феврале отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В феврале никто не отмечает день рождения");
                        break;
                    case Mode.mar:
                        text.Append(persons.Count == 1
                            ? $"В марте отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В марте никто не отмечает день рождения");
                        break;
                    case Mode.apr:
                        text.Append(persons.Count == 1
                            ? $"В апреле отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В апреле никто не отмечает день рождения");
                        break;
                    case Mode.may:
                        text.Append(persons.Count == 1
                            ? $"В мае отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В мае никто не отмечает день рождения");
                        break;
                    case Mode.jun:
                        text.Append(persons.Count == 1
                            ? $"В июне отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В июне никто не отмечает день рождения");
                        break;
                    case Mode.jul:
                        text.Append(persons.Count == 1
                            ? $"В июле отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В июле никто не отмечает день рождения");
                        break;
                    case Mode.aug:
                        text.Append(persons.Count == 1
                            ? $"В августе отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В августе никто не отмечает день рождения");
                        break;
                    case Mode.sep:
                        text.Append(persons.Count == 1
                            ? $"В сентябре отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В сентябре никто не отмечает день рождения");
                        break;
                    case Mode.oct:
                        text.Append(persons.Count == 1
                            ? $"В октябре отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В октябре никто не отмечает день рождения");
                        break;
                    case Mode.nov:
                        text.Append(persons.Count == 1
                            ? $"В ноябре отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В ноябре никто не отмечает день рождения");
                        break;
                    case Mode.dec:
                        text.Append(persons.Count == 1
                            ? $"В декабре отмечает день рождения\n\n\t{persons[0].Name} ({ToString(persons[0].Birthday)}.{ToString(persons[0].Birthmonth)})\n"
                            : $"В декабре никто не отмечает день рождения");
                        break;
                }
            }
            return text.ToString();
        }


        public List<Person> AddPerson(
            List<Person> persons,
            string name,
            string birthday)
        {
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
            Person p = new(name.ToUpper());
            string[] bdate = birthday.Split(".");
            p.Birthday = int.Parse(bdate[0]);
            p.Birthmonth = int.Parse(bdate[1]);
            p.Birthyear = bdate.Length == 3 ? int.Parse(bdate[2]) : 0;
            persons ??= new();
            persons.Add(p);
            return persons;
        }


        public List<Person> RemovePerson(
            List<Person> persons,
            Person personToRemove)
        {
            persons?.Remove(personToRemove);
            return persons ?? new();
        }


        public List<Person> FindPersonByName(List<Person> persons, string name)
        {
            return persons?.FindAll(x => x.Name.Contains(name)) ?? new();
        }


        public List<Person> FindPersonByDate(
            List<Person> persons,
            int day = -1,
            int month = -1,
            int year = -1)
        {
            return persons?.FindAll(x =>
            {
                return (ToString(x.Birthday) == ToString(day) || day == -1)
                && (ToString(x.Birthmonth) == ToString(month) || month == -1)
                && (ToString(x.Birthyear) == ToString(year) || year == -1)
                && day + month + year != -3;
            }) ?? new();
        }


        public string UpdateText(List<Person> persons)
        {
            StringBuilder text = new("");
            if (persons == null)
            {
                return text.ToString();
            }
            persons.Sort();
            for (int i = 0; i < persons.Count; i++)
            {
                persons[i].number = i + 1;
            }
            foreach (Person p in persons)
            {
                text.Append($"{p.number};;{p.Name};;{ToString(p.Birthday)}.{ToString(p.Birthmonth)}.{p.Birthyear};\n");
            }
            return text.ToString();
        }


        public bool ValidInput(
            string name = "nameParam",
            string birthday = "birthdayParam")
        {
            if (name == "nameParam" && birthday == "birthdayParam")
            {
                return false;
            }
            if (name != "nameParam" && name.Length == 0)
            {
                return false;
            }
            if (birthday != "birthdayParam")
            {
                if (birthday.Length < 1
                    || birthday.Split(".").Length > 3
                    || birthday.Split(".").Length < 2)
                {
                    return false;
                }
                if (birthday.Split(".").Length == 3)
                {
                    if (!ValidateDay(birthday.Split(".")[0])
                        || !ValidateMonth(birthday.Split(".")[1])
                        || !ValidateYear(birthday.Split(".")[2]))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!ValidateDay(birthday.Split(".")[0])
                        || !ValidateMonth(birthday.Split(".")[1]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        public bool ValidateDay(string d)
        {
            return int.TryParse(d, out int intD)
            && d.Length == 2
            && !d.Contains("-")
            && !d.Contains("+")
            && intD <= 31;
        }


        public bool ValidateMonth(string m)
        {
            return int.TryParse(m, out int intM)
            && m.Length == 2
            && !m.Contains("-")
            && !m.Contains("+")
            && intM <= 12;
        }


        public bool ValidateYear(string y)
        {
            return int.TryParse(y, out int intY)
            && (y.Length == 4 || intY == 0)
            && !y.Contains("-")
            && !y.Contains("+")
            && intY <= YearNow;
        }


        public string ToString(int num)
        {
            return num.ToString().Length >= 2
                ? num.ToString()
                : "0" + num.ToString();
        }
    }
}