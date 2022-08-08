using System.Collections.Generic;
using System.Linq;
using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using NorthwoodLib.Pools;
using Respawning;
using Random = UnityEngine.Random;

namespace Core.Modules.RespawnTimer;

public class EventHandler
{
    private CoroutineHandle? _timerCoroutine;
    public static List<string> Tips = new();

    public void OnRoundStarted()
    {
        if(_timerCoroutine is { IsRunning: true })
            Timing.KillCoroutines(_timerCoroutine.Value);
                
        _timerCoroutine = Timing.RunCoroutine(Timer());
    }

    public void OnEndedRound(RoundEndedEventArgs ev)
    {
        if(_timerCoroutine.HasValue)
            Timing.KillCoroutines(_timerCoroutine.Value);
    }

    private static IEnumerator<float> Timer()
    {
        int i = 0;
        var tip = "This is a secret message, wow.";
        for (;;)
        {
            var builder = StringBuilderPool.Shared.Rent(Respawn.IsSpawning ? "\n\n\n\nY<lowercase>ou will respawn in:</lowercase>\n" : "\n\n\n\nN<lowercase>ext team is on the way!</lowercase>\n");
            var tipBuilder = StringBuilderPool.Shared.Rent("\n");
                
            if (i == 16)
            {
                i = 0;
                tip = Tips[Random.Range(0, Tips.Count)];
            }
                
            yield return Timing.WaitForSeconds(0.99f);
                
            if (Respawn.TimeUntilSpawnWave.Minutes != 0)
                builder.Append(Respawn.TimeUntilSpawnWave.Minutes + " minutes ");
            builder.Append(Respawn.TimeUntilSpawnWave.Seconds + " seconds");

                
            if (Respawn.NextKnownTeam != SpawnableTeamType.None)
            {
                tipBuilder.Append("as a ");
                if (Respawn.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
                    tipBuilder.Append("<color=#18f240>chaos</color>");
                else
                    tipBuilder.Append("<color=#2542e6>m.t.f.</color>");
            }

            tipBuilder.Append("\n\n" + GetCount() + "<size=70%><color=#9342f5>❓</color>" + tip + "</size>");

            var text = StringBuilderPool.Shared.ToStringReturn(builder);
            var tipText = StringBuilderPool.Shared.ToStringReturn(tipBuilder);
                
            foreach (var player in Player.Get(Team.RIP))
            {
                player.SendHint(ScreenZone.Center, text, 1.2f);
                player.SendHint(ScreenZone.Bottom, tipText, 1.2f);
            }

            i++;
        }
    }

    private static string GetCount()
    {
        return $"<color=#9effe0>👻 spectators:</color> {Player.Get(RoleType.Spectator).Count()} | <color=#9ecfff>⛨ mtf tickets:</color> {Respawn.NtfTickets} | <color=#9effa6>⏣ chaos tickets:</color> {Respawn.ChaosTickets}\n";
    }
}