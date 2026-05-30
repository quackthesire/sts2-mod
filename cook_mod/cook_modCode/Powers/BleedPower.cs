using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Powers;

public sealed class BleedPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount,
            ValueProp.Unblockable | ValueProp.Unpowered, applier, cardSource);
        await CookHook.OnBleed(new BlockingPlayerChoiceContext(), applier.Player, base.Amount, base.Amount, cardSource);
    }

    public override async Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target,
        Creature? applier,
        CardModel? cardSource)
    {
        if (!(power is BleedPower) || target != base.Owner)
            return;
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), base.Owner, base.Amount + amount,
                    ValueProp.Unblockable | ValueProp.Unpowered, applier, cardSource);
        await CookHook.OnBleed(new BlockingPlayerChoiceContext(), applier.Player, base.Amount + (int) amount, (int) amount, cardSource);
    }
}