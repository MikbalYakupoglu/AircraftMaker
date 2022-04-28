using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Game.Library.Abstract;
using Game.Library.Enum;
using Timer = System.Windows.Forms.Timer;

namespace Game.Library.Concrete
{
    public class Oyun : IOyun
    {
        #region Alanlar
        private readonly Timer _kalanSureTimer = new Timer() { Interval = 1000 };
        private readonly Timer _toplananCisimTimer = new Timer() { Interval = 500 };
        private int _kalanSure;

        private readonly Panel _oyunPanel;
        private readonly Panel _bilgiPanel;
        private Sepet _sepet;

        private readonly List<ToplananCisim> _toplananCisimler = new List<ToplananCisim>();
        #endregion


        #region Olaylar
        public event EventHandler KalanSureDegisti;

        #endregion


        #region Özellikler
        public bool DevamEdiyorMu { get; private set; }
        public int PanelUzunlugu { get; private set; }
        public int PanelGenisligi { get; private set; }

        #endregion




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
            _toplananCisimTimer.Tick += ToplananCisimTimer_Tick;
            _oyunPanel = oyunPanel;
            _bilgiPanel = bilgiPanel;

        }

        private void KalanSureTimer_Tick(object sender, EventArgs e)
        {
           KalanSure -= 1;
        }
        private void ToplananCisimTimer_Tick(object sender, EventArgs e)
        {
            ToplananCisimOlustur();
        }

        public void Basla()
        {
            if (DevamEdiyorMu) return;

            PanelUzunlugu = _oyunPanel.Height;
            PanelGenisligi = _oyunPanel.Width - _bilgiPanel.Width;

            DevamEdiyorMu = true;

            ZamanlayicilariBaslat();

            SepetOlustur();
            ToplananCisimOlustur(); 
        }



        private void Bitir()
        {
            if (!DevamEdiyorMu) return;

            ZamanlayicilariDurdur();
            DevamEdiyorMu = false;
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

        private void ZamanlayicilariBaslat()
        {
            _kalanSureTimer.Start();
            _toplananCisimTimer.Start();
        }

        private void ZamanlayicilariDurdur()
        {
            _kalanSureTimer.Stop();
            _toplananCisimTimer.Stop();
        }

        private void SepetOlustur()
        {
            _sepet = new Sepet(PanelUzunlugu, PanelGenisligi);
            _oyunPanel.Controls.Add(_sepet);
        }

        private void ToplananCisimOlustur()
        {
            Random r = new Random();

            var random = new Random(r.Next());
            var sayi = random.Next(4);

            switch (sayi)
            {
                case 1:
                    var bardak = new Bardak(PanelUzunlugu, PanelGenisligi);
                    _toplananCisimler.Add(bardak);
                    _oyunPanel.Controls.Add(bardak);
                    break;
                case 2:
                    var cay = new Cay(PanelUzunlugu, PanelGenisligi);
                    _toplananCisimler.Add(cay);
                    _oyunPanel.Controls.Add(cay);
                    break;
                case 3:
                    var seker = new Seker(PanelUzunlugu, PanelGenisligi);
                    _toplananCisimler.Add(seker);
                    _oyunPanel.Controls.Add(seker);
                    break;
            }
        }
    }
}