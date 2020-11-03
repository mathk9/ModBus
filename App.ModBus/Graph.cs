using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;

namespace App.ModBus
{
    public partial class Graph : Form
    {
        public Graph()
        {
            InitializeComponent();

        }

        DialogResult dr;

        private void btn_Buscar_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            dr = this.openFileDialog1.ShowDialog();
            timer1.Start();
            btn_Pausar.Visible = true;
            btn_Pausar.Enabled = true;
            if (dr == DialogResult.OK)
            {
                dataGridView1.Visible = true;
                chart1.Visible = true;
                label1.Text = openFileDialog1.FileName.ToString();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dr == DialogResult.OK)
            {
                string[] filelines = File.ReadAllLines(openFileDialog1.FileName);

                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                chart1.Series.Clear();
                //chart1.Legends.Clear();

                dataGridView1.Columns.Add("Data", "Data");
                dataGridView1.Columns.Add("Hora", "Hora");
                dataGridView1.Columns.Add("Ponto0", "Ponto 1");
                dataGridView1.Columns.Add("Ponto1", "Ponto 2");
                dataGridView1.Columns.Add("Ponto2", "Ponto 3");
                dataGridView1.Columns.Add("Ponto3", "Ponto 4");
                dataGridView1.Columns.Add("Ponto4", "Ponto 5");
                dataGridView1.Columns.Add("Ponto5", "Ponto 6");
                dataGridView1.Columns.Add("Ponto6", "Ponto 7");
                dataGridView1.Columns.Add("Ponto7", "Ponto 8");

                for (int i = 0; i < filelines.Length; i++)
                {
                    dataGridView1.Rows.Add(filelines[i].Split('@'));
                }


                for (int k = 0; k < 9; k++)
                {
                    this.chart1.Series.Add("Ponto " + (k + 1));
                    chart1.Series[k].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                }

            }
            else
            {
                MessageBox.Show("Erro na Leitura do Arquivo !");
                return;
            }

            if (dataGridView1.Rows.Count > 1 && dataGridView1.Columns.Count > 1)
            {
                chart1.DataSource = dataGridView1.DataSource;

                Series serie = new Series();

                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    chart1.Series[0].Points.AddXY(DateTime.ParseExact(dataGridView1.Rows[k].Cells[0].Value + " " + dataGridView1.Rows[k].Cells[1].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Convert.ToDouble(dataGridView1.Rows[k].Cells[2].Value));
                }
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    chart1.Series[1].Points.AddXY(DateTime.ParseExact(dataGridView1.Rows[k].Cells[0].Value + " " + dataGridView1.Rows[k].Cells[1].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Convert.ToDouble(dataGridView1.Rows[k].Cells[3].Value));
                }
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    chart1.Series[2].Points.AddXY(DateTime.ParseExact(dataGridView1.Rows[k].Cells[0].Value + " " + dataGridView1.Rows[k].Cells[1].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Convert.ToDouble(dataGridView1.Rows[k].Cells[4].Value));
                }
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    chart1.Series[3].Points.AddXY(DateTime.ParseExact(dataGridView1.Rows[k].Cells[0].Value + " " + dataGridView1.Rows[k].Cells[1].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Convert.ToDouble(dataGridView1.Rows[k].Cells[5].Value));
                }
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    chart1.Series[4].Points.AddXY(DateTime.ParseExact(dataGridView1.Rows[k].Cells[0].Value + " " + dataGridView1.Rows[k].Cells[1].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Convert.ToDouble(dataGridView1.Rows[k].Cells[6].Value));
                }
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    chart1.Series[5].Points.AddXY(DateTime.ParseExact(dataGridView1.Rows[k].Cells[0].Value + " " + dataGridView1.Rows[k].Cells[1].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Convert.ToDouble(dataGridView1.Rows[k].Cells[7].Value));
                }
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    chart1.Series[6].Points.AddXY(DateTime.ParseExact(dataGridView1.Rows[k].Cells[0].Value + " " + dataGridView1.Rows[k].Cells[1].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Convert.ToDouble(dataGridView1.Rows[k].Cells[8].Value));
                }
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    chart1.Series[7].Points.AddXY(DateTime.ParseExact(dataGridView1.Rows[k].Cells[0].Value + " " + dataGridView1.Rows[k].Cells[1].Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture), Convert.ToDouble(dataGridView1.Rows[k].Cells[9].Value));
                }

                chart1.Show();
                chart1.DataBind();
                chart1.Update();
            }

        }

        private void btn_Pausar_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            btn_Pausar.Visible = false;
            btn_Pausar.Enabled = false;

            button1.Enabled = true;
            button1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            btn_Pausar.Visible = true;
            btn_Pausar.Enabled = true;

            button1.Enabled = false;
            button1.Visible = false;

        }
    }
}
