using MD1; //Importē MD1, kas satur nepieciešamās klases

namespace MD2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            try
            {
                InitializeComponent();

                databaseManager = App.DatabaseManager;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in AppShell: {ex.Message}");
                Console.WriteLine($"Error in AppShell: {ex.Message}");
                throw;
            }
        }
        //Īpašība "databaseManager" nodrošina piekļuvi DatabaseManager instancei visā aplikācijā (1)
        public static DatabaseManager databaseManager { get; private set; }
    }
}

/*
Atsauces:
    1.ChatGPT
*/