/*
 * B211200300
 * Muhammet İkbal Yakupoğlu
 * Bilişim Sistemleri Mühendisliği
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private readonly Timer _hareketTimer = new Timer() { Interval = 80 };
        private readonly Timer _toplananCisimTimer = new Timer() { Interval = 400 };

        private int _kalanSure;
        private int _oyunSkoru;
        private int _kanatSayisi;
        private int _motorSayisi;
        private int _kodSayisi;

        private readonly Panel _oyunPanel;
        private readonly Panel _bilgiPanel;
        private readonly Panel _anaMenuPanel;
        private Sepet _sepet;
        private readonly TextBox _oyuncuAdiTextBox;
        private readonly TextBox _oyunSuresiTextBox;
        private readonly Label _gizliKutuLabel;

        private readonly List<ToplananCisim> _toplananCisimler = new List<ToplananCisim>();
        #endregion


        #region Olaylar
        public event EventHandler KalanSureDegisti;
        public event EventHandler CisimToplandi;
        public event EventHandler SkorDegisti;

        #endregion


        #region Özellikler
        public bool DevamEdiyorMu { get; private set; }
        public int PanelUzunlugu { get; private set; }
        public int PanelGenisligi { get; private set; }
        #endregion


        public int OyunSkoru
        {
            get => _oyunSkoru;
            set
            {
                _oyunSkoru = value;
                SkorDegisti?.Invoke(this, EventArgs.Empty);
            }
        }

        public int KanatSayisi
        {
            get => _kanatSayisi;
            private set
            {
                _kanatSayisi = value;
                CisimToplandi?.Invoke(this, EventArgs.Empty);
            }
        }

        public int KodSayisi
        {
            get => _kodSayisi;
            private set
            {
                _kodSayisi = value;
                CisimToplandi?.Invoke(this, EventArgs.Empty);
            }
        }

        public int MotorSayisi
        {
            get => _motorSayisi;
            private set
            {
                _motorSayisi = value;
                CisimToplandi?.Invoke(this, EventArgs.Empty);
            }
        }

        public int KalanSure
        {
            get => _kalanSure;
            private set
            {
                _kalanSure = value;

                KalanSureDegisti?.Invoke(this, EventArgs.Empty);
            }
        }


        public Oyun(Panel oyunPanel, Panel bilgiPanel, Panel anaMenuPanel, TextBox oyuncuAdiTextBox, TextBox oyunSuresiTextBox, Label gizliKutuLabel)
        {
            _kalanSureTimer.Tick += KalanSureTimer_Tick;
            _toplananCisimTimer.Tick += ToplananCisimTimer_Tick;
            _hareketTimer.Tick += HareketTimer_Tick;

            _oyunPanel = oyunPanel;
            _bilgiPanel = bilgiPanel;
            _anaMenuPanel = anaMenuPanel;
            _oyuncuAdiTextBox = oyuncuAdiTextBox;
            _oyunSuresiTextBox = oyunSuresiTextBox;
            _gizliKutuLabel = gizliKutuLabel;


        }

        int labelSayaci = 0;
        private void KalanSureTimer_Tick(object sender, EventArgs e)
        {
            KalanSure--;
            labelSayaci++;

            if (labelSayaci > 3)
            {
                _gizliKutuLabel.Visible = false;
                labelSayaci = 0;
            }

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

            //OyunPaneliTemizle();
            OyunBaslangıcınıAyarla();
            PanelleriAyarla();
            ZamanlayicilariBaslat();
            SepetOlustur();
        }

        // Bitirildiğinde paneller gidiyor
        public void Bitir()
        {
            if (!DevamEdiyorMu) return;

            DevamEdiyorMu = false;

            OyunPaneliTemizle();
            ZamanlayicilariDurdur();
            SkoruYaz();
            PanelleriAyarla();
        }

        public void DurdurveBaslat()
        {
            if (_kalanSureTimer.Enabled && DevamEdiyorMu)
            {
                ZamanlayicilariDurdur();
                if (KalanSure > 0)
                {
                    _kalanSure--;
                }
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

        public void PanelleriAyarla()
        {
            if (!DevamEdiyorMu)
            {
                _bilgiPanel.Visible = false;
                _anaMenuPanel.Visible = true;
            }
            else if (DevamEdiyorMu)
            {
                _bilgiPanel.Visible = true;
                _anaMenuPanel.Visible = false;
            }
        }

        private void OyunPaneliTemizle()
        {
            _oyunPanel.Controls.Clear();
            _oyunPanel.Controls.Add(_anaMenuPanel);
            _oyunPanel.Refresh();
            _toplananCisimler.Clear();
        }

        private void OyunBaslangıcınıAyarla()
        {
            int oyunSuresi = int.Parse(_oyunSuresiTextBox.Text);
            KalanSure = oyunSuresi;

            PanelUzunlugu = _oyunPanel.Height;
            PanelGenisligi = _oyunPanel.Width - _bilgiPanel.Width;

            KanatSayisi = 0;
            KodSayisi = 0;
            MotorSayisi = 0;
            OyunSkoru = 0;
        }

        private void ToplananCisimleriHareketEttir()
        {
            OyunHiziniHesapla();

            for (int i = _toplananCisimler.Count - 1; i >= 0; i--)
            {
                var toplananCisim = _toplananCisimler[i];
                toplananCisim.HareketEttir(Yon.Asagi);

                var yereDustuMu = toplananCisim.YereDustuMu();
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
                            if (toplananCisim.GetType().Name == "Kod")
                            {
                                KodSayisi++;
                                _toplananCisimler.Remove(toplananCisim);
                                _oyunPanel.Controls.Remove(toplananCisim);
                            }
                            else if (toplananCisim.GetType().Name == "Kanat")
                            {
                                KanatSayisi++;
                                _toplananCisimler.Remove(toplananCisim);
                                _oyunPanel.Controls.Remove(toplananCisim);
                            }
                            else if (toplananCisim.GetType().Name == "Motor")
                            {
                                MotorSayisi++;
                                _toplananCisimler.Remove(toplananCisim);
                                _oyunPanel.Controls.Remove(toplananCisim);
                            }
                            else if (toplananCisim.GetType().Name == "GizliKutu")
                            {
                                Random rnd = new Random();
                                var sayi = rnd.Next(100);

                                _gizliKutuLabel.Visible = true;
                                _oyunPanel.Controls.Add(_gizliKutuLabel);

                                if (sayi >= 0 && sayi < 50)
                                {
                                    KodSayisi += 3;
                                    KanatSayisi += 2;
                                    MotorSayisi += 1;
                                    _gizliKutuLabel.Text = "Tebrikler! Bu ay fazladan 1 iha ürettik.";
                                }
                                else if (sayi >= 50 && sayi < 100)
                                {
                                    if (KodSayisi >= 3 && KanatSayisi >= 2 && MotorSayisi >= 1)
                                    {
                                        KodSayisi -= 3;
                                        KanatSayisi -= 2;
                                        MotorSayisi -= 1;
                                        OyunSkoru--;
                                        _gizliKutuLabel.Text = "Üzgünüz! Yaptığımız 1 iha arıza çıkardı.";
                                    }
                                    else
                                    {
                                        _gizliKutuLabel.Text = "Fabrikada herşey normal gidiyor. Hiçbirşey Değişmedi.";
                                    }
                                }
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
            var motorSayisi = MotorSayisi;
            var kanatSayisi = KanatSayisi;
            var kodSayisi = KodSayisi;

            while (motorSayisi >= 1 && kanatSayisi >= 2 && kodSayisi >= 3)
            {

                for (int i = 0; i < OyunSkoru; i++)
                {
                    motorSayisi -= 1;
                    kanatSayisi -= 2;
                    kodSayisi -= 3;
                }

                if (motorSayisi >= 1 && kanatSayisi >= 2 && kodSayisi >= 3)
                {
                    motorSayisi -= 1;
                    kanatSayisi -= 2;
                    kodSayisi -= 3;
                    OyunSkoru++;
                }
            }
        }

        public void OyunHiziniHesapla()
        {
            if (OyunSkoru > 5 && OyunSkoru < 10)
            {
                _hareketTimer.Interval = 60;
            }
            else if (OyunSkoru >= 10 && OyunSkoru < 15)
            {
                _hareketTimer.Interval = 45;

            }
            else if (OyunSkoru >= 15 && OyunSkoru < 20)
            {
                _hareketTimer.Interval = 30;

            }
            else if (OyunSkoru >= 20)
            {
                _hareketTimer.Interval = 20;

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
                    var motor = new Motor(PanelUzunlugu, PanelGenisligi);
                    _toplananCisimler.Add(motor);
                    _oyunPanel.Controls.Add(motor);
                    break;
                case 2:
                    var kanat = new Kanat(PanelUzunlugu, PanelGenisligi);
                    _toplananCisimler.Add(kanat);
                    _oyunPanel.Controls.Add(kanat);
                    break;
                case 3:
                    var kod = new Kod(PanelUzunlugu, PanelGenisligi);
                    _toplananCisimler.Add(kod);
                    _oyunPanel.Controls.Add(kod);
                    break;
            }

            if (KalanSure % 10 == 0)
            {
                var gizliKutu = new GizliKutu(PanelUzunlugu, PanelGenisligi);

                _toplananCisimler.Add(gizliKutu);
                _oyunPanel.Controls.Add(gizliKutu);
                KalanSure--;

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