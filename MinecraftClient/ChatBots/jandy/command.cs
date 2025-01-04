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

public class JCommandBot:ChatBot
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
            text = GetVerbatim(text);
            string sender = "";
            string message = "";

            if (IsChatMessage(text, ref message, ref sender))
            {
                Match match = Regex.Match(message, @"[!！](.*)");
                if (match.Success)
                {
                    string comm = match.Groups[1].Value;
                    string parameters = match.Groups[2].Value;
                    age = parameters.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            else if (IsPrivateMessage(text, ref message, ref sender))
            {
                Match match = Regex.Match(message, @"[!！](.*)");
                if (match.Success)
                {
                    string comm = match.Groups[1].Value;
                    string parameters = match.Groups[2].Value;
                    age = parameters.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
        }

    

    public void commands(string command, List<string> parameters)
    {
        switch (command)
        {
            case "help":
                SendText("帮助");
                break;
            case "echo":
                SendText(parameters[0]);
                break;
            case "stat":
                SendText("当前服务器版本:1.20.4；客户端版本:1.20.4,已加载模块：loginv1.0;msgv1.0;commandv1.1。");
                break;
        }
    }
}

