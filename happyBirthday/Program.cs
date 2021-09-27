using System;

namespace happyBirthday
{
    class Program
    {
        private static void Main()
        {
            HBApp app = new();
            try
            {
                app.Lines = app.ReadFile("db.csv");
                //today
                app.People = app.GetPeopleList(app.Lines);
                app.Text = app.GetText(app.People);
                app.ShowText(app.Text);
                //thisMonth
                app.People = app.GetPeopleList(app.Lines, TimeMode.thisMonth);
                app.Text = app.GetText(app.People, TimeMode.thisMonth);
                app.ShowText(app.Text);
                //createNewBirthDay
                app.WriteFile("db.csv", app.AddText(app.Lines));
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