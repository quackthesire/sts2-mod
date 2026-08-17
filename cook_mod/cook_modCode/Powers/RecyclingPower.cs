using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
using Godot;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace cook_mod.cook_modCode.Powers;
public class RecyclingPower : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/recycling_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/recycling_power.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CustomKeywords.Food), HoverTipFactory.FromKeyword(CustomKeywords.Generic_Flavor)];
    
    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator.Creature != this.Owner || !(card is FoodCardModel))
            return;
        if (CombatManager.Instance.History.Entries.OfType<CardGeneratedEntry>().Count<CardGeneratedEntry>((Func<CardGeneratedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.Actor == this.Owner && e.Card is FoodCardModel)) > 1)
            return;
        await FlavorCmd.AddRandomGenericFlavor(new BlockingPlayerChoiceContext(), this.Owner.Player, null, this.Amount);
    }
}