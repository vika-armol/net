using MD1;

namespace MD2
{
    public partial class App : Application
    {
        public static DatabaseManager DatabaseManager { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        //Šī metode tiek izsaukta, kad lietotne uzsākas (1)
        protected override async void OnStart()
        {
            
            var contextFactory = new SchoolSystemContextFactory();
            DatabaseManager = new DatabaseManager(contextFactory);

            // await CreateTestData();
        }

        private async Task CreateTestData()
        {
            try
            {
                await DatabaseManager.CreateTestDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating test data: {ex.Message}");
            }
        }
    }
}

/**
 * Atsauces:
 *     1. ChatGPT
 * **/