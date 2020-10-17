# Implementacja interfejsów `IEquatable`, `IComparable`, `IComparer` - rozwiązanie

* Autor: _Krzysztof Molenda_
* Wersja: 2019-10-03

[Treść ćwiczenia](../ReadMe.md)

## Krok 4 - porządkowanie z wykorzystaniem LINQ


```csharp
// file: Pracownik.cs
public class Pracownik : IEquatable<Pracownik>, IComparable<Pracownik>
{
  // bez zmian - z kroku 3
}
```

```csharp
// file: Program.cs, class: Program
static void Main(string[] args)
{
    //Krok1();
    //Krok2();
    //Krok3();
    Krok4();
}

static void Krok4()
{
    var lista = new List<Pracownik>()
    {
        new Pracownik("CCC", new DateTime(2010, 10, 02), 1050),
        new Pracownik("AAA", new DateTime(2010, 10, 01), 100),
        new Pracownik("DDD", new DateTime(2010, 10, 03), 2000),
        new Pracownik("AAA", new DateTime(2011, 10, 01), 1000),
        new Pracownik("BBB", new DateTime(2010, 10, 01), 1050)
    };

    Console.WriteLine(lista); //wypisze typ, a nie zawartość listy
    foreach (var pracownik in lista)
        System.Console.WriteLine(pracownik);

    Console.WriteLine("--- Porządkowanie za pomocą metod rozszerzających Linq" + Environment.NewLine
                        + "kolejno: rosnąco według wynagrodzenia, " + Environment.NewLine
                        + "później malejąco według nazwiska");
    var query = lista.OrderBy(p => p.Wynagrodzenie).ThenByDescending(p => p.Nazwisko);
    foreach (var pracownik in query)
        Console.WriteLine(pracownik);
}
```

Output:

```plaintext
System.Collections.Generic.List`1[step4.Pracownik]
(CCC, 2 paź 2010 (109), 1050 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
--- Porządkowanie za pomocą metod rozszerzających Linq
kolejno: rosnąco według wynagrodzenia,
później malejąco według nazwiska
(AAA, 1 paź 2010 (109), 100 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(CCC, 2 paź 2010 (109), 1050 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
```
