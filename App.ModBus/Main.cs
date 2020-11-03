using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FtdAdapter;
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace App.ModBus
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

            cbb_IP.Text = "COM1";
            comboBox2.Text = ".txt";
            // Start dos Timers
            timer1.Start();

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            maskedTextBox1.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            ModbusSerialRtuMasterReadRegisters();
            timer3.Interval = Convert.ToInt32(nud_Exibi.Value * 1000);

            try
            {
                double a;
                a = Convert.ToDouble(textBox1.Text);
                textBox1.Text = (a / Math.Pow(10, Convert.ToDouble(dp_1.Value))).ToString();

                a = Convert.ToDouble(textBox2.Text);
                textBox2.Text = (a / Math.Pow(10, Convert.ToDouble(dp_2.Value))).ToString();

                a = Convert.ToDouble(textBox3.Text);
                textBox3.Text = (a / Math.Pow(10, Convert.ToDouble(dp_3.Value))).ToString();

                a = Convert.ToDouble(textBox4.Text);
                textBox4.Text = (a / Math.Pow(10, Convert.ToDouble(dp_4.Value))).ToString();

                a = Convert.ToDouble(textBox5.Text);
                textBox5.Text = (a / Math.Pow(10, Convert.ToDouble(dp_5.Value))).ToString();

                a = Convert.ToDouble(textBox6.Text);
                textBox6.Text = (a / Math.Pow(10, Convert.ToDouble(dp_6.Value))).ToString();

                a = Convert.ToDouble(textBox7.Text);
                textBox7.Text = (a / Math.Pow(10, Convert.ToDouble(dp_7.Value))).ToString();

                a = Convert.ToDouble(textBox8.Text);
                textBox8.Text = (a / Math.Pow(10, Convert.ToDouble(dp_8.Value))).ToString();

            }
            catch
            { }

        }

        public void ModbusSerialRtuMasterReadRegisters()
        {

            using (SerialPort port = new SerialPort(cbb_IP.Text))
            {
                if (checkLeitura.Checked == true)
                {
                    try
                    {
                        // configure serial port
                        port.BaudRate = 9600;
                        port.DataBits = 8;
                        port.Parity = Parity.None;
                        port.StopBits = StopBits.One;
                        port.Open();

                        // create modbus master
                        IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(port);
                        master.Transport.ReadTimeout = 550;

                        byte slaveId = 1;
                        ushort startAddress = 0;
                        ushort numRegisters = 8;

                        // read five registers		
                        ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                        textBox1.Text = registers[0].ToString();
                        textBox2.Text = registers[1].ToString();
                        textBox3.Text = registers[2].ToString();
                        textBox4.Text = registers[3].ToString();
                        textBox5.Text = registers[4].ToString();
                        textBox6.Text = registers[5].ToString();
                        textBox7.Text = registers[6].ToString();
                        textBox8.Text = registers[7].ToString();

                        txt_Status.Text = "Status: Leitura On";

                        checkBox1.Visible = true;

                    }
                    catch
                    {
                        timer1.Stop();
                        txt_Status.Text = "Status: Leitura Off";
                        MessageBox.Show(" Erro \n Verifique a Porta");
                        checkLeitura.Checked = false;
                        timer1.Start();
                    }
                }
                else
                {
                    txt_Status.Text = "Status: Leitura Off";
                    checkBox1.Visible = false;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Local de Salvamento";
            folderBrowserDialog1.ShowNewFolderButton = true;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_local.Text = folderBrowserDialog1.SelectedPath;
            }

        }

        private void btn_Salvar_Click(object sender, EventArgs e)
        {
            try
            {
                string arquivo = txt_local.Text + "\\" + txt_Arquivo.Text + comboBox2.Text;

                string nomeArquivo = @arquivo;

                StreamWriter writer = new StreamWriter(nomeArquivo);

                writer.Close();

                timer2.Interval = Convert.ToInt32(txtTempo.Text) * 1000;
                timer2.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Erro ao Salvar");
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                timer2.Start();
                string arquivo = txt_local.Text + "\\" + txt_Arquivo.Text + comboBox2.Text;
                string linhadata = DateTime.Now.ToString("dd/MM/yyyy");
                string linhahora = DateTime.Now.ToString("HH:mm:ss");

                label6.Text = "Último salvamento - " + linhadata + " " + linhahora;

                string valores = textBox1.Text + "@" + textBox2.Text + "@" + textBox3.Text + "@" + textBox4.Text +
                    "@" + textBox5.Text + "@" + textBox6.Text + "@" + textBox7.Text + "@" + textBox8.Text;

                StreamWriter s = File.AppendText(@arquivo);
                s.WriteLine(linhadata + "@" + linhahora + "@" + valores.ToString());


                Application.DoEvents();
                s.Close();
                Thread.Sleep(100);

                if (checkLeitura.Checked == false)
                {
                    timer2.Enabled = false;
                }

                Application.DoEvents();
            }
            catch
            {

            }

        }

        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            string arquivo = txt_local.Text;
            System.Diagnostics.Process.Start("explorer.exe", arquivo);
        }

        private void btn_PararSave_Click(object sender, EventArgs e)
        {
            timer2.Stop();
        }

        private void btn_Graph_Click(object sender, EventArgs e)
        {
            btn_Graph.Enabled = false;
            Graph tela = new Graph();
            tela.ShowDialog();
            btn_Graph.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
             
            
        }


        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {  
                chart1.Visible = true;
                panel1.Visible = true;
                this.Size = new Size(690 + 610, 721);
                chart1.Series.Clear();

                for (int k = 0; k < 8; k++)
                {
                    this.chart1.Series.Add("Ponto " + (k+1));
                    chart1.Series[k].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                }
                
                timer3.Start();
            }
            else
            {
                chart1.Visible = false;
                panel1.Visible = false;
                this.Size = new Size(690, 721);
                timer3.Stop();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

            chart1.Series[0].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), Convert.ToDouble(textBox1.Text));
            
            chart1.Series[1].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), Convert.ToDouble(textBox2.Text));

            chart1.Series[2].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), Convert.ToDouble(textBox3.Text));

            chart1.Series[3].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), Convert.ToDouble(textBox4.Text));

            chart1.Series[4].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), Convert.ToDouble(textBox5.Text));

            chart1.Series[5].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), Convert.ToDouble(textBox6.Text));

            chart1.Series[6].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), Convert.ToDouble(textBox7.Text));

            chart1.Series[7].Points.AddXY(DateTime.Now.ToString("HH:mm:ss"), Convert.ToDouble(textBox8.Text));
            
            chart1.Show();
            chart1.DataBind();
            chart1.Update();   

        }

    }

}
