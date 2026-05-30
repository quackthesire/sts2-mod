using BaseLib.Abstracts;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace cook_mod.cook_modCode.Abstract;

public class FlavorsModel() : CustomSingletonModel (true, false)
{
    private static Dictionary<Player, Flavors> flavors = new Dictionary<Player, Flavors>();

    public static Flavors Get(Player player)
    {
        if (!flavors.ContainsKey(player))
        {
            flavors[player] = new Flavors();
        }

        return flavors[player];
    }
    
    public override Task BeforeCombatStart()
    {
        var state = CombatManager.Instance.DebugOnlyGetState();
        if (state == null) return Task.CompletedTask;
        foreach (var player in state.Players)
            FlavorCmd.SetFlavor(new BlockingPlayerChoiceContext(), player, null, sweet: 0, sour: 0, salty: 0, bitter: 0, spicy: 0);
        return Task.CompletedTask;
    }
}