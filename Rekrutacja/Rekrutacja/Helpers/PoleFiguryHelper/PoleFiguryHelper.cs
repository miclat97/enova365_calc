using Rekrutacja.Enums;
using Soneta.Produkcja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rekrutacja.Helpers.PoleFiguryHelper
{
    public static class PoleFiguryHelper
    {
        public static int ObliczPoleFigury(int a, int b, FiguraEnum figura)
        {
            double wynikPrzedZaokragleniem = 0; //zmienna pomocnicza do zaokrąglenia wyniku (zgodnie z zadaniem, wynik ma być typu int
                                                // ale wydaje mi się, że szczególnie w takiej aplikacji jak system ERP nie możemy sobie pozwolić na utratę precyzji, więc
                                                // nie wykonuję tutaj zwykłego rzutowania na int, tylko zaokrąglam wynik do najbliższej liczby całkowitej
            switch (figura)
            {
                case FiguraEnum.Kwadrat:
                    return (int)a * a;
                case FiguraEnum.Prostokat:
                    return (int)a * b;
                case FiguraEnum.Trojkat:
                    wynikPrzedZaokragleniem = (double)((a * b) / 2);
                    return (int)Math.Round(wynikPrzedZaokragleniem); // a jest podstawą trójkąta, b to wysokość trójkąta; wzór na pole trójkąta to (podstawa * wysokość) / 2
                case FiguraEnum.Kolo:
                    wynikPrzedZaokragleniem = (double)(Math.PI * Math.Pow(a, 2)); //a jest promieniem koła, więc pole koła to π * r^2
                    return (int)Math.Round(wynikPrzedZaokragleniem); // zaokrąglenie jak przy dzieleniu w metodzie prostego kalkulatora - w systemie ERP wartość po przecinku może być szczególnie istotna (np. do obliczania jakichś podatków), dlatego żeby spełnić wymagania zadania (zwracany typ INT) ale jednocześie chcąc uzyskąć jak najdokadniejszy wynik zaokrąglam go przed rzutowaniem
                default:
                    throw new InvalidOperationException($"Nieprawidłowa figura!");
            }
        }
    }
}
