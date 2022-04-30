/*
 * B211200300
 * Muhammet İkbal Yakupoğlu
 * Bilişim Sistemleri Mühendisliği
 */

using System;
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


            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (StreamWriter w = File.AppendText(path + @"\topfive.txt"))
            {

            }

            string[] lines = File.ReadAllLines(path + @"\topfive.txt");
            string[,] result = new string[5,4];


            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('◘');
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = values[j];
                }
            }

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    dataGridView1[j, i].Value = result[i, j];
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
