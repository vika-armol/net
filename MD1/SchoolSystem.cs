using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD1
{
    public class SchoolSystem
    {
        //Saraksti, kas glabā informāciju par skolotājiem, studentiem, kursiem, uzdevumiem un iesniegšanu (1) (2)
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Assignement> Assignements { get; set; } = new List<Assignement>();
        public List<Submission> Submissions { get; set; } = new List<Submission>();

        //Metode, lai pievienotu skolotāju sistēmai
        public void AddTeacher(Teacher teacher)
        {
            Teachers.Add(teacher); //Pievieno skolotāju sistēmai
        }

        //Metode, lai pievienotu studentu sistēmai
        public void AddStudent(Student student)
        {
            Students.Add(student); //Pievieno studentu sistēmai
        }

        //Metode, lai pievienotu kursu sistēmai
        public void AddCourse(Course course)
        {
            Courses.Add(course); //Pievieno kursu sistēmai
        }

        //Metode, lai pievienotu uzdevumu sistēmai
        public void AddAssignment(Assignement assignment)
        {
            Assignements.Add(assignment); //Pievieno uzdevumu sistēmai
        }

        //Metode, lai pievienotu iesniegšanas informāciju sistēmai
        public void AddSubmission(Submission submission)
        {
            Submissions.Add(submission); //Pievieno iesniegšanas informāciju sistēmai
        }

        //Metode, lai noņemtu skolotāju no sistēmas
        public void RemoveTeacher(Teacher teacher)
        {
            Teachers.Remove(teacher); //Noņem skolotāju no sistēmas
        }

        //Metode, lai noņemtu studentu no sistēmas
        public void RemoveStudent(Student student)
        {
            Students.Remove(student); //Noņem studentu no sistēmas
        }

        //Metode, lai noņemtu kursu no sistēmas
        public void RemoveCourse(Course course)
        {
            Courses.Remove(course); //Noņem kursu no sistēmas
        }

        //Metode, lai noņemtu uzdevumu no sistēmas
        public void RemoveAssignment(Assignement assignment)
        {
            Assignements.Remove(assignment); //Noņem uzdevumu no sistēmas
        }

        //Metode, lai noņemtu informāciju par iesniegšanu no sistēmas
        public void RemoveSubmission(Submission submission)
        {
            Submissions.Remove(submission); //Noņem informāciju par iesniegšanu no sistēmas
        }

        //Metode ToString()
        public override string ToString()
        {
            return "School System has " + Teachers.Count + " teacher/s, " + Students.Count + " student/s, " + Courses.Count + " course/s, " + Assignements.Count + " assignement/s and " + Submissions.Count + " submission/s";
        }
    }
}

/*
Atsauces:
    1. ChatGPT
    2. Lekcijas prezentācija "02_03_04_C#Sintakse", 157.-190.slaidam
*/