﻿using Core.Loader.Features;
using Core.Modules.Logs.Enums;
using Exiled.API.Features;
using HarmonyLib;
using MEC;
using RemoteAdmin;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace Core.Modules.Logs;

public class LogsModule : CoreModule<LogsConfig>
{
    public override string Name { get; } = "Logs";

    public static LogsConfig LogsConfig;

    public override void OnEnabled()
    {
        LogsConfig = Config;

        WebhookSender.AddMessage("`SERVER CONNECTED ✨`", WebhookType.GameLogs);
            
        Timing.RunCoroutine(WebhookSender.ManageQueue());

        Player.Verified += ev => WebhookSender.AddMessage($"`Join ✨` >> {ev.Player.Nickname.DiscordParse()} ({ev.Player.UserId}) [{ev.Player.IPAddress}]", WebhookType.GameLogs); 
        Player.Left += ev => WebhookSender.AddMessage($"`Left ⛔` >> {ev.Player.Nickname.DiscordParse()} ({ev.Player.UserId}) [{ev.Player.IPAddress}] as {ev.Player.Role}", WebhookType.GameLogs); 
        Player.Handcuffing += ev => WebhookSender.AddMessage($"`Disarmed 🗝️` {ev.Cuffer.Nickname.DiscordParse()} ({ev.Cuffer.Role}) disarmed {ev.Target.Nickname.DiscordParse()} ({ev.Target.Role})", WebhookType.GameLogs);
        Player.Dying += ev =>
        {
            if (ev.Killer is null || ev.Target is null)
                return;

            WebhookSender.AddMessage($"`Died ☠️` {ev.Killer.Nickname.DiscordParse()} ({ev.Killer.Role}) killed {ev.Target.Nickname.DiscordParse()} ({ev.Target.Role}) with {ev.Handler.Type}", WebhookType.KillLogs);
        };
            
        Server.RoundEnded += _ => WebhookSender.AddMessage($"`🏁🔴🏁 The round has ended!`", WebhookType.GameLogs);  
        Server.RoundStarted += () => WebhookSender.AddMessage($"`🏁🟢🏁 New round has started!`", WebhookType.GameLogs);  
            
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        LogsConfig = null;

        base.OnDisabled();
    }

    public override void UnPatch()
    {
        Core.Harmony.Unpatch(typeof(CommandProcessor).GetMethod(nameof(CommandProcessor.ProcessQuery)), HarmonyPatchType.Prefix, Core.Harmony.Id);
        Core.Harmony.Unpatch(typeof(Log).GetMethod(nameof(Log.Error)), HarmonyPatchType.Prefix, Core.Harmony.Id);
    }
}