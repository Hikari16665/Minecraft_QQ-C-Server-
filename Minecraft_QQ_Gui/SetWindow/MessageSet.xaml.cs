﻿using Minecraft_QQ_Gui.Windows;

namespace Minecraft_QQ.SetWindow
{
    /// <summary>
    /// MessageSet.xaml 的交互逻辑
    /// </summary>
    public partial class MessageSet : Window
    {
        public Server Server { get; set; }
        public MessageSet(Server Server = null)
        {
            InitializeComponent();
            if (Server == null)
            {
                Server = new Server();
            }
            this.Server = Server;
            DataContext = this;
        }
        public Server Set()
        {
            ShowDialog();
            return Server;
        }
    }
}