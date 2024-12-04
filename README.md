# Minecraft服务器与QQ群聊天的插件  FORK
Minecraft_QQ 插件本体(Cmd/NewGui)

机器人使用[Onebot](https://onebot.dev/)v11协议的机器人即可   
例如[Lagrange](https://github.com/LagrangeDev/Lagrange.Core)  

[Minebbs](https://www.minebbs.com/resources/minecraft_qq.8250)

## 连接说明

链接顺序不要搞错了  
Minecraft_QQ->Minecraft_QQ_Cmd/NewGui->Onebot  
Minecraft_QQ->Minecraft_QQ_Cmd/NewGui->Onebot  
Minecraft_QQ->Minecraft_QQ_Cmd/NewGui->Onebot  

## 部署教程：
1. 下载

[Minecraft_QQ_Cmd/NewGui](https://github.com/Coloryr/Minecraft_QQ-C-Server-/actions)  
[Minecraft_QQ插件](https://github.com/Coloryr/Minecraft_QQ/actions)  
任意OneBot机器人

2. 启动

启动Onebot机器人，并登录账户  
开启正向WebSocket，绑定IP为0.0.0.0  
默认使用端口为8081

将`Minecraft_QQ_Cmd/Gui`放到一个文件夹，选择启动`Cmd`或者是`NewGui`  

Linux下使用
```
chmod a+x ./Minecraft_QQ_Cmd
./Minecraft_QQ_Cmd
```  
或者
```
chmod a+x ./Minecraft_QQ_NewGui
./Minecraft_QQ_NewGui
``` 

启动后进行[第一次使用配置](#第一次使用配置)  

将插件放进服务器的插件文件夹
重启服务器  
让服务器插件连接`Minecraft_QQ_Cmd/NewGui`

## 第一次使用配置

`NewGui`下，添加主群即可  
没有时会出现一个弹窗，输入群号后需要勾选主群  

`Cmd`下，打开`Group.json`调整配置
```json
{
    "Groups": {
        "123456789": {
            "Group": 123456789,
            "EnableCommand": true,
            "EnableSay": true,
            "IsMain": true
        }
    }
}
```
将`123456789`更换为你的群号

或者根据提示输入群号  

重启`Minecraft_QQ_Cmd/NewGui`

## 插件默认指令

普通用户指令：
- #在线人数        查看服务器在线人数
- #服务器状态      查看服务器当前状态
- #绑定：[ID]      绑定游戏ID
- #服务器：[内容]  向服务器内发送内容

需要绑定ID后才能向服务器发送内容  
若AutoSend处于开启状态，则`#服务器：`无效，且内容自动发送到服务器内

管理员指令：
- #禁言：[ID/@qq]      禁言某个ID的玩家发言
- #解禁：[ID/@qq]      解禁某个ID的玩家发言
- #查询：[@qq/qq号]    查询某个玩家的ID
- #修改：[@qq] [新ID]  修改某个玩家的ID
- #切换维护         
- #重读文件
- #昵称：[@qq] [昵称]  修改某个玩家的昵称
- #禁止绑定列表
- #禁言列表

## 配置文件说明

`MainConfig.json`主要配置文件
```json
{
    //主要配置
  "Setting": {
    //开启自动应答
    "AskEnable": true,
    //删除聊天中的颜色代码
    "ColorEnable": true,
    //维护模式
    "FixMode": false,
    //自动发送
    "AutoSend": true,
    //替换为昵称发送到服务器内
    "SendNickServer": true,
    //替换为昵称发送到群内
    "SendNickGroup": true,
    //允许绑定ID
    "CanBind": true,
    //发送日志到主群
    "SendLog": true,
    //发送玩家在群里打的指令到服务器
    "SendCommand": false,
    //发送延迟
    "SendDelay": 100,
    //绑定提示私聊给某人
    "SendQQ": 0,
    //机器人链接地址
    "BotUrl": "ws://localhost:8081/",
    "BotAuthorization": null
  },
  "Message": {
    "FixText": "服务器正在维护，请等待维护结束！",
    "UnknowText": "未知指令",
    "CantBindText": "绑定ID已关闭",
    "NoneBindID": "你没有绑定服务器ID，发送：#绑定：[ID]来绑定，如：\r\n绑定：Color_yr",
    "AlreadyBindID": "你已经绑定ID了，请找腐竹更改"
  },
  "Check": {
    //检测头
    "Head": "#",
    "PlayList": "在线人数",
    "ServerCheck": "服务器状态",
    "Bind": "绑定：",
    "Send": "服务器："
  },
  "Admin": {
    "Mute": "禁言：",
    "UnMute": "解禁：",
    "CheckBind": "查询：",
    "Rename": "修改：",
    "Fix": "切换维护",
    "Reload": "重读文件",
    "Nick": "昵称：",
    "GetCantBindList": "禁止绑定列表",
    "GetMuteList": "禁言列表",
    "NoInput": false
  },
  "Socket": {
    //开启端口，默认就好
    "Port": 25555,
    "Check": true
  },
  "Database": {
    "Url": "SslMode=none;Server=127.0.0.1;Port=3306;User ID=root;Password=123456;Database=minecraft;Charset=utf8;",
    "Enable": false
  }
}
```
检测头必须写，所有指令都需要检测头在最前面才能执行

`Ask.json`自动应答配置文件
```json
{
  "AskList": {
    "服务器菜单": "..........."
  }
}
```
可以自行添加，注意json格式就行
```json
{
  "AskList": {
    "a": "xxx",
   ......
    "b": "xxx"
  }
}
```

`Command.json`自定义命令配置文件
```json
{
  "CommandList": {
    "插件帮助": {
      "Command": "qq help",
      "PlayerUse": false,
      "PlayerSend": false,
      "Servers": []
    },
    "查钱": {
      "Command": "money {arg:name}",
      "PlayerUse": true,
      "PlayerSend": false,
      "Servers": []
    },
    "禁言": {
      "Command": "mute {arg:x}",
      "PlayerUse": false,
      "PlayerSend": false,
      "Servers": []
    },
    "传送": {
      "Command": "tpa {arg:at}",
      "PlayerUse": true,
      "PlayerSend": false,
      "Servers": []
    },
    "给权限": {
      "Command": "lp user {arg:at} permission set {arg:next} true",
      "PlayerUse": false,
      "PlayerSend": false,
      "Servers": []
    },
    "说话": {
      "Command": "say {arg:x}",
      "PlayerUse": false,
      "PlayerSend": false,
      "Servers": []
    }
  }
}
 ```
 命令可以自己添加，注意json格式
 - `Command`：发送到服务器的格式
 - `PlayerUse`：该命令是否非管理员可用
 - `PlayerSend`：命令执行是否是玩家
 - `Servers`：发送给的服务器，服务器名字记得加上`"`标起来，空或者null则表示发送给所有服务器

参数说明
- `{arg:at}`：将会被替换为@QQ之后的游戏ID
- `{arg:name}`：将会被替换为自己的游戏ID
- `{arg:qq}`：将会被替换为自己的QQ
- `{arg:atqq}`：将会被替换为@QQ之后的QQ
- `{arg:next}`：将会被替换为后面的参数
- `{arg:x}`：将剩下的参数直接填进去
例如
```json
{
    "禁言": {
        "Command": "mute {arg:next}",
        "PlayerUse": false,
        "PlayerSend": false,
        "Servers": []
    }
}
```
在群里输入 `#禁言 Color_yr` 
那么发送到服务器的指令为 `mute Color_yr`  

```json
{
    "给权限": {
        "Command": "lp user {arg:at} permission set {arg:next} true",
        "PlayerUse": false,
        "PlayerSend": false,
        "Servers": []
    }
}
```
在群里输入 `#给权限 @qq admin.*`(@一个群成员)  
那么发送到服务器的指令为 `lp user 该群员绑定的ID permission set admin.* true`  
参数注意空格，不注意会错乱

```json
{
    "说话": {
        "Command": "say {arg:x}",
        "PlayerUse": false,
        "PlayerSend": false,
        "Servers": []
    }
}
```
在群里输入 `#说话 test test`  
那么发送到服务器的指令为 `say test test`  

`Player.json`玩家绑定储存
```json
{
  //禁止绑定Id列表
  "NotBindList": [
    "Color_yr",
    "id"
  ],
  //禁言列表
  "MuteList": [
    "playerid"
  ],
  "PlayerList": {
    "402067010": {
      "Name": "测试",
      "Nick": "昵称",
      "QQ": 402067010,
      //是否为管理员
      "IsAdmin": true
    }
  }
}
```
自行添加注意格式

## 端口说明

Minecraft_QQ_Cmd/NewGui的默认端口为25555  
如果没有必要，请不要随便改这两个端口

## 不在一台机器上部署

如果你有公网IP，直接在防火墙开放端口就行了  
然后Minecraft_QQ的IP设置填你机器的公网IP
如果你没有公网IP，那就去用端口映射，能映射出去就行了  

## 自己写Minecraft_QQ
1. 首先确定你的环境是.net8
2. 在你的项目里面导入`Minecraft_QQ_Core.dll`  
如果你导入的是`ref`文件夹里面的dll，请另外安装[Newtonsoft.Json](https://www.newtonsoft.com/json)

3. 核心启动  
首先设置回调，然后使用`Start`方法
```C#
private static void Message(string message)
{
    Console.WriteLine(message);
}

private static void ConfigInit() 
{
    if (Environment.UserInteractive)
    {
        Console.WriteLine("进行初始配置");
        while (true)
        {
            Console.Write("请输入主群号：");
            string a = Console.ReadLine();
            if (long.TryParse(a, out var group))
            {
                group = Math.Abs(group);
                Minecraft_QQ.AddGroup(new()
                {
                    Group = group,
                    EnableCommand = true,
                    EnableSay = true,
                    IsMain = true
                });
                break;
            }
            Console.WriteLine("非法输入");
        }
    }
}

IMinecraft_QQ.ConfigInitCall = ConfigInit;
IMinecraft_QQ.ShowMessageCall = Message;
IMinecraft_QQ.LogCall = Message;

//启动
await Minecraft_QQ.Start();
```

Minecraft_QQ核心方法API
```C#
/// <summary>
/// QQ号取玩家
/// </summary>
/// <param name="qq">qq号</param>
/// <returns>玩家信息</returns>
public PlayerObj GetPlayer(long qq);
/// <summary>
/// ID取玩家
/// </summary>
/// <param name="id">玩家ID</param>
/// <returns>玩家信息</returns>
public PlayerObj GetPlayer(string id);
/// <summary>
/// 设置玩家昵称
/// </summary>
/// <param name="qq">qq号</param>
/// <param name="nick">昵称</param>
public void SetNick(long qq, string nick);
/// <summary>
/// 设置玩家ID，如果存在直接修改，不存在创建
/// </summary>
/// <param name="qq">qq号</param>
/// <param name="name">玩家ID</param>
public void SetPlayerName(long qq, string name);
/// <summary>
/// 直接设置一个玩家数据
/// </summary>
/// <param name="player">玩家</param>
public static void SetPlayer(PlayerObj player);
/// <summary>
/// 禁言玩家
/// </summary>
/// <param name="qq">QQ号</param>
public void MutePlayer(long qq);
/// <summary>
/// 禁言玩家
/// </summary>
/// <param name="name">名字</param>
public void MutePlayer(string name);
/// <summary>
/// 添加禁止绑定
/// </summary>
/// <param name="name">名字</param>
public static void AddNotBind(string name);
 /// <summary>
 /// 删除禁止绑定
 /// </summary>
 /// <param name="name">名字</param>
 public static void RemoveNotBind(string name);
/// <summary>
/// 解除禁言
/// </summary>
/// <param name="qq">玩家QQ号</param>
public void UnmutePlayer(long qq);
/// <summary>
/// 解除禁言
/// </summary>
/// <param name="name">玩家ID</param>
public void UnmutePlayer(string name);
/// <summary>
/// 设置维护模式状态
/// </summary>
/// <param name="open">状态</param>
public void FixModeChange(bool open);
/// <summary>
/// 重载配置
/// </summary>
public bool Reload();
/// <summary>
/// 启动
/// </summary>
public void Start();
/// <summary>
/// 停止
/// </summary>
public void Stop();
/// <summary>
/// 添加自动应答
/// </summary>
public static void AddAsk(string check, string res);
```

如果你要通过Minecraft_QQ发送消息，可以这样写
```C#
RobotCore.SendGroupMessage(123456, [MsgText.Build("text")]);
```
或者发送私聊消息
```C#
RobotCore.SendPrivateMessage(123456, [MsgText.Build("text")]);
```
