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
                app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.thisYear), Mode.thisYear);
                Console.WriteLine(app.Text);
                //create person or delete
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
                        Console.WriteLine("Введите ФИО для поиска");
                        string name = Console.ReadLine().ToUpper();
                        app.Text = app.GetText(app.FindPerson(app.Persons, name), Mode.findResults);
                        Console.WriteLine(app.Text);
                    }
                    else if (answer == "r")
                    {
                        Console.WriteLine("Введите ФИО для удаления");
                        string name = Console.ReadLine().ToUpper();
                        List<Person> found = app.FindPerson(app.Persons, name);
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
                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, Mode.thisYear), Mode.thisYear);
                        Console.WriteLine(app.Text);
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