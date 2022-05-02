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
    internal class Muhendis : Cisim
    {
        public Muhendis(int panelUzunlugu, int panelGenisligi) : base(panelUzunlugu, panelGenisligi)
        {
            var muhendisLocation = panelUzunlugu - Height;

            Center = panelGenisligi / 2;
            Top = muhendisLocation;

            hareketMesafesi = Width;
        }
    }
}
