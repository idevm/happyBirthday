using System;

namespace happyBirthday
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines("db.csv");
                string message;
                string day = DateTime.Now.Day.ToString();
                if (day.Length < 2)
                {
                    day = "0" + day;
                }
                string month = DateTime.Now.Month.ToString();
                if (month.Length < 2)
                {
                    month = "0" + month;
                }
                int year = DateTime.Now.Year;
                message = "Сегодня отмечают День рождения:\n\n";
                foreach (string line in lines)
                {
                    string[] values = line.Split(";");
                    if (values[1].Split(".")[1] == month && values[1].Split(".")[0] == day)
                    {
                        message += "\t - " + values[0] +
                            " " + "(" + (year - int.Parse(values[1].Split(".")[2])) + ")" + "\n";

                    }
                }
                Console.WriteLine(message);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                Console.WriteLine("Не найден файл с данными, а именно: " + ex.Message);
            }
        }
    }
}
