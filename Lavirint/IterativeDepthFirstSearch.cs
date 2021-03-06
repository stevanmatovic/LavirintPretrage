﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Lavirint
{
    class IterativeDeepFirstSeach
    {
        public State search(State state, int maxDepth)
        {
            for (int lvl = 1; lvl < maxDepth; lvl++)
            {
                List<State> stanjaNaObradi = new List<State>();
                Hashtable predjeniPut = new Hashtable();
                stanjaNaObradi.Add(state);
                while (stanjaNaObradi.Count > 0)
                {
                    State naObradi = stanjaNaObradi[stanjaNaObradi.Count - 1];

                    if (!predjeniPut.ContainsKey(naObradi.GetHashCode()))
                    {
                        Main.allSearchStates.Add(naObradi);
                        if (naObradi.isKrajnjeStanje())
                        {
                            return naObradi;
                        }
                        predjeniPut.Add(naObradi.GetHashCode(), null);
                        if (naObradi.level < lvl)
                        {
                            List<State> mogucaSledecaStanja = naObradi.mogucaSledecaStanja();
                            foreach (State sledeceStanje in mogucaSledecaStanja)
                            {
                                stanjaNaObradi.Add(sledeceStanje);
                            }
                        }
                    }
                    stanjaNaObradi.Remove(naObradi);
                }
            }
            return null;
        }
    }
}
