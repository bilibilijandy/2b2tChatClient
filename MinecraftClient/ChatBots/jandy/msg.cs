using System;
using System.IO;
using MinecraftClient.Scripting;
using YamlDotNet.Serialization;

namespace MinecraftClient.ChatBots.JandyBot;



public class JMsgBot:ChatBot
{
    private JMsgBotConfig _config = new JMsgBotConfig();
    private float _ableTime = 5;
    private int _count = 0;
    private Random _random = new Random();
    private int _player = 1;
    private string[] _players = new[] { "" };
    private bool _lock = false;

    public override void Initialize()
    {
        _lock = false;
        if (!File.Exists("JMsgBotConf.yaml"))
        {
            LogToConsole("没有找到配置文件，正在释放....");
            string yaml = YamlHelper.Serialize(_config);
            yaml = "# 请参阅用户手册来了解如何使用此处\n"+yaml;
            File.WriteAllText("./JMsgBotConf.yaml", yaml);
        }

        _config = YamlHelper.DeserializeFromFile<JMsgBotConfig>("JMsgBotConf.yaml");
        _ableTime = _config.Speed * 1000 / 100;
        LogToConsole("JMsg模块 @ 0.0.1 已经加载");
    }

    public override void AfterGameJoined()
    {
        _lock = true;
    }

    private void GetPlayers()
    {
        _config = YamlHelper.DeserializeFromFile<JMsgBotConfig>("JMsgBotConf.yaml");
        _players = GetOnlinePlayers();
        LogToConsole("当前在线玩家：");
        string res = "";
        foreach(string player in _players){
            res += player;
            res += " ";
        }
        LogToConsole(res);
    }

    public override void Update()
    {
        if (!_lock)
        { return; }

        if (_count >= _ableTime)
        {
            if (_player >= _players.Length)
            {
                GetPlayers();
                _player = 0;
            }
            int msgIndex = (int)_random.NextInt64(_config.Messages.Length);
            string msg = _config.Messages[msgIndex];
            string playerId = _players[_player];
            SendText($"/msg {playerId} {msg}");

            _player += 1;
            _count = 0;
        }


        _count += 1;
    }

}

public class JMsgBotConfig {
    [YamlMember(Alias = "speed", ApplyNamingConventions = false)]
    public float Speed = 5f;

    [YamlMember(Alias = "messages", ApplyNamingConventions = false)]
    public string[] Messages =
        { "默认测试宣传文本 XTeam @ XotBot", "测试宣传文本2 XTeam @ XotBot", "测试宣传文本3 XTeam @ XotBot" };

}

