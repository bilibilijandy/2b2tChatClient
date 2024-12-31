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

    private string result;
    private string mm;

    public override void Initialize()
    {
        _enteredLock = false;
        if (!File.Exists("JLoginConf.yaml"))
        {
            LogToConsole("没有找到配置文件，正在释放....");
            string yaml = YamlHelper.Serialize(_config);
            yaml = "# XotBot @ XTeam\n# 请参阅用户手册来了解如何使用此处\n"+yaml;
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

        //随机密码功能
        var random = new Random();
        string filePath = "mm.txt";
        string oldMM = "oldmm.txt";
        result = GenerateRandomString(random);//新密码

        mm = File.ReadAllText(filePath); 
        LogToConsole($"{mm},{result}");

        // 将字符串写入文件
        using (StreamWriter writer = new StreamWriter(filePath, false)) // true表示追加模式
        {
            writer.WriteLine(result);
        }

        using (StreamWriter writer = new StreamWriter(oldMM, true)) // true表示追加模式
        {
            writer.WriteLine($"{result}\n");
        }

        SendText($"/login {mm}");
        LogToConsole($"登录成功,当前密码 {mm} ");
        Thread.Sleep(GetSleepTime());
        SendText($"/cp {mm} {result}");
        LogToConsole($"修改密码成功,当前密码 {result} ,原密码 {mm} ");
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

    static string GenerateRandomString(Random random)
    {
        // 确保 'e', 'd', 'e', 'n' 顺序
        List<char> chars = new List<char> { 'e', 'd', 'e', 'n', 'n', 'b', '6', '6', '6' };

        // 随机打乱除 'e', 'd', 'e', 'n' 之外的字符
        var shuffleChars = chars.Skip(4).ToList();
        shuffleChars = shuffleChars.OrderBy(a => random.Next()).ToList();

        // 重新组合字符串
        string shuffled = new string(chars.Take(4).Concat(shuffleChars).ToArray());

        // 插入随机数量的下划线
        int maxUnderscore = 8; // 根据字符数量设置最大下划线数
        int underscoreCount = random.Next(maxUnderscore + 1);

        for (int i = 0; i < underscoreCount; i++)
        {
            int insertIndex = random.Next(shuffled.Length + 1);
            shuffled = shuffled.Insert(insertIndex, "_");
        }

        // 检查是否包含 'eden'，如果不包含，重新生成
        if (!shuffled.Contains("eden"))
        {
            return GenerateRandomString(random); // 递归调用以确保包含 'eden'
        }

        return shuffled;
    }
}

[Serializable]
public class JLoginBotConfig
{
    [YamlMember(Alias = "defaultPasswd", ApplyNamingConventions = false)]
    public string DefaultPasswd = "#XBotDefaultPasswd123";
    [YamlMember(Alias = "reconnectTimeout", ApplyNamingConventions = false)]
    public float ReconnectTimeout = 3.1f;

    [YamlMember(Alias = "sleepTime", ApplyNamingConventions = false)]
    public float SleepTime = 2.2f;
}
