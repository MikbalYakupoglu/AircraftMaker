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

        private int _tamamlananUrun;
        private int _kalanUrun;
        private int _uretilecekMiktar;
        private int _oyunSkoru;

        private int labelSayaci;

        private readonly Panel _oyunPanel;
        private readonly Panel _bilgiPanel;
        private readonly Panel _anaMenuPanel;
        private Muhendis _muhendis;
        private readonly TextBox _oyuncuAdiTextBox;
        private readonly TextBox _oyunSuresiTextBox;
        private readonly TextBox _uretilecekMiktarTextBox;
        private Label _oyunPanelLabel;

        private static readonly Random Random = new Random();

        private readonly List<ToplananCisim> _toplananCisimler = new List<ToplananCisim>();

        #endregion


        #region Olaylar

        public event EventHandler KalanSureDegisti;
        public event EventHandler CisimToplandi;
        public event EventHandler UrunTamamlandi;

        #endregion


        #region Özellikler

        public bool DevamEdiyorMu { get; private set; }
        public bool OyunDuraklatildiMi { get; set; }
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
                if (_toplananCisimler.Count <= 0) return;
                

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

                                _oyunPanelLabel.Visible = true;

                                if (sayi >= 0 && sayi < 50)
                                {
                                    KodSayisi += 3;
                                    KanatSayisi += 2;
                                    MotorSayisi += 1;
                                    _oyunPanelLabel.Text = "Tebrikler! Bu ay fazladan 1 iha ürettik.";
                                }
                                else if (sayi >= 50 && sayi < 100)
                                {
                                    if (KodSayisi >= 3 && KanatSayisi >= 2 && MotorSayisi >= 1)
                                    {
                                        KodSayisi -= 3;
                                        KanatSayisi -= 2;
                                        MotorSayisi -= 1;
                                        TamamlananUrun--;
                                        KalanUrun++;
                                        _oyunPanelLabel.Text = "Üzgünüz! Yaptığımız 1 iha arıza çıkardı.";
                                    }
                                    else
                                    {
                                        _oyunPanelLabel.Text = "Fabrikada herşey normal gidiyor. Hiçbirşey Değişmedi.";
                                    }
                                }

                                _toplananCisimler.Remove(toplananCisim);
                                _oyunPanel.Controls.Remove(toplananCisim);
                            }
                        }

                        UrunHesapla();
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

            if (_oyunPanelLabel.Visible)
            {
                labelSayaci++;
            }

            if (labelSayaci > 2)
            {
                _oyunPanelLabel.Visible = false;
                labelSayaci = 0;
            }

            if (KalanSure < 5)
            {
                _oyunPanel.Controls.Add(_oyunPanelLabel);
                _oyunPanelLabel.Visible = true;
                _oyunPanelLabel.Text = $"Sürenin Bitmesine {KalanSure} Saniye Kaldı !";
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


        #region Urun/Skor
        public int TamamlananUrun
        {
            get => _tamamlananUrun;
            set
            {
                _tamamlananUrun = value;
                UrunTamamlandi?.Invoke(this, EventArgs.Empty);
            }
        }
        public int KalanUrun
        {
            get => _kalanUrun;
            set
            {
                _kalanUrun = value;
                UrunTamamlandi?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UrunHesapla()
        {
            var motorSayisi = MotorSayisi;
            var kanatSayisi = KanatSayisi;
            var kodSayisi = KodSayisi;
            while (motorSayisi >= 1 && kanatSayisi >= 2 && kodSayisi >= 3)
            {

                for (int i = 0; i < TamamlananUrun; i++)
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
                    TamamlananUrun++;
                    KalanUrun = _uretilecekMiktar - TamamlananUrun;
                    if (KalanUrun <= 0)
                    {
                        Bitir();
                    }
                }
            }
        }
        private void SkorHesapla()
        {
            while (MotorSayisi >= 1)
            {
                _oyunSkoru += 15;
                MotorSayisi--;
            }

            while (KanatSayisi >= 1)
            {
                _oyunSkoru += 15;
                KanatSayisi--;
            }

            while (KodSayisi >= 1)
            {
                _oyunSkoru += 15;
                KodSayisi--;
            }

            while (TamamlananUrun >= 1)
            {
                _oyunSkoru += 100;
                TamamlananUrun--;
            }
        }
        private void SkoruYaz()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (_oyunSkoru > 0)
            {
                using (StreamWriter streamWriter = File.AppendText(path + @"\topfive.txt"))
                {
                    streamWriter.Write($"0◘{_oyuncuAdiTextBox.Text}◘{DateTime.Now}◘{_oyunSkoru}\n");
                }
            }
        }

        #endregion


        #region Paneller
        private void gizliKutuLabelOlustur()
        {
            _oyunPanelLabel = new Label();
            _oyunPanel.Controls.Add(_oyunPanelLabel);
            _oyunPanelLabel.Location = new Point((_oyunPanel.Width - _bilgiPanel.Width) / 2, 40);
            _oyunPanelLabel.ForeColor = Color.Red;
            _oyunPanelLabel.BackColor = Color.Aqua;
            _oyunPanelLabel.TextAlign = ContentAlignment.TopLeft;
            _oyunPanelLabel.Font = new Font(FontFamily.GenericSansSerif, 15);
            _oyunPanelLabel.AutoSize = true;
        }

        private bool TextBoxlarinDegerleriGecerliMi()
        {
            var sureText = _oyunSuresiTextBox.Text;
            foreach (var harf in sureText)
            {
                if (!char.IsDigit(harf))
                {
                    MessageBox.Show("Süre Değerine 0'dan Büyük Sayı Değer Girmelisiniz.");
                    return false;
                }
            }

            var uretilecekMiktarText = _uretilecekMiktarTextBox.Text;
            foreach (var harf in uretilecekMiktarText)
            {
                if (!char.IsDigit(harf))
                {
                    MessageBox.Show("Üretilecek Miktara 0'dan Büyük Sayı Değer Girmelisiniz.");
                    return false;
                }
            }

            return true;
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
            OyunDuraklatildiMi = false;

            if (TextBoxlarinDegerleriGecerliMi())
            {
                _uretilecekMiktar = int.Parse(_uretilecekMiktarTextBox.Text);

                OyunBaslangıcınıAyarla();
                PanelleriAyarla();
                ZamanlayicilariBaslat();
                ToplayiciOlustur();
            }
            else
            {
                Bitir();
            }
        }

        public void Bitir()
        {
            if (!DevamEdiyorMu) return;

            DevamEdiyorMu = false;

            ZamanlayicilariDurdur();
            SkorHesapla();
            SkoruYaz();
            OyunPaneliTemizle();
            PanelleriAyarla();
        }

        public void DurdurveBaslat()
        {
            if (_kalanSureTimer.Enabled && DevamEdiyorMu)
            {
                OyunDuraklatildiMi = true;
                ZamanlayicilariDurdur();
                if (KalanSure > 0)
                {
                    _kalanSure--;
                }
            }
            else if (DevamEdiyorMu)
            {
                OyunDuraklatildiMi = false;
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

            _oyunSkoru = 0;
            KanatSayisi = 0;
            KodSayisi = 0;
            MotorSayisi = 0;

            TamamlananUrun = 0;
            if (_uretilecekMiktar <= 0)
            {
                MessageBox.Show("Üretilecek Miktar 0'dan Büyük Olmalıdır");
                Bitir();
            }
            KalanUrun = _uretilecekMiktar;
        }


        public void OyunHiziniHesapla()
        {
            if (TamamlananUrun > 5 && TamamlananUrun < 10)
            {
                _hareketTimer.Interval = 60;
            }
            else if (TamamlananUrun >= 10 && TamamlananUrun < 15)
            {
                _hareketTimer.Interval = 45;

            }
            else if (TamamlananUrun >= 15 && TamamlananUrun < 20)
            {
                _hareketTimer.Interval = 30;

            }
            else if (TamamlananUrun >= 20)
            {
                _hareketTimer.Interval = 20;

            }
        }

        #endregion

        public Oyun(Panel oyunPanel, Panel bilgiPanel, Panel anaMenuPanel,TextBox oyuncuAdiTextBox,
            TextBox oyunSuresiTextBox, TextBox uretilecekMilktarTextBox)
        {
            _kalanSureTimer.Tick += KalanSureTimer_Tick;
            _toplananCisimTimer.Tick += ToplananCisimTimer_Tick;
            _hareketTimer.Tick += HareketTimer_Tick;

            _oyunPanel = oyunPanel;
            _bilgiPanel = bilgiPanel;
            _anaMenuPanel = anaMenuPanel;
            _oyuncuAdiTextBox = oyuncuAdiTextBox;
            _oyunSuresiTextBox = oyunSuresiTextBox;
            _uretilecekMiktarTextBox = uretilecekMilktarTextBox;

            gizliKutuLabelOlustur();
        }
    }
}