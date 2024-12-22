using MD1;
using System;
using System.Linq;
using MD2;
using Microsoft.Maui.Controls;

namespace MD2
{
    public partial class AddNewThings : ContentPage
    {
        private readonly IDatabaseManager _databaseManager;

        public AddNewThings() : this(new DatabaseManager(new SchoolSystemContextFactory())) { }

        public AddNewThings(IDatabaseManager databaseManager)
        {
            InitializeComponent();
            _databaseManager = databaseManager;

            LoadCourses(); //Ielâdç kursus no datubâzes (1)
            LoadStudents(); //Ielâdç studentus no datubâzes (1)
            LoadAssignments(); //Ielâdç uzdevumus no datubâzes (1)
        }

        //Metode, kas ielâdç kursus no datubâzes un pievieno tos izvçlnç (1)
        private async void LoadCourses()
        {
            try
            {
                var courses = await _databaseManager.GetCourses(); 
                CoursePicker.Items.Clear();

                if (courses != null && courses.Any())
                {
                    foreach (var course in courses)
                    {
                        CoursePicker.Items.Add(course.Name);
                    }
                }
                else
                {
                    CoursePicker.Items.Add("No courses available");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading courses: {ex.Message}");
                CoursePicker.Items.Add("Error loading courses");
            }
        }

        //Metode, kas ielâdç studentus no datubâzes un pievieno tos izvçlnç (1)
        private async void LoadStudents()
        {
            try
            {
                var students = await _databaseManager.GetStudents();
                StudentPicker.Items.Clear();

                if (students != null && students.Any())
                {
                    foreach (var student in students)
                    {
                        StudentPicker.Items.Add(student.FullName);
                    }
                }
                else
                {
                    StudentPicker.Items.Add("No students available");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading students: {ex.Message}");
                StudentPicker.Items.Add("Error loading students");
            }
        }

        //Metoda, kas ielâdç uzdevumus no datubâzes un ielâdç tos izvçlnç (10
        private async void LoadAssignments()
        {
            try
            {
                var assignments = await _databaseManager.GetAssignments();
                AssignmentPicker.Items.Clear();

                if (assignments != null && assignments.Any())
                {
                    foreach (var assignment in assignments)
                    {
                        AssignmentPicker.Items.Add(assignment.Description);
                    }
                }
                else
                {
                    AssignmentPicker.Items.Add("No assignments available");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading assignments: {ex.Message}");
                AssignmentPicker.Items.Add("Error loading assignments");
            }
        }

        //Metode, kas pievieno jaunu studentu (1)
        private async void AddStudent_Clicked(object sender, EventArgs e)
        {
            try
            {
                string name = Name.Text;
                string surname = Surname.Text;
                string genderInput = Gender.Text;
                string studentIDNumber = StudentIDNumber.Text;

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                    string.IsNullOrWhiteSpace(genderInput) || string.IsNullOrWhiteSpace(studentIDNumber))
                {
                    Result.Text = "All fields are required.";
                    return;
                }

                if (!Enum.TryParse(genderInput, true, out Person.GenderEnum gender))
                {
                    Result.Text = "Invalid gender. Please enter 'Man' or 'Woman'.";
                    return;
                }

                var newStudent = new Student(name, surname, gender, studentIDNumber);
                await _databaseManager.AddStudentAsync(newStudent);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Result.Text = $"Student added: {newStudent.FullName}";
                    LoadStudents();
                });
            }
            catch (Exception ex)
            {
                Result.Text = $"Error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error adding student: {ex.Message}");
            }
        }

        //Metode, kas pievieno jaunu uzdevumu (1)
        private async void AddAssignment_Clicked(object sender, EventArgs e)
        {
            try
            {
                string description = AssignmentDescription.Text;
                if (string.IsNullOrWhiteSpace(description))
                {
                    AssignmentResult.Text = "Please enter a description for the assignment.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(AssignmentDeadline.Text) ||
                    !DateTime.TryParseExact(AssignmentDeadline.Text, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var assignmentDeadline))
                {
                    AssignmentResult.Text = "Please enter a valid deadline in the format DD.MM.YYYY.";
                    return;
                }

                if (CoursePicker.SelectedItem == null)
                {
                    AssignmentResult.Text = "Please select a course.";
                    return;
                }

                var selectedCourseName = CoursePicker.SelectedItem.ToString();
                var course = await _databaseManager.GetCourseByNameAsync(selectedCourseName); 

                if (course == null)
                {
                    AssignmentResult.Text = "Selected course not found.";
                    return;
                }

                var newAssignment = new Assignement
                {
                    Description = description,
                    Deadline = new MD1.Date(assignmentDeadline.Day, assignmentDeadline.Month, assignmentDeadline.Year),
                    Course = course
                };

                await _databaseManager.AddAssignmentAsync(newAssignment); 

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    AssignmentResult.Text = $"Assignment added: {newAssignment.Description}";
                    LoadAssignments();
                });
            }
            catch (Exception ex)
            {
                AssignmentResult.Text = $"Error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error adding assignment: {ex.Message}");
            }
        }

        //Metode, kas pievieno jaunu uzdevuma iesniegumu (1)
        private async void AddSubmission_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (StudentPicker.SelectedItem == null || AssignmentPicker.SelectedItem == null)
                {
                    SubmissionResult.Text = "Please select a student and an assignment.";
                    return;
                }

                var selectedStudentName = StudentPicker.SelectedItem.ToString();
                var selectedAssignmentDescription = AssignmentPicker.SelectedItem.ToString();

                var student = await _databaseManager.GetStudentByNameAsync(selectedStudentName);
                var assignment = await _databaseManager.GetAssignmentByDescriptionAsync(selectedAssignmentDescription);

                if (student == null || assignment == null)
                {
                    SubmissionResult.Text = "Invalid student or assignment selected.";
                    return;
                }

                if (!int.TryParse(SubmissionScore.Text, out var score))
                {
                    SubmissionResult.Text = "Invalid score. Please enter a number.";
                    return;
                }

                var newSubmission = new Submission
                {
                    Student = student,
                    Assignement = assignment,
                    SubmissionTime = DateTime.Now,
                    Score = score
                };

                await _databaseManager.AddSubmissionAsync(newSubmission);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SubmissionResult.Text = $"Submission added: {newSubmission.Assignement.Description}";
                });
            }
            catch (Exception ex)
            {
                SubmissionResult.Text = $"Error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error adding submission: {ex.Message}");
            }
        }
    }
}

/**
 * Atsauces:
 *     1. ChatGPT
 * **/