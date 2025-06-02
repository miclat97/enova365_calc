using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Helpers.ProstyKalkulatorStaticHelper
{
    /// <summary>
    /// Statyczna klasa helper do prostego kalkulatora.
    /// Ze względu na to, że jest to kalkulator prosty, wykonujący tylko 4 uniwersalne operacje (mam na myśli że w przypadku takich wymagań 
    /// nie widzę tu potrzeby tworzenia instancji klasy - gdyż przy tych najprostszych operacjach tak czy tak nie będziemy w stanie 
    /// szczególnie tych danych zapisać/analizować
    /// 
    /// W przypadku bardziej złożonych operacji, gdzie będziemy chcieli np. obliczać podatek od wynagrodzenia lub inne złożone obliczenia/operacje
    /// jak najbardziej będzie miało sens tworzenie instancji tych klas, aby móc przykładowo dla obliczania podatku od wynagrodzenia
    /// będziemy mogli zastosować inny wzór dla instancji klasy, która będzie obliczała podatki dla pracowników z Umową o Pracę,
    /// inne dla pracowników z Umową Zlecenie, a jeszcze inne dla Umów o Dzieło
    /// </summary>
    public static class ProstyKalkulatorStaticHelper
    {
        public static int WykonajOperacje(int a, int b, char operacja)
        {
            switch (operacja)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    if (b == 0)
                        throw new DivideByZeroException("Nie można dzielić przez zero.");
                    return (int)Math.Round((double)(a / b)); // Z powodu identycznego jak w przypadku obliczania pola figury, zaokrąglamy wynik dzielenia
                                                             // do najbliższej liczby całkowitej, ponieważ zakładam że w prograie ERP precyzja
                                                             // jest kluczowa i nie możemy sobie pozwolić na ucięcie wartości po przecinku
                default:
                    throw new InvalidOperationException($"Nieprawidłowa operacja: {operacja} Dozwolone są: + - * /");
            }
        }
    }
}
