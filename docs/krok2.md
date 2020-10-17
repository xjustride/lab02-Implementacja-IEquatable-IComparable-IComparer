# Implementacja interfejsów `IEquatable`, `IComparable`, `IComparer` - rozwiązanie

* Autor: _Krzysztof Molenda_
* Wersja: 2019-10-03

[Treść ćwiczenia](../ReadMe.md)

## Krok 2 - implementacja naturalnego porządku w klasie

### Ad. 1, 2, 3

```csharp
// file: Pracownik.cs
public class Pracownik : IEquatable<Pracownik>
{
  // bez zmian - z kroku 1
}
```

```csharp
// file: Program.cs, class: Program
static void Main(string[] args)
{
    //Krok1();
    Krok2();
}

static void Krok2()
{
    var lista = new List<Pracownik>();
    lista.Add( new Pracownik("CCC", new DateTime(2010, 10, 02), 1050 ) );
    lista.Add( new Pracownik("AAA", new DateTime(2010, 10, 01), 100 ) );
    lista.Add( new Pracownik("DDD", new DateTime(2010, 10, 03), 2000 ) );
    lista.Add( new Pracownik("AAA", new DateTime(2011, 10, 01), 1000 ) );
    lista.Add( new Pracownik("BBB", new DateTime(2010, 10, 01), 1050 ) );

    Console.WriteLine( lista ); //wypisze typ, a nie zawartość listy

    Console.WriteLine("-- Wariant 1 --");
    foreach( var pracownik in lista )
        Console.WriteLine( pracownik );

    Console.WriteLine("-- Wariant 2 --");
    lista.ForEach( (p) => {Console.Write(p + ",");} );
    Console.WriteLine(   );

    Console.WriteLine("-- Wariant 3 --");
    Console.WriteLine( string.Join('\n', lista) );

    lista.Sort(); //zadziała, jeśli klasa Pracownik implementuje IComparable<Pracownik>

    Console.WriteLine("Po posortowaniu:");
    foreach( var pracownik in lista )
        Console.WriteLine( pracownik );
}
```

Output:

```plaintext
Unhandled Exception: System.InvalidOperationException: Failed to compare two elements in the array. ---> System.ArgumentException: At least one object must implement IComparable.
```

### Ad. 4, 5

```csharp
// file: Pracownik.cs
public class Pracownik : IEquatable<Pracownik>, IComparable<Pracownik>
{
  // bez zmian - z kroku 1

    #region implementacja IComparable<Pracownik>
    public int CompareTo(Pracownik other)
    {
        if (other is null) return 1; // w C#: null jest najmniejszą wartością (`this > null`)
        if (this.Equals(other)) return 0; //zgodność z Equals (`this == other`)

        if (this.Nazwisko != other.Nazwisko)
            return this.Nazwisko.CompareTo(other.Nazwisko);

        // ponieważ nazwiska równe, porządkujemy wg daty
        if (!this.DataZatrudnienia.Equals(other.DataZatrudnienia)) // != zamiast !Equals
            return this.DataZatrudnienia.CompareTo(other.DataZatrudnienia);

        // ponieważ nazwiska równe i daty równe, porządkujemy wg wynagrodzenia
        return this.Wynagrodzenie.CompareTo(other.Wynagrodzenie);
    }

    #endregion implementacja IComparable<Pracownik>
}
```

Output:

```plaintext
System.Collections.Generic.List`1[step2.Pracownik]
-- Wariant 1 --
(CCC, 2 paź 2010 (109), 1050 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
-- Wariant 2 --
(CCC, 2 paź 2010 (109), 1050 PLN),(AAA, 1 paź 2010 (109), 100 PLN),(DDD, 3 paź 2010 (109), 2000 PLN),(AAA, 1 paź 2011 (97), 1000 PLN),(BBB, 1 paź 2010 (109), 1050 PLN),
-- Wariant 3 --
(CCC, 2 paź 2010 (109), 1050 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
Po posortowaniu:
(AAA, 1 paź 2010 (109), 100 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
(CCC, 2 paź 2010 (109), 1050 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
```

