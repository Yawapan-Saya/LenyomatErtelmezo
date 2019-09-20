using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LenyomatÉrtelmező
{
    public class Alaprajz
    {
        private class Koordináta
        {
            public int x, y;
            public Koordináta(int X, int Y)
            {
                x = X;
                y = Y;
            }
            public bool Equals(Koordináta k2)
            {
                return x == k2.x && y == k2.y;
            }
        }
        private enum Típus { Ismeretlen, Üres, Fal, Szék, Asztal, Kanapé }
        public readonly int Szélesség, Magasság;
        public int SzobákSzáma { get; private set; }
        private Típus[,] épület;

        #region KONSTRUKTOR
        public Alaprajz(bool[,] lenyomat)
        {
            Szélesség = lenyomat.GetLength(0);
            Magasság = lenyomat.GetLength(1);

            LenyomatTípussá(lenyomat);
            FalDekektálás();
            BerendezésDekektálás();
            SzobaMegszámlálás();
        }
        #endregion

        #region print
        public string Print()
        {
            string eredmeny = "";
            for (int y = 0; y < Magasság; y++)
            {
                for (int x = 0; x < Szélesség; x++)
                    eredmeny += PrintPozíció(x, y);
                eredmeny += '\n';
            }
            return eredmeny;
        }

        private char PrintPozíció(int x, int y, char asztal = 'A', char fal = 'F', char kanapé = 'K', char szék = 'S', char üres = '.')
        {
            Típus t = this[x, y];

            switch (t)
            {
                case Típus.Asztal:
                    return asztal;
                case Típus.Fal:
                    return fal;
                case Típus.Kanapé:
                    return kanapé;
                case Típus.Szék:
                    return szék;
                case Típus.Üres:
                    return üres;
                case Típus.Ismeretlen:
                    throw new Exception("Nem osztályzott mező");
                default:
                    throw new Exception("Nem kezelt object típus!");
            }
        }
        #endregion

        #region this[] operátor
        private Típus this[int x, int y]
        {
            get
            {
                return épület[x, y];
            }
            set
            {
                épület[x, y] = value;
            }
        }
        private Típus this[Koordináta k]
        {
            get
            {
                return this[k.x, k.y];
            }
            set
            {
                this[k.x, k.y] = value;
            }
        }
        #endregion

        #region OBJEKT DETEKTÁLÁS
        private void LenyomatTípussá(bool[,] lenyomat)
        {
            /// <summary>
            /// Feltölti az épületet Típus.Üres és Típus.Ismeretlen értékekkel a lenyomat szerint
            /// </summary>
            épület = new Típus[Szélesség, Magasság];
            for (int y = 0; y < Magasság; y++)
                for (int x = 0; x < Szélesség; x++)
                    épület[x, y] = lenyomat[x, y] ? Típus.Ismeretlen : Típus.Üres;
        }
        private void FalDekektálás()
        {
            /// <summary>
            /// Felcímkézi az összes falat
            /// Majd elmenti azt
            /// </summary>
            var start = KeressEgyFalat();
            var összesFal = CsatlakozókEhhez(start);

            foreach (var fal in összesFal)
            {
                this[fal] = Típus.Fal; // Na és ez miért ad olvasás eseményt???
            }
        }

        private void BerendezésDekektálás()
        {
            /// <summary>
            /// Felcímkézi a Szék, Asztal és kanapé típusú objecteket
            /// </summary>

            // A ciklus működési elve:
            // mindig a bal felső sarkát találja meg az objectnek
            // Ha az object feltöltése már elkezdődött, akkor folytatja
            // Egyébként felcímkézi az alábbi szabályok alapján:

            // Szék, ha se jobbra, se lefelé nem folytatódik
            // Asztal ja jobbra, lefelé és jobbra-lefelé is folytatódik
            // Egyébként pedig kanapé
            for (int y = 1; y < Magasság - 1; y++)
            {
                for (int x = 1; x < Szélesség - 1; x++)
                {
                    if (this[x, y] == Típus.Ismeretlen)
                    {
                        // Ha az object detektálása már elkezdődött
                        Típus balraTípus = this[x - 1, y];
                        Típus felTípus = this[x, y - 1];

                        if (balraTípus == Típus.Asztal || balraTípus == Típus.Kanapé || balraTípus == Típus.Szék)
                        {
                            this[x, y] = balraTípus;
                            continue;
                        }
                        if (felTípus == Típus.Asztal || felTípus == Típus.Kanapé || felTípus == Típus.Szék)
                        {
                            this[x, y] = felTípus;
                            continue;
                        }

                        // Ha az object még ismeretlen
                        bool jobbraIsmert = this[x + 1, y] == Típus.Ismeretlen;
                        bool lefeléIsmert = this[x, y + 1] == Típus.Ismeretlen;
                        bool jobbraÉsLefeléIsmert = this[x + 1, y + 1] == Típus.Ismeretlen;

                        if (!jobbraIsmert && !lefeléIsmert)
                        {
                            this[x, y] = Típus.Szék;
                        }

                        else if (jobbraIsmert && lefeléIsmert && jobbraÉsLefeléIsmert)
                        {
                            this[x, y] = Típus.Asztal;
                        }
                        else
                        {
                            this[x, y] = Típus.Kanapé;
                        }
                    }
                }
            }
        }
        #endregion

        #region SZOBA DETEKTÁLÁS
        private void SzobaMegszámlálás()
        {

            int eredmény = 0; // Szobák száma

            /// <summary>
            /// Megkeresi a szobákat
            /// Majd megszámolja, hogy hány darab szoba van
            /// </summary>

            // Minden szobába nem tartozó mező összegyűjtése
            List<Koordináta> nemIsmertÜresek = new List<Koordináta>();
            for (int x = 0; x < Szélesség; x++)
            {
                for (int y = 0; y < Magasság; y++)
                {
                    Koordináta k = new Koordináta(x, y);
                    if (this[k] == Típus.Üres)
                    {
                        nemIsmertÜresek.Add(k);
                    }
                }
            }

            // szoba = fallal körülzárt terület
            while (nemIsmertÜresek.Count != 0)
            {
                // nemIsmertÜresek -= szoba

                Koordináta start = nemIsmertÜresek[0];

                // Annak megállapítása, hogy körbe van-e zárva
                bool épületenBelül = true;
                {
                    int elsőFalIndex = 999999999;
                    int utolsóFalIndex = 0;
                    for (int x = 0; x < Szélesség; x++)
                    {
                        if (this[x, start.y] == Típus.Fal)
                        {
                            elsőFalIndex = Math.Min(elsőFalIndex, x);
                            utolsóFalIndex = x;
                        }
                    }

                    if (elsőFalIndex > start.x || utolsóFalIndex < start.x)
                    {
                        épületenBelül = false;
                    }
                }
                {
                    int elsőFalIndex = 999999999;
                    int utolsóFalIndex = 0;
                    for (int y = 0; y < Magasság; y++)
                    {
                        if (this[start.x, y] == Típus.Fal)
                        {
                            elsőFalIndex = Math.Min(elsőFalIndex, y);
                            utolsóFalIndex = y;
                        }
                    }

                    if (elsőFalIndex > start.y || utolsóFalIndex < start.y)
                    {
                        épületenBelül = false;
                    }
                }

                // Csatlakozó mezők megkeresése
                List<Koordináta> üresRész = CsatlakozókEhhez(start);

                if (épületenBelül)
                {
                    eredmény += 1;
                }

                // nemismert üresek -= szoba
                foreach (var k in üresRész)
                {
                    nemIsmertÜresek = nemIsmertÜresek.Where(k1 => !k1.Equals(k)).ToList();
                }


            }

            SzobákSzáma = eredmény;
        }
            #endregion

            private Koordináta KeressEgyFalat()
        {
            /// <returns>
            /// Egy fal koordinátáját
            /// </returns>
            for (int x = 0; x < Szélesség; x++)
            {
                for (int y = 0; y < Magasság; y++)
                {
                    switch (this[x, y])
                    {
                        case Típus.Fal:
                        case Típus.Ismeretlen:
                            return new Koordináta(x, y);
                    }
                }
            }
            throw new Exception("Nem található fal");
        }

        private List<Koordináta> CsatlakozókEhhez(Koordináta start)
        {
            /// <summary>
            /// Visszaad egy listát a start ponthoz csatlakozó mezőhöz
            /// Csatlakozó mező = el lehet jutni a start pontból hozzá csak azonos típusú mezőkön
            /// </summary>

            /// <returns>
            /// Minden olyan mező ahová el lehet jutni
            /// </returns>

            /// <param name="start">
            /// Ehhez keres csatlakozó mezőket
            /// </param>

            var üres = this[start] == Típus.Üres;

            // Dijsktra féle gráf útvonal keresés
            var eredmeny = new List<Koordináta>();

            // 0. lépés:) Kezdő pont és kezdő útvonalak felvétele
            eredmeny.Add(start);

            var utvonalak = üres ? ÜresSzomszédok(start) : NemÜresSzomszédok(start);

            // 1. lépés) Az összes többi lépés szimulálása
            while (utvonalak.Count > 0)
            {
                var kovetkezoUtvonalak = new List<Koordináta>(); // Következő lépés adatai

                foreach (var célPont in utvonalak)
                {
                    // Egy út szimulálása
                    // var kezdőPont = út.KezdőPont; // Nem tudom hogy ezt miért szerettem volna

                    if (eredmeny.Any(e => e.Equals(célPont)))
                    {
                        // Ha az adott ponthoz már korábban eljutott az útvonalkeresés
                        // Akkor most hagyja ki
                        // Így elkerüli a végtelen ciklust (kör a gráfban)
                        continue;
                    }

                    // Egyébként vegye fel az eredmények közé
                    eredmeny.Add(célPont);

                    // És mentse el a hozzá tartozó utakat
                    var szomszedok = üres ? ÜresSzomszédok(célPont) : NemÜresSzomszédok(célPont);

                    szomszedok
                        .ForEach(x => kovetkezoUtvonalak.Add(x));
                }

                utvonalak = kovetkezoUtvonalak;

            }

            return eredmeny;
        }

        #region SZOMSZÉD KERESŐ
        /// <summary>
        /// A gráf bejáráshoz szükséges
        /// Megadja, hogy egy lépésben hová lehet eljutni
        /// </summary>

        private List<Koordináta> ÜresSzomszédok(Koordináta start)
        {
            return Szomszédok(start, t => t == Típus.Üres);
        }
        private List<Koordináta> NemÜresSzomszédok(Koordináta start)
        {
            return Szomszédok(start, t => t != Típus.Üres);
        }
        private List<Koordináta> Szomszédok(Koordináta start, Func<Típus, bool> szomszédTípusSzűrő)
        {
            /// <summary>
            /// Minden szomszéd, ami a szomszédTípusSzűrő-nek megfelel
            /// </summary>

            /// <param name="szomszédTípusSzűrő">
            /// Egy olyan függvény, amely a szomszéd típusa alapján szűr
            /// </param>
            /// 

            /// UNSAFE ÖTLET: Minden lehetséges szomszéd visszaadása és külső szűrés
            /// CONS: Könnyen lemaradhat a szűrés

            var eredmeny = new List<Koordináta>();

            bool mehetFel = start.y - 1 >= 0;
            bool mehetLe = start.y + 1 < Magasság;
            bool mehetBalra = start.x - 1 >= 0;
            bool mehetJobbra = start.x + 1 < Szélesség;

            if (mehetBalra && szomszédTípusSzűrő(this[start.x - 1, start.y]))
            {
                eredmeny.Add(new Koordináta(start.x - 1, start.y));
            }
            if (mehetJobbra && szomszédTípusSzűrő(this[start.x + 1, start.y]))
            {
                eredmeny.Add(new Koordináta(start.x + 1, start.y));
            }
            if (mehetFel && szomszédTípusSzűrő(this[start.x, start.y - 1]))
            {
                eredmeny.Add(new Koordináta(start.x, start.y - 1));
            }
            if (mehetLe && szomszédTípusSzűrő(this[start.x, start.y + 1]))
            {
                eredmeny.Add(new Koordináta(start.x, start.y + 1));
            }

            return eredmeny;
        }
        #endregion

    }
}
