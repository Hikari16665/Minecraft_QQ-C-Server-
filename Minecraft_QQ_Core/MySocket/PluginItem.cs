﻿using System;
using System.Threading.Tasks;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using Minecraft_QQ_Core.Robot;
using Minecraft_QQ_Core.Utils;
using OneBotSharp.Objs.Message;

namespace Minecraft_QQ_Core.MySocket;

public class PluginItem(IChannel client)
{
    public string Name { get; private set; }
    public IChannel Channel { get; private set; } = client;
    private bool IsSameStop = false;

    public void Stop()
    {
        Channel.DisconnectAsync();
        Channel.CloseAsync();
    }
    public void Read(IByteBuffer pack)
    {
        try
        {
            int type = pack.ReadInt();
            if (type == 120)
            {
                return;
            }
            else if (type == 0)
            {
                Name = pack.ReadString();
                if (string.IsNullOrWhiteSpace(Name))
                {
                    Channel.CloseAsync();
                    return;
                }
                if (Minecraft_QQ.Config.Setting.SendLog)
                {
                    RobotCore.SendGroupMessage(Minecraft_QQ.MainGroup,
                        [MsgText.Build($"[Minecraft_QQ]服务器{Name}已连接")]);
                }
                Logs.LogOut($"[Socket]服务器{Name}已连接");
                IMinecraft_QQ.GuiCall?.Invoke(GuiCallType.ServerList);
                if (Minecraft_QQ.Config.Setting.SendLog)
                {
                    RobotCore.SendGroupMessage(Minecraft_QQ.MainGroup,
                        [MsgText.Build("[Minecraft_QQ]服务器已连接")]);
                }
                PluginServer.AddServer(Name, this);
                IMinecraft_QQ.GuiCall?.Invoke(GuiCallType.ServerList);
            }
            else
                Task.Run(() => Message.MessageDo(Name, pack));
        }
        catch (Exception e)
        {
            if (Minecraft_QQ.Config.Setting.SendLog)
                RobotCore.SendGroupMessage(Minecraft_QQ.MainGroup,
                    [MsgText.Build($"[Minecraft_QQ]服务器{Name}异常断开")]);
            Logs.LogError(e);
            Stop();
            if (!IsSameStop)
            {
                Channel.CloseAsync();
                IMinecraft_QQ.GuiCall?.Invoke(GuiCallType.ServerList);
            }
            return;
        }
    }
    /// <summary>
    /// 关闭上一个服务器
    /// </summary>
    public void StopSame()
    {
        IsSameStop = true;
    }
}
