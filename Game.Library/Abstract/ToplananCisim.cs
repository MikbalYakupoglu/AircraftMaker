using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Library.Concrete;

namespace Game.Library.Abstract
{
    internal abstract class ToplananCisim : Cisim
    {
        private static readonly Random Random = new Random();
        public ToplananCisim(int panelUzunlugu, int panelGenisligi) : base(panelUzunlugu, panelGenisligi)
        {
            Left = Random.Next(panelGenisligi + 1 - Width);
        }

        public bool YereDustuMu(Sepet sepet)
        {
            if (Bottom >= (PanelUzunlugu - Height))
            {
                var yereDustuMu = Right <= sepet.Left || Left >= sepet.Right;
                if (yereDustuMu) return true;

            }
            return false;

        }
    }
}
