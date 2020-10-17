# Implementacja interfejsów `IEquatable`, `IComparable`, `IComparer`

* Autor: _Krzysztof Molenda_
* Wersja: 2019-10-03

Celem ćwiczenia jest nabycie umiejętności w zakresie tworzenia "well formed class" i implementowania podstawowych interfejsów umożliwiających porównywanie obiektów i stosowanie bibliotecznych algorytmów (sortowanie, wyszukiwanie).

Po wykonaniu ćwiczenia powinieneś:

* umieć implementować równość obiektów (w sensie "taki sam") - implementacja `IEquatable<T>`, przesłonięcie `Equals`, `GetHashCode`,

* umieć określać naturalny porządek dla projektowanego typu - implementacja `IComparable` i wykorzystać go w procesie sortowania i wyszukiwania,

* umieć określać porządek w zbiorze elementów danego typu poprzez wykorzystanie interfejsu `IComparer` oraz delegata `Comparison` i wykorzystać go w procesie sortowania i wyszukiwania,

* umieć zaprogramować prostą metodę generyczną (na przykładzie metody sortującej),

* umieć wykorzystać w praktyce implementację algorytmu wyszukiwania binarnego (metoda `BinarySearch` w klasie `Array` oraz `List<T>`),

* poznać środowisko programowania VisualCode w aspekcie programowania w C#.

> Ćwiczenie wykonaj wykorzystując lekkie środowisko VS Code z dodatkiem C#. ( [Get started with C# and Visual Studio Code](https://docs.microsoft.com/pl-pl/dotnet/core/tutorials/with-visual-studio-code) )

---

Wykonaj kolejno podane poniżej kroki.

## Krok 1 - realizacja klasy, implementacja pojęcia "taki sam"

1. Utwórz klasę `Pracownik` opisującą obiekt, zawierający:
   * `Nazwisko` (typu `string`),
   * `DataZatrudnienia` (typu `DateTime`),
   * `Wynagrodzenie` (typu `decimal`).

2. Dostęp do danych zrealizuj jako publiczne _properties_ (read-write).
   * dla `Nazwisko` zaimplementuj obcinanie spacji przed i po
   * dla `DataZatrudnienia` zaimplementuj: data zatrudnienia nie później niż dzisiaj, w przeciwnym przypadku `throw new ArgumentException()`
   * dla `Wynagrodzenie` zaimplementuj: przy próbie podstawienia wartości ujemnej, przypisz `0`.

3. Przesłoń metodę `ToString()` zwracającą tekstową reprezentację obiektu w formie:

    ```(Nazwisko, DataZatrudnienia, Wynagrodzenie)```

4. Zaimplementuj konstruktor (trójargumentowy). Zaimplementuj konstruktor domyślny (bezargumentowy), przyjmując nazwisko `"Anonim"`, datę przyjęcia dzisiaj oraz wynagrodzenie `0` PLN.

5. Zaimplementuj publiczne _property_ `CzasZatrudnienia` zwracające aktualny czas zatrudnienia pracownika w pełnych miesiącach (miesiąc = 30 dni).

6. Zmodyfikuj metodę `ToString()` tak, aby po dacie zatrudnienia wypisywała czas zatrudnienia w miesiącach podany w nawiasie.

    ```(Nazwisko, DataZatrudnienia (CzasZatrudnienia), Wynagrodzenie)```

7. Aby określić pojęcie "taki sam" zaimplementuj w klasie `Pracownik` interfejs `IEquatable<Pracownik>`. Dwa obiekty typu `Pracownik` są takie same, jeśli mają takie same nazwiska, daty zatrudnienia i wynagrodzenia.
   Powinieneś również przesłonić metodę `Equals()` oraz `GetHashCode()` w taki sposób, aby wyniki wszystkich były spójne. Przeczytaj:
   * <https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1>
   * <https://blogs.msdn.microsoft.com/jaredpar/2009/01/15/if-you-implement-iequatablet-you-still-must-override-objects-equals-and-gethashcode/>

8. Zaimplementuj statyczną metodę `Equals(Pracownik p1, Pracownik p2)` przesłaniając tę, zdefiniowaną w klasie `Object`.

9. Zaimplementuj przeciążenie operatora `==` (równocześnie musisz przeciążyć `!=`).

10. Przetestuj w `Main()` poprawność powyższych implementacji.

[Kod po kroku 1](etapy/krok1.md)

---

## Krok 2 - implementacja naturalnego porządku w klasie

1. Utwórz listę 5 pracowników:

    * dwóch mających te same nazwiska,
    * dwóch zatrudnionych w tym samym roku i miesiącu,
    * dwóch mających to samo wynagrodzenie.

2. Wypisz tę listę w porządku oryginalnym.

3. Posortuj listę (metoda `Sort<T>` zdefiniowana w klasie `List<T>`). Dlaczego nie można tego zrobić?

4. Zadeklaruj implementację przez klasę `Pracownik` interfejsu `IComparable<Pracownik>` i zaimplementuj go. Ustal naturalny porządek w zbiorze pracowników: najpierw według nazwiska, potem według daty zatrudnienia i na końcu według wynagrodzenia - wszystko rosnąco.

5. Posortuj listę według naturalnego porządku zdefiniowanego w klasie `Pracownik`.

[Kod po kroku 2](etapy/krok2.md)

---

## Krok 3 - wykorzystanie definicji zewnętrznego porządku sortowania

Dla listy utworzonej w poprzednim kroku wykonaj:

1. Posortuj listę najpierw według czasu zatrudnienia (w miesiącach), a później według wynagrodzenia - wszystko rosnąco:
    * utwórz klasę o nazwie `WgCzasuZatrudnieniaPotemWgWynagrodzeniaComparer` implementującą interfejs `IComparer<Pracownik>`,
    * posortuj listę (przeciążona metoda `Sort<T>` zdefiniowana w klasie `List<T>` wymagająca dostarczenia obiektu typu `IComparer`),
    * wypisz listę i sprawdź poprawność sortowania.

2. Posortuj listę najpierw według czasu zatrudnienia (w miesiącach), a później kolejno według nazwiska i wynagrodzenia:
    * posortuj listę (przeciążona metoda `Sort<T>` zdefiniowana w klasie `List<T>` wymagająca dostarczenia delegata typu `Comparison`),
    * wypisz listę i sprawdź poprawność sortowania.

3. Posortuj listę (metoda z `Comparison`) kolejno: malejąco według wynagrodzenia, później rosnąco według czasu zatrudnienia.

[Kod po kroku 3](etapy/krok3.md)

---

## Krok 4 - porządkowanie z wykorzystaniem LINQ

Lista pracowników jest typu `List<Pracownik>`, ale również obiektem typu `IEnumerable`. Wykorzystaj metodę `OrderBy` oraz `ThenBy` z klasy `Enumerable` do uporządkowania listy według kolejno: wynagrodzenia, nazwiska. Eksperymentuj.

> Musisz na początku kodu programu użyć dyrektywy `using System.Linq`.

[Kod po kroku 4](etapy/krok4.md)

---

## Krok 5 - własna generyczna metoda sortująca

> Realizacja tego kroku pozwoli Ci lepiej zrozumieć mechanizmy dostarczania metod generycznych (uniwersalnych) - wykonasz to samo, co programiści bibliotek standardowych C#.

W statycznej klasie `Sortowanie` napisz statyczną procedurę generyczną `Sortuj<T>`, sortującą zadaną jako parametr listę obiektów typu `T` - w trzech wariantach przeciążonych:

* `void Sortuj<T>( this IList<T> lista )` - wykorzystująca wewnętrzny porządek w zbiorze

* `void Sortuj<T>( this IList<T> lista, IComparer<T> comparer )` - wykorzystująca zewnętrzny porządek dostarczony w formie obiektu typu `IComparer`

* `void Sortuj<T>( this IList<T> lista, Comparison<T> comparison )` - wykorzystująca zewnętrzny porządek w zbiorze dostarczony w formie delegata.

Wykorzystaj pseudokod dowolnego algorytmu sortującego (np. _BubleSort_, _InsertionSort_, _SelectionSort_, _ShellSort_, _QuickSort_, _MergeSort_) - skorzystaj z Wikipedii lub WikiBooks.

Każdy z algorytmów sortowania (uniwersalny) wymagać będzie porównywania oraz przestawiania elementów. Zatem najpierw napisz generyczną metodę `void SwapElements<T>(this IList<T> list, int i, int j)` zamieniającą elementy listy miejscami (`list[i] ⇆ list[j]`).

> Użycie interfejsu `IList<T>` pozwala na rozszerzenie funkcjonalności Twojego sortowania na dowolne listy (zarówno `List<T>` jak i np. `LinkedList<T>` i inne, implementujące ten interfejs).
> Użycie słowa kluczowego `this` przy pierwszym argumencie (typu `IList`) powoduje, że tworzysz [metodę rozszerzającą](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods).

Sprawdź działanie swoich algorytmów na liście obiektów oraz na liście elementów typu strukturalnego (np. `int`).

[Kod po kroku 5](etapy/krok5.md)

---

## Krok 6 - wyszukiwanie w zbiorze elementów

W klasie `Array` oraz w klasie `List<T>` zdefiniowana jest metoda `BinarySearch` efektywnie wyszukująca zadany element.

Zapoznaj się z dokumentacją tej metody. Jakie warunki musi spełniać zbiór, w którym może ona wyszukiwać wystąpienie elementu. Jaka jest jej reakcja w sytuacji, gdy elementu nie ma zbiorze?

Wykonaj stosowne eksperymenty.
