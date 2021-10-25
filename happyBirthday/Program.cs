using System;
using System.Collections.Generic;

namespace happyBirthday
{
    internal class Program
    {
        private static void Main()
        {
            HBApp app = new();
            try
            {
                app.Persons = app.GetPeopleList(app.ReadFile("db.csv"));
                //today
                Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons)));
                //this month
                Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.thisMonth), Mode.thisMonth));
                //all
                Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.all), Mode.all));
                //menu
                while (true)
                {
                    Console.WriteLine($"\nHappyBirthdayApp\n[c]reate, [f]ind, [r]emove, [s]how or [q]uit? [enter]");
                    string answer = Console.ReadLine();
                    if (answer == "c")
                    {
                        Console.WriteLine("Введите ФИО");
                        string name = Console.ReadLine();
                        while (true)
                        {
                            if (!app.ValidInput(name: name))
                            {
                                Console.WriteLine("Введите корректные ФИО (например Иванов Иван Иванович):");
                                name = Console.ReadLine().ToUpper();
                            }
                            else
                            {
                                break;
                            }
                        }
                        Console.WriteLine("Введите дату рождения (в формате дд.мм.гггг):");
                        string birthday = Console.ReadLine();
                        while (true)
                        {
                            if (!app.ValidInput(birthday: birthday))
                            {
                                Console.WriteLine("Введите корректную дату рождения (например 02.08.1999):");
                                birthday = Console.ReadLine();
                            }
                            else
                            {
                                break;
                            }
                        }
                        app.Persons = app.AddPerson(app.Persons, name, birthday);
                        Console.WriteLine("Успешно добавлено");
                        app.WriteFile("db.csv", app.UpdateText(app.Persons));
                    }
                    else if (answer == "f")
                    {
                        while (true)
                        {
                            Console.WriteLine("Find by [n]ame or by [d]ate? Press [q] to quit. [enter]");
                            string choose = Console.ReadLine();
                            if (choose == "n")
                            {
                                Console.WriteLine("Введите ФИО для поиска");
                                string name = Console.ReadLine().ToUpper();
                                Console.WriteLine(app.GetText(app.FindPersonByName(app.Persons, name), Mode.findResults));
                            }
                            else if (choose == "d")
                            {
                                Console.WriteLine("Введите день");
                                int day = -1;
                                try
                                {
                                    day = int.Parse(Console.ReadLine());
                                }
                                catch (Exception) { }
                                Console.WriteLine("Введите месяц (опционально)");
                                int month = -1;
                                try
                                {
                                    month = int.Parse(Console.ReadLine());
                                }
                                catch (Exception) { }
                                Console.WriteLine(app.GetText(app.FindPersonByDate(app.Persons, day, month), Mode.findResults));
                            }
                            else if (choose == "q")
                            {
                                break;
                            }
                        }
                    }
                    else if (answer == "r")
                    {
                        Console.WriteLine("Введите ФИО для удаления");
                        string name = Console.ReadLine().ToUpper();
                        List<Person> found = app.FindPersonByName(app.Persons, name);
                        if (found.Count == 0)
                        {
                            Console.WriteLine("Совпадений не найдено");
                        }
                        int ndx = 1;
                        foreach (Person p in found)
                        {
                            Console.WriteLine($"Совпадение {ndx++} из {found.Count}:");
                            Console.WriteLine($"Удалить {p.Name} ({app.ToString(p.Birthday)}.{app.ToString(p.Birthmonth)}.{p.Birthyear})? [y/n]");
                            string answ = Console.ReadLine();
                            while (true)
                            {
                                if (answ == "y")
                                {
                                    app.Persons = app.RemovePerson(app.Persons, p);
                                    Console.WriteLine("Успешно удалено");
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        app.WriteFile("db.csv", app.UpdateText(app.Persons));
                    }
                    else if (answer == "s")
                    {
                        while (true)
                        {
                            Console.WriteLine("Show [t]oday, [m]onth or [a]ll? Press [q] to quit. [enter]");
                            string choose = Console.ReadLine();
                            if (choose == "t")
                            {
                                Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.today), Mode.today));
                            }
                            else if (choose == "m")
                            {
                                Console.WriteLine("Choose month:\n[1]jan\n[2]feb\n[3]mar\n[4]apr\n[5]may\n[6]jun\n[7]jul\n[8]aug\n[9]sep\n[10]oct\n[11]nov\n[12]dec\nPress [q] to quit. [enter]");
                                string m = Console.ReadLine();
                                switch (m)
                                {
                                    case "1":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.jan), Mode.jan));
                                        break;
                                    case "2":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.feb), Mode.feb));
                                        break;
                                    case "3":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.mar), Mode.mar));
                                        break;
                                    case "4":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.apr), Mode.apr));
                                        break;
                                    case "5":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.may), Mode.may));
                                        break;
                                    case "6":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.jun), Mode.jun));
                                        break;
                                    case "7":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.jul), Mode.jul));
                                        break;
                                    case "8":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.aug), Mode.aug));
                                        break;
                                    case "9":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.sep), Mode.sep));
                                        break;
                                    case "10":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.oct), Mode.oct));
                                        break;
                                    case "11":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.nov), Mode.nov));
                                        break;
                                    case "12":
                                        Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.dec), Mode.dec));
                                        break;
                                    case "q":
                                        break;
                                }
                            }
                            else if (choose == "a")
                            {
                                Console.WriteLine(app.GetText(app.PeopleListFilter(app.Persons, Mode.all), Mode.all));
                            }
                            else if (choose == "q")
                            {
                                break;
                            }
                        }
                    }
                    else if (answer == "q")
                    {
                        break;
                    }
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}