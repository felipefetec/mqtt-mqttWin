using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading;

namespace WindowsFormsmqtt2
{
    public partial class Form1 : Form
    {
        MqttClient clientSub;
        delegate void SetTextCallback(string text);
        String HostIP;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
            //    String HostIP;
              //  HostIP = TextBox1.Text;
             //   clientSub = new MqttClient(HostIP);
             //   clientSub.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(EventPublished);
            }
            catch (InvalidCastException ex)
            {
            }
         }

        private void EventPublished(Object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            try
            {
                if (e.Topic.Equals("arduino/sensor/temp/c"))
                {
                    SetValue(System.Text.UTF8Encoding.UTF8.GetString(e.Message));
                }
                if (e.Topic.Equals("arduino/sensor/temp/f"))
                {
                    SetValue2(System.Text.UTF8Encoding.UTF8.GetString(e.Message));
                }
                if (e.Topic.Equals("arduino/sensor/umidade"))
                {
                    SetValue3(System.Text.UTF8Encoding.UTF8.GetString(e.Message));
                }

                SetText("Mensagem Recebida");
                SetText("Topico: " + e.Topic);
                SetText("Mensagem: " + System.Text.UTF8Encoding.UTF8.GetString(e.Message));
                SetText("");
            }
            catch (InvalidCastException ex)
            {
            }
        }

        private void SetValue(string text)
        {
            if (this.ListBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetValue);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lbTempC.Text = text + " Cº";
            }
        }
        private void SetValue2(string text)
        {
            if (this.ListBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetValue2);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lbTempF.Text = text + " Fº";
            }
        }
        private void SetValue3(string text)
        {
            if (this.ListBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetValue3);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                lbUmid.Text = text + " %";
            }
        }

        private void SetText(string text)
        {
            if (this.ListBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.ListBox1.Items.Add(text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               
                HostIP = TextBox1.Text;
                clientSub = new MqttClient(HostIP);
                clientSub.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(EventPublished);

                clientSub.Connect(TextBox2.Text + "_sub", txtUser.Text, txtSenha.Text);
                ListBox1.Items.Add("Cliente connectado");
                if(TextBox3.Text != "") clientSub.Subscribe(new string[] { TextBox3.Text }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                clientSub.Subscribe(new string[] {"arduino/sensor/temp/c"}, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                clientSub.Subscribe(new string[] {"arduino/sensor/temp/f"}, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                clientSub.Subscribe(new string[] {"arduino/sensor/umidade"}, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
                ListBox1.Items.Add("Subscribe: " + TextBox3.Text);
            }
            catch (InvalidCastException ex)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                clientSub.Publish(TextBox3.Text, Encoding.UTF8.GetBytes(TextBox4.Text), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
                ListBox1.Items.Add("Publicando: " + TextBox3.Text);
            }
            catch (InvalidCastException ex)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
            try
            {
                clientSub.Disconnect();
                ListBox1.Items.Add("* Client disconnected");
                //Application.Exit();
            }
            catch (InvalidCastException ex)
            {
            }
        }
    }
}
