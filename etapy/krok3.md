# Implementacja interfejsów `IEquatable`, `IComparable`, `IComparer` - rozwiązanie

* Autor: _Krzysztof Molenda_
* Wersja: 2019-10-03

[Treść ćwiczenia](../ReadMe.md)

## Krok 3 - wykorzystanie definicji zewnętrznego porządku sortowania

### Ad. 1

```csharp
// file: Pracownik.cs
public class Pracownik : IEquatable<Pracownik>, IComparable<Pracownik>
{
  // bez zmian - z kroku 2
}
```

```csharp
// file: Program.cs, class: Program
static void Main(string[] args)
{
    //Krok1();
    //Krok2();
    Krok3();
}

static void Krok3()
{
    var lista = new List<Pracownik>();
    lista.Add(new Pracownik("CCC", new DateTime(2010, 10, 02), 1050));
    lista.Add(new Pracownik("AAA", new DateTime(2010, 10, 01), 100));
    lista.Add(new Pracownik("DDD", new DateTime(2010, 10, 03), 2000));
    lista.Add(new Pracownik("AAA", new DateTime(2011, 10, 01), 1000));
    lista.Add(new Pracownik("BBB", new DateTime(2010, 10, 01), 1050));

    Console.WriteLine(lista); //wypisze typ, a nie zawartość listy
    foreach (var pracownik in lista)
        System.Console.WriteLine(pracownik);

    Console.WriteLine("--- Zewnętrzny porządek - obiekt typu IComparer" + Environment.NewLine
                        + "najpierw według czasu zatrudnienia (w miesiącach), " + Environment.NewLine
                        + "a później według wynagrodzenia - wszystko rosnąco");

    lista.Sort(new WgCzasuZatrudnieniaPotemWgWynagrodzeniaComparer());
    foreach (var pracownik in lista)
        System.Console.WriteLine(pracownik);
}
```

```csharp
// file: WgCzasuZatrudnieniaPotemWgWynagrodzeniaComparer.cs
public class WgCzasuZatrudnieniaPotemWgWynagrodzeniaComparer : IComparer<Pracownik>
{
    public int Compare(Pracownik x, Pracownik y)
    {
        if (x is null && y is null) return 0;
        if (x is null && !(y is null)) return -1;
        if (!(x is null) && y is null) return +1;

        //x oraz y nie są null
        if (x.CzasZatrudnienia != y.CzasZatrudnienia)
            return (x.CzasZatrudnienia).CompareTo(y.CzasZatrudnienia);

        //daty są takie same
        return x.Wynagrodzenie.CompareTo(y.Wynagrodzenie);
    }
}
```

Output:

```plaintext
System.Collections.Generic.List`1[step3.Pracownik]
(CCC, 2 paź 2010 (109), 1050 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
--- Zewnętrzny porządek - obiekt typu IComparer
najpierw według czasu zatrudnienia (w miesiącach),
a później według wynagrodzenia - wszystko rosnąco
(AAA, 1 paź 2011 (97), 1000 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
(CCC, 2 paź 2010 (109), 1050 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
```

### Ad. 2

Uzupełnienie metody `Krok3()` z pkt. 1

```csharp
static void Krok3()
{
    // ... c.d.
    Console.WriteLine("--- Zewnętrzny porządek - delegat typu Comparison" + Environment.NewLine
                        + "najpierw według czasu zatrudnienia (w miesiącach), " + Environment.NewLine
                        + "a później kolejno według nazwiska i wynagrodzenia - wszystko rosnąco");
    // sklejamy odpowiednio napisy i je porównujemy
    lista.Sort((p1, p2) => (p1.CzasZatrudnienia.ToString("D3")
                                + p1.Nazwisko + p1.Wynagrodzenie.ToString("00000.00")
                            )
                            .CompareTo
                            (p2.CzasZatrudnienia.ToString("D3")
                                + p2.Nazwisko + p2.Wynagrodzenie.ToString("00000.00")
                            )
                );
    foreach (var pracownik in lista)
        System.Console.WriteLine(pracownik);
}
```

Output:

```plaintext
...
...
--- Zewnętrzny porządek - delegat typu Comparison
najpierw według czasu zatrudnienia (w miesiącach),
a później kolejno według nazwiska i wynagrodzenia - wszystko rosnąco
(AAA, 1 paź 2011 (97), 1000 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
(CCC, 2 paź 2010 (109), 1050 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
```

### Ad. 3

Uzupełnienie metody `Krok3()` z pkt. 2

```csharp
static void Krok3()
{
    // ... c.d.
    Console.WriteLine("--- Zewnętrzny porządek - delegat typu Comparison" + Environment.NewLine
                        + "kolejno: malejąco według wynagrodzenia, " + Environment.NewLine
                        + "później rosnąca według czasu zatrudnienia");
    //budujemy warunek wyrażeniem warunkowym ()?:
    lista.Sort((p1, p2) => (p1.Wynagrodzenie != p2.Wynagrodzenie) ?
                                (-1) * (p1.Wynagrodzenie.CompareTo(p2.Wynagrodzenie)) :
                                p1.CzasZatrudnienia.CompareTo(p2.CzasZatrudnienia)
                );
    foreach (var pracownik in lista)
        System.Console.WriteLine(pracownik);
}
```

Output:

```plaintext
...
...
--- Zewnętrzny porządek - delegat typu Comparison
kolejno: malejąco według wynagrodzenia,
później rosnąca według czasu zatrudnienia
(DDD, 3 paź 2010 (109), 2000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
(CCC, 2 paź 2010 (109), 1050 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
```
