using System;
using Core.Modules.Logs.Enums;
using Exiled.API.Features;
using HarmonyLib;
using RemoteAdmin;

namespace Core.Modules.Logs.Patches;

[HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
public static class CommandLogging
{
    [HarmonyPrefix]
    public static void Prefix(string q, CommandSender sender) 
    {
        try
        {
            string[] args = q.Trim().Split(QueryProcessor.SpaceArray, 512, StringSplitOptions.RemoveEmptyEntries);
            if (args[0].StartsWith("$"))
                return;

            Player player = sender is PlayerCommandSender playerCommandSender
                ? Player.Get(playerCommandSender)
                : Server.Host;

            if(player != null)
                WebhookSender.AddMessage($"{sender.Nickname.DiscordParse()} ({sender.SenderId ?? "Srv"}) >> **`{q}`**", WebhookType.CommandLogs);
        }
        catch (Exception e)
        {
            Log.Error(e);
        }
    }
    
}