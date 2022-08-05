﻿using Minecraft_QQ_Core;
using Minecraft_QQ_Core.Config;
using Minecraft_QQ_Core.MySocket;
using Minecraft_QQ_Gui.SetWindow;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Minecraft_QQ_Gui
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isGet;
        private bool isSearch;
        private Server GetServer;

        public void AddLog(string logs)
        {
            Dispatcher.BeginInvoke(() =>
            {
                log.Text += logs + "\n";
                if (log.Text.Length > 100000)
                    log.Text = "";
            });
        }
        public void ServerConfig(string server, string config)
        {
            if (isGet)
                return;
            if (GetServer.Name != server)
                return;
            isGet = true;
            var temp = JsonConvert.DeserializeObject<ConfigOBJ>(config);
            Dispatcher.Invoke(() => temp = new ServerSet(temp).Set());
            var temp1 = new TranObj
            {
                command = DataType.set,
                message = JsonConvert.SerializeObject(temp, Formatting.Indented)
            };
            PluginServer.Send(temp1, new List<string>
            {
                server
            });
            isGet = false;
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                Click(null, null);
                e.Handled = true;
            }
        }
        private void InitQQList()
        {
            var list = Minecraft_QQ.GroupConfig.Groups.Values;
            QQList.Items.Clear();
            foreach (var item in list)
            {
                QQList.Items.Add(item);
            }
        }

        public void InitServerList()
        {
            ServerList.Items.Clear();
            if (!PluginServer.Start)
            {
                State.Content = "未就绪";
                SocketST.Content = "启动端口";
                Port.IsEnabled = true;
                return;
            }
            else if (PluginServer.Start && !PluginServer.IsReady())
            {
                State.Content = "等待连接";
                SocketST.Content = "关闭端口";
                Port.IsEnabled = false;
                return;
            }
            else if (PluginServer.IsReady())
            {
                State.Content = "运行中";
                SocketST.Content = "关闭端口";
                 Port.IsEnabled = false;
            }
            foreach (var item in PluginServer.MCServers)
            {
                ServerList.Items.Add(new Server
                {
                    Name = item.Key,
                    Addr = item.Value.Channel.RemoteAddress.ToString()
                });
            }
        }
        private void InitMessageList()
        {
            MessageList.Items.Clear();
            foreach (var item in Minecraft_QQ.AskConfig.AskList)
            {
                MessageList.Items.Add(new Server
                {
                    Name = item.Key,
                    Addr = item.Value
                });
            }
        }
        public void InitPlayerList()
        {
            var list = Minecraft_QQ.PlayerConfig.PlayerList.Values;
            if (Minecraft_QQ.PlayerConfig.PlayerList == null)
            {
                new MessageWindow("数据错误，请检查Mysql数据库是否连接，检查后重启");
                return;
            }
            PlayerList.Items.Clear();
            foreach (var item in list)
            {
                PlayerList.Items.Add(item);
            }
            var list1 = Minecraft_QQ.PlayerConfig.NotBindList;
            NoIDList.Items.Clear();
            foreach (var item in list1)
            {
                NoIDList.Items.Add(item);
            }
            list1 = Minecraft_QQ.PlayerConfig.MuteList;
            MuteList.Items.Clear();
            foreach (var item in list1)
            {
                MuteList.Items.Add(item);
            }
        }
        private void InitCommandList()
        {
            var list = Minecraft_QQ.CommandConfig.CommandList;
            CommandList.Items.Clear();
            foreach (var item in list)
            {
                string data = "";
                foreach (var temp in item.Value.Servers)
                {
                    data += temp;
                    data += ',';
                }
                if (!string.IsNullOrWhiteSpace(data))
                    data = data[0..^1];
                CommandList.Items.Add(new CommandOBJ
                {
                    Check = item.Key,
                    Command = item.Value.Command,
                    Use = item.Value.PlayerUse,
                    Send = item.Value.PlayerSend,
                    Server = data,
                    ServerS = item.Value.Servers
                });
            }
        }
        private void InitMysql()
        {
            MysqlPassword.Password = Minecraft_QQ.MainConfig.Database.Password;
            if (Minecraft_QQ.MysqlOK)
            {
                MysqlState.Content = "已连接";
                MysqlConnect.Content = "断开";
                MysqlIP.IsEnabled = MysqlPort.IsEnabled = MysqlUser.IsEnabled =
                    MysqlPassword.IsEnabled = MysqlDataBase.IsEnabled = false;
            }
            else
            {
                MysqlState.Content = "未连接";
                MysqlConnect.Content = "连接";
                MysqlIP.IsEnabled = MysqlPort.IsEnabled = MysqlUser.IsEnabled =
                    MysqlPassword.IsEnabled = MysqlDataBase.IsEnabled = true;
            }
        }

        private void ChangeQQ(object sender, RoutedEventArgs e)
        {
            if (QQList.SelectedItems.Count == 0)
                return;
            var olditem = (GroupObj)QQList.SelectedItem;
            var item = olditem.Copy();
            item = new QQSet(item).Set();
            long group;
            if (string.IsNullOrWhiteSpace(item.Group))
            {
                return;
            }
            else if (!long.TryParse(item.Group, out group))
            {
                new MessageWindow("请检查你修改后的群号", "修改失败");
                return;
            }
            Minecraft_QQ.GroupConfig.Groups.Remove(long.Parse(olditem.Group));
            Minecraft_QQ.GroupConfig.Groups.Add(group, item);
            InitQQList();
        }

        private void DeleteQQ(object sender, RoutedEventArgs e)
        {
            if (QQList.SelectedItems.Count == 0)
                return;
            foreach (var item in QQList.SelectedItems)
            {
                var temp = (GroupObj)item;
                long.TryParse(temp.Group, out long group);
                Minecraft_QQ.GroupConfig.Groups.Remove(group);
            }
            InitQQList();
        }

        private void AddQQ(object sender, RoutedEventArgs e)
        {
            var item = new QQSet().Set();
            if (string.IsNullOrWhiteSpace(item.Group))
            {
                return;
            }
            if (!long.TryParse(item.Group, out long group))
            {
                new MessageWindow("群号错误", "添加失败");
                return;
            }
            if (Minecraft_QQ.GroupConfig.Groups.ContainsKey(group))
            {
                Minecraft_QQ.GroupConfig.Groups.Remove(group);
            }
            Minecraft_QQ.GroupConfig.Groups.Add(group, item);
            InitQQList();
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Port.Text, out int port))
            {
                new MessageWindow("端口设置不为数字", "保存失败");
                return;
            }
            else if (port < 0 || port > 65535)
            {
                new MessageWindow("端口设置范围超出", "保存失败");
                return;
            }
            ConfigWrite.All();
            new MessageWindow("配置已经保存", "已保存");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Minecraft_QQ.Reload();
            new MessageWindow("配置文件已重载", "已重读");
            DataContext = Minecraft_QQ.MainConfig;
        }

        private void SocketST_Click(object sender, RoutedEventArgs e)
        {
            if (PluginServer.Start)
            {
                PluginServer.ServerStop();
                 Port.IsEnabled = true;
                SocketST.Content = "启动端口";
            }
            else
            {
                PluginServer.StartServer();
                Port.IsEnabled = false;
                SocketST.Content = "关闭端口";
            }
            InitServerList();
        }
        private void SocketE(object sender, RoutedEventArgs e)
        {
            if (ServerList.SelectedItem == null)
                return;
            GetServer = (Server)ServerList.SelectedItem;
            PluginServer.Send(new TranObj
            {
                command = DataType.config
            },
            new List<string>
            {
                GetServer.Name
            });
        }
        private void SocketD(object sender, RoutedEventArgs e)
        {
            foreach (var item in ServerList.SelectedItems)
            {
                var temp = (Server)item;
                PluginServer.Close(temp.Name);
            }
            InitServerList();
        }

        private void PlayerD(object sender, RoutedEventArgs e)
        {
            foreach (var item in PlayerList.SelectedItems)
            {
                var temp = (PlayerObj)item;
                Minecraft_QQ.PlayerConfig.PlayerList.Remove(temp.QQ);
            }
            InitPlayerList();
        }
        private void PlayerC(object sender, RoutedEventArgs e)
        {
            if (PlayerList.SelectedItem == null)
                return;
            var item = (PlayerObj)PlayerList.SelectedItem;
            long olditem = item.QQ;
            item = new PlayerSet(item).Set();
            if (item.QQ == 0 || item.QQ < 0)
            {
                new MessageWindow("请检查你写的QQ号", "修改失败");
                return;
            }
            Minecraft_QQ.PlayerConfig.PlayerList.Remove(olditem);
            Minecraft_QQ.PlayerConfig.PlayerList.Add(item.QQ, item);
            InitPlayerList();
        }
        private void PlayerA(object sender, RoutedEventArgs e)
        {
            var item = new PlayerSet().Set();
            if (item.QQ == 0 || item.QQ < 0)
            {
                return;
            }
            Minecraft_QQ.PlayerConfig.PlayerList.Add(item.QQ, item);
            InitPlayerList();
        }

        private void NoIDD(object sender, RoutedEventArgs e)
        {
            foreach (var item in NoIDList.SelectedItems)
            {
                Minecraft_QQ.PlayerConfig.NotBindList.Remove((string)item);
            }
            InitPlayerList();
        }
        private void NoIDE(object sender, RoutedEventArgs e)
        {
            if (NoIDList.SelectedItem != null)
            {
                var item = (string)NoIDList.SelectedItem;
                Minecraft_QQ.PlayerConfig.NotBindList.Remove(item);
                item = new IDSet(item).Set();
                Minecraft_QQ.PlayerConfig.NotBindList.Add(item);
            }
            InitPlayerList();
        }
        private void NoIDA(object sender, RoutedEventArgs e)
        {
            var item = new IDSet().Set();
            if (string.IsNullOrWhiteSpace(item))
            {
                return;
            }
            if (Minecraft_QQ.PlayerConfig.NotBindList.Contains(item))
            {
                return;
            }
            Minecraft_QQ.PlayerConfig.NotBindList.Add(item);
            InitPlayerList();
        }

        private void MuteDD(object sender, RoutedEventArgs e)
        {
            foreach (var item in MuteList.SelectedItems)
            {
                Minecraft_QQ.PlayerConfig.MuteList.Remove((string)item);
            }
            InitPlayerList();
        }
        private void MuteDE(object sender, RoutedEventArgs e)
        {
            if (MuteList.SelectedItem != null)
            {
                var item = (string)MuteList.SelectedItem;
                Minecraft_QQ.PlayerConfig.MuteList.Remove(item);
                item = new IDSet(item).Set();
                Minecraft_QQ.PlayerConfig.MuteList.Add(item);
            }
            InitPlayerList();
        }
        private void MuteDA(object sender, RoutedEventArgs e)
        {
            var item = new IDSet().Set();
            if (string.IsNullOrWhiteSpace(item))
            {
                return;
            }
            if (Minecraft_QQ.PlayerConfig.MuteList.Contains(item))
            {
                return;
            }
            Minecraft_QQ.PlayerConfig.MuteList.Add(item);
            InitPlayerList();
        }

        private void MessageD(object sender, RoutedEventArgs e)
        {
            foreach (var item in MessageList.SelectedItems)
            {
                var temp = (Server)item;
                Minecraft_QQ.AskConfig.AskList.Remove(temp.Name);
            }
            InitMessageList();
        }
        private void MessageC(object sender, RoutedEventArgs e)
        {
            if (MessageList.SelectedItem == null)
                return;
            var item = (Server)MessageList.SelectedItem;
            string olditem = item.Name;
            item = new MessageSet(item).Set();
            if (string.IsNullOrWhiteSpace(item.Name) ||
                string.IsNullOrWhiteSpace(item.Addr))
            {
                new MessageWindow("请检查你写内容", "修改失败");
                return;
            }
            Minecraft_QQ.AskConfig.AskList.Remove(olditem);
            Minecraft_QQ.AskConfig.AskList.Add(item.Name, item.Addr);
            InitMessageList();
        }
        private void MessageA(object sender, RoutedEventArgs e)
        {
            var item = new MessageSet().Set();
            if (string.IsNullOrWhiteSpace(item.Name) ||
               string.IsNullOrWhiteSpace(item.Addr))
            {
                return;
            }
            Minecraft_QQ.AskConfig.AskList.Add(item.Name, item.Addr);
            InitMessageList();
        }

        private void CommandD(object sender, RoutedEventArgs e)
        {
            foreach (var item in CommandList.SelectedItems)
            {
                var temp = (CommandOBJ)item;
                Minecraft_QQ.CommandConfig.CommandList.Remove(temp.Check);
            }
            InitCommandList();
        }
        private void CommandC(object sender, RoutedEventArgs e)
        {
            if (CommandList.SelectedItem == null)
                return;
            var item = (CommandOBJ)CommandList.SelectedItem;
            string olditem = item.Check;
            item = new CommandSet(item).Set();
            if (string.IsNullOrWhiteSpace(item.Check) ||
                string.IsNullOrWhiteSpace(item.Command))
            {
                new MessageWindow("请检查你写内容", "修改失败");
                return;
            }
            Minecraft_QQ.CommandConfig.CommandList.Remove(olditem);
            Minecraft_QQ.CommandConfig.CommandList.Add(item.Check, new CommandObj
            {
                Command = item.Command,
                PlayerUse = item.Use,
                PlayerSend = item.Send,
                Servers = item.ServerS
            });
            InitCommandList();
        }
        private void CommandA(object sender, RoutedEventArgs e)
        {
            var item = new CommandSet().Set();
            if (string.IsNullOrWhiteSpace(item.Check) ||
                string.IsNullOrWhiteSpace(item.Command))
            {
                return;
            }
            Minecraft_QQ.CommandConfig.CommandList.Add(item.Check, new CommandObj
            {
                Command = item.Command,
                PlayerUse = item.Use,
                PlayerSend = item.Send,
                Servers = item.ServerS
            });
            InitCommandList();
        }

        private void MysqlPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Minecraft_QQ.MainConfig.Database.Password = MysqlPassword.Password;
        }

        private void MysqlConnect_Click(object sender, RoutedEventArgs e)
        {
            if (Minecraft_QQ.MysqlOK)
                MyMysql.MysqlStop();
            else
                MyMysql.MysqlStart();
            InitMysql();
        }

        private void Turnto(PlayerObj obj)
        {
            Dispatcher.Invoke(() =>
            {
                PlayerList.SelectedItem = obj;
                PlayerList.ScrollIntoView(obj);
            });
        }

        private async void SearchQQ_(object sender, RoutedEventArgs e)
        {
            if (isSearch)
                return;
            var set = new PlayerSet(null).Set();
            bool ok = !string.IsNullOrWhiteSpace(set.Name)
                || !string.IsNullOrWhiteSpace(set.Nick)
                || set.QQ != 0;
            if (!ok)
            {
                new MessageWindow("请输入要搜索的内容", "选择内容空");
                return;
            }
            bool haveName = !string.IsNullOrWhiteSpace(set.Name);
            bool haveNick = !string.IsNullOrWhiteSpace(set.Nick);
            isSearch = true;
            await Task.Run(() =>
            {
                foreach (var item in Minecraft_QQ.PlayerConfig.PlayerList)
                {
                    if (item.Key == set.QQ)
                    {
                        Turnto(item.Value);
                        return;
                    }
                    else if (haveName && !string.IsNullOrWhiteSpace(item.Value.Name))
                    {
                        if (item.Value.Name.Contains(set.Name) || set.Name.Contains(item.Value.Name))
                        {
                            Turnto(item.Value);
                            return;
                        }
                    }
                    else if (haveNick && !string.IsNullOrWhiteSpace(item.Value.Nick))
                    {
                        if (item.Value.Nick.Contains(set.Nick) || set.Nick.Contains(item.Value.Nick))
                        {
                            Turnto(item.Value);
                            return;
                        }
                    }
                }
                new MessageWindow("你搜索的内容找不到", "找不到内容");
                isSearch = false;
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var res = new SelectWindow("你确定要关闭Minecraft_QQ吗?", "关闭窗口").Set();
            if (res)
            {
                App.Stop();
            }
            else
                e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitQQList();
            InitServerList();
            InitPlayerList();
            InitMessageList();
            InitCommandList();
            InitMysql();
            DataContext = Minecraft_QQ.MainConfig;
            App.MainWindow_ = this;
            App.CloseWin();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            log.Text = "";
        }
    }
}
