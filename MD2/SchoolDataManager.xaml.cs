using MD1;
namespace MD2;

public partial class SchoolDataManager : ContentPage
{
    private readonly IDatabaseManager dataManager;

    public SchoolDataManager() : this(new DatabaseManager(new SchoolSystemContextFactory()))
    {
    }

    public SchoolDataManager(IDatabaseManager databaseManager)
    {
        InitializeComponent();
        dataManager = databaseManager; 
    }

    //Metode, kas izprint� �obr�d�jos visus datus no datub�zes (1)
    private async void ButtonPrint_Clicked(object sender, EventArgs e)
    {
        try
        {
            SchoolSystemDataManager.Text = await dataManager.PrintAsync();
        }
        catch (Exception ex)
        {
            SchoolSystemDataManager.Text = $"Error printing data: {ex.Message}";
            Console.WriteLine($"Error printing data: {ex.Message}");
        }
    }

    //Metode, kas iel�d� datus no faila, bet �� ir nevajadz�ga funkcija, jo m�s darbojamies ar datiem no datub�zes (1)
    private async void ButtonLoad_Clicked(object sender, EventArgs e)
    {
        try
        {
            string path = "C:\\Temp\\MD1\\data.txt";
            await dataManager.LoadAsync();
            SchoolSystemDataManager.Text = "Poga str�d�, bet t� k� m�s str�d�jam ar datiem no datub�zes, �� funkcija nav vajadz�ga!";
        }
        catch (Exception ex)
        {
            SchoolSystemDataManager.Text = $"Error loading data: {ex.Message}";
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    //Metode, kas saglab� �obr�d�jos datus datub�z� (1)
    private async void ButtonSave_Clicked(object sender, EventArgs e)
    {
        try
        {
            string path = "C:\\Temp\\MD1\\data.txt";
            using (var dbContext = dataManager.GetDbContext())
            {
                await dataManager.SaveAsync(dbContext);
            }
            SchoolSystemDataManager.Text = "Data saved successfully!";
        }
        catch (Exception ex)
        {
            SchoolSystemDataManager.Text = $"Error saving data: {ex.Message}";
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    //metode, kas izveido un iel�d� testa datus (1)
    private async void ButtonCreate_Clicked(object sender, EventArgs e)
    {
        try
        {
            await dataManager.CreateTestDataAsync();
            SchoolSystemDataManager.Text = "Test data generated!";
        }
        catch (Exception ex)
        {
            SchoolSystemDataManager.Text = $"Error creating test data: {ex.Message}";
            Console.WriteLine($"Error creating test data: {ex.Message}");
        }
    }

    //Metode, kas resets datus datub�z� (1)
    private async void ButtonReset_Clicked(object sender, EventArgs e)
    {
        try
        {
            await dataManager.ResetAsync();
            SchoolSystemDataManager.Text = "Data reset!";
        }
        catch (Exception ex)
        {
            SchoolSystemDataManager.Text = $"Error resetting data: {ex.Message}";
            Console.WriteLine($"Error resetting data: {ex.Message}");
        }
    }

    //Mwetode,kas tiek izsaukta, kad tiek p�riets uz �o lapu, lai uzdruk�tu pa�reiz�jos datus (es nezinu vai ir saprotami, bet tas ir k� es to izskaidrotu) (1) 
    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        try
        {
            // Print the current data when navigating to the page
            SchoolSystemDataManager.Text = await dataManager.PrintAsync();
        }
        catch (Exception ex)
        {
            SchoolSystemDataManager.Text = $"Error printing data on page load: {ex.Message}";
            Console.WriteLine($"Error printing data on page load: {ex.Message}");
        }
    }
}

/*
Atsauces:
    1. ChatGPT
*/