using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD1
{
    public abstract class Person
    {
        //Atribūti:
        private string name;
        private string surname;
        private GenderEnum gender;

        //īpašības:
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                //Pārbauda, vai vērtība ir null vai tukša pirms tās iestatīšanas (1)
                if (!string.IsNullOrEmpty(value))
                {
                    name = value;
                }
            }
        }

        public string Surname
        {
            get
            {
                return surname;
            }

            set
            {
                surname = value;
            }
        }
        public string FullName
        {
            get
            {
                return (Name + " " + Surname);
            }
        }

        //Enumerātors, kas nosaka personas dzimumu - Man vai Woman. (2)
        public enum GenderEnum
        {
            Man, Woman
        }

        //Īpašība, kura izmanto GenderEnum tipu
        public GenderEnum Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        //Metode ToString()
        public override string ToString()
        {
            return ("Name: " + Name + ", Surname: " + Surname + ", Full Name: " + FullName + ", Gender: " + Gender);
        }
    }
}

/*
Atsauces:
    1. https://dl.ebooksworld.ir/books/CSharp.12.in.a.Nutshell.The.Definitive.Reference.9781098147440.pdf, llp. 294
    2. ChatGPT
*/