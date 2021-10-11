using System;
namespace happyBirthday
{
    public class Person: IComparable
    {
        public Person(string name)
        {
            Name = name;
        }

        private string name;
        private int birthday;
        private int birthmonth;
        private int birthyear;

        public string Name { get => name; set => name = value; }

        public int Birthday { get => birthday; set => birthday = value; }

        public int Birthmonth { get => birthmonth; set => birthmonth = value; }

        public int Birthyear { get => birthyear; set => birthyear = value; }

        public int number;


        public string GetAge(int yearNow)
        {
            int age = yearNow - Birthyear;
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


        public int CompareTo(object obj)
        {
            Person temp = obj as Person;
            if (temp!=null)
            {
                if (this.Birthmonth > temp.Birthmonth)
                {
                    return 1;
                }
                else if (this.Birthmonth<temp.Birthmonth)
                {
                    return -1;
                }
                else
                {
                    if (this.Birthday > temp.Birthday)
                    {
                        return 1;
                    }
                    else if (this.Birthday < temp.Birthday)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                throw new ArgumentException("параметр не является Person");
            }
        }
    }
}
