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
        private int[,] moves = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        public State sledeceStanje(int markI, int markJ)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1;
            return rez;
        }

        
        public List<State> mogucaSledecaStanja()
        {
            //TO DO1: Implementirati metodu tako da odredjuje dozvoljeno kretanje u lavirintu
            //TO DO2: Prosiriti metodu tako da se ne moze prolaziti kroz sive kutije
            List<State> rez = new List<State>();

            for (int ind = 0; ind < moves.GetLength(0); ind++)
            {
                int newI = this.markI + moves[ind, 0];
                int newJ = this.markJ + moves[ind, 1];

                if (isAllowedState(newI, newJ))
                {
                    rez.Add(sledeceStanje(newI, newJ));
                }
            }

            return rez;
        }

        private bool isAllowedState(int newI, int newJ)
        {
            if (newI < 0 || newJ < 0)
                return false;
            if (newI >= Main.brojVrsta || newJ >= Main.brojKolona)
                return false;
            if (lavirint[newI, newJ] == 1)      //ukoliko je siv polje
                return false;

            return true;
            

        }

        public override int GetHashCode()
        {
            return 100*markI + markJ;
        }

        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ;
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

        
    }
}
