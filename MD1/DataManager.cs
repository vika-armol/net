using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD1
{
    //DataManager klase implementē IDataManager interfeisu
    public class DataManager : IDataManager
    {
        //Atribūti: 
        private SchoolSystem schoolSystem;

        //Konstruktors
        public DataManager(SchoolSystem schoolSystem)
        {
            this.schoolSystem = schoolSystem; //(1)
        }

        public DataManager()
        {
        }

        //Metode print (2)
        public string print()
        {
            //StringBuilder() uzkrāj teksta datus efektīvā veidā, neveidojot jaunas instances (notikumus, gadījumus) katru reizi, kad dati tiek modificēti 
            var result = new System.Text.StringBuilder();

            //Pievienojam vispārīgu informāciju par skolas sistēmu
            result.AppendLine("School System Overview:");
            result.AppendLine(schoolSystem.ToString()); //Tiek saukta klases SchoolSystem metode ToString()

            //Pievienojam detalizētu informāciju par katru skolotāju sistēmā
            result.AppendLine("Teachers:");
            foreach (var teacher in schoolSystem.Teachers)
            {
                result.AppendLine(teacher.ToString()); 
            }

            //Pievienojam detalizētu informāciju par katru studentu sistēmā
            result.AppendLine("Students:");
            foreach (var student in schoolSystem.Students)
            {
                result.AppendLine(student.ToString());
            }

            //Pievienojam detalizētu informāciju par katru kursu sistēmā
            result.AppendLine("Courses:");
            foreach (var course in schoolSystem.Courses)
            {
                result.AppendLine(course.ToString());
            }

            //Pievienojam detalizētu informāciju par katru uzdevumu sistēmā
            result.AppendLine("Assignments:");
            foreach (var assignment in schoolSystem.Assignements)
            {
                result.AppendLine(assignment.ToString());
            }

            //Pievienojam detalizētu informāciju par katru iesniegšanu sistēmā
            result.AppendLine("Submissions:");
            foreach (var submission in schoolSystem.Submissions)
            {
                if (submission.Assignement != null && submission.Student != null)
                {
                    result.AppendLine($"Submission for: {submission.Assignement.Description} by {submission.Student.FullName}, Score: {submission.Score}"); //Ja iesniegšanas informācijā ir uzrādīts gan uzdevums, gan students, tad tiek parādīts šis "Submission for: {submission.Assignement.Description} by {submission.Student.FullName}, Score: {submission.Score}"
                }
                else
                {
                    result.AppendLine($"Submission has missing data. Assignment: {(submission.Assignement?.Description ?? "N/A")} Student: {(submission.Student?.FullName ?? "N/A")}, Score: {submission.Score}"); //Ja iesniegšanas informācijā ir iztrūkoši dati, tad tiek parādīts šis "Submission has missing data. Assignment: {(submission.Assignement?.Description ?? "N/A")} Student: {(submission.Student?.FullName ?? "N/A")}, Score: {submission.Score}"
                }

            }

            //Atgriež visus uzkrātos datus kā string
            return result.ToString();
        }

        //Metode save (2)
        public void save(string filePath)
        {
            //Ieraksta metodes print izvadi failā, kas ir norādīta filePath
            File.WriteAllText(filePath, print());
        }

        //Metode load (2)
        public void load(string filePath)
        {
            //Pārbauda vai fails, kas ir norādīts filePath, eksistē
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}"); //Ja fails neeksistē, tad tiek parādīts šis paziņojums "File not found: {filePath}"
                return; //Iziet ārā no metodes load 
            }

            //Nolasa visas rindiņas no faila masīvā
            string[] lines = File.ReadAllLines(filePath);

            //Iztīra sistēmu no esošajiem datiem, lai pārliecinātos par jaunu datu ielādi sistēmā
            reset();

            //Mainīgais, lai izsekotu, kurā faila sadaļā mēs esam
            string currentSection = "";

            //Iet cauri katrai rindiņai failā
            foreach (var line in lines)
            {
                //Identificē jaunas sadaļas sākumu, balstoties uz atslēgvārdiem
                if (line.StartsWith("Teachers:"))
                {
                    currentSection = "Teachers";
                    continue;
                }
                else if (line.StartsWith("Students:"))
                {
                    currentSection = "Students";
                    continue;
                }
                else if (line.StartsWith("Courses:"))
                {
                    currentSection = "Courses";
                    continue;
                }
                else if (line.StartsWith("Assignments:"))
                {
                    currentSection = "Assignments";
                    continue;
                }
                else if (line.StartsWith("Submissions:"))
                {
                    currentSection = "Submissions";
                    continue;
                }

                //Ja mēs esam "Teachers" sadaļā, tad
                if (currentSection == "Teachers")
                {
                    //Sadalam rindu daļās, balstoties uz atslēgas vārdiem, kas ir saistīti ar skolotāju - "Name:", "Surname:", "Full Name:", "Gender:", "Contract Date:"
                    var teacherParts = line.Split(new[] { "Name:", "Surname:", "Full Name:", "Gender:", "Contract Date:" }, StringSplitOptions.RemoveEmptyEntries);

                    //Pārliecinamies, ka mums ir 5 daļas
                    if (teacherParts.Length < 5)
                    {
                        Console.WriteLine($"Insufficient data for teacher entry: {line}"); //Ja mums nav pietiekami daudz daļu, tad tiek paziņots šāds paziņojums "Insufficient data for teacher entry: {line}"
                        continue; //Iet uz nākamo rindiņu, ja iztrūkst datu
                    }

                    //Izņem ārā papildus atstarpes un komatus, kas nav vajadzīgi, no katras daļas
                    for (int i = 0; i < teacherParts.Length; i++)
                    {
                        teacherParts[i] = teacherParts[i].Trim().TrimEnd(',', ' ');
                    }

                    //Izņem ārā papildus atstarpes un komatus, kas nav vajadzīgi, no dzimuma daļas
                    string genderStr = teacherParts[3].TrimEnd(',', ' ');

                    //Pārvērš dzimuma virknes vērtību uz enumerācijas jeb enum tipu
                    if (!Enum.TryParse(genderStr, out Person.GenderEnum gender))
                    {
                        Console.WriteLine($"Invalid gender value: {genderStr}. Expected 'Man' or 'Woman'."); //Ja dzimuma virknes vērtība nav derīga, tad izmet šo paziņojumu "Invalid gender value: {genderStr}. Expected 'Man' or 'Woman'."
                        continue;
                    }

                    //Tiek izveidots jauns skolotāja objekts un tas tiek aizpildīts ar "sadalītajiem" datiem
                    var currentTeacher = new Teacher
                    {
                        Name = teacherParts[0],
                        Surname = teacherParts[1],
                        Gender = gender,
                        ContractDate = DateOnly.Parse(teacherParts[4])
                    };

                    //Skolotājs tiek pievienots skolas sistēmai
                    schoolSystem.AddTeacher(currentTeacher);
                }
                //Ja mēs esam "Students" sadaļā, tad
                else if (currentSection == "Students")
                {
                    //Sadalam rindu daļās, balstoties uz atslēgas vārdiem, kas ir saistīti ar studentu - "Name:", "Surname:", "Full Name:", "Gender:", "Student ID Number:"
                    var studentParts = line.Split(new[] { "Name:", "Surname:", "Full Name:", "Gender:", "Student ID Number:" }, StringSplitOptions.RemoveEmptyEntries);

                    //Pārliecinamies, ka mums ir 5 daļas
                    if (studentParts.Length < 5)
                    {
                        Console.WriteLine($"Insufficient data for student entry: {line}"); //Ja mums nav pietiekami daudz daļu, tad tiek paziņots šāds paziņojums "Insufficient data for student entry: {line}"
                        continue; //Iet uz nākamo rindiņu, ja iztrūkst datu
                    }

                    //Izņem ārā papildus atstarpes un komatus, kas nav vajadzīgi, no katras daļas
                    for (int i = 0; i < studentParts.Length; i++)
                    {
                        studentParts[i] = studentParts[i].Trim().TrimEnd(',', ' ');
                    }

                    //Izņem ārā papildus atstarpes un komatus, kas nav vajadzīgi, no dzimuma daļas
                    string genderStr = studentParts[3].Trim();

                    //Pārvērš dzimuma virknes vērtību uz enumerācijas jeb enum tipu
                    if (!Enum.TryParse(genderStr, out Person.GenderEnum gender))
                    {
                        Console.WriteLine($"Invalid gender value: {genderStr}. Expected 'Man' or 'Woman'."); //Ja dzimuma virknes vērtība nav derīga, tad izmet šo paziņojumu "Invalid gender value: {genderStr}. Expected 'Man' or 'Woman'."
                        continue;
                    }

                    //Tiek izveidots jauns studenta objekts un tas tiek aizpildīts ar "sadalītajiem" datiem
                    var currentStudent = new Student(studentParts[0], studentParts[1], gender, studentParts[4]);

                    //Students tiek pievienots skolas sistēmai
                    schoolSystem.AddStudent(currentStudent);
                }
                //Ja mēs esam "Courses" sadaļā, tad
                else if (currentSection == "Courses")
                {
                    //Sadalam rindu daļās, balstoties uz atslēgas vārdiem, kas ir saistīti ar kursu - "Course Name:", "Teacher:"
                    var courseParts = line.Split(new[] { "Course Name:", "Teacher:" }, StringSplitOptions.RemoveEmptyEntries);

                    //Pārliecinamies, ka mums ir 2 daļas
                    if (courseParts.Length < 2)
                    {
                        Console.WriteLine($"Insufficient data for course entry: {line}"); //Ja mums nav pietiekami daudz daļu, tad tiek paziņots šāds paziņojums "Insufficient data for course entry: {line}"
                        continue; //Iet uz nākamo rindiņu, ja iztrūkst datu
                    }

                    //Izņem ārā papildus atstarpes un komatus, kas nav vajadzīgi, no kursa nosaukuma daļas
                    string courseName = courseParts[0].Trim().TrimEnd(',', ' ');

                    var teacherDetails = courseParts[1].Trim().Split(new[] { "Name:", "Surname:", "Full Name:", "Gender:", "Contract Date:" }, StringSplitOptions.RemoveEmptyEntries);

                    if (teacherDetails.Length < 5)
                    {
                        Console.WriteLine($"Insufficient data to parse teacher details: {string.Join(", ", teacherDetails)}");
                        continue;
                    }

                    for (int i = 0; i < teacherDetails.Length; i++)
                    {
                        teacherDetails[i] = teacherDetails[i].Trim().TrimEnd(',', ' ');
                    }

                    if (!Enum.TryParse(teacherDetails[3].Trim(), out Person.GenderEnum gender))
                    {
                        Console.WriteLine($"Invalid gender value: {teacherDetails[3]}. Expected 'Man' or 'Woman'.");
                        continue;
                    }

                    //Pārbaude tam, vai skolotājs jau eksistē sistēmā
                    var existingTeacher = schoolSystem.Teachers.FirstOrDefault(t =>
                        t.Name.Equals(teacherDetails[0], StringComparison.OrdinalIgnoreCase) &&
                        t.Surname.Equals(teacherDetails[1], StringComparison.OrdinalIgnoreCase));

                    //Ja skolotājs neeksistē sistēmā, tad izveido jaunu
                    Teacher courseTeacher = existingTeacher ?? new Teacher
                    {
                        Name = teacherDetails[0],
                        Surname = teacherDetails[1],
                        Gender = gender,
                        ContractDate = DateOnly.Parse(teacherDetails[4])
                    };

                    //Pievieno jaunu skolotāju sistēmā, ja tas iepriekš neeksistēja sistēmā
                    if (existingTeacher == null)
                    {
                        schoolSystem.AddTeacher(courseTeacher);
                    }

                    //Tiek izveidots jauns kursa objekts un tas tiek aizpildīts ar "sadalītajiem" datiem
                    var currentCourse = new Course
                    {
                        Name = courseName.Trim(),
                        Teacher = courseTeacher
                    };

                    //Kurss tiek pievienots skolas sistēmai
                    schoolSystem.AddCourse(currentCourse);
                }
                //Ja mēs esam "Assignments" sadaļā, tad
                else if (currentSection == "Assignments")
                {
                    //Sadalam rindu daļās, balstoties uz atslēgas vārdiem, kas ir saistīti ar uzdevumu - "Deadline:", "Course Name:", "Description:"
                    var assignmentParts = line.Split(new[] { "Deadline:", "Course Name:", "Description:" }, StringSplitOptions.RemoveEmptyEntries);

                    //Pārliecinamies, ka mums ir 3 daļas
                    if (assignmentParts.Length < 3)
                    {
                        Console.WriteLine($"Insufficient data for assignment entry: {line}"); //Ja mums nav pietiekami daudz daļu, tad tiek paziņots šāds paziņojums "Insufficient data for assignment entry: {line}"
                        continue; //Iet uz nākamo rindiņu, ja iztrūkst datu
                    }

                    //Izņem ārā papildus atstarpes un komatus, kas nav vajadzīgi, no uzdevuma iesniegšanas datuma, kursa nosaukuma un apraksta daļas
                    string deadlineStr = assignmentParts[0].Trim().TrimEnd(',', ' ');
                    string courseName = assignmentParts[1].Trim().TrimEnd(',', ' ');
                    string description = assignmentParts[2].Trim();

                    //Izveido jaunu uzdevuma objektu un sasaisti to ar pareizo kursa nosaukumu
                    var currentAssignment = new Assignement
                    {
                        Course = schoolSystem.Courses.FirstOrDefault(c => c.Name.Equals(courseName, StringComparison.OrdinalIgnoreCase)),
                        Description = description,
                        Deadline = Date.Parse(deadlineStr)
                    };

                    //Ja kursa nosaukums netiek atrasts, tad tiek izprintēts šis paziņojums
                    if (currentAssignment.Course == null)
                    {
                        Console.WriteLine($"Warning: Course not found for assignment: {description}, Course Name: {courseName}");
                    }

                    //Uzdevums tiek pievienots skolas sistēmai
                    schoolSystem.AddAssignment(currentAssignment);
                }
                //Ja mēs esam "Submissions" sadaļā, tad
                else if (currentSection == "Submissions")
                {
                    //Sadalam rindu daļās, balstoties uz atslēgas vārdiem, kas ir saistīti ar iesniegšanu - "Submission for:", "by", "Score:"
                    var submissionParts = line.Split(new[] { "Submission for:", "by", "Score:" }, StringSplitOptions.RemoveEmptyEntries);

                    //Pārliecinamies, ka mums ir 3 daļas
                    if (submissionParts.Length < 3)
                    {
                        Console.WriteLine($"Insufficient data for submission entry: {line}"); //Ja mums nav pietiekami daudz daļu, tad tiek paziņots šāds paziņojums "Insufficient data for submission entry: {line}"
                        continue; //Iet uz nākamo rindiņu, ja iztrūkst datu
                    }

                    //Izņem ārā papildus atstarpes un komatus, kas nav vajadzīgi, no uzdevuma iesniegšanas apraksta, studenta pilnā vārda un vērtējuma 
                    var assignmentDescription = submissionParts[0].Trim();
                    var studentFullName = submissionParts[1].Trim().TrimEnd(',', ' ');
                    var scoreStr = submissionParts[2].Trim();

                    //Izveido jaunu iesniegšanas objektu, un piešķir tam pareizo uzdevumu un studentu
                    var submission = new Submission
                    {
                        Assignement = schoolSystem.Assignements.FirstOrDefault(a => a.Description.Equals(assignmentDescription, StringComparison.OrdinalIgnoreCase)),
                        Student = schoolSystem.Students.FirstOrDefault(s => s.FullName.Equals(studentFullName, StringComparison.OrdinalIgnoreCase)),
                        Score = int.TryParse(scoreStr, out var score) ? score : 0
                    };

                    //Ja uzdevums vai students netiek atrasts, tad tiek izprintēts viens no šiem paziņojumiem
                    if (submission.Assignement == null)
                    {
                        Console.WriteLine($"Warning: Assignment not found for: {assignmentDescription}");
                    }
                    if (submission.Student == null)
                    {
                        Console.WriteLine($"Warning: Student not found for: {studentFullName}");
                    }

                    //Iesniegšanas informācija tiek pievienota skolas sistēmai
                    schoolSystem.AddSubmission(submission);
                }
            }
        }

        //Metode createTestData (2)
        public void createTestData()
        {
            //Izveido divus skolotāja objektus
            Teacher teacher1 = new Teacher { Name = "Jānis", Surname = "Dumbergs", Gender = Person.GenderEnum.Man, ContractDate = new DateOnly(2023, 05, 01) };
            Teacher teacher2 = new Teacher { Name = "Kate", Surname = "Sveķe", Gender = Person.GenderEnum.Woman, ContractDate = new DateOnly(2014, 06, 14) };
            
            //Pievieno abus skolotājus skolas sistēmai
            schoolSystem.AddTeacher(teacher1);
            schoolSystem.AddTeacher(teacher2);

            //izveido divus studentu objektus
            Student student1 = new Student("Alise", "Johansone", Person.GenderEnum.Woman, "aj23056");
            Student student2 = new Student("Bruno", "Erkšķis", Person.GenderEnum.Man, "be21033");
            
            //Pievieno abus studentus skolas sistēmai
            schoolSystem.AddStudent(student1);
            schoolSystem.AddStudent(student2);

            //Izvaido divus kursa objektus
            Course course1 = new Course { Name = "Matemātika", Teacher = teacher1 };
            Course course2 = new Course { Name = "Fizika", Teacher = teacher2 };
            
            //Pievieno abus kursus skolas sistēmai
            schoolSystem.AddCourse(course1);
            schoolSystem.AddCourse(course2);

            //Izveido divus uzdevuma objektus
            Assignement assignment1 = new Assignement
            {
                Course = course1,
                Description = "Algebras Mājasdarbs",
                Deadline = new Date(10, 10, 2024)
            };
            Assignement assignment2 = new Assignement
            {
                Course = course2,
                Description = "Fizikas Patstāvīgais Projekts",
                Deadline = new Date(13, 11, 2023)
            };

            //Pievieno abus uzdevumus skolas sistēmai
            schoolSystem.AddAssignment(assignment1);
            schoolSystem.AddAssignment(assignment2);

            //Izveido divus iesniegšanas objektus
            Submission submission1 = new Submission
            {
                Assignement = assignment1,
                Student = student1,
                SubmissionTime = new DateTime(2023, 10, 01),
                Score = 85
            };
            Submission submission2 = new Submission
            {
                Assignement = assignment2,
                Student = student2,
                SubmissionTime = new DateTime(2023, 10, 06),
                Score = 90
            };

            //Pievieno abus iesniegšanas objektus skolas sistēmai
            schoolSystem.AddSubmission(submission1);
            schoolSystem.AddSubmission(submission2);
        }

        //Metode reset
        public void reset()
        {
            //.Clear() palīdz iztīrīt skolas sistēmu no visiem skolotājiem, studentiem, kursiem, uzdevumiem un iesniegumiem
            schoolSystem.Teachers.Clear();
            schoolSystem.Students.Clear();
            schoolSystem.Courses.Clear();
            schoolSystem.Assignements.Clear();
            schoolSystem.Submissions.Clear();
        }
    }
}

/*
Atsauces:
    1. https://dl.ebooksworld.ir/books/CSharp.12.in.a.Nutshell.The.Definitive.Reference.9781098147440.pdf, llp. 108
    2. ChatGPT
 */