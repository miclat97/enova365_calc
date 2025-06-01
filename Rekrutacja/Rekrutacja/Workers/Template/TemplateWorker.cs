using Soneta.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soneta.Kadry;
using Soneta.KadryPlace;
using Soneta.Types;
using Rekrutacja.Workers.Template;
using Rekrutacja.Helpers.ProstyKalkulatorStaticHelper;

/// Należy utworzyć kalkulator który posiada cztery operacje (+,-,*,/), wynik zostanie zapisany
/// na WSZYSTKICH ZAZNACZONYCH OBIEKTACH PRACOWNIK w polu Cecha "Wynik" oraz po wykonaniu działania
/// musi zapisać się data wykonania zadania w polu Cecha "DataObliczen". Aby tego dokonać należy zrobić Worker (przycisk)
/// który będzie mieć parametry przyjmujące 4 zmienne: 
/// Zmienna A, Zmienna B, Operacja (znak: +, -, *, /), Data obliczeń.


//Rejetracja Workera - Pierwszy TypeOf określa jakiego typu ma być wyświetlany Worker, Drugi parametr wskazuje na jakim Typie obiektów będzie wyświetlany Worker
[assembly: Worker(typeof(TemplateWorker), typeof(Pracownicy))]
namespace Rekrutacja.Workers.Template
{
    public class TemplateWorker
    {
        //Aby parametry działały prawidłowo dziedziczymy po klasie ContextBase
        public class TemplateWorkerParametry : ContextBase
        {
            [Caption("Data obliczeń")]
            public Date DataObliczen { get; set; }
            [Caption("A")]
            public int A { get; set; }
            [Caption("B")]
            public int B { get; set; }
            [Caption("Operacja")]
            public char Operacja { get; set; }
            public TemplateWorkerParametry(Context context) : base(context)
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
        public TemplateWorkerParametry Parametry { get; set; }
        //Atrybut Action - Wywołuje nam metodę która znajduje się poniżej
        [Action("Kalkulator",
           Description = "Prosty kalkulator ",
           Priority = 10,
           Mode = ActionMode.ReadOnlySession,
           Icon = ActionIcon.Accept,
           Target = ActionTarget.ToolbarWithText)]
        public void WykonajAkcje()
        {
            //Włączenie Debug, aby działał należy wygenerować DLL w trybie DEBUG
            DebuggerSession.MarkLineAsBreakPoint();

            Pracownik pracownik = null;

            if (this.Cx.Contains(typeof(Pracownik)))
            {
                pracownik = (Pracownik)this.Cx[typeof(Pracownik)];
            }

            //Modyfikacja danych
            //Aby modyfikować dane musimy mieć otwartą sesję, któa nie jest read only
            using (Session nowaSesja = this.Cx.Login.CreateSession(false, false, "ModyfikacjaPracownika"))
            {
                try // Zrobię to w bloku try catch, gdyż sama metoda ProstyKalkulatorStaticHelper.WykonajOperacje może rzucić wyjątek, np. w przypadku
                    // dzielenia przez zero, a chciałbym uniknąć problemów z UI aplikacji Evova 365 lub problemów z samą bazą danych
                    // przy tak na prawdę pojedynczej prostej operacji
                    // Oczywiście gdyby było tutaj więcej danych do przetworzenia (a co za tym idzie również do potencjalnego stracenia w przypaku
                    // wyjątku) to należałoby dodać tutaj więcej logiki, podzielić to na mniejsze operacje - jednak w tym konkretnym Workerze
                    // nie widzę takiej dużej konieczności, jednak można to rozbudować
                {
                    //Otwieramy Transaction aby można było edytować obiekt z sesji
                    using (ITransaction trans = nowaSesja.Logout(true))
                    {
                        //Pobieramy obiekt z sesji, aby móc go modyfikować
                        var pracownikZSesja = nowaSesja.Get(pracownik);


                        //Features - są to pola rozszerzające obiekty w bazie danych, dzięki czemu nie jestesmy ogarniczeni to kolumn jakie zostały utworzone przez producenta

                        pracownikZSesja.Features["DataObliczen"] = this.Parametry.DataObliczen;

                        // Zastanawiałem się czy blokiem try catch nie objąć tylko tej linii, ale jako że cały Worker robi tak na prawdę jedną operację matematyczną
                        // to bardziej obawiam się problemów z przechwyceniem wyjątku w trakcie działania UI i otwartej transakcji, niż po prostu
                        // jej nie zatwierdzając (commitując)

                        pracownikZSesja.Features["Wynik"] = ProstyKalkulatorStaticHelper.WykonajOperacje(this.Parametry.A, this.Parametry.B, this.Parametry.Operacja);

                        //Zatwierdzamy zmiany wykonane w sesji
                        trans.CommitUI();
                    }

                    //Zapisujemy zmiany
                    nowaSesja.Save();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Wystąpił błąd");
                    //TODO: zapis do loga błędu, zgodnie z dobrymi praktykami nie wyświetlamy exceptiona bezpośrednio użytkownikowi
                }

            }
        }
    }
}