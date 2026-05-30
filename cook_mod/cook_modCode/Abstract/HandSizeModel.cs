using BaseLib.Abstracts;
using BaseLib.Hooks;
using cook_mod.cook_modCode.Character;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Players;

namespace cook_mod.cook_modCode.Abstract;

public class HandSizeModel() : CustomSingletonModel(true, false), IMaxHandSizeModifier
{

    public int ModifyMaxHandSize(Player player, int currentMaxHandSize)
    {
        if (player.Character is TheCook)
            return currentMaxHandSize + 2;

        return currentMaxHandSize;
    }
}