using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD4
{
    //Klase Student tiek mantota no klases Person
    public class Student : Person
    {
        //Atribūti:
        string studentIdNumber;

        public Student() { }

        //Konstruktors, kas kā parametru saņem visu īpašību vērtības, kas tiek uzstādītas jaunajam objektam
        public Student(string n, string sn, GenderEnum g, string ID)
        {
            Name = n;
            Surname = sn;
            Gender = g;
            studentIdNumber = ID;
        }

        //Konstruktors, kas kā parametru pieņem tikai studenta ID numuru
        public Student(string ID)
        {
            studentIdNumber = ID;
        }

        //Īpašības:
        public int Id { get; set; } //Šis tiek izmantots kā primārā atslēga tabulā "Student"

        public string StudentIdNumber
        {
            get
            {
                return studentIdNumber;
            }

            set
            {
                studentIdNumber = value;
            }
        }

        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();

        //Metode ToString()
        public override string ToString()
        {
            return "Name: " + Name + ", Surname: " + Surname + ", Full Name: " + FullName + ", Gender: " + Gender + ", Student ID Number: " + StudentIdNumber;
        }
    }
}
