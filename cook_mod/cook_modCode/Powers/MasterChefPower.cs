using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Powers;
public class MasterChefPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;
    
    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (card.Owner.Creature != Owner || !card.Owner.Creature.HasPower<MasterChefPower>() || !(card is FoodCardModel))
            return Task.CompletedTask;
        if (card is { IsUpgradable: true, IsUpgraded: false }) CardCmd.Upgrade(card);
        return Task.CompletedTask;
    }
}