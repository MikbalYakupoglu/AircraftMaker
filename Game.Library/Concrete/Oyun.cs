using System;
using System.Windows.Forms;
using Game.Library.Abstract;
using Game.Library.Enum;

namespace Game.Library.Concrete
{
    public class Oyun : IOyun
    {

        public bool DevamEdiyorMu { get; private set; }
        public TimeSpan KalanSure { get; }

        public void Basla()
        {
            if (DevamEdiyorMu) return;

            DevamEdiyorMu = true;
        }

        private void Bitir()
        {
            if (!DevamEdiyorMu) return;

            DevamEdiyorMu = false;
        }

        public void Topla()
        {
            throw new NotImplementedException();
        }

        public void HareketEt(Yon yon)
        {
            throw new NotImplementedException();
        }
    }
}