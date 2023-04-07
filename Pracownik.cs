using System;

public class Pracownik
{
	private string nazwisko;
	private DateTime dataZatrudnienia;
	private decimal wynagrodzenie;

	public Pracownik(string nazwisko, DateTime dataZatrudnienia, decimal wynagrodzenie)
	{
		this.nazwisko = nazwisko.Trim();
		if (dataZatrudnienia > DateTime.Now)
		{
			throw new ArgumentException("Data zatrudnienia nie może być późniejsza niż dzisiaj.");
		}
		this.dataZatrudnienia = dataZatrudnienia;
		this.wynagrodzenie = (wynagrodzenie < 0) ? 0 : wynagrodzenie;
	}

	public Pracownik() : this("Anonim", DateTime.Now, 0) { }

	public string Nazwisko
	{
		get { return nazwisko; }
		set { nazwisko = value.Trim(); }
	}

	public DateTime DataZatrudnienia
	{
		get { return dataZatrudnienia; }
		set
		{
			if (value > DateTime.Now)
			{
				throw new ArgumentException("Data zatrudnienia nie może być późniejsza niż dzisiaj.");
			}
			dataZatrudnienia = value;
		}
	}

	public decimal Wynagrodzenie
	{
		get { return wynagrodzenie; }
		set { wynagrodzenie = (value < 0) ? 0 : value; }
	}

	public int CzasZatrudnienia
    {
        get
        {
	        TimeSpan czas = DateTime.Now - DataZatrudnienia;
            int dni = (int)czas.TotalDays;
            int miesiace = (int)Math.Round((double)dni / 30);
    
            return miesiace;
        }
    }


	public override string ToString()
	{
		return $"{Nazwisko}, {DataZatrudnienia.ToString("dd MMM yyyy")}, {Wynagrodzenie} PLN";
	}
}
