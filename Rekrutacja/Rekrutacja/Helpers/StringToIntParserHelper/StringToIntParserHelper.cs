using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Helpers.StringToIntParserHelper
{

    /// <summary>
    /// Helper który parsuje string do inta w systemie dziesiętnym
    /// Wykorzystuje fakt, że wartość liczby w systemie dziesiętnym to suma wartości każdej jej cyfr,
    /// pomnożonej przez 10 (dlatego jest to SYSTEM DZIESIĘTNY) do odpowiedniej potęgi, zależnej od pozycji cyfry w liczbie.
    /// Liczba do której potęgujemy 10 to jej tzw. "podstawnik" liczby w systemie dziesiętnym, czyli "odwrócona" wartość numeru pozycji MINUS JEDEN
    /// na którym znajduje się cyfra w liczbie
    /// 
    /// Odwrócona, dlatego że dla przykladu liczba 5564 ma 4 cyfry, tzn. długość stringa to 4
    /// PIerwsza cyfrwa ma wartość 5 i jest na pozycji 0 stringa a idąc tym tokiem rozumowania pomocna będzie dodatkowa zmienna w której będziemy
    /// na biężąco obliczać wartość jej "podstawnika" czyli 10^3 (inaczej mówiąc jest to pozostała ilość znaków w stringu minus 1)
    /// Więc 4-1 = 3
    /// A więc 5 * 10^3
    /// Co daje 5000
    /// 
    /// Kolejna cyfra to 5, która jest na pozycji 1 stringa, więc jej "podstawnik" to 10^2 (długość stringa - pozycja cyfry - 1)
    /// lub jak wcześniej mówiłem odwrócony numer pozycji cyfry w stringu
    /// Czyli 5 * 10^2 = 500
    /// 
    /// Dalej jest 6 i analogicznie: 6 * 10^1 = 60
    /// 
    /// I ostatnia cyfra zawsze będzie miała podstawnik 10^0, czyli zawszse będzie 1
    /// 
    /// TODO: Przetestować wydajność rozwiązania z pomocniczą zmienną odwracającą pozycję cyfry w stringu oraz bez niej
    /// (czyli oblizając podstawnik każdej cyfry jako 10^(długość stringa - 1 - i), gdzie i to aktualna pozycja cyfry w stringu)
    /// Ale wtedy już bez marnowaina paięci i czasu procesora na inkremenrację zmiennej pononiczej w każdej itracji pętli
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
