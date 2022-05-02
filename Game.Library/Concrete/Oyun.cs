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
        private readonly Timer _toplananCisimTimer = new Timer() { Interval = 300 };

        private int _kalanSure;

        private int _kanatSayisi;
        private int _motorSayisi;
        private int _kodSayisi;

        private int _oyunSkoru;
        private int _kalanSkor;
        private int _uretilecekMiktar;

        private int labelSayaci;

        private readonly Panel _oyunPanel;
        private readonly Panel _bilgiPanel;
        private readonly Panel _anaMenuPanel;
        private readonly Panel _oyuncuBilgiPanel;
        private Muhendis _muhendis;
        private readonly TextBox _oyuncuAdiTextBox;
        private readonly TextBox _oyunSuresiTextBox;
        private readonly TextBox _uretilecekMiktarTextBox;
        private Label _gizliKutuLabel;

        private static readonly Random Random = new Random();

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


        #region Toplanan Cisimler

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


        private void ToplananCisimTimer_Tick(object sender, EventArgs e)
        {
            ToplananCisimOlustur();
        }

        private void HareketTimer_Tick(object sender, EventArgs e)
        {
            ToplananCisimleriHareketEttir();
        }


        private void CisimOlustur(ToplananCisim toplananCisim)
        {
            bool ustUsteGeldiMi;

            ustUsteGeldiMi = ToplananCisimUstUsteGeliyorMu(toplananCisim);
            if (ustUsteGeldiMi)
            {
                ToplananCisimOlustur();
                return;
            }

            _toplananCisimler.Add(toplananCisim);
            _oyunPanel.Controls.Add(toplananCisim);
        }

        private void ToplananCisimOlustur()
        {
            var sayi = Random.Next(4);
            switch (sayi)
            {
                case 1:
                    var motor = new Motor(PanelUzunlugu, PanelGenisligi);
                    CisimOlustur(motor);

                    break;
                case 2:
                    var kanat = new Kanat(PanelUzunlugu, PanelGenisligi);
                    CisimOlustur(kanat);
                    break;
                case 3:
                    var kod = new Kod(PanelUzunlugu, PanelGenisligi);
                    CisimOlustur(kod);
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
        private bool ToplananCisimUstUsteGeliyorMu(ToplananCisim toplananCisim)
        {
            foreach (var _toplananCisim in _toplananCisimler)
            {
                if ((_toplananCisim.Top <= toplananCisim.Bottom && _toplananCisim.Left <= toplananCisim.Right)
                    || _toplananCisim.Top <= toplananCisim.Bottom && _toplananCisim.Right >= toplananCisim.Left)
                {
                    return true;
                }
            }

            return false;
        }

        private void ToplananCisimleriHareketEttir()
        {
            OyunHiziniHesapla();

            for (int i = _toplananCisimler.Count - 1; i >= 0; i--)
            {
                var toplananCisim = _toplananCisimler[i];
                toplananCisim.HareketEttir(Yon.Asagi);

                var yereDustuMu = toplananCisim.YereDustuMu();
                var muhendiseDegdiMi = _muhendis.Left <= toplananCisim.Right && _muhendis.Right >= toplananCisim.Left
                                                                       && toplananCisim.Bottom >=
                                                                       (PanelUzunlugu - _muhendis.Height);

                switch (yereDustuMu)
                {
                    case true:

                        {
                            _toplananCisimler.Remove(toplananCisim);
                            _oyunPanel.Controls.Remove(toplananCisim);
                        }
                        break;

                    case false:
                        if (muhendiseDegdiMi)
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
                                var sayi = Random.Next(100);

                                _gizliKutuLabel.Visible = true;

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




        #endregion


        #region Zamanlayıcılar
        public int KalanSure
        {
            get => _kalanSure;
            private set
            {
                _kalanSure = value;

                KalanSureDegisti?.Invoke(this, EventArgs.Empty);
            }
        }

        private void KalanSureTimer_Tick(object sender, EventArgs e)
        {
            KalanSure--;

            if (_gizliKutuLabel.Visible)
            {
                labelSayaci++;
            }

            if (labelSayaci > 2)
            {
                _gizliKutuLabel.Visible = false;
                labelSayaci = 0;
            }

            if (KalanSure < 5)
            {
                _oyunPanel.Controls.Add(_gizliKutuLabel);
                _gizliKutuLabel.Visible = true;
                _gizliKutuLabel.Text = $"Sürenin Bitmesine {KalanSure} Saniye Kaldı !";
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
        #endregion


        #region Skor
        public int OyunSkoru
        {
            get => _oyunSkoru;
            set
            {
                _oyunSkoru = value;
                SkorDegisti?.Invoke(this, EventArgs.Empty);
            }
        }
        public int KalanSkor
        {
            get => _kalanSkor;
            set
            {
                _kalanSkor = value;
                SkorDegisti?.Invoke(this, EventArgs.Empty);
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
                    KalanSkor = _uretilecekMiktar - OyunSkoru;
                }
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

        #endregion


        #region Paneller
        private void gizliKutuLabelOlustur()
        {
            _gizliKutuLabel = new Label();
            _oyunPanel.Controls.Add(_gizliKutuLabel);
            _gizliKutuLabel.Location = new Point((_oyunPanel.Width - _bilgiPanel.Width) / 2, 40);
            _gizliKutuLabel.ForeColor = Color.Red;
            _gizliKutuLabel.BackColor = Color.Aqua;
            _gizliKutuLabel.TextAlign = ContentAlignment.TopLeft;
            _gizliKutuLabel.Font = new Font(FontFamily.GenericSansSerif, 15);
            _gizliKutuLabel.AutoSize = true;
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

        #endregion


        #region Oyun
        private void ToplayiciOlustur()
        {
            _muhendis = new Muhendis(PanelUzunlugu, PanelGenisligi);
            _oyunPanel.Controls.Add(_muhendis);
        }
        public void Basla()
        {
            if (DevamEdiyorMu) return;

            DevamEdiyorMu = true;
            _uretilecekMiktar = int.Parse(_uretilecekMiktarTextBox.Text);

            OyunBaslangıcınıAyarla();
            PanelleriAyarla();
            ZamanlayicilariBaslat();
            ToplayiciOlustur();
        }

        public void Bitir()
        {
            if (!DevamEdiyorMu) return;

            DevamEdiyorMu = false;

            ZamanlayicilariDurdur();
            SkoruYaz();
            OyunPaneliTemizle();
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
                _muhendis.HareketEttir(yon);
            }
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
            KalanSkor = _uretilecekMiktar;
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

        #endregion

        public Oyun(Panel oyunPanel, Panel bilgiPanel, Panel anaMenuPanel,Panel oyuncuBilgiPanel, TextBox oyuncuAdiTextBox,
            TextBox oyunSuresiTextBox, TextBox uretilecekMilktarTextBox)
        {
            _kalanSureTimer.Tick += KalanSureTimer_Tick;
            _toplananCisimTimer.Tick += ToplananCisimTimer_Tick;
            _hareketTimer.Tick += HareketTimer_Tick;

            _oyunPanel = oyunPanel;
            _bilgiPanel = bilgiPanel;
            _anaMenuPanel = anaMenuPanel;
            _oyuncuBilgiPanel = oyuncuBilgiPanel;
            _oyuncuAdiTextBox = oyuncuAdiTextBox;
            _oyunSuresiTextBox = oyunSuresiTextBox;
            _uretilecekMiktarTextBox = uretilecekMilktarTextBox;

            gizliKutuLabelOlustur();
        }
    }
}