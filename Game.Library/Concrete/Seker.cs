using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Library.Abstract;

namespace Game.Library.Concrete
{
    internal class Seker : ToplananCisim
    {
        public Seker(int panelUzunlugu, int panelGenisligi) : base(panelUzunlugu, panelGenisligi)
        {
        }
    }
}
