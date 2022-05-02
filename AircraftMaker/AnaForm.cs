/*
 * B211200300
 * Muhammet İkbal Yakupoğlu
 * Bilişim Sistemleri Mühendisliği
 */


using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Game.Library.Concrete;
using Game.Library.Enum;

namespace AircraftMaker
{
    public partial class AnaForm : Form
    {
        private readonly Oyun _oyun;

        public AnaForm()
        {
            InitializeComponent();

            Focus();

            MinimumSize = Size;
            FormBorderStyle = FormBorderStyle.None;
            oyunBaslatPictureBox.Location = new Point(this.Width - 70, this.Height - 100);

            _oyun = new Oyun(oyunPanel, bilgiPanel, anaMenuPanel, oyuncuAdiTextBox,
                oyunSuresiTextBox, uretilecekMiktarTextBox);

            _oyun.KalanSureDegisti += Oyun_KalanSureDegisti;
            _oyun.CisimToplandi += Oyun_CisimToplandi;
            _oyun.UrunTamamlandi += Oyun_UrunTamamlandi;

            textBox4.ReadOnly = true;
        }



        private void AnaForm_Load(object sender, EventArgs e)
        {
            if (!_oyun.DevamEdiyorMu)
            {
                bilgiPanel.Visible = false;
            }
        }

        private void AnaForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    _oyun.Basla();
                    break;
                case Keys.Escape:
                    if (!_oyun.DevamEdiyorMu)
                    {
                        Close();
                    }
                    else
                    {
                        _oyun.Bitir();
                    }

                    break;

                case Keys.P:
                    _oyun.DurdurveBaslat();
                    kalansure.Text = _oyun.KalanSure.ToString();
                    break;


                case Keys.Right:
                    if (!_oyun.OyunDuraklatildiMi)
                    {
                        _oyun.HareketEt(Yon.Sag);
                    }
                    break;
                case Keys.D:
                    if (!_oyun.OyunDuraklatildiMi)
                    {
                        _oyun.HareketEt(Yon.Sag);
                    }
                    break;

                case Keys.Left:
                    if (!_oyun.OyunDuraklatildiMi)
                    {
                        _oyun.HareketEt(Yon.Sol);
                    }
                    break;
                case Keys.A:
                    if (!_oyun.OyunDuraklatildiMi)
                    {
                        _oyun.HareketEt(Yon.Sol);
                    }
                    break;
            }
        }

        #region Olaylar
        private void Oyun_KalanSureDegisti(object sender, EventArgs e)
        {
            if (_oyun.KalanSure >= 0) kalansure.Text = _oyun.KalanSure.ToString();
            else _oyun.Bitir();
        }

        private void Oyun_CisimToplandi(object sender, EventArgs e)
        {
            tamamlananihaLabel.Text = _oyun.TamamlananUrun.ToString();

            motorLabel.Text = _oyun.MotorSayisi.ToString();
            kanatLabel.Text = _oyun.KanatSayisi.ToString();
            kodLabel.Text = _oyun.KodSayisi.ToString();
        }

        private void Oyun_UrunTamamlandi(object sender, EventArgs e)
        {
            tamamlananihaLabel.Text = _oyun.TamamlananUrun.ToString();
            kalanihaLabel.Text = _oyun.KalanUrun.ToString();
        }

        #endregion


        #region Resimler

        private void bilgiPictureBox_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Oyunu Başlatmak İçin -Enter- Tuşuna Basınız.\n" +
                            "Oyundan Çıkmak İçin -ESC- Tuşuna Basınız.\n" +
                            "Hareket Etmek İçin -A , D- veya -Sol Ok , Sağ Ok- Tuşlarını Kullanınız.\n" +
                            "Oyunu Durdurmak İçin -P- Tuşuna Basınız.");
        }

        private void SkorPictureBox_Click(object sender, EventArgs e)
        {
            Top5Form.Top5Form topFiveForm = new Top5Form.Top5Form();
            topFiveForm.Show();
        }

        private void OyunBaslatPictureBox_Click(object sender, EventArgs e)
        {
            _oyun.Basla();
            Focus();
        }

        #endregion


        #region TextBoxlar

        private void oyuncuAdiTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            SiradakiTexteGec(sender, e);
        }

        private void oyunSuresiTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            SiradakiTexteGec(sender, e);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Tab)
            {
                e.Handled = true;
                _oyun.Basla();
                Focus();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                Close();
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            ActiveControl = oyuncuAdiTextBox;
        }

        #endregion

        private void SiradakiTexteGec(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Tab)
            {
                e.Handled = true;
                SelectNextControl(ActiveControl, true, true, true, true);
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                Close();
            }
        }



    }
}
