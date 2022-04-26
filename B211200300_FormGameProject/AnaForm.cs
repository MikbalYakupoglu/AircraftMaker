using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.Library.Concrete;
using Game.Library.Enum;
using TimeSpan = System.TimeSpan;

namespace B211200300_FormGameProject
{
    public partial class AnaForm : Form
    {
        private readonly Oyun _oyun = new Oyun();

        public AnaForm()
        {
            InitializeComponent();

            _oyun.KalanSure = 120;
            _oyun.KalanSureDegisti += Oyun_KalanSureDegisti;
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
                case Keys.Right:
                    _oyun.HareketEt(Yon.Sag);
                    break;
                case Keys.Left:
                    _oyun.HareketEt(Yon.Sol);
                    break;
            }
            if (_oyun.DevamEdiyorMu)
            {
                anaMenuPanel.Visible = false;
                bilgiPanel.Visible = true;
            }
        }

        private void Oyun_KalanSureDegisti(object sender, EventArgs e)
        {
            kalansure.Text = _oyun.KalanSure.ToString();
        }
    }
}
