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
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CustomKeywords.Generic_Flavor)];
    
    protected override object InitInternalData() => (object) new RecyclingPower.Data();
    
    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return Task.CompletedTask;
        this.GetInternalData<RecyclingPower.Data>().foodsCreatedThisTurn = 0;
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator == null || creator.Creature != this.Owner || !(card is FoodCardModel) || this.GetInternalData<RecyclingPower.Data>().foodsCreatedThisTurn >= 1)
            return;
        ++this.GetInternalData<RecyclingPower.Data>().foodsCreatedThisTurn;
        await FlavorCmd.AddRandomGenericFlavor(new BlockingPlayerChoiceContext(), this.Owner.Player, null, this.Amount);
    }
    private class Data
    {
        public int foodsCreatedThisTurn;
    }
}