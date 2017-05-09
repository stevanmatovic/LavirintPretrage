using System;
using System.Collections.Generic;
using System.Text;

namespace Lavirint
{
    public class State
    {
        public static int[,] lavirint;
        State parent;
        public int markI, markJ; //vrsta i kolona
        public double cost;
        public bool pokupio=false;
        private List<Kutija> plaveKutije = new List<Kutija>();
        private List<Kutija> narandzasteKutije = new List<Kutija>();
        private int pokupljenoP;
        private int pokupljenoN;
        public int level { get; set; }
        private bool gotovo = false;
        private int[,] movesKralj = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 }, { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
        private int[,] movesKonj = { { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }, { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 } };

        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            rez.level = level + 1;
            if (lavirint[markI, markJ] == 6)        // ukoliko je na vatri cena +20
                rez.cost += 20;
            else if (aroundFire(markI,markJ)) {     //ukoliko je jedno polje od vatre cena +10
                rez.cost += 10;
            }

            foreach (Kutija k in this.plaveKutije)
                rez.plaveKutije.Add(k);
            foreach (Kutija k in this.narandzasteKutije)
                rez.narandzasteKutije.Add(k);

            rez.pokupljenoN = this.pokupljenoN;
            rez.pokupljenoP = this.pokupljenoP;
            rez.gotovo = this.gotovo;

            return rez;
        }

        

        public List<State> mogucaSledecaStanja()
        {
            List<State> rez = new List<State>();

            if (lavirint[markI, markJ] == 4)
            {
                bool nadjeno = false;
                foreach (Kutija kutija in plaveKutije)
                {
                    if ((kutija.I == markI) && (kutija.J == markJ))
                    {
                        nadjeno = true;
                        break;
                    }
                }
                if (!nadjeno && (pokupljenoP < Main.brojPlavih))
                {
                    this.plaveKutije.Add(new Kutija(markI,markJ));
                    pokupljenoP++;
                }
            }

            if (lavirint[markI, markJ] == 5)
            {
                bool nadjeno = false;
                foreach (Kutija kutija in narandzasteKutije)
                {
                    if ((kutija.I == markI) && (kutija.J == markJ))
                    {
                        nadjeno = true;
                        break;
                    }
                }
                if (!nadjeno && (pokupljenoN < Main.brojNarandzastih))
                {
                    this.narandzasteKutije.Add(new Kutija(markI, markJ));
                    pokupljenoN++;
                }
            }


            if ((pokupljenoP == Main.brojPlavih) && (pokupljenoN == Main.brojNarandzastih))
            {
                this.gotovo = true;
            }

            dodajStanjaZaKralja(rez);

            return rez;
        }

        private bool isAllowedState(int newI, int newJ)
        {
            if (newI < 0 || newJ < 0)
                return false;
            if (newI >= Main.brojVrsta || newJ >= Main.brojKolona)
                return false;
            if (lavirint[newI, newJ] == 1)      //ukoliko je sivo polje
                return false;

            return true;
            

        }

        public override int GetHashCode()
        {
            return 100000*narandzasteKutije.Count+10000*plaveKutije.Count + 100*markI + markJ;
        }

        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && this.gotovo;
        }

        public List<State> path()
        {
            List<State> putanja = new List<State>();
            State tt = this;
            while (tt != null)
            {
                putanja.Insert(0, tt);
                tt = tt.parent;
            }
            return putanja;
        }

        private bool aroundFire(int markI, int markJ)       // ukoliko je bar jedno polje oko vatre vraca ture
        {
            for (int ind = 0; ind < movesKralj.GetLength(0); ind++)
            {
                int newI = this.markI + movesKralj[ind, 0];
                int newJ = this.markJ + movesKralj[ind, 1];

                if (isAllowedState(newI, newJ) && lavirint[newI, newJ] == 6)
                    return true;
            }
            return false;
        }



        public void dodajStanjaZaKralja(List<State> rez)
        {
            for (int ind = 0; ind < movesKralj.GetLength(0); ind++)
            {
                int newI = this.markI + movesKralj[ind, 0];
                int newJ = this.markJ + movesKralj[ind, 1];

                if (isAllowedState(newI, newJ))
                {
                    rez.Add(sledeceStanje(newI, newJ));
                }
            }
        }

        public void dodajStanjaZaKonja(List<State> rez) {
            for (int ind = 0; ind < movesKonj.GetLength(0); ind++)
            {
                int newI = this.markI + movesKonj[ind, 0];
                int newJ = this.markJ + movesKonj[ind, 1];

                if (isAllowedState(newI, newJ))
                {
                    rez.Add(sledeceStanje(newI, newJ));
                }
            }
        }

        public void dodajStanjaZaTopa(List<State> rez)
        {
            int brojac = 1;

            while ((markI + brojac < Main.brojVrsta) && (lavirint[markI + brojac, markJ] != 1))  // dole
            {
                rez.Add(sledeceStanje(markI + brojac, markJ));
                brojac++;
            }

            brojac = 1;

            while ((markI - brojac > -1) && (lavirint[markI - brojac, markJ] != 1))  // gore
            {
                rez.Add(sledeceStanje(markI - brojac, markJ));
                brojac++;
            }

            brojac = 1;

            while ((markJ + brojac < Main.brojKolona) && (lavirint[markI, markJ + brojac] != 1))  // desno
            {
                rez.Add(sledeceStanje(markI, markJ + brojac));
                brojac++;
            }

            brojac = 1;

            while ((markJ - brojac > -1) && (lavirint[markI, markJ - brojac] != 1))  // levo
            {
                rez.Add(sledeceStanje(markI, markJ - brojac));
                brojac++;
            }
        }

        public void dodajStanjaZaLovca(List<State> rez)
        {
            int brojac = 1;

            while ((markI + brojac < Main.brojVrsta) && (markJ + brojac < Main.brojKolona) && (lavirint[markI + brojac, markJ + brojac] != 1))  // dole-desno
            {
                rez.Add(sledeceStanje(markI + brojac, markJ + brojac));
                brojac++;
            }

            brojac = 1;

            while ((markI + brojac < Main.brojVrsta) && (markJ - brojac > -1) && (lavirint[markI + brojac, markJ - brojac] != 1))  // dole-levo
            {
                rez.Add(sledeceStanje(markI + brojac, markJ - brojac));
                brojac++;
            }

            brojac = 1;

            while ((markI - brojac > -1) && (markJ + brojac < Main.brojKolona) && (lavirint[markI - brojac, markJ + brojac] != 1))  // gore-desno
            {
                rez.Add(sledeceStanje(markI - brojac, markJ + brojac));
                brojac++;
            }

            brojac = 1;

            while ((markI - brojac > -1) && (markJ - brojac > -1) && (lavirint[markI - brojac, markJ - brojac] != 1))  // gore-levo
            {
                rez.Add(sledeceStanje(markI - brojac, markJ - brojac));
                brojac++;
            }
        }

        public void dodajStanjaZaKraljicu(List<State> rez)
        {
            int brojac = 1;

            while ((markI + brojac < Main.brojVrsta) && (lavirint[markI + brojac, markJ] != 1))  // dole
            {
                rez.Add(sledeceStanje(markI + brojac, markJ));
                brojac++;
            }

            brojac = 1;

            while ((markI - brojac > -1) && (lavirint[markI - brojac, markJ] != 1))  // gore
            {
                rez.Add(sledeceStanje(markI - brojac, markJ));
                brojac++;
            }

            brojac = 1;

            while ((markJ + brojac < Main.brojKolona) && (lavirint[markI, markJ + brojac] != 1))  // desno
            {
                rez.Add(sledeceStanje(markI, markJ + brojac));
                brojac++;
            }

            brojac = 1;

            while ((markJ - brojac > -1) && (lavirint[markI, markJ - brojac] != 1))  // levo
            {
                rez.Add(sledeceStanje(markI, markJ - brojac));
                brojac++;
            }

            brojac = 1;

            while ((markI + brojac < Main.brojVrsta) && (markJ + brojac < Main.brojKolona) && (lavirint[markI + brojac, markJ + brojac] != 1))  // dole-desno
            {
                rez.Add(sledeceStanje(markI + brojac, markJ + brojac));
                brojac++;
            }

            brojac = 1;

            while ((markI + brojac < Main.brojVrsta) && (markJ - brojac > -1) && (lavirint[markI + brojac, markJ - brojac] != 1))  // dole-levo
            {
                rez.Add(sledeceStanje(markI + brojac, markJ - brojac));
                brojac++;
            }

            brojac = 1;

            while ((markI - brojac > -1) && (markJ + brojac < Main.brojKolona) && (lavirint[markI - brojac, markJ + brojac] != 1))  // gore-desno
            {
                rez.Add(sledeceStanje(markI - brojac, markJ + brojac));
                brojac++;
            }

            brojac = 1;

            while ((markI - brojac > -1) && (markJ - brojac > -1) && (lavirint[markI - brojac, markJ - brojac] != 1))  // gore-levo
            {
                rez.Add(sledeceStanje(markI - brojac, markJ - brojac));
                brojac++;
            }
        }

    }
}
