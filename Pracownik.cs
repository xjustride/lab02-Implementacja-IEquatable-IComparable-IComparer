using System;

public class Pracownik : IEquatable<Pracownik>
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
		if (CzasZatrudnienia > 20 && CzasZatrudnienia % 2 == 0)
		{
			return ($"{Nazwisko}, {DataZatrudnienia.ToString("dd MMM yyyy")}, {Wynagrodzenie} PLN, {CzasZatrudnienia} miesiace");
		}
		else
		{
			return (
				$"{Nazwisko}, {DataZatrudnienia.ToString("dd MMM yyyy")}, {Wynagrodzenie} PLN, {CzasZatrudnienia} miesiecy");
		}
		
	}

	public bool Equals(Pracownik? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return nazwisko == other.nazwisko && dataZatrudnienia.Equals(other.dataZatrudnienia) && wynagrodzenie == other.wynagrodzenie;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Pracownik)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(nazwisko, dataZatrudnienia, wynagrodzenie);
	}
}


