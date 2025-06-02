using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Helpers.StringToIntParserHelper
{

    /// <summary>
    /// 
    /// Helper który parsuje string do inta w systemie dziesiętnym
    /// Wykorzystuje fakt, że wartość liczby w systemie dziesiętnym to suma wartości każdej jej cyfr,
    /// pomnożonej przez 10 (dlatego jest to SYSTEM DZIESIĘTNY) do odpowiedniej potęgi, zależnej od pozycji cyfry w liczbie.
    /// Liczba, do której potęgujemy 10 to jej tzw. "podstawnik" liczby w systemie dziesiętnym, czyli "odwrócona" wartość numeru
    /// pozycji MINUS JEDEN na którym znajduje się cyfra w liczbie
    /// Przykład dla liczby 5564: 
    /// W zmiennej wynik będziemy na bieżąco obliczać wartość liczby poprzez sumowanie wartości, które są określane przez cyfry na
    /// poszczególnych pozycjach. Początkowo zmienna wynik jest równa 0
    /// Liczba 5564 ma 4 cyfry, tzn. długość stringa to 4
    /// Pierwsza cyfra ma wartość 5 i jest na pozycji 0 stringa a idąc tym tokiem rozumowania pomocna będzie dodatkowa zmienna, w której
    /// będziemy na bieżąco obliczać wartość jej "podstawnika" czyli 10^3 (inaczej mówiąc jest to pozostała ilość znaków w stringu minus 1)
    /// Więc skoro 4-1 = 3
    /// To potęgujemy do tej wartości: 5 * 10^3
    /// wynik += 5000
    /// Kolejna cyfra to 5, która jest na pozycji 1 stringa, więc jej "podstawnik" to 10^2 (długość stringa - pozycja cyfry - 1) lub
    /// jak wcześniej mówiłem odwrócony numer pozycji cyfry w stringu czyli 
    /// wynik += 5*10^3 = 500
    /// I tym schematem obliczamy wszystkie wartości każdej z cyfr w danej liczbie:
    /// wynik += 6 * 10^1 = 60
    /// wynik += 4 * 10^0 = 4
    /// wynik = 5564
    /// 
    /// </summary>
    public class StringToIntParserHelper
    {
        const int ASCIICode_ZERO = 48;

        public static int ParsePositiveNumberFromStringToInt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Błędna wartość na wejściu - pusty string bądź NULL!");
            }


            int resultInt = 0;
            int factorIteratorHelper = value.Length - 1;

            for (int i = 0; i < value.Length; i++)
            {
                //Sprawdzamy czy znak jest cyfrą (dla tego przypadku nie sprawdzamy znaku minus, ponieważ metoda ma parsować tylko liczby dodatnie)
                if (value[i] < '0' || value[i] > '9')
                {
                    throw new Exception($"Błędna wartość na wejściu - niepoprawny znak ASCII - nie jest to cyfra! Kod ASCII: {value[i]}");
                }


                //Obliczamy wartość cyfry
                int digitValue = value[i] - ASCIICode_ZERO;

                //Obliczamy wartość całkowitą
                resultInt += digitValue * (int)Math.Pow(10, factorIteratorHelper);

                factorIteratorHelper--;
            }

            //Zwracamy wynik
            return resultInt;
        }
    }
}
