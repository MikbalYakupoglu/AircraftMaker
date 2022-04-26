using System;
using System.Windows.Forms;
using Game.Library.Abstract;
using Game.Library.Enum;

namespace Game.Library.Concrete
{
    public class Oyun : IOyun
    {
        private readonly Timer _kalanSureTimer = new Timer() { Interval = 1000 };
        private int _kalanSure;

        public event EventHandler KalanSureDegisti;
        public bool DevamEdiyorMu { get; private set; }

        public int KalanSure
        {
            get => _kalanSure; 
            set
            {
                _kalanSure = value;

                KalanSureDegisti?.Invoke(this,EventArgs.Empty);
            }
        }




        public Oyun()
        {
            _kalanSureTimer.Tick += KalanSureTimer_Tick;
        }
        private void KalanSureTimer_Tick(object sender, EventArgs e)
        {
           KalanSure -= 1;
        }
        public void Basla()
        {
            if (DevamEdiyorMu) return;

            DevamEdiyorMu = true;
            _kalanSureTimer.Start();
        }

        private void Bitir()
        {
            if (!DevamEdiyorMu) return;

            DevamEdiyorMu = false;
            _kalanSureTimer.Stop();
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