using Rekrutacja.Enums;
using Rekrutacja.Helpers.PoleFiguryHelper;
using Rekrutacja.Helpers.ProstyKalkulatorStaticHelper;
using Rekrutacja.Workers.Template;
using Rekrutacja.Workers.Zadanie2;
using Soneta.Business;
using Soneta.Kadry;
using Soneta.KadryPlace;
using Soneta.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// Należy utworzyć kalkulator który posiada cztery operacje (+,-,*,/), wynik zostanie zapisany
/// na WSZYSTKICH ZAZNACZONYCH OBIEKTACH PRACOWNIK w polu Cecha "Wynik" oraz po wykonaniu działania
/// musi zapisać się data wykonania zadania w polu Cecha "DataObliczen". Aby tego dokonać należy zrobić Worker (przycisk)
/// który będzie mieć parametry przyjmujące 4 zmienne: 
/// Zmienna A, Zmienna B, Operacja (znak: +, -, *, /), Data obliczeń.


//Rejetracja Workera - Pierwszy TypeOf określa jakiego typu ma być wyświetlany Worker, Drugi parametr wskazuje na jakim Typie obiektów będzie wyświetlany Worker
[assembly: Worker(typeof(Zadanie2Worker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Zadanie2
{
    public class Zadanie2Worker
    {
        //Aby parametry działały prawidłowo dziedziczymy po klasie ContextBase
        public class Zadanie2WorkerParametry : ContextBase
        {
            [Caption("Data obliczeń")]
            public Date DataObliczen { get; set; }
            [Caption("A")]
            public int A { get; set; }
            [Caption("B")]
            public int B { get; set; }
            [Caption("Figura")]
            public FiguraEnum Figura { get; set; }
            public Zadanie2WorkerParametry(Context context) : base(context)
            {
                this.DataObliczen = Date.Today;
            }
        }
        //Obiekt Context jest to pudełko które przechowuje Typy danych, aktualnie załadowane w aplikacji
        //Atrybut Context pobiera z "Contextu" obiekty które aktualnie widzimy na ekranie
        [Context]
        public Context Cx { get; set; }
        //Pobieramy z Contextu parametry, jeżeli nie ma w Context Parametrów mechanizm sam utworzy nowy obiekt oraz wyświetli jego formatkę
        [Context]
        public Zadanie2WorkerParametry Parametry { get; set; }
        //Atrybut Action - Wywołuje nam metodę która znajduje się poniżej
        [Action("Pole figury",
           Description = "Kalkulator obliczający pola figur geometrycznych",
           Priority = 10,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public void WykonajAkcje()
        {
            //Włączenie Debug, aby działał należy wygenerować DLL w trybie DEBUG
            DebuggerSession.MarkLineAsBreakPoint();

            Pracownik[] pracownik = null;

            if (this.Cx.Contains(typeof(Pracownik[])))
            {
                pracownik = (Pracownik[])this.Cx[typeof(Pracownik[])];
            }

            //Modyfikacja danych
            //Aby modyfikować dane musimy mieć otwartą sesję, któa nie jest read only
            using (Session nowaSesja = this.Cx.Login.CreateSession(false, false, "ModyfikacjaPracownika"))
            {
                try // Zrobię to w bloku try catch, gdyby PoleFiguryHelper.ObliczPoleFigury wyrzuciła wyjątek
                {
                    //Otwieramy Transaction aby można było edytować obiekt z sesji
                    using (ITransaction trans = nowaSesja.Logout(true))
                    {
                        // Zmienna wynik będzie przechowywać wynik, a ponieważ wszyscy zaznaczeni pracownicy będą mieli aktualizowane pole na tą samą wartość
                        // to lepiej obliczenia wykonać raz, a nie powtarzać te same obliczenia na tych samych danych dla każdego pracownika w pętli niżej
                        int wynik = PoleFiguryHelper.ObliczPoleFigury(this.Parametry.A, this.Parametry.B, this.Parametry.Figura);

                        //Pętla foreach, ponieważ może być zaznaczony więcej niż jeden pracownik - a co za tym idzie refleksja zwróci nam kolekcję obiektów typu Soneta.Kadry.Pracownik
                        foreach (var p in pracownik)
                        {
                            //Pobieramy obiekt z sesji, aby móc go modyfikować
                            var pracownikZSesja = nowaSesja.Get(p);

                            //Features - są to pola rozszerzające obiekty w bazie danych, dzięki czemu nie jestesmy ogarniczeni to kolumn jakie zostały utworzone przez 

                            pracownikZSesja.Features["DataObliczen"] = this.Parametry.DataObliczen;

                            // Zastanawiałem się czy blokiem try catch nie objąć tylko tej linii, ale jako że cały Worker robi tak na prawdę jedną operację matematyczną
                            // to bardziej obawiam się problemów z przechwyceniem wyjątku w trakcie działania UI i otwartej transakcji, niż po prostu
                            // jej nie zatwierdzając (commitując)

                            pracownikZSesja.Features["Wynik"] = (double)wynik;

                        }

                        //Zatwierdzamy zmiany wykonane w sesji
                        trans.CommitUI();
                    }

                    //Zapisujemy zmiany
                    nowaSesja.Save();
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message}");
                    //TODO: zapis do loga błędu, zgodnie z dobrymi praktykami nie wyświetlamy exceptiona bezpośrednio użytkownikowi
                }

            }
        }
    }
}