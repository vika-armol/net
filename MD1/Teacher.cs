using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MD1
{
    //Klase Teacher tiek mantota no klases Person
    public class Teacher : Person
    {
        //Atribūti: (1)
        DateOnly contractDate;

        //Īpašības:
        public int Id { get; set; } ////Šī tiek izmantota kā primārā atslēga tabulā "Teacher"
        public DateOnly ContractDate
        {
            get
            {
                return contractDate;
            }

            set
            {
                contractDate = value;
            }
        }

        public ICollection<Course> Courses { get; set; } = new List<Course>();

        //Metode ToString()
        public override string ToString()
        {
            return "Name: " + Name + ", Surname: " + Surname + ", Full Name: " + FullName + ", Gender: " + Gender + ", Contract Date: " + ContractDate;
        }
    }
}

/*
Atsauces:
    1. https://dl.ebooksworld.ir/books/CSharp.12.in.a.Nutshell.The.Definitive.Reference.9781098147440.pdf, llp. 312
*/