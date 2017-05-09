using System;
using System.Collections.Generic;
using System.Text;

namespace Lavirint
{
    class Kutija
    {
        private int i { get; set; }
        private int j { get; set; }

        public int getI() {
            return i;
        }

        public int getJ()
        {
            return j;
        }
        public Kutija(int i,int j) {
            this.i = i;
            this.j = j;
        }

        public Kutija(Kutija k) {
            this.i = k.i;
            this.j = k.j;
        }

    }
}
