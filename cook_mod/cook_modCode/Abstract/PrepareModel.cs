using BaseLib.Abstracts;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace cook_mod.cook_modCode.Abstract;

public class PrepareModel() : CustomSingletonModel (true, false)
{
    private static Dictionary<Player, int> prepared = new Dictionary<Player, int>();
    
    private static Dictionary<Player, int> times = new Dictionary<Player, int>();

    public static int GetPrepared(Player player)
    {
        if (!prepared.ContainsKey(player))
        {
            prepared[player] = 0;
        }

        return prepared[player];
    }
    
    public static int GetTimes(Player player)
    {
        if (!times.ContainsKey(player))
        {
            times[player] = 0;
        }

        return times[player];
    }

    public static void Add(Player player, int amount)
    {
        if (!prepared.ContainsKey(player))
            prepared[player] = amount;
        else
            prepared[player] += amount;
        if (!times.ContainsKey(player))
            times[player] = 1;
        else
            times[player] ++;
    }

    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        var state = CombatManager.Instance.DebugOnlyGetState();
        if (state == null) return Task.CompletedTask;
        prepared[player] = 0;
        times[player] = 0;
        return Task.CompletedTask;
    }
}