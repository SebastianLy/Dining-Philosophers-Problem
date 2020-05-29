using System;
using System.Threading;

namespace Filozofowie
{
    class UcztujacyFilozofowie
    {
        public int Id { get; set; }
        public int Czas { get; set; }
        public Semaphore[] Widelce { get; set; }
        public int LiczbaFilozofow { get; set; }

        public UcztujacyFilozofowie(int id, int czas, Semaphore[] widelce, int liczbaFilozofow)
        {
            Id = id;
            Czas = czas;
            Widelce = widelce;
            LiczbaFilozofow = liczbaFilozofow;
        }
    }


    class Program
    {
        public bool CzyDziala(Thread[] watki)
        {
            bool czyDziala = false;
            for (int i = 0; i < watki.Length; i++)
            {
                if (watki[i].IsAlive)
                {
                    czyDziala = true;
                }
            }
            return czyDziala;
        }

        private void Start(Object args)
        {
            UcztujacyFilozofowie filozof = (UcztujacyFilozofowie)args;
            int prawy = filozof.Id;
            int lewy = (filozof.LiczbaFilozofow + filozof.Id + 1) % filozof.LiczbaFilozofow;
            while (true)
            {
                Myslenie(filozof);
                if (prawy == filozof.LiczbaFilozofow - 1)
                {
                    WezWidelec(lewy, filozof);
                    WezWidelec(prawy, filozof);                 
                }
                else
                {
                    WezWidelec(prawy, filozof);
                    WezWidelec(lewy, filozof);
                }
                Jedzenie(filozof);
                OdlozWidelec(lewy, filozof);
                OdlozWidelec(prawy, filozof);
            }
        }

        private void WezWidelec(int i, UcztujacyFilozofowie filozof)
        {
            filozof.Widelce[i].WaitOne();
            Console.WriteLine("Filozof " + filozof.Id + " bierze widelec " + i + ".");
        }

        private void OdlozWidelec(int i, UcztujacyFilozofowie filozof)
        {
            filozof.Widelce[i].Release();
            Console.WriteLine("Filozof " + filozof.Id + " odkłada widelec " + i + ".");
        }

        private void Myslenie(UcztujacyFilozofowie filozof)
        {
            int czas = new Random().Next(filozof.Czas);
            Console.WriteLine("Filozof " + filozof.Id + " zaczął myśleć.");
            Thread.Sleep(czas);
            Console.WriteLine("Filozof " + filozof.Id + " skończył myśleć.");
        }

        private void Jedzenie(UcztujacyFilozofowie filozof)
        {
            int czas = new Random().Next(filozof.Czas);
            Console.WriteLine("Filozof " + filozof.Id + " zaczął jeść.");
            Thread.Sleep(czas);
            Console.WriteLine("Filozof " + filozof.Id + " skończył jeść.");
        }

        static void Main(string[] args)
        {
            int liczbaFilozofow = 10;
            Thread[] watki = new Thread[liczbaFilozofow];
            Semaphore[] semafory = new Semaphore[liczbaFilozofow];
            for (int i = 0; i < liczbaFilozofow; i++)
            {
                semafory[i] = new Semaphore(1, 1);
            }

            Program Filozof = new Program();
            for (int i = 0; i < liczbaFilozofow; i++)
            {
                watki[i] = new Thread(Filozof.Start);
                watki[i].Start(new UcztujacyFilozofowie(i, new Random().Next(500), semafory, liczbaFilozofow));
            }
        }
    }
}
