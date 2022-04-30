/*
 * B211200300
 * Muhammet İkbal Yakupoğlu
 * Bilişim Sistemleri Mühendisliği
 */

using System;
using Game.Library.Enum;

namespace Game.Library.Abstract
{
    internal interface IOyun
    {
        event EventHandler KalanSureDegisti;
        bool DevamEdiyorMu { get;}
        int KalanSure { get; }

        void Basla();
        void Bitir();
        void DurdurveBaslat();
        void HareketEt(Yon yon);


    }
}