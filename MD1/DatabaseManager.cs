using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD1
{
    //Database manager, kas implementē IDatabaseManager interfeisu (1)
    public class DatabaseManager : IDatabaseManager
    {
        private readonly SchoolSystemContextFactory _contextFactory;

        //Konstruktors,kas pieņem datu context factory (1)
        public DatabaseManager(SchoolSystemContextFactory contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        //Metode, lai iegūtu datu context (1)
        public SchoolSystemContext GetDbContext()
        {
            return _contextFactory.CreateDbContext(new string[0]);
        }

        //Metode, lai iegūtu visus uzdevuma iesniegumus (1)
        public async Task<List<Submission>> GetSubmissions()
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Submission
                        .Include(s => s.Assignement)  //Ieķļauj uzdevuma detaļas (1)
                        .Include(s => s.Student)  //Iekļauj studenta detaļas (1)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting submissions: {ex.Message}");
                return new List<Submission>();
            }
        }

        //Metode, lai rediģētu/atjauninātu uzdevuma iesniegumu (1)
        public async Task UpdateSubmission(Submission submission)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    dbContext.Submission.Update(submission);
                    await SaveAsync(dbContext); //Saglabā izmaiņas datu bāzē (1)
                    Console.WriteLine("Submission updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating submission: {ex.Message}");
            }
        }

        //Meotde, lai izdzēstu uzdevuma iesniegumu (1)
        public async Task DeleteSubmission(Submission submission)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    dbContext.Submission.Remove(submission); //Noņem uzdevuma iesniegumu (1)
                    await SaveAsync(dbContext); //Saglabā izmaiņas datubāzē (1)
                    Console.WriteLine("Submission deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting submission: {ex.Message}");
            }
        }

        //Metode, lai glabātu izmaiņas datu bāzē (1)
        public async Task SaveAsync(SchoolSystemContext dbContext)
        {
            try
            {
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Data successfully saved to the database.");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("Database update failed. Check your data or connection.");
                Console.WriteLine($"Details: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        //Metode, lai atjauninātu/rediģētu uzdevumu (1)
        public async Task UpdateAssignment(Assignement assignment)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    dbContext.Assignement.Update(assignment);
                    await SaveAsync(dbContext);
                    Console.WriteLine("Assignment updated successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating assignment: {ex.Message}");
            }
        }

        //Metode, lai izdzēstu uzdevumu (1)
        public async Task DeleteAssignment(Assignement assignment)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    dbContext.Assignement.Remove(assignment);
                    await SaveAsync(dbContext);
                    Console.WriteLine("Assignment deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting assignment: {ex.Message}");
            }
        }

        //Metode, lai iegūtu visus kursus (1)
        public async Task<List<Course>> GetCourses()
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Course.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting courses: {ex.Message}");
                return new List<Course>(); 
            }
        }

        //Metode, lai iegūtu visus studentus (1)
        public async Task<List<Student>> GetStudents()
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Student.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting students: {ex.Message}");
                return new List<Student>(); 
            }
        }

        //Metode, lai iegūtu visus uzdevumus (1)
        public async Task<List<Assignement>> GetAssignments()
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Assignement.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting assignments: {ex.Message}");
                return new List<Assignement>();
            }
        }

        //Metode, lai iegūtu kursu pēc tā nosaukuma (1)
        public async Task<Course> GetCourseByNameAsync(string courseName)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Course.FirstOrDefaultAsync(c => c.Name == courseName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting course by name: {ex.Message}");
                return null;
            }
        }

        //Metode, lai iegūtu studentu pēc pilnā vārda un uzvārda (1)
        public async Task<Student> GetStudentByNameAsync(string studentName)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Student.FirstOrDefaultAsync(s => (s.Name + " " + s.Surname) == studentName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting student by name: {ex.Message}");
                return null;
            }
        }

        //Metode, lai iegūtu uzdevumu pēc tā apraksta (1)
        public async Task<Assignement> GetAssignmentByDescriptionAsync(string description)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Assignement.FirstOrDefaultAsync(a => a.Description == description);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting assignment by description: {ex.Message}");
                return null; 
            }
        }

        //Metode, lai iegūtu visus skolotājus (1)
        public async Task<List<Teacher>> GetTeachers()
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Teacher.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting teachers: {ex.Message}");
                return new List<Teacher>();
            }
        }

        //Metode, lai pievienotu skolotāju (1)
        public async Task AddTeacherAsync(Teacher teacher)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    if (teacher == null) throw new ArgumentNullException(nameof(teacher));
                    await dbContext.Teacher.AddAsync(teacher);
                    await SaveAsync(dbContext);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding teacher: {ex.Message}");
            }
        }

        //Metode, lai pievienotu studentu (1)
        public async Task AddStudentAsync(Student student)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    if (student == null)
                    {
                        throw new ArgumentNullException(nameof(student), "Student cannot be null.");
                    }
                    await dbContext.Student.AddAsync(student);
                    await SaveAsync(dbContext);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding student: {ex.Message}");
            }
        }

        //Metode, lai pievienotu uzdevumu (1)
        public async Task AddAssignmentAsync(Assignement assignment)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    if (assignment == null)
                    {
                        throw new ArgumentNullException(nameof(assignment), "Assignment cannot be empty.");
                    }

                    //Pārbauda vai ir norādīts kurss (1)
                    if (assignment.Course == null)
                    {
                        throw new ArgumentException("Assignment must have a valid course.");
                    }

                    await dbContext.Assignement.AddAsync(assignment);
                    await SaveAsync(dbContext);
                }
            }
            catch (ArgumentNullException argEx)
            {
                Console.WriteLine($"Argument error: {argEx.Message}");
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"Database update failed: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the assignment: {ex.Message}");
            }
        }

        //Metode, lai pievienotu iesniegumu (1)
        public async Task AddSubmissionAsync(Submission submission)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    //Pārbauda vai uzdevuma apraksts un students ir izvēlēti (1)
                    if (submission.Student == null || submission.Assignement == null)
                    {
                        throw new InvalidOperationException("Submission must have both Student and Assignment set.");
                    }
                    await dbContext.Submission.AddAsync(submission);
                    await SaveAsync(dbContext);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding submission: {ex.Message}");
            }
        }

        //Metode, lai pievienotu kursu (1)
        public async Task AddCourseAsync(Course course)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    if (course == null) throw new ArgumentNullException(nameof(course));

                    await dbContext.Course.AddAsync(course);
                    await SaveAsync(dbContext);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding course: {ex.Message}");
            }
        }

        //Metode, lai ielādētu datus no faila (bet šī metode īstenībā nav vajadzīga, jo mēs ar failu nestrādājam) (1)
        public async Task LoadAsync()
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    var teachers = await dbContext.Teacher.ToListAsync();
                    var students = await dbContext.Student.ToListAsync();
                    var courses = await dbContext.Course.ToListAsync();
                    var assignments = await dbContext.Assignement.ToListAsync();
                    var submissions = await dbContext.Submission.ToListAsync();

                    Console.WriteLine("Data successfully loaded from the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
        }

        //Metode, lai atiestatītu datu bāzi (1)
        public async Task ResetAsync()
        {
            using (var dbContext = GetDbContext())
            {
                try
                {
                    dbContext.Teacher.RemoveRange(dbContext.Teacher);
                    dbContext.Student.RemoveRange(dbContext.Student);
                    dbContext.Course.RemoveRange(dbContext.Course);
                    dbContext.Assignement.RemoveRange(dbContext.Assignement);
                    dbContext.Submission.RemoveRange(dbContext.Submission);

                    await SaveAsync(dbContext); //Saglabā izmaiņas datu bāzē (1)

                    Console.WriteLine("Database has been reset.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error resetting the database: {ex.Message}");
                }
            }
        }

        //Metode, kas izdrukā visus datu bāzes datus (1)
        public async Task<string> PrintAsync()
        {
            var result = new StringBuilder();

            try
            {
                using (var dbContext = GetDbContext())
                {
                    var teachers = await dbContext.Teacher.ToListAsync();
                    var students = await dbContext.Student.ToListAsync();
                    var courses = await dbContext.Course.Include(c => c.Teacher).ToListAsync(); // Include Teacher
                    var assignments = await dbContext.Assignement.ToListAsync();
                    var submissions = await dbContext.Submission
                        .Include(s => s.Assignement)
                        .Include(s => s.Student)
                        .ToListAsync();

                    result.AppendLine("School System Overview:");

                    //Skolotāji (1)
                    result.AppendLine("\nTeachers:");
                    foreach (var teacher in teachers)
                    {
                        result.AppendLine($"Name: {teacher.Name} {teacher.Surname}, " +
                                           $"Gender: {teacher.Gender}, " +
                                           $"Contract Date: {teacher.ContractDate.ToShortDateString()}");
                    }

                    //Studenti (1)
                    result.AppendLine("\nStudents:");
                    foreach (var student in students)
                    {
                        result.AppendLine($"Full Name: {student.FullName}, " +
                                           $"Gender: {student.Gender}, " +
                                           $"Student ID: {student.StudentIdNumber}");
                    }

                    //Kursi (1)
                    result.AppendLine("\nCourses:");
                    foreach (var course in courses)
                    {
                        result.AppendLine($"Course: {course.Name}, " +
                                           $"Teacher: {(course.Teacher != null ? $"{course.Teacher.Name} {course.Teacher.Surname}" : "N/A")}");
                    }

                    //Uzdevumi (1)
                    result.AppendLine("\nAssignments:");
                    foreach (var assignment in assignments)
                    {
                        result.AppendLine($"Assignment: {assignment.Description}, " +
                                           $"Course: {assignment.Course.Name}, " +
                                           $"Deadline: {assignment.Deadline.ToString()}");
                    }

                    //Uzdevumu iesniegumi (1)
                    result.AppendLine("\nSubmissions:");
                    foreach (var submission in submissions)
                    {
                        result.AppendLine($"Assignment: {submission.Assignement?.Description ?? "N/A"}, " +
                                           $"Student: {submission.Student?.FullName ?? "N/A"}, " +
                                           $"Score: {submission.Score}, " +
                                           $"Submission Time: {submission.SubmissionTime.ToString("g")}");
                    }
                }
            }
            catch (Exception ex)
            {
                result.AppendLine($"Error printing data: {ex.Message}");
                Console.WriteLine($"Error: {ex.Message}");
            }

            return result.ToString();
        }

        //Metoda, kas izveido testa datus (1)
        public async Task CreateTestDataAsync()
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    var teacher1 = new Teacher { Name = "Jānis", Surname = "Dumbergs", Gender = Person.GenderEnum.Man, ContractDate = new DateOnly(2023, 05, 01) };
                    var teacher2 = new Teacher { Name = "Kate", Surname = "Sveķe", Gender = Person.GenderEnum.Woman, ContractDate = new DateOnly(2014, 06, 14) };

                    await dbContext.Teacher.AddRangeAsync(teacher1, teacher2);

                    var student1 = new Student("Alise", "Johansone", Person.GenderEnum.Woman, "aj23056");
                    var student2 = new Student("Bruno", "Erkšķis", Person.GenderEnum.Man, "be21033");

                    await dbContext.Student.AddRangeAsync(student1, student2);

                    var course1 = new Course { Name = "Matemātika", Teacher = teacher1 };
                    var course2 = new Course { Name = "Fizika", Teacher = teacher2 };

                    await dbContext.Course.AddRangeAsync(course1, course2);

                    var assignment1 = new Assignement
                    {
                        Course = course1,
                        Description = "Algebras Mājasdarbs",
                        Deadline = new Date(10, 10, 2024)
                    };
                    var assignment2 = new Assignement
                    {
                        Course = course2,
                        Description = "Fizikas Projekts",
                        Deadline = new Date(13, 11, 2023)
                    };

                    await dbContext.Assignement.AddRangeAsync(assignment1, assignment2);

                    var submission1 = new Submission
                    {
                        Assignement = assignment1,
                        Student = student1,
                        SubmissionTime = DateTime.Now,
                        Score = 85
                    };
                    var submission2 = new Submission
                    {
                        Assignement = assignment2,
                        Student = student2,
                        SubmissionTime = DateTime.Now,
                        Score = 90
                    };

                    await dbContext.Submission.AddRangeAsync(submission1, submission2);

                    await SaveAsync(dbContext); //Saglabā testa datus datu bāzē

                    Console.WriteLine("Test data successfully created.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test data: {ex.Message}");
            }
        }

        //Metode, lai iegūtu uzdevuma iesniegumu pēc studenta pilnā vārda un uzvārda un uzdevuma apraksta (1)
        public async Task<Submission> GetSubmissionByStudentAndAssignmentAsync(string studentName, string assignmentDescription)
        {
            try
            {
                using (var dbContext = GetDbContext())
                {
                    return await dbContext.Submission
                        .Include(s => s.Student)
                        .Include(s => s.Assignement)
                        .FirstOrDefaultAsync(s => (s.Student.Name + " " + s.Student.Surname) == studentName && s.Assignement.Description == assignmentDescription);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting submission by student and assignment: {ex.Message}");
                return null;
            }
        }
    }
}

/**
 * Atsauces:
 *    1.ChatGPT
 * **/