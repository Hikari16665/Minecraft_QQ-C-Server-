﻿using Native.Csharp.App;

namespace Color_yr.Minecraft_QQ
{
    class message
    {
        public static string Head;
        public static string End;
        public static void Message(string read)
        {
            while (read.IndexOf(Head) == 0 && read.IndexOf(End) != -1)
            {
                string buff = use.get_string(read, Head, End);
                if (buff.IndexOf("[群消息]") == 0)
                {
                    buff = buff.Replace("[群消息]", string.Empty);
                    string z = use.get_string(buff, "(", ")");
                    if (use.check_mute(z) == false)
                    {
                        buff = buff.Replace("(" + z + ")", "");
                        buff = use.code_CQ(buff);
                        if (config_read.color_code == false)
                            buff = use.RemoveColorCodes(buff);
                        Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, buff);
                        if(Minecraft_QQ.GroupSet2 != 0 && Minecraft_QQ.Group2_on == true)
                        {
                            Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet2, buff);
                        }
                        if (Minecraft_QQ.GroupSet3 != 0 && Minecraft_QQ.Group3_on == true)
                        {
                            Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet3, buff);
                        }
                    }
                    Minecraft_QQ.Group = 0;
                }
                else if (Minecraft_QQ.Group == 1)
                {
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet1, buff);
                    Minecraft_QQ.Group = 0;
                }
                else if (Minecraft_QQ.Group == 2)
                {
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet2, buff);
                    Minecraft_QQ.Group = 0;
                }
                else if (Minecraft_QQ.Group == 3)
                {
                    Common.CqApi.SendGroupMessage(Minecraft_QQ.GroupSet3, buff);
                    Minecraft_QQ.Group = 0;
                }
                int i = read.IndexOf(End);
                read = read.Substring(i + End.Length);
            }
        }
    }
}