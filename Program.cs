namespace cs_lab02_implementacjainterfejsów;

class Program
{
	static void Main(string[] args)
	{
		var p1 = new Pracownik();
        p1.Wynagrodzenie = 100;
        p1.Nazwisko = "   Molenda  ";
        p1.DataZatrudnienia = new DateTime(2010, 10, 01);
        Console.WriteLine(p1);
	}
}