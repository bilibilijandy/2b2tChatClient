using System;
using System.IO;
using System.Threading;
using System.Collections.Generic; // 添加此行
using System.Linq; // 添加此行
using MinecraftClient.Scripting;
using YamlDotNet.Serialization;
using MinecraftClient.Commands;
using System.Text.RegularExpressions;

namespace MinecraftClient.ChatBots.JandyBot;

public class JCommandBot : ChatBot
{
    private float _ableTime = 5;
    private int _count = 0;
    private int _player = 1;
    private List<string> age = new List<string>();

    public override void Initialize()
    {
        
    }

    public override void AfterGameJoined()
    {
        
    }

    public override void GetText(string text)
    {
        text=GetVerbatim(text);//去除颜色代码
        Match match = Regex.Match(text, @"[^\w]*[!！](\w+)\s*(.*)");//匹配命令
        if (match.Success)//匹配成功
            {
                string comm = match.Groups[1].Value;
                string parameters = match.Groups[2].Value;
                age = parameters.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                commands(comm, age);
            }
    }

    public void commands(string command, List<string> parameters)
    {
        /**
         * 命令列表
         * help 帮助
         * echo 回显
         * stat 状态
         */
        LogToConsole("command: " + command);
        switch (command)
        {
            case "help":
                SendText(cleanText("帮助"));
                break;
            case "echo":
                if (parameters.Count > 0)
                {
                    SendText(cleanText(parameters[0]));
                }
                else
                {
                    SendText("echo");
                }
                break;
            case "stat":
                SendText(cleanText("当前服务器版本:1.20.4；客户端版本:1.20.4,已加载模块：loginv1.0;msgv1.0;commandv1.5。"));
                break;
            case "admin":
                SendText(cleanText("管理员列表：bilibili_jandy;jandy的2b2t;jandy的2b2t号"));
                break;
        }
    }

    public string cleanText(string text)
    {
        text = text.Replace("§", "");
        text = text.Replace("/", "");
        text = GetVerbatim(text);
        return text;
    }

}