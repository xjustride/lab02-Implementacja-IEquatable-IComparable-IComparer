# Implementacja interfejsów `IEquatable`, `IComparable`, `IComparer` - rozwiązanie

* Autor: _Krzysztof Molenda_
* Wersja: 2019-10-03

[Treść ćwiczenia](../ReadMe.md)

## Krok 5 - własna generyczna metoda sortująca

```csharp
// file: Pracownik.cs
public class Pracownik : IEquatable<Pracownik>, IComparable<Pracownik>
{
  // bez zmian - z kroku 4
}
```

### Metoda wykorzystująca wewnętrzny porządek w zbiorze

* `void Sortuj<T>( this IList<T> lista )` 

```csharp
// file: Sortowanie.cs
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;


/// <summary>
/// Statyczna klasa pomocnicza, dostarczająca metody sortowania listy.
/// </summary>
/// <remarks>
/// Implementacja algorytmu sortowania bąbelkowego.
/// Pseudokod:
/// <code>
/// procedure bubbleSort( A : lista elementów do posortowania )
///   n = liczba_elementów(A)
///    do
///     for (i = 0; i < n-1; i++) do:
///       if A[i] > A[i+1] then
///         swap(A[i], A[i+1])
///       end if
///     end for
///     n = n-1
///   while n > 1
/// end procedure
/// </code>
/// </remarks>
public static class Sortowanie
{
    // metoda rozszerzająca IList<T>
    // zamienia miejscami elementy listy o wskazanych indeksach
    public static void SwapElements<T>(this IList<T> list, int firstIndex, int secondIndex)
    {
        Contract.Requires(list != null);
        Contract.Requires(firstIndex >= 0 && firstIndex < list.Count);
        Contract.Requires(secondIndex >= 0 && secondIndex < list.Count);
        if (firstIndex == secondIndex)
        {
            return;
        }
        T temp = list[firstIndex];
        list[firstIndex] = list[secondIndex];
        list[secondIndex] = temp;
    }

    public static void Sortuj<T>(this List<T> lista) where T : IComparable<T>
    {
        int n = lista.Count;
        do
        {
            for (int i = 0; i < n - 1; i++)
            {
                if (lista[i].CompareTo(lista[i + 1]) > 0)
                {
                    lista.SwapElements(i, i+1);
                }
            }
            n--;
        }
        while (n > 1);
    }
}
```

```csharp
// file: Program.cs, class: Program
static void Main(string[] args)
{
    //Krok1();
    //Krok2();
    //Krok3();
    //Krok4();
    Krok5();
}

static void Krok5()
{
    var lista = new List<Pracownik>()
    {
        new Pracownik("CCC", new DateTime(2010, 10, 02), 1050),
        new Pracownik("AAA", new DateTime(2010, 10, 01), 100),
        new Pracownik("DDD", new DateTime(2010, 10, 03), 2000),
        new Pracownik("AAA", new DateTime(2011, 10, 01), 1000),
        new Pracownik("BBB", new DateTime(2010, 10, 01), 1050)
    };
    Console.WriteLine($"Lista pracowników:\n{string.Join('\n', lista)}");

    var listaInt = new List<int> { 2, 5, 1, 2, 1, 7, 4, 5 };
    Console.WriteLine($"Lista liczb: {string.Join(',', listaInt)}");

    // wewnętrzny porządek w zbiorze
    Console.WriteLine("--- Porządkowanie za pomocą własnej metody sortującej" + Environment.NewLine
        + "zgodnie z naturalnym porządkiem zdefiniowanym w klasie Pracownik ---");
    Sortowanie.Sortuj(lista); // wywołanie metody "tradycyjnie"
    Console.WriteLine(string.Join('\n', lista));
    listaInt.Sortuj(); // wywołanie jako metody rozszerzajacej
    Console.WriteLine(string.Join(',', listaInt));
}
```

Output:

```plaintext
Lista pracowników:
(CCC, 2 paź 2010 (109), 1050 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
Lista liczb: 2,5,1,2,1,7,4,5
--- Porządkowanie za pomocą własnej metody sortującej
zgodnie z naturalnym porządkiem zdefiniowanym w klasie Pracownik ---
(AAA, 1 paź 2010 (109), 100 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
(CCC, 2 paź 2010 (109), 1050 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
1,1,2,2,4,5,5,7
```

### Metoda wykorzystująca obiekt typu `IComparer`

* `void Sortuj<T>( this IList<T> lista, IComparer<T> comparer )`

```csharp
public static class Sortowanie
{
    // ... c.d.

    public static void Sortuj<T>(this List<T> lista, IComparer<T> comparer)  
    {
        int n = lista.Count;
        do
        {
            for (int i = 0; i < n - 1; i++)
            {
                if( comparer.Compare(lista[i], lista[i+1]) > 0 )
                {
                    lista.SwapElements(i, i+1);
                }
            }
            n--;
        }
        while (n > 1);
    }
}
```

```csharp
// file: Program.cs, class: Program
static void Krok5()
{
// ...

// zewnętrzny porządek - obiekt IComparer
Console.WriteLine("--- Porządkowanie za pomocą własnej metody sortującej" + Environment.NewLine
    + "zgodnie z porządkiem zdefiniowanym w klasie typu IComparer ---");
Sortowanie.Sortuj(lista, new WgCzasuZatrudnieniaPotemWgWynagrodzeniaComparer()); // wywołanie metody "tradycyjnie"
Console.WriteLine(string.Join('\n', lista));

listaInt.Sortuj(new MyIntComparer()); // wywołanie jako metody rozszerzajacej
Console.WriteLine(string.Join(',', listaInt));
}

// prywatna klasa wawnętrzna definiująca porządek sortowania liczb całkowitych
private class MyIntComparer : IComparer<int>
{
    public int Compare(int x, int y) => (y - x); // malejąco
}
```

Output:

```plaintext
--- Porządkowanie za pomocą własnej metody sortującej
zgodnie z porządkiem zdefiniowanym w klasie typu IComparer ---
(AAA, 1 paź 2011 (97), 1000 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
(CCC, 2 paź 2010 (109), 1050 PLN)
(DDD, 3 paź 2010 (109), 2000 PLN)
7,5,5,4,2,2,1,1
```

### Metoda wykorzystująca delegat typu `Comparison`

* `void Sortuj<T>( this IList<T> lista, Comparison<T> comparison )`

```csharp
public static class Sortowanie
{
    // ... c.d.

    public static void Sortuj<T>(this List<T> lista, Comparison<T> comparison)  
    {
        int n = lista.Count;
        do
        {
            for (int i = 0; i < n - 1; i++)
            {
                if( comparison( lista[i], lista[i+1] ) > 0 )
                {
                    lista.SwapElements(i, i+1);
                }
            }
            n--;
        }
        while (n > 1);
    }
}
```

```csharp
// file: Program.cs, class: Program
static void Krok5()
{
// ...

// zewnętrzny porządek - delegat Comparison
Console.WriteLine("--- Porządkowanie za pomocą własnej metody sortującej" + Environment.NewLine
    + "zgodnie z porządkiem zdefiniowanym przez delegat Comparison ---");
Comparison<Pracownik> porownywacz
    = (p1, p2) => (p1.Wynagrodzenie != p2.Wynagrodzenie) ?
        (-1) * (p1.Wynagrodzenie.CompareTo(p2.Wynagrodzenie)) :
        p1.CzasZatrudnienia.CompareTo(p2.CzasZatrudnienia);
Sortowanie.Sortuj(lista, porownywacz); // wywołanie metody "tradycyjnie"
Console.WriteLine(string.Join('\n', lista));

listaInt.Sortuj( (x,y) => y - x ); // wywołanie jako metody rozszerzajacej
Console.WriteLine(string.Join(',', listaInt));
```

Output:

```plaintext
--- Porządkowanie za pomocą własnej metody sortującej
zgodnie z porządkiem zdefiniowanym przez delegat Comparison ---
(DDD, 3 paź 2010 (109), 2000 PLN)
(BBB, 1 paź 2010 (109), 1050 PLN)
(CCC, 2 paź 2010 (109), 1050 PLN)
(AAA, 1 paź 2011 (97), 1000 PLN)
(AAA, 1 paź 2010 (109), 100 PLN)
7,5,5,4,2,2,1,1
```

