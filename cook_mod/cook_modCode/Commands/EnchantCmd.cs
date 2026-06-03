using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Control;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace cook_mod.cook_modCode.Commands;

public static class EnchantCmd
{
    
    public static async Task OnEnchant(PlayerChoiceContext ctx, Player player, CardModel card, CardModel? cardSource)
    {
        await CookHook.OnEnchant(ctx, player, card, cardSource);
    }
}