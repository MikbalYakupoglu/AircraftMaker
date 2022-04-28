using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.Library.Abstract;

namespace Game.Library.Concrete
{
    internal class Sepet : Cisim
    {
        public Sepet(int panelUzunlugu, int panelGenisligi) : base(panelUzunlugu, panelGenisligi)
        {
            var sepetLocation = panelUzunlugu - Height;

            Center = panelGenisligi / 2;
            Top = sepetLocation;

            hareketMesafesi = Width;
        }
    }
}
