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

    //Metode, kas izprintç ðobrîdçjos visus datus no datubâzes (1)
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

    //Metode, kas ielâdç datus no faila, bet ðî ir nevajadzîga funkcija, jo mçs darbojamies ar datiem no datubâzes (1)
    private async void ButtonLoad_Clicked(object sender, EventArgs e)
    {
        try
        {
            string path = "C:\\Temp\\MD1\\data.txt";
            await dataManager.LoadAsync();
            SchoolSystemDataManager.Text = "Poga strâdâ, bet tâ kâ mçs strâdâjam ar datiem no datubâzes, ðî funkcija nav vajadzîga!";
        }
        catch (Exception ex)
        {
            SchoolSystemDataManager.Text = $"Error loading data: {ex.Message}";
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    //Metode, kas saglabâ ðobrîdçjos datus datubâzç (1)
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

    //metode, kas izveido un ielâdç testa datus (1)
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

    //Metode, kas resets datus datubâzç (1)
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

    //Mwetode,kas tiek izsaukta, kad tiek pâriets uz ðo lapu, lai uzdrukâtu paðreizçjos datus (es nezinu vai ir saprotami, bet tas ir kâ es to izskaidrotu) (1) 
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