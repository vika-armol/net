using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MD4
{
    public class Assignement
    {
        //Atribūti: 
        private Date deadline; //Šim atribūtam varēja arī izmantot DateTime vai DateOnly, bet es uztaisīju atsevišķu klasi Date
        private Course course;
        private string description;

        //Īpašības:
        public int Id { get; set; } //Šis tiek izmantots kā primārā atslēga datu bāzes tabulā "Assignment"

        public Date Deadline
        {
            get
            {
                return deadline;
            }

            set
            {
                deadline = value;
            }
        }

        public int CourseId { get; set; }

        public Course Course
        {
            get
            {
                return course;
            }

            set
            {
                course = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();

        //Metode ToString()
        public override string ToString()
        {
            return "Deadline: " + Deadline + ", Course Name: " + (Course != null ? Course.Name : "N/A") + ", Description: " + Description;
        }
    }
}
