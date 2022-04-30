/*
 * B211200300
 * Muhammet İkbal Yakupoğlu
 * Bilişim Sistemleri Mühendisliği
 */

using System;
using Game.Library.Concrete;

namespace Game.Library.Abstract
{
    internal abstract class ToplananCisim : Cisim
    {
        private static readonly Random Random = new Random();
        public ToplananCisim(int panelUzunlugu, int panelGenisligi) : base(panelUzunlugu, panelGenisligi)
        {
            hareketMesafesi = (int)(Height * 0.1);

            Left = Random.Next(panelGenisligi + 1 - Width);
        }

        public bool YereDustuMu(Sepet sepet)
        {
            if (Bottom >= (PanelUzunlugu))
            {
                return true;

            }
            return false;

        }
    }
}
