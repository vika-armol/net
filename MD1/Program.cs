// See https://aka.ms/new-console-template for more information

using MD1;

//Tiek norādīts ceļš, kur tiks saglabāti dati
string path = "C:\\Temp\\MD1\\data.txt";

//Tiek izveidota direktorija norādītajam ceļam, ja tā vēl nepastāv
System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));

var dm = new DataManager(new SchoolSystem());


Console.WriteLine("Pēc CreateTestData: ");
dm.createTestData(); //Skolas sistēmā tiek ielikti dati
Console.WriteLine(dm.print()); //Dati tiek izprintēti

dm.save(path); //Saglabā šobrīdējos datus teksta failā

Console.WriteLine("Pēc Reset: ");
dm.reset(); //Dati tiek izdzēsti no skolas sistēmas, bet ne no teksta faila
Console.WriteLine(dm.print()); //Dati tiek izprintēti

dm.load(path); //Ielādē iepriekš saglabātos failus no teksta faila
Console.WriteLine("Pēc Load: ");
Console.WriteLine(dm.print()); //Dati tiek izprintēti

Console.ReadLine();