using System;

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
                app.ShowText(app.Text);
                //thisMonth
                app.Text = app.GetText(app.PeopleListFilter(app.Persons, TimeMode.thisMonth), TimeMode.thisMonth);
                app.ShowText(app.Text);
                //thisYear
                app.Text = app.GetText(app.PeopleListFilter(app.Persons, TimeMode.thisYear), TimeMode.thisYear);
                app.ShowText(app.Text);
                //create person or delete
                while (true)
                {
                    app.ShowText($"[c]reate, [r]emove, [s]how or [q]uit? [enter]");
                    string answer = HBApp.GetTextFromUser();
                    if (answer == "c")
                    {
                        app.Persons = app.AddPerson(app.Persons);
                        app.WriteFile("db.csv", app.UpdateText(app.Persons));
                    }
                    else if (answer == "r")
                    {
                        app.Persons = app.RemovePerson(app.Persons);
                        app.WriteFile("db.csv", app.UpdateText(app.Persons));
                    }
                    else if (answer == "s")
                    {
                        app.Text = app.GetText(app.PeopleListFilter(app.Persons, TimeMode.thisYear), TimeMode.thisYear);
                        app.ShowText(app.Text);
                    }
                    else if (answer == "q")
                    {
                        break;
                    }
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                app.ShowText(ex.Message);
            }
            catch (FormatException ex)
            {
                app.ShowText(ex.Message);
            }
            catch (ArgumentException ex)
            {
                app.ShowText(ex.Message);
            }
        }
    }
}