﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Features.Data.Enums;
using Core.Features.Handlers;
using Core.Features.Wrappers;
using Core.Modules.RespawnTimer;
using Core.Modules.Subclasses.Features;
using NorthwoodLib.Pools;

namespace Core.Features.Display;

public class GameDisplayBuilder 
{
    private readonly StringBuilder _builder;
    
    public GameDisplayBuilder(StringBuilder builder) => _builder = builder;

    ~GameDisplayBuilder() => StringBuilderPool.Shared.Return(_builder);

    private readonly Dictionary<ScreenZone, string> _saved = new ();
    private List<string> _notifications = new ();
    private Subclass _subclass;
    private string _color = "#fff";
    private string _level = string.Empty;
    private string _name = string.Empty;
    private int _spectators;
    
    public void Clear()
    {
        _saved.Clear();
        _notifications.Clear();
        _spectators = 0;
    }
    
    public void WithContent(ScreenZone zone, string content)
    {
        if (_saved.ContainsKey(zone))
        {
            _saved[zone] = content;
            return;
        }
        
        _saved.Add(zone, content);
    }

    public void WithNotifications(List<string> notifications) => _notifications = notifications;
    public void WithSubclass(Subclass subclass) => _subclass = subclass;
    public void WithColor(string color) => _color = color;
    public void WithLevelMessage(string levelMsg) => _level = levelMsg;
    public void WithName(string name) => _name = name;
    public void WithSpectators(int number) => _spectators = number;
    
    public string BuildForHuman()
    {
        _builder.Clear();
        _builder.Append("<size=60%><line-height=100%><voffset=14em>");

        if (_subclass is not null)
        {
            _builder.AppendLine($"<color={_color}>" + _subclass.TopBar);
            _builder.AppendLine(_subclass.SecondaryTopBar + "</color>");
            _builder.Append(GetZone(ScreenZone.SubclassAlert));
        }
        else
        {
            _builder.Append("\n\n\n");
        }

        int i = 0;

        for (; i < _notifications.Count; i++)
            _builder.AppendLine(_notifications[i]);
        for (; i < 6; i++)
            _builder.AppendLine();
        
        if (PollHandler.Enabled)
            _builder.AppendLine($"<align=left>Poll by <color={_color}>{PollHandler.PollAuthor}</color>\n<i>{PollHandler.PollName}</i>\n\n{PollHandler.YesVotes} - <color=#84ff80>.vote yes</color>\n{PollHandler.NoVotes} - <color=#ff8080>.vote no</color>\n<size=40%>time left: {PollHandler.TimeLeft}</size></align>");
        else
            _builder.Append(RenderZone(ScreenZone.Top));
        
        _builder.Append(RenderZone(ScreenZone.CenterTop));
        _builder.Append(RenderZone(ScreenZone.Center));
        _builder.Append(RenderZone(ScreenZone.CenterBottom));
        _builder.Append(RenderZone(ScreenZone.Bottom));
        _builder.Append(FormatStringForHud(GetZone(ScreenZone.InteractionMessage), 1));
        _builder.Append(FormatStringForHud(GetZone(ScreenZone.KillMessage), 1));

        _builder.Append($"<color={_color}>");
        
        if (_spectators != 0)
            _builder.Append($"<align=right>👥 S<lowercase>pectators: {_spectators}</lowercase></align>");

        _builder.AppendLine();

        _builder.AppendLine($"<b><size=50%><color=#c862ff>C</color><color=#c684ff>u</color><color=#c4a7ff>r</color><color=#c1c9ff>s</color><color=#bfebff>e</color><color=#cbf0eb>d</color> <color=#e3fac4>S</color><color=#efffb0>L</color> - {Core.GlobalVersion}");
        _builder.Append($"{_name} | {_level} | tps: {ServerCore.Tps}");

        return _builder.ToString();
    }
    
    public string BuildForSpectator()
    {
        _builder.Clear();
        _builder.Append("<size=60%><line-height=100%><voffset=14em>");

        if (_subclass is not null)
        {
            _builder.AppendLine($"<color={_color}>" + _subclass.TopBar);
            _builder.AppendLine(_subclass.SecondaryTopBar + "</color>");
            _builder.Append(GetZone(ScreenZone.SubclassAlert));
        }
        else
        {
            _builder.Append("\n\n\n");
        }

        int i = 0;

        for (; i < _notifications.Count; i++)
            _builder.AppendLine(_notifications[i]);
        for (; i < 6; i++)
            _builder.AppendLine();

        if (PollHandler.Enabled)
            _builder.AppendLine($"<align=left>Poll by <color={_color}>{PollHandler.PollAuthor}</color>\n<i>{PollHandler.PollName}</i>\n\n{PollHandler.YesVotes} - <color=#84ff80>.vote yes</color>\n{PollHandler.NoVotes} - <color=#ff8080>.vote no</color>\n<size=40%>time left: {PollHandler.TimeLeft}</size></align>");
        else
            _builder.Append(RenderZone(ScreenZone.Top));

        _builder.Append(RenderZone(ScreenZone.CenterTop));
        _builder.Append(RenderZone(ScreenZone.Center));
        _builder.Append(RenderZone(ScreenZone.CenterBottom));
        _builder.Append(FormatStringForHud(EventHandler.RenderedZone));
        _builder.Append(FormatStringForHud(GetZone(ScreenZone.InteractionMessage), 1));
        _builder.Append(FormatStringForHud(GetZone(ScreenZone.KillMessage), 1));

        _builder.Append($"<color={_color}>");
        _builder.AppendLine(EventHandler.Tip);
        _builder.AppendLine($"<b><size=50%><color=#c862ff>C</color><color=#c684ff>u</color><color=#c4a7ff>r</color><color=#c1c9ff>s</color><color=#bfebff>e</color><color=#cbf0eb>d</color> <color=#e3fac4>S</color><color=#efffb0>L</color> - {Core.GlobalVersion}");
        _builder.Append($"{_name} | {_level} | tps: {ServerCore.Tps}");

        return _builder.ToString();
    }

    private string GetZone(ScreenZone zone) => _saved.ContainsKey(zone) ? _saved[zone] : string.Empty;

    private string RenderZone(ScreenZone zone) => FormatStringForHud(GetZone(zone));
    
    private string FormatStringForHud(string text, int linesNeeded = 6)
    {
        int textLines = text.Count(x => x == '\n');

        for (int i = 0; i < linesNeeded - textLines; i++)
            text += '\n';

        return text;
    }
}