using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MD1
{
    public class Submission
    {
        //Atribūti:
        private Assignement assignment;
        private Student student;
        private DateTime submissionTime;
        private int score;

        //Īpašības:
        public int Id { get; set; } //Šī tiek izmantota kā primārā atslēga tabulā "Suhbmission"

        public int AssignementId { get; set; } //Šī tiek izmantota kā ārējā atslēga tabulā "Suhbmission"

        public Assignement Assignement
        {
            get
            {
                return assignment;
            }

            set
            {
                assignment = value;
            }
        }

        public int StudentId { get; set; }

        public Student Student
        {
            get
            {
                return student;
            }

            set
            {
                student = value;
            }
        }

        public DateTime SubmissionTime
        {
            get
            {
                return submissionTime;
            }

            set
            {
                submissionTime = value;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }

            set
            {
                score = value;
            }
        }

        //Metode ToString()
        public override string ToString()
        {
            return $"Submission for: {Assignement?.Description}, Student: {Student?.FullName}, Submission Time: {SubmissionTime}, Score: {Score}";
        }
    }
}
