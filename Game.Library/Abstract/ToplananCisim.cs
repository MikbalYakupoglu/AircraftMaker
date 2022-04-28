using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Library.Abstract
{
    internal abstract class ToplananCisim : Cisim
    {
        private static readonly Random Random = new Random();
        public ToplananCisim(int panelUzunlugu, int panelGenisligi) : base(panelUzunlugu, panelGenisligi)
        {
            Left = Random.Next(panelGenisligi + 1 - Width);
        }
    }
}
