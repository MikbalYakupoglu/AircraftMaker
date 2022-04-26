using System;
using Game.Library.Enum;

namespace Game.Library.Abstract
{
    internal interface IOyun
    {
        bool DevamEdiyorMu { get;}
        TimeSpan KalanSure { get; }

        void Basla();
        void Topla();
        void HareketEt(Yon yon);


    }
}