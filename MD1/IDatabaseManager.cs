using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD1
{
    //Interfeiss IDatabaseManager definē visas nepieciešamās metode priekš DatabaseManager (1)
    public interface IDatabaseManager
    {
        Task<List<Submission>> GetSubmissions();
        Task UpdateSubmission(Submission submission);
        Task DeleteSubmission(Submission submission);
        Task UpdateAssignment(Assignement assignment);
        Task DeleteAssignment(Assignement assignment);
        Task<List<Course>> GetCourses();
        Task<List<Student>> GetStudents();
        Task<List<Assignement>> GetAssignments();
        Task<Course> GetCourseByNameAsync(string courseName);
        Task<Student> GetStudentByNameAsync(string studentName);
        Task<Assignement> GetAssignmentByDescriptionAsync(string description);
        Task AddStudentAsync(Student student);
        Task AddAssignmentAsync(Assignement assignment);
        Task AddSubmissionAsync(Submission submission);
        Task AddCourseAsync(Course course);
        Task LoadAsync();
        Task ResetAsync();
        Task<string> PrintAsync();
        Task CreateTestDataAsync();
        Task SaveAsync(SchoolSystemContext dbContext);
        SchoolSystemContext GetDbContext();
        Task<Submission> GetSubmissionByStudentAndAssignmentAsync(string studentName, string assignmentDescription);
    }
}

/**
 * Atsauces:
 *     1. ChatGPT
 * **/