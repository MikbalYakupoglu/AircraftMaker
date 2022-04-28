using System;
using System.Drawing;
using System.Windows.Forms;
using Game.Library.Abstract;
using Game.Library.Enum;

namespace Game.Library.Concrete
{
    public class Oyun : IOyun
    {
        private readonly Timer _kalanSureTimer = new Timer() { Interval = 1000 };
        private int _kalanSure;
        private readonly Panel _oyunPanel;
        private readonly Panel _bilgiPanel;
        private Sepet _sepet;

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




        public Oyun(Panel oyunPanel, Panel bilgiPanel)
        {
            _kalanSureTimer.Tick += KalanSureTimer_Tick;
            _oyunPanel = oyunPanel;
            _bilgiPanel = bilgiPanel;
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

            SepetOlustur();
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
            if (DevamEdiyorMu)
            {
                _sepet.HareketEttir(yon);
            }
        }

        private void SepetOlustur()
        {
            var oyunAlani = _oyunPanel.Size.Width - _bilgiPanel.Size.Width;
            _sepet = new Sepet(_oyunPanel.Size, oyunAlani);

            _oyunPanel.Controls.Add(_sepet);
        }
    }
}