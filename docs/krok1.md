# Implementacja interfejsów `IEquatable`, `IComparable`, `IComparer` - rozwiązanie

* Autor: _Krzysztof Molenda_
* Wersja: 2019-10-03

[Treść ćwiczenia](index.md)

## Krok 1 - realizacja klasy, implementacja pojęcia "taki sam"

### Ad. 1, 2, 3

```csharp
// file: Pracownik.cs
public class Pracownik
{
    public string Nazwisko { get; set; } //zaimplementuj obcinanie spacji (trim)

    public DateTime DataZatrudnienia { get; set; } //zaimplementuj: data zatrudnienia nie później niż dzisiaj, w przeciwnym przypadku throw ArgumentException

    private decimal _wyn;
    public decimal Wynagrodzenie
    {
        get => _wyn;
        set => _wyn = (value < 0)? 0: value;
            // {
            //     if (value < 0) _wyn = 0;
            //     else _wyn = value;
            // }
    }
    public override string ToString() => $"({Nazwisko}, {DataZatrudnienia:d MMM yyyy}, {Wynagrodzenie} PLN)";
}
```

```csharp
// file: Program.cs, class: Program, method: Main()
var p1 = new Pracownik();
p1.Wynagrodzenie = 100;
p1.Nazwisko = "   Molenda  ";
p1.DataZatrudnienia = new DateTime(2010, 10, 01);
Console.WriteLine(p1);
```

Output:

```plaintext
(   Molenda  , 1 paź 2010, 100 PLN)
```

### Ad. 4, 5, 6

```csharp
// file: Pracownik.cs
public class Pracownik
{
    // ... c.d.

    public override string ToString() 
        => $"({Nazwisko}, {DataZatrudnienia:d MMM yyyy} ({CzasZatrudnienia}), {Wynagrodzenie} PLN)";

    public int CzasZatrudnienia => (DateTime.Now - DataZatrudnienia).Days / 30;

    public Pracownik(string nazwisko, DateTime dataPrzyjecia, decimal wynagrodzenie)
    {
        Nazwisko = nazwisko;
        DataZatrudnienia = dataPrzyjecia;
        Wynagrodzenie = wynagrodzenie;
    }

}
```

```csharp
// file: Program.cs, class: Program, method: Main()
var p1 = new Pracownik();
Console.WriteLine(p1);
p1.Wynagrodzenie = 100;
p1.Nazwisko = "   Molenda  ";
p1.DataZatrudnienia = new DateTime(2010, 10, 01);
Console.WriteLine(p1);

var p2 = new Pracownik("Abacki", new DateTime(2011,8,9), 200);
Console.WriteLine(p2);
```

Output:

```plaintext
(Anonomm, 9 paź 2019 (0), 0 PLN)
(   Molenda  , 1 paź 2010 (109), 100 PLN)
(Abacki, 9 sie 2011 (99), 200 PLN)
```

### Ad. 7, 8, 9

```csharp
// file: Pracownik.cs
public class Pracownik : IEquatable<Pracownik>
{
    // ... c.d.

    #region implementacja IEquatable<Pracownik>

    // implementacja Equals wymagana przez implementację interfejsu IEquatable<Pracownik>
    // Equals nie może zgłaszać wyjątków !
    public bool Equals(Pracownik other)
    {
        if (other is null) return false;
        if (Object.ReferenceEquals(this, other)) //other i this są referencjami do tego samego obiektu
            return true;

        return (Nazwisko == other.Nazwisko &&
                DataZatrudnienia == other.DataZatrudnienia &&
                Wynagrodzenie == other.Wynagrodzenie);
    }

    // --- formalnie wymagane przesłonięcie Equals i GetHashCode z klasy Object - równocześnie
    // https://docs.microsoft.com/en-Us/dotnet/api/system.object.gethashcode?view=netstandard-2.1#System_Object_GetHashCode
    public override bool Equals(object obj)
    {
        if (obj is Pracownik)
            return Equals((Pracownik)obj);
        else
            return false;
    }

    public override int GetHashCode() => (Nazwisko, DataZatrudnienia, Wynagrodzenie).GetHashCode();
    // --- koniec przesłonięcia Equals i GetHashCode z klasy Object - równocześnie

    // dodatkowo statyczny wariant Equals(Pracownik, Pracownik)
    public static bool Equals(Pracownik p1, Pracownik p2)
    {
        if ((p1 is null) && (p2 is null)) return true; // w C#: null == null
        if ((p1 is null)) return false;

        //p1 nie jest `null`, nie będzie NullReferenceException
        return p1.Equals(p2);
    }

    // przeciążenie operatora `==` i `!=`
    public static bool operator ==(Pracownik p1, Pracownik p2) => Equals(p1, p2);
    public static bool operator !=(Pracownik p1, Pracownik p2) => !(p1 == p2);

    #endregion implementacja IEquatable<Pracownik>
}
```

### Ad. 10

```csharp
class Program
{
    static void Main(string[] args)
    {
        Krok1();
    }

    static void Krok1()
    {
        Console.WriteLine("--- Sprawdzenie poprawności tworzenia obiektu ---");
        Pracownik p = new Pracownik("Kowalski", new DateTime(2010, 10, 1), 1_000);
        Console.WriteLine(p);

        Console.WriteLine("--- Sprawdzenie równości obiektów ---");
        Pracownik p1 = new Pracownik("Nowak", new DateTime(2010, 10, 1), 1_000);
        Pracownik p2 = new Pracownik("Nowak", new DateTime(2010, 10, 1), 1_000);
        Pracownik p3 = new Pracownik("Kowalski", new DateTime(2010, 10, 1), 1_000);
        Pracownik p4 = p1;
        Console.WriteLine($"p1: {p1} hashCode: {p1.GetHashCode()}");
        Console.WriteLine($"p2: {p2} hashCode: {p2.GetHashCode()}");
        Console.WriteLine($"p3: {p3} hashCode: {p3.GetHashCode()}");
        Console.WriteLine($"p4: {p4} hashCode: {p4.GetHashCode()}");
        Console.WriteLine();

        Console.WriteLine($"--- Równość dla p1 oraz p2 -");
        Console.WriteLine($"Object.ReferenceEquals(p1, p2): {Object.ReferenceEquals(p1, p2)}");
        Console.WriteLine($"p1.Equals(p2): {p1.Equals(p2)}");
        Console.WriteLine($"p1 == p2: {p1 == p2}");
        Console.WriteLine();

        Console.WriteLine($"--- Równość dla p1 oraz p3 -");
        Console.WriteLine($"Object.ReferenceEquals(p1, p3): {Object.ReferenceEquals(p1, p3)}");
        Console.WriteLine($"p1.Equals(p3): {p1.Equals(p3)}");
        Console.WriteLine($"p1 == p3: {p1 == p3}");
        Console.WriteLine();

        Console.WriteLine($"--- Równość dla p1 oraz p4 -");
        Console.WriteLine($"Object.ReferenceEquals(p1, p4): {Object.ReferenceEquals(p1, p4)}");
        Console.WriteLine($"p1.Equals(p3): {p1.Equals(p4)}");
        Console.WriteLine($"p1 == p4: {p1 == p4}");
    }
}
```

Output:

```plaintext
--- Sprawdzenie poprawności tworzenia obiektu ---
(Kowalski, 1 paź 2010 (109), 1000 PLN)
--- Sprawdzenie równości obiektów ---
p1: (Nowak, 1 paź 2010 (109), 1000 PLN) hashCode: 752904271
p2: (Nowak, 1 paź 2010 (109), 1000 PLN) hashCode: 752904271
p3: (Kowalski, 1 paź 2010 (109), 1000 PLN) hashCode: 18179216
p4: (Nowak, 1 paź 2010 (109), 1000 PLN) hashCode: 752904271

--- Równość dla p1 oraz p2 -
Object.ReferenceEquals(p1, p2): False
p1.Equals(p2): True
p1 == p2: True

--- Równość dla p1 oraz p3 -
Object.ReferenceEquals(p1, p3): False
p1.Equals(p3): False
p1 == p3: False

--- Równość dla p1 oraz p4 -
Object.ReferenceEquals(p1, p4): True
p1.Equals(p3): True
p1 == p4: True
```

