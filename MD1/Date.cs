using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD1
{
    public class Date
    {
        //Atribūti
        private int day;
        private int month;
        private int year;

        //Konstrutors, kas kā parametrus pieņem dienu, mēnesi un gadu
        public Date(int day, int month, int year)
        {
            //Pārbauda, vai ievadītās dienas, mēneša un gada vērtības ir derīgas
            if (day < 1 || day > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 31."); //Ja ievadītā diena ir mazāka par 1 un lielāka par 31, tad konsolē parādās "Day must be between 1 and 31." (1)
            }
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12."); //Ja ievadītais mēnesis ir mazāks par 1 un lielāka par 12, tad konsolē parādās "Month must be between 1 and 12." (1)
            }
            if (year < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(year), "Year must be a positive number."); //Ja ievadītais ir mazāks par 1, tad konsolē parādās "Year must be a positive number." (1)
            }

            //Uzstāda vērtības atribūtiem
            this.day = day; //(2)
            this.month = month; //(2)
            this.year = year; //(2)
        }

        //Īpašības
        public int Day
        {
            get
            {
                return day;
            }

            set
            {
                if (value < 1 || value > 31)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Day must be between 1 and 31."); //Ja ievadītā diena ir mazāka par 1 un lielāka par 31, tad konsolē parādās "Day must be between 1 and 31." (1)
                }
                day = value;
            }
        }

        public int Month
        {
            get { return month; }
            set
            {
                if (value < 1 || value > 12)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Month must be between 1 and 12."); //Ja ievadītais mēnesis ir mazāks par 1 un lielāka par 12, tad konsolē parādās "Month must be between 1 and 12." (1)
                }
                month = value;
            }
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Year must be a positive number."); //Ja ievadītais ir mazāks par 1, tad konsolē parādās "Year must be a positive number." (1)
                }
                year = value;
            }
        }

        // Method ToString()
        public override string ToString()
        {
            return Day + "." + Month + "." + Year;
        }

        //Metode, kas no teksta formāta sadala datumu daļās (1)
        public static Date Parse(string dateStr)
        {
            //Datuma virknē iekļautās daļas tiek sadalītas pēc punktiem, veidojot masīvu. Kopā jāsanāk trim daļām, jo ir divi punkti.
            var parts = dateStr.Split('.');

            //Pārbauda, vai datuma formāts ir derīgs
            if (parts.Length != 3)
            {
                throw new FormatException($"Invalid date format '{dateStr}'. Expected format: DD.MM.YYYY"); //Ja daļu daudzums nav vienāds ar trīs, tad izmet šo paziņojumu "Invalid date format '{dateStr}'. Expected format: DD.MM.YYYY"
            }

            //Pārbauda, vai diena, mēnesis un gads ir veseli skaitļi
            if (!int.TryParse(parts[0], out int day) || !int.TryParse(parts[1], out int month) || !int.TryParse(parts[2], out int year))
            {
                throw new FormatException($"Invalid date components in '{dateStr}'. Day, month, and year must be integers."); //Ja diena, mēnesis vai gads nav veseli skaitļi, tad tiek izmest šis paziņojums "Invalid date components in '{dateStr}'. Day, month, and year must be integers."
            }

            return new Date(day, month, year);
        }

        public DateTime ToDateTime(TimeOnly time)
        {
            return new DateTime(this.Year, this.Month, this.Day, time.Hour, time.Minute, time.Second);
        }
    }
}

/*
Atsauces:
    1. ChatGPT
    2. https://dl.ebooksworld.ir/books/CSharp.12.in.a.Nutshell.The.Definitive.Reference.9781098147440.pdf, llp. 108
*/