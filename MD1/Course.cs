using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MD1
{
    public class Course
    {
        //Atribūti:
        private string name;
        private Teacher teacher;

        //Īpašības
        public int Id { get; set; } //Šis tiek izmantots kā primārā atslēga datu bāzes tabulā "Course"
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int TeacherId { get; set; } //Šis tiek izmantots kā ārējā atslēga tabulā "Course"

        public Teacher Teacher
        {
            get
            {
                return teacher;
            }

            set
            {
                teacher = value;
            }
        }

        public ICollection<Assignement> Assignements { get; set; } = new List<Assignement>();

        //Metode ToString()
        public override string ToString()
        {
            return "Course Name: " + Name + ", Teacher: " + (Teacher != null ? Teacher.ToString() : "No Teacher is Assigned!");
        }
    }
}
