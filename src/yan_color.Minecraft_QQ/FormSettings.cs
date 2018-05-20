﻿using System;
using System.Windows.Forms;

namespace yan_color.Minecraft_QQ
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            //加载标题。
            Text = System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name + "参数设置";
        }

        /// <summary>
        /// 退出按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            int tmp;
            LinqXML.write(Minecraft_QQ.config, "群号1", textBox1.Text);
            Minecraft_QQ.GroupSet1 = long.Parse(textBox1.Text);
            LinqXML.write(Minecraft_QQ.config, "群号2", textBox3.Text);
            Minecraft_QQ.GroupSet2 = long.Parse(textBox3.Text);
            LinqXML.write(Minecraft_QQ.config, "IP", textBox4.Text);
            LinqXML.write(Minecraft_QQ.config, "Port", textBox5.Text);
            LinqXML.write(Minecraft_QQ.config, "群号3", textBox6.Text);
            Minecraft_QQ.GroupSet3 = long.Parse(textBox6.Text);
            if (!int.TryParse(textBox5.Text, out tmp))
            {
                MessageBox.Show("请输入端口数字");
                return;
            }
            Minecraft_QQ.Port = int.Parse(textBox5.Text);
            Minecraft_QQ.ipaddress = textBox4.Text;
            Close();
        }

        /// <summary>
        /// 保存按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //参数保存处理代码。

            this.btnExit_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LinqXML.write(Minecraft_QQ.config, "群号1", textBox1.Text);
            Minecraft_QQ.GroupSet1 = long.Parse(textBox1.Text);
            button1.Text = "设置成功";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LinqXML.write(Minecraft_QQ.admin, textBox2.Text, "admin");
            button2.Text = "添加成功";
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            textBox1.Text = LinqXML.read(Minecraft_QQ.config, "群号1");
            textBox3.Text = LinqXML.read(Minecraft_QQ.config, "群号2");
            textBox6.Text = LinqXML.read(Minecraft_QQ.config, "群号3");

            textBox4.Text = LinqXML.read(Minecraft_QQ.config, "IP");
            textBox5.Text = LinqXML.read(Minecraft_QQ.config, "Port");
            if (LinqXML.read(Minecraft_QQ.config, "编码") == "UTF-8")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else if (LinqXML.read(Minecraft_QQ.config, "编码") == "ANSI（GBK）")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            if (LinqXML.read(Minecraft_QQ.config, "发送消息") == "当然！")
            {
                checkBox1.Checked = true;
            }
            else if (LinqXML.read(Minecraft_QQ.config, "发送消息") == "不！")
            {
                checkBox1.Checked = false;
            }
            if (LinqXML.read(Minecraft_QQ.config, "维护模式") == "关")
            {
                checkBox2.Checked = false;
                checkBox2.Text = "服务器维护模式：关";
            }
            else if (LinqXML.read(Minecraft_QQ.config, "维护模式") == "开")
            {
                checkBox2.Checked = true;
                checkBox2.Text = "服务器维护模式：开";
            }
            if (LinqXML.read(Minecraft_QQ.config, "群2发送消息") == "开")
                checkBox4.Checked = true;
            else
                checkBox4.Checked = false;
            if (LinqXML.read(Minecraft_QQ.config, "群3发送消息") == "开")
                checkBox5.Checked = true;
            else
                checkBox5.Checked = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            int tmp;
            LinqXML.write(Minecraft_QQ.config, "IP", textBox4.Text);
            LinqXML.write(Minecraft_QQ.config, "Port", textBox5.Text);
            if (!int.TryParse(textBox5.Text, out tmp))
            {
                MessageBox.Show("请正确输入数字");
                return;
            }
            Minecraft_QQ.Port = int.Parse(textBox5.Text);
            Minecraft_QQ.ipaddress = textBox4.Text;
            button.Text = "已设置";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "正在关闭";
            //socket.stop_socket();
            button3.Text = "已关闭";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LinqXML.write(Minecraft_QQ.config, "群号2", textBox3.Text);
            Minecraft_QQ.GroupSet2 = long.Parse(textBox3.Text);
            button4.Text = "设置成功";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LinqXML.write(Minecraft_QQ.config, "群号3", textBox6.Text);
            Minecraft_QQ.GroupSet3 = long.Parse(textBox6.Text);
            button5.Text = "设置成功";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            LinqXML.write(Minecraft_QQ.config, "编码", "UTF-8");
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            LinqXML.write(Minecraft_QQ.config, "编码", "ANSI（GBK）");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true) LinqXML.write(Minecraft_QQ.config, "发送消息", "当然！");
            else LinqXML.write(Minecraft_QQ.config, "发送消息", "不！");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                LinqXML.write(Minecraft_QQ.config, "维护模式", "开");
                Minecraft_QQ.server = true;
                checkBox2.Text = "服务器维护模式：开";
            }
            else
            {
                LinqXML.write(Minecraft_QQ.config, "维护模式", "关");
                Minecraft_QQ.server = false;
                checkBox2.Text = "服务器维护模式：关";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                LinqXML.write(Minecraft_QQ.config, "群2发送消息", "开");
                Minecraft_QQ.Group2_on = true;
            }
            else
            {
                LinqXML.write(Minecraft_QQ.config, "群2发送消息", "关");
                Minecraft_QQ.Group2_on = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                LinqXML.write(Minecraft_QQ.config, "群3发送消息", "开");
                Minecraft_QQ.Group3_on = true;
            }
            else
            {
                LinqXML.write(Minecraft_QQ.config, "群3发送消息", "关");
                Minecraft_QQ.Group3_on = false;
            }
        }
    }
}
