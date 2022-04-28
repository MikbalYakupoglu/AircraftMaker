using System;
using System.Drawing;
using System.Windows.Forms;
using Game.Library.Enum;

namespace Game.Library.Abstract
{
    internal abstract class Cisim : PictureBox, IHareketEden
    {
        public int OyunAlani { get; }
        public int hareketMesafesi { get; protected set; }

        public new int Right
        {
            get => base.Right;
            set => Left = value - Width;
        }

        public int Center
        {
            get => Left + Width / 2;
            set => Left = value - Width / 2;
        }

        public int Middle
        {
            get => Top + Height / 2;
            set => Top = value - Height / 2;
        }
        
        public Cisim(int oyunAlani)
        {
            Image = Image = Image.FromFile($@"Images\{GetType().Name}.png");
            this.OyunAlani = oyunAlani;
            SizeMode = PictureBoxSizeMode.AutoSize;
        }

        public bool HareketEttir(Yon yon)
        {
            switch (yon)
            {
                case Yon.Sag:
                    SagaHareketEttir();
                    break;
                case Yon.Sol:
                    SolaHareketEttir();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(yon), yon, null);
            }

            return false;
        }

        private bool SolaHareketEttir()
        {
            if (Left == 0) return true;

            var yeniLeft = Left - hareketMesafesi;
            var tasacakMi = yeniLeft < 0;

            Left = tasacakMi ? 0 : yeniLeft;

            return Left == 0;

        }

        private bool SagaHareketEttir()
        {
            if (Right == OyunAlani) return true;

            var yeniRight = Right + hareketMesafesi;
            var tasacakMi = yeniRight >  OyunAlani;

            Right = tasacakMi ? OyunAlani : yeniRight;

            return Right == OyunAlani;
        }
    }
}