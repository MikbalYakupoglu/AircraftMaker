using System;
using System.Drawing;
using System.Windows.Forms;
using Game.Library.Enum;

namespace Game.Library.Abstract
{
    internal abstract class Cisim : PictureBox, IHareketEden
    {
        public int PanelUzunlugu { get; }
        public int PanelGenisligi { get; }
        public int hareketMesafesi { get; protected set; }

        public new int Right
        {
            get => base.Right;
            set => Left = value - Width;
        }
        public new int Bottom
        {
            get => base.Bottom;
            set => Top = value - Height;
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



        public Cisim(int panelUzunlugu, int panelGenisligi)
        {
            Image = Image.FromFile($@"..\..\Images\{GetType().Name}.png");
            PanelUzunlugu = panelUzunlugu;
            PanelGenisligi = panelGenisligi;
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
                case Yon.Asagi:
                    AsagiHareketEttir();
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
            if (Right == PanelGenisligi) return true;

            var yeniRight = Right + hareketMesafesi;
            var tasacakMi = yeniRight > PanelGenisligi;

            Right = tasacakMi ? PanelGenisligi : yeniRight;

            return Right == PanelGenisligi;
        }

        private bool AsagiHareketEttir()
        {
            if (Bottom == PanelUzunlugu) return true;

            var yeniBottom = Bottom + hareketMesafesi;
            var tasacakMi = yeniBottom > PanelUzunlugu;

            Bottom = tasacakMi ? PanelUzunlugu : yeniBottom;

            return Bottom == PanelUzunlugu;
        }
    }
}