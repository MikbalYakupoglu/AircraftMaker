using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Library.Abstract;

namespace Game.Library.Concrete
{
    internal class Kanat : ToplananCisim
    {
        public Kanat(int panelUzunlugu, int panelGenisligi) : base(panelUzunlugu, panelGenisligi)
        {
        }
    }
}
