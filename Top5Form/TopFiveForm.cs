/*
 * B211200300
 * Muhammet İkbal Yakupoğlu
 * Bilişim Sistemleri Mühendisliği
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Top5Form
{
    public partial class Top5Form : Form
    {
        public Top5Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;

            dataGridView1.ColumnCount = 4;
            dataGridView1.RowCount = 5;

            var result = EnİyiSkorlariHesapla();

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    dataGridView1[j, i].Value = result[i, j];
                }
            }
        }


        private string[,] EnİyiSkorlariHesapla()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter w = File.AppendText(path + @"\topfive.txt"))
            {

            }

            string[] lines = File.ReadAllLines(path + @"\topfive.txt");

            List<string> stringSkorlar = new List<string>();
            List<int> skorlar = new List<int>();


            SatirlarinSkorlariniListeyeAktar(lines, stringSkorlar, skorlar);

            SkorlariSirala(skorlar, lines);

            var topFive = SiralananDegerleriDiziyeAta(lines);

            return topFive;
        }

        private static string[,] SiralananDegerleriDiziyeAta(string[] lines)
        {
            int sayac = 0;
            string[] siralanmisValues = new string[lines.Length * 4];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tempArray = lines[i].Split('◘');
                for (int j = 0; j < tempArray.Length; j++)
                {
                    siralanmisValues[sayac] = tempArray[j];
                    sayac++;
                }
            }

            string[,] topFive = new string[5, 4];
            sayac = 0;

            for (int i = 0; i < topFive.GetLength(0); i++)
            {
                for (int j = 0; j < topFive.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        topFive[i, j] = (i + 1).ToString();
                    }
                    else
                    {
                        if (siralanmisValues.Length >= sayac)
                        {
                            topFive[i, j] = siralanmisValues[sayac];
                        }
                    }

                    sayac++;
                }
            }

            return topFive;
        }

        private static void SatirlarinSkorlariniListeyeAktar(string[] lines, List<string> stringSkorlar, List<int> skorlar)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('◘');
                stringSkorlar.Add(values[3]);
            }

            foreach (var stringSkor in stringSkorlar)
            {
                skorlar.Add(int.Parse(stringSkor));
            }
        }


        private void SkorlariSirala(List<int> skorlar, string[] lines)
        {

            for (int i = 0; i < skorlar.Count - 1; i++)
            {
                for (int j = 0; j < skorlar.Count - i - 1; j++)
                {
                    if (skorlar[j] < skorlar[j + 1])
                    {
                        int temp = skorlar[j];
                        string tempLines = lines[j];

                        skorlar[j] = skorlar[j + 1];
                        lines[j] = lines[j + 1];

                        skorlar[j + 1] = temp;
                        lines[j + 1] = tempLines;
                    }
                }
            }
        }
    }
}
