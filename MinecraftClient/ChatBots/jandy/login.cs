using System;
using System.IO;
using System.Threading;
using System.Collections.Generic; // 添加此行
using System.Linq; // 添加此行
using MinecraftClient.Scripting;
using YamlDotNet.Serialization;
using MinecraftClient.Commands;

namespace MinecraftClient.ChatBots.JandyBot;

public class JLoginBot:ChatBot
{
    private Random _random = new Random();
    private JLoginBotConfig _config = new JLoginBotConfig();
    private bool _enteredLock;

    public override void Initialize()
    {
        _enteredLock = false;
        if (!File.Exists("JLoginConf.yaml"))
        {
            LogToConsole("没有找到配置文件，正在释放....");
            string yaml = YamlHelper.Serialize(_config);
            //yaml = yaml;
            File.WriteAllText("./JLoginConf.yaml", yaml);
        }

        _config = YamlHelper.DeserializeFromFile<JLoginBotConfig>("JLoginConf.yaml");
        LogToConsole($"你的默认密码：{_config.DefaultPasswd}");
        LogToConsole("JLogin模块 @ 0.0.1 已经加载");

    }

    public override void AfterGameJoined()
    {
        if (_enteredLock)
        {
            LogToConsole("检测到你成功登录服务器，恭喜");
            return;
        }

        SendText($"/login {_config.DefaultPasswd}");
        Thread.Sleep(GetSleepTime());
        ChangeSlot(2);
        LogToConsole("切换物品成功");
        Thread.Sleep(GetSleepTime());
        UseItemInHand();
        LogToConsole("使用物品成功");
        LogToConsole("成功进入排队");
        _enteredLock = true;
    }

    public override void OnTitle(int action, string titletext, string subtitletext, string actionbartext, int fadein, int stay, int fadeout,
        string json)
    {
        LogToConsole(actionbartext);
        LogToConsole(titletext);
        LogToConsole(subtitletext);
    }


    private int GetSleepTime()
    {
        int res = (int)(_config.SleepTime * 1000 + _random.NextInt64(1000));
        LogToConsole($"将在{res}ms后进行下一步操作...");
        return res;
    }

    
}

[Serializable]
public class JLoginBotConfig
{
    [YamlMember(Alias = "defaultPasswd", ApplyNamingConventions = false)]
    public string DefaultPasswd = "#JBotDefaultPasswd123";
    [YamlMember(Alias = "reconnectTimeout", ApplyNamingConventions = false)]
    public float ReconnectTimeout = 3.1f;

    [YamlMember(Alias = "sleepTime", ApplyNamingConventions = false)]
    public float SleepTime = 2.2f;
}
