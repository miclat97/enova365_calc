using Rekrutacja.Enums;
using Rekrutacja.Helpers.PoleFiguryHelper;
using Rekrutacja.Helpers.ProstyKalkulatorStaticHelper;
using Rekrutacja.Helpers.StringToIntParserHelper;
using Rekrutacja.Workers.Template;
using Rekrutacja.Workers.Zadanie3;
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
[assembly: Worker(typeof(Zadanie3Worker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Zadanie3
{
    public class Zadanie3Worker
    {
        //Aby parametry działały prawidłowo dziedziczymy po klasie ContextBase
        public class Zadanie3WorkerParametry : ContextBase
        {
            [Caption("Data obliczeń")]
            public Date DataObliczen { get; set; }
            [Caption("A")]
            public string A { get; set; }
            [Caption("B")]
            public string B { get; set; }
            [Caption("Figura")]
            public FiguraEnum Figura { get; set; }
            public Zadanie3WorkerParametry(Context context) : base(context)
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
        public Zadanie3WorkerParametry Parametry { get; set; }
        //Atrybut Action - Wywołuje nam metodę która znajduje się poniżej
        [Action("Parser string na int",
           Description = "Kalkulator pola figur geometrycznych i parser string na int",
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
                        // Parsujemy zmienne A i B z stringa na int, zgodnie z wymaganiami zadania napisałem parser ręcznie
                        int parsedA = StringToIntParserHelper.ParsePositiveNumberFromStringToInt(this.Parametry.A);
                        int parsedB = StringToIntParserHelper.ParsePositiveNumberFromStringToInt(this.Parametry.B);


                        // Dzięki temu, że metody są wydzielone do osobnych klas statycznych, mogę teraz łatwo wykorzystać już zaimplementowaną metodę do poprzedniego zadania
                        int wynik = PoleFiguryHelper.ObliczPoleFigury(parsedA, parsedB, this.Parametry.Figura);

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