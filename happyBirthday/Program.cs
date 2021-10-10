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
                //createNewBirthDay
                app.Persons = app.AddPerson(app.Persons);
                app.WriteFile("db.csv", app.AddText(app.Persons));
            }
            catch (System.IO.FileNotFoundException ex)
            {
                app.ShowText(ex.Message);
            }
            catch (FormatException ex)
            {
                app.ShowText(ex.Message);
            }
        }
    }
}