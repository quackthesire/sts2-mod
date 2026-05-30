using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Commands;

public class CookHook
{
    private static async Task Dispatch<T>(PlayerChoiceContext ctx, Player player, Func<T, Task> invoke)
        where T : class
    {
        var combatState = player.Creature.CombatState;
        if (combatState == null) return;
        foreach (var model in combatState.IterateHookListeners().OfType<T>())
        {
            var abstractModel = (AbstractModel)(object)model;
            ctx.PushModel(abstractModel);
            await invoke(model);
            ctx.PopModel(abstractModel);
        }
    }

    private static TResult Aggregate<T, TResult>(ICombatState combatState, TResult seed,
        Func<T, TResult, TResult> action)
        where T : class
    {
        return combatState.IterateHookListeners().OfType<T>()
            .Aggregate(seed, (current, model) => action(model, current));
    }

    public static Task OnPrepared(PlayerChoiceContext ctx, Player player, int amount, int discardedAmount, CardPlay? cardPlay)
    {
        return Dispatch<IOnPrepared>(ctx, player, m => m.OnPrepared(ctx, player, amount, discardedAmount, cardPlay));
    }

    public static Task OnBleed(PlayerChoiceContext ctx, Player player, int amount, int changed, CardModel? cardSource)
    {
        return Dispatch<IOnBleed>(ctx, player, m => m.OnBleed(ctx, player, amount, changed, cardSource));
    }
    
    public static Task OnFlavor(PlayerChoiceContext ctx, Player player, Flavors original, Flavors modified)
    {
        return Dispatch<IOnFlavor>(ctx, player, m => m.OnFlavor(ctx, player, original, modified));
    }
}