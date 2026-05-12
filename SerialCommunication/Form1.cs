using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {

            try
            {
                if (serialPortArduino.IsOpen)
                {
                    // ik heb een verbinding -> de gebruiker wil deze verbreken
                    serialPortArduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Status: Not connected";


                }
                else
                {
                    // ik heb geen verbinding -> de gebruiker wil een verbinding maken
                    serialPortArduino.PortName = (string) comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = Int32.Parse ((string) comboBoxBaudrate.SelectedItem);
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortArduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortArduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPortArduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPortArduino.StopBits = StopBits.Two;

                    if (radioButtonHandshakeNone.Checked) serialPortArduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortArduino.Handshake = Handshake.XOnXOff;

                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;


                    serialPortArduino.Open();
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "Disconnect";
                        labelStatus.Text = "Status: Connected";
                    }
                    else
                    {
                        serialPortArduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";
                    }
                }

            }
            catch (Exception  exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close() ;
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ( serialPortArduino.IsOpen)
                {
                    string commando; // set d2 high/low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPortArduino.WriteLine(commando);
                }

            }
            catch (Exception exception)
            {

                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set d3 high/low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPortArduino.WriteLine(commando);
                }

            }
            catch (Exception exception)
            {

                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando; // set d4 high/low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPortArduino.WriteLine(commando);
                }

            }
            catch (Exception exception)
            {

                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void timerOefening5_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                    return;

                serialPortArduino.ReadExisting();
                serialPortArduino.WriteLine("get a0");


                string antwoordA0 =
                     serialPortArduino.ReadLine();

                antwoordA0 = antwoordA0.Replace("a0: ", "");

                antwoordA0 = antwoordA0.Trim();

                int analoog0 =
                    int.Parse(antwoordA0);

                serialPortArduino.WriteLine("get a1");

                string antwoordA1 =
                    serialPortArduino.ReadLine();
                antwoordA1 = antwoordA1.Replace("a1:", "");
                antwoordA1 = antwoordA1.Trim();
                int analoog1 =
                    int.Parse(antwoordA1);
               
                //---------------------------------
                // Gewenste temperatuur
                //---------------------------------

                double ricoGewenst =
                    (45.0 - 5.0) / 1023.0;
                double gewensteTemp =
                    analoog0 * ricoGewenst + 5;

                labelGewensteTemp.Text =
                    gewensteTemp.ToString("F1") + " °C";

                //---------------------------------
                // Huidige temperatuur
                //---------------------------------

                double ricoHuidig =
                    500.0 / 1023.0;
                double huidigeTemp =
                    analoog1 * 5.0 * 100.0 / 1023.0;

                labelHuidigeTemp.Text =
                    huidigeTemp.ToString("F1") + " °C";

                //---------------------------------
                // LED sturen
                //---------------------------------

                if (huidigeTemp < gewensteTemp)
                {
                    serialPortArduino.WriteLine("set d2 1");
                }
                else
                {
                    serialPortArduino.WriteLine("set d2 0");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }



        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab ==tabPageOefening5)
            {
                timerOefening5.Start();
            }
            else
            {
                timerOefening5.Stop();
            }
        }
    }
    }
    


