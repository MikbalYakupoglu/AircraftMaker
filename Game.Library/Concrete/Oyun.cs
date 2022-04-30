/*
 * B211200300
 * Muhammet İkbal Yakupoğlu
 * Bilişim Sistemleri Mühendisliği
 */

using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly Timer _hareketTimer = new Timer() { Interval = 100 };
        private readonly Timer _toplananCisimTimer = new Timer() { Interval = 500 };

        private int _kalanSure;
        private int _caySayisi;
        private int _bardakSayisi;
        private int _sekerSayisi;

        private readonly Panel _oyunPanel;
        private readonly Panel _bilgiPanel;
        private readonly Panel _anaMenuPanel;
        private Sepet _sepet;
        private TextBox _oyuncuAdiTextBox;
        private TextBox _oyunSuresiTextBox;

        private readonly List<ToplananCisim> _toplananCisimler = new List<ToplananCisim>();
        #endregion


        #region Olaylar
        public event EventHandler KalanSureDegisti;
        public event EventHandler SkorDegisti;

        #endregion


        #region Özellikler
        public bool DevamEdiyorMu { get; private set; }
        public int PanelUzunlugu { get; private set; }
        public int PanelGenisligi { get; private set; }
        public int OyunSkoru { get; set; }

        #endregion

        public int CaySayisi
        {
            get => _caySayisi;
            set
            {
                _caySayisi = value;
                SkorDegisti?.Invoke(this, EventArgs.Empty);
            }
        }

        public int SekerSayisi
        {
            get => _sekerSayisi;
            set
            {
                _sekerSayisi = value;
                SkorDegisti?.Invoke(this, EventArgs.Empty);
            }
        }

        public int BardakSayisi
        {
            get => _bardakSayisi;
            set
            {
                _bardakSayisi = value;
                SkorDegisti?.Invoke(this, EventArgs.Empty);
            }
        }

        public int KalanSure
        {
            get => _kalanSure;
            set
            {
                _kalanSure = value;

                KalanSureDegisti?.Invoke(this, EventArgs.Empty);
            }
        }


        public Oyun(Panel oyunPanel, Panel bilgiPanel, Panel anaMenuPanel, TextBox oyuncuAdiTextBox, TextBox oyunSuresiTextBox)
        {
            _kalanSureTimer.Tick += KalanSureTimer_Tick;
            _toplananCisimTimer.Tick += ToplananCisimTimer_Tick;
            _hareketTimer.Tick += HareketTimer_Tick;

            _oyunPanel = oyunPanel;
            _bilgiPanel = bilgiPanel;
            _anaMenuPanel = anaMenuPanel;
            _oyuncuAdiTextBox = oyuncuAdiTextBox;
            _oyunSuresiTextBox = oyunSuresiTextBox;
        }

        private void KalanSureTimer_Tick(object sender, EventArgs e)
        {
            KalanSure -= 1;
        }
        private void ToplananCisimTimer_Tick(object sender, EventArgs e)
        {
            ToplananCisimOlustur();
        }

        private void HareketTimer_Tick(object sender, EventArgs e)
        {
            ToplananCisimleriHareketEttir();
        }


        public void Basla()
        {
            if (DevamEdiyorMu) return;

            DevamEdiyorMu = true;

            CaySayisi = 0;
            SekerSayisi = 0;
            BardakSayisi = 0;

            int oyunSuresi = int.Parse(_oyunSuresiTextBox.Text);
            KalanSure = oyunSuresi;

            PanelUzunlugu = _oyunPanel.Height;
            PanelGenisligi = _oyunPanel.Width - _bilgiPanel.Width;

            PanelleriAyarla();
            ZamanlayicilariBaslat();
            SepetOlustur();
        }

        public void Bitir()
        {
            if (!DevamEdiyorMu) return;

            DevamEdiyorMu = false;
            ZamanlayicilariDurdur();
            SkoruYaz();
            PanelleriAyarla();
        }

        public void DurdurveBaslat()
        {
            if (_kalanSureTimer.Enabled && DevamEdiyorMu)
            {
                ZamanlayicilariDurdur();
                _kalanSure--;
            }
            else if (DevamEdiyorMu)
            {
                ZamanlayicilariBaslat();
            }
        }

        public void HareketEt(Yon yon)
        {
            if (DevamEdiyorMu)
            {
                _sepet.HareketEttir(yon);
            }
        }

        private void PanelleriAyarla()
        {
            if (!DevamEdiyorMu)
            {
                _anaMenuPanel.Visible = true;
                _oyunPanel.Visible = false;
                _bilgiPanel.Visible = false;
            }
            else if (DevamEdiyorMu)
            {
                _anaMenuPanel.Visible = false;
                _oyunPanel.Visible = true;
                _bilgiPanel.Visible = true;
            }
        }

        private void ToplananCisimleriHareketEttir()
        {
            for (int i = _toplananCisimler.Count - 1; i >= 0; i--)
            {
                var toplananCisim = _toplananCisimler[i];
                toplananCisim.HareketEttir(Yon.Asagi);

                var yereDustuMu = toplananCisim.YereDustuMu(_sepet);
                var sepeteDegdiMi = _sepet.Left <= toplananCisim.Right && _sepet.Right >= toplananCisim.Left
                                                                       && toplananCisim.Bottom >= (PanelUzunlugu - _sepet.Height);

                switch (yereDustuMu)
                {
                    case true:

                        {
                            _toplananCisimler.Remove(toplananCisim);
                            _oyunPanel.Controls.Remove(toplananCisim);
                        }
                        break;

                    case false:
                        if (sepeteDegdiMi)
                        {
                            if (toplananCisim.GetType().Name == "Seker")
                            {
                                SekerSayisi++;
                                _toplananCisimler.Remove(toplananCisim);
                                _oyunPanel.Controls.Remove(toplananCisim);
                            }
                            else if (toplananCisim.GetType().Name == "Cay")
                            {
                                CaySayisi++;
                                _toplananCisimler.Remove(toplananCisim);
                                _oyunPanel.Controls.Remove(toplananCisim);
                            }
                            else if (toplananCisim.GetType().Name == "Bardak")
                            {
                                BardakSayisi++;
                                _toplananCisimler.Remove(toplananCisim);
                                _oyunPanel.Controls.Remove(toplananCisim);
                            }
                        }
                        SkorHesapla();
                        break;
                }
            }
        }

        private void SkorHesapla()
        {
            var bardakSayisi = BardakSayisi;
            var caySayisi = CaySayisi;
            var sekerSayisi = SekerSayisi;

            while (bardakSayisi >= 1 && caySayisi >= 2 && sekerSayisi >= 3)
            {

                for (int i = 0; i < OyunSkoru; i++)
                {
                    bardakSayisi -= 1;
                    caySayisi -= 2;
                    sekerSayisi -= 3;
                }
                ;

                if (bardakSayisi >= 1 && caySayisi >= 2 && sekerSayisi >= 3)
                {
                    bardakSayisi -= 1;
                    caySayisi -= 2;
                    sekerSayisi -= 3;
                    OyunSkoru++;
                }

            }
        }

        private void ZamanlayicilariBaslat()
        {
            _kalanSureTimer.Start();
            _toplananCisimTimer.Start();
            _hareketTimer.Start();
        }

        private void ZamanlayicilariDurdur()
        {
            _kalanSureTimer.Stop();
            _toplananCisimTimer.Stop();
            _hareketTimer.Stop();
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
        private void SkoruYaz()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter streamWriter = File.AppendText(path + @"\topfive.txt"))
            {
                streamWriter.Write($"0◘{_oyuncuAdiTextBox.Text}◘{DateTime.Now}◘{OyunSkoru}\n");
            }
        }
    }
}