using System.Drawing;
using Game.Library.Enum;

namespace Game.Library.Abstract
{
    internal interface IHareketEden
    {
        int PanelUzunlugu { get; }
        int PanelGenisligi { get; }
        int hareketMesafesi { get; }

        /// <summary>
        /// Cismi İstenilen Yönde Hareket Ettirir
        /// </summary>
        /// <param name="yon">Hangi Yöne Hareket Edilecek</param>
        /// <returns>Cisim Duvara Çarparsa true Döndürür</returns>
        bool HareketEttir(Yon yon);

    }
}