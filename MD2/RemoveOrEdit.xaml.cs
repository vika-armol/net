using System;
using System.Linq;
using MD1;
using Microsoft.Maui.Controls;

namespace MD2
{
    public partial class RemoveOrEdit : ContentPage
    {
        private readonly IDatabaseManager _databaseManager;

        public RemoveOrEdit() : this(new DatabaseManager(new SchoolSystemContextFactory())) { }

        public RemoveOrEdit(IDatabaseManager databaseManager)
        {
            InitializeComponent();
            _databaseManager = databaseManager;
            MainThread.BeginInvokeOnMainThread(InitializeDataAsync);
        }

        //Metode, kas inicializç datus (1)
        private async void InitializeDataAsync()
        {
            try
            {
                await LoadAssignmentsAsync();
                await LoadSubmissionsAsync();
                await LoadStudentsAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Initialization error: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        //Metode, lai ielâdçtu uzdevumus (1)
        private async Task LoadAssignmentsAsync()
        {
            try
            {
                AssignmentPicker.Items.Clear();
                AssignmentChangePicker.Items.Clear();

                var assignments = await _databaseManager.GetAssignments();
                foreach (var assignment in assignments)
                {
                    AssignmentPicker.Items.Add(assignment.Description);
                    AssignmentChangePicker.Items.Add(assignment.Description);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error loading assignments: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error loading assignments: {ex.Message}");
            }
        }

        //Metode, lai ielâdçtu uzdevuma iesniegumus (1)
        private async Task LoadSubmissionsAsync()
        {
            try
            {
                SubmissionPicker.Items.Clear();

                var submissions = await _databaseManager.GetSubmissions();
                foreach (var submission in submissions)
                {
                    SubmissionPicker.Items.Add($"{submission.Student.FullName} - {submission.Assignement.Description}");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error loading submissions: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error loading submissions: {ex.Message}");
            }
        }

        //Metode, lai ielâdçtu studentus (1)
        private async Task LoadStudentsAsync()
        {
            try
            {
                StudentPicker.Items.Clear();

                var students = await _databaseManager.GetStudents();
                foreach (var student in students)
                {
                    StudentPicker.Items.Add(student.FullName);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error loading students: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error loading students: {ex.Message}");
            }
        }

        //Event, kas tiek izsaukts, kad tiek izvçlçts uzdevums no izvçles saraksta (1)
        private async void OnAssignmentPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (AssignmentPicker.SelectedIndex < 0) return;

                string selectedAssignmentDescription = AssignmentPicker.SelectedItem.ToString();
                var assignment = await _databaseManager.GetAssignmentByDescriptionAsync(selectedAssignmentDescription);

                if (assignment != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AssignmentDeadlineEntry.Text = assignment.Deadline.ToDateTime(new TimeOnly()).ToString("dd.MM.yyyy");
                        AssignmentDeadlineEntry.IsEnabled = true;
                        EditAssignmentButton.IsEnabled = true;
                        DeleteAssignmentButton.IsEnabled = true;
                        AssignmentResultLabel.Text = string.Empty;
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error selecting assignment: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error selecting assignment: {ex.Message}");
            }
        }

        //Event, kas tiek izsaukts, kad tiek tieði nomainît uzdevuma apraksts no viena uz citu (1)
        private async void OnAssignmentChangePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (AssignmentChangePicker.SelectedIndex < 0) return;

                string selectedAssignmentDescription = AssignmentChangePicker.SelectedItem.ToString();
                var assignment = await _databaseManager.GetAssignmentByDescriptionAsync(selectedAssignmentDescription);

                if (assignment != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AssignmentDeadlineEntry.Text = assignment.Deadline.ToDateTime(new TimeOnly()).ToString("dd.MM.yyyy");
                        AssignmentDeadlineEntry.IsEnabled = true;
                        EditAssignmentButton.IsEnabled = true;
                        DeleteAssignmentButton.IsEnabled = true;
                        AssignmentResultLabel.Text = string.Empty;
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while selecting the assignment for changes: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error selecting assignment: {ex.Message}");
            }
        }

        //Event, kas tiek izsaukts, kad tiek noklikðíinâta rediìçðanas poga (1)
        private async void OnEditAssignmentButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (AssignmentPicker.SelectedIndex < 0) return;

                string selectedAssignmentDescription = AssignmentPicker.SelectedItem.ToString();
                var assignment = await _databaseManager.GetAssignmentByDescriptionAsync(selectedAssignmentDescription);

                if (assignment != null)
                {
                    if (DateTime.TryParseExact(AssignmentDeadlineEntry.Text, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var deadline))
                    {
                        assignment.Deadline = new MD1.Date(deadline.Day, deadline.Month, deadline.Year);
                        await _databaseManager.UpdateAssignment(assignment);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            AssignmentResultLabel.Text = $"Assignment '{assignment.Description}' updated successfully.";
                        });
                    }
                    else
                    {
                        await DisplayAlert("Error", "Invalid date format. Use DD.MM.YYYY.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error editing assignment: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error editing assignment: {ex.Message}");
            }
        }

        //Event, kas tiek izsaukts, kad izdzçðanas poga tiek nospiesta (1)
        private async void OnDeleteAssignmentButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (AssignmentPicker.SelectedIndex < 0) return;

                string selectedAssignmentDescription = AssignmentPicker.SelectedItem.ToString();
                var assignment = await _databaseManager.GetAssignmentByDescriptionAsync(selectedAssignmentDescription);

                if (assignment != null)
                {
                    await _databaseManager.DeleteAssignment(assignment);

                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        AssignmentResultLabel.Text = $"Assignment '{assignment.Description}' deleted successfully.";
                        await LoadAssignmentsAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error deleting assignment: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error deleting assignment: {ex.Message}");
            }
        }

        //Event, kas tiek izsaukts, kad tiek izvçlçts uzdevuma iesniegums no izvçlnes saraksta (1)
        private async void OnSubmissionPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (SubmissionPicker.SelectedIndex < 0) return;

                string selectedSubmission = SubmissionPicker.SelectedItem.ToString();
                var parts = selectedSubmission.Split(new[] { " - " }, StringSplitOptions.None);

                if (parts.Length < 2) return;

                string studentName = parts[0];
                string assignmentDescription = parts[1];

                var submission = await _databaseManager.GetSubmissionByStudentAndAssignmentAsync(studentName, assignmentDescription);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    EditSubmissionButton.IsEnabled = submission != null;
                    DeleteSubmissionButton.IsEnabled = submission != null;
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error selecting submission: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error selecting submission: {ex.Message}");
            }
        }

        //Event,kas tiek izsaukts, kad uzdevuma iesnieguma autoru vçlas nomainît (1)
        private async void OnStudentPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (StudentPicker.SelectedIndex < 0 || StudentPicker.SelectedItem == null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        EditSubmissionButton.IsEnabled = false;
                        DeleteSubmissionButton.IsEnabled = false;
                    });
                    return;
                }

                string selectedStudentName = StudentPicker.SelectedItem.ToString();
                var student = await _databaseManager.GetStudentByNameAsync(selectedStudentName);

                if (student != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        EditSubmissionButton.IsEnabled = true;
                        DeleteSubmissionButton.IsEnabled = true;
                    });
                }
                else
                {
                    await DisplayAlert("Error", "Selected student could not be found in the database.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while selecting the student: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error selecting student: {ex.Message}");
            }
        }

        //Event,kas tiek izsaukts, kad tiek nospiesta uzdevuma iesnieguma rediìçðanas poga (1)
        private async void OnEditSubmissionButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (SubmissionPicker.SelectedIndex < 0) return;

                string selectedSubmission = SubmissionPicker.SelectedItem.ToString();
                var parts = selectedSubmission.Split(new[] { " - " }, StringSplitOptions.None);

                if (parts.Length < 2) return;

                string studentName = parts[0];
                string assignmentDescription = parts[1];

                var submission = await _databaseManager.GetSubmissionByStudentAndAssignmentAsync(studentName, assignmentDescription);

                if (submission != null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AssignmentResultLabel.Text = $"Editing submission for {studentName} on assignment '{assignmentDescription}'.";
                    });
                }
                else
                {
                    await DisplayAlert("Error", "Selected submission could not be found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while editing the submission: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error editing submission: {ex.Message}");
            }
        }

        //Event,kas tiek izsaukts, kad tiek nospiesta izdzçðanas poga uzdevuma iesniegumam (1)
        private async void OnDeleteSubmissionButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (SubmissionPicker.SelectedIndex < 0) return;

                string selectedSubmission = SubmissionPicker.SelectedItem.ToString();
                var parts = selectedSubmission.Split(new[] { " - " }, StringSplitOptions.None);

                if (parts.Length < 2) return;

                string studentName = parts[0];
                string assignmentDescription = parts[1];

                var submission = await _databaseManager.GetSubmissionByStudentAndAssignmentAsync(studentName, assignmentDescription);

                if (submission != null)
                {
                    await _databaseManager.DeleteSubmission(submission);

                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        AssignmentResultLabel.Text = $"Submission deleted for '{studentName}' on assignment '{assignmentDescription}'.";
                        await LoadSubmissionsAsync();
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error deleting submission: {ex.Message}", "OK");
                System.Diagnostics.Debug.WriteLine($"Error deleting submission: {ex.Message}");
            }
        }
    }
}

/**
 * Atsauces:
 *     1. ChatGPT
 * **/