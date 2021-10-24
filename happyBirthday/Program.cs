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
                app.Text = app.GetText(app.PeopleListFilter(app.Persons));
                Console.WriteLine(app.Text);
                //thisMonth
                app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.thisMonth), Mode.thisMonth);
                Console.WriteLine(app.Text);
                //thisYear
                app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.all), Mode.all);
                Console.WriteLine(app.Text);
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
                                app.Text = app.GetText(app.FindPersonByName(app.Persons, name), Mode.findResults);
                                Console.WriteLine(app.Text);
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
                                app.Text = app.GetText(app.FindPersonByDate(app.Persons, day, month), Mode.findResults);
                                Console.WriteLine(app.Text);
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
                                app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.today), Mode.today);
                                Console.WriteLine(app.Text);
                            }
                            else if (choose == "m")
                            {
                                Console.WriteLine("Choose month:\n[1]jan\n[2]feb\n[3]mar\n[4]apr\n[5]may\n[6]jun\n[7]jul\n[8]aug\n[9]sep\n[10]oct\n[11]nov\n[12]dec\nPress [q] to quit. [enter]");
                                string m = Console.ReadLine();
                                switch (m)
                                {
                                    case "1":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.jan), Mode.jan);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "2":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.feb), Mode.feb);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "3":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.mar), Mode.mar);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "4":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.apr), Mode.apr);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "5":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.may), Mode.may);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "6":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.jun), Mode.jun);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "7":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.jul), Mode.jul);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "8":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.aug), Mode.aug);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "9":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.sep), Mode.sep);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "10":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.oct), Mode.oct);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "11":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.nov), Mode.nov);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "12":
                                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.dec), Mode.dec);
                                        Console.WriteLine(app.Text);
                                        break;
                                    case "q":
                                        break;
                                }
                            }
                            else if (choose == "a")
                            {
                                app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.all), Mode.all);
                                Console.WriteLine(app.Text);
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