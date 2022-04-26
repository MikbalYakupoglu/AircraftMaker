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
        void Topla();
        void HareketEt(Yon yon);


    }
}