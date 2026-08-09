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
public class ByproductPower : CustomPowerModel, IOnEnchant
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/byproduct_power.png";
    
    public sealed override string CustomBigIconPath => "res://cook_mod/byproduct_power.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => (object) new ByproductPower.Data();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            return (IEnumerable<IHoverTip>) [HoverTipFactory.ForEnergy((PowerModel) this)];
        }
    }
    
    public override Task BeforeSideTurnStart(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return Task.CompletedTask;
        this.GetInternalData<ByproductPower.Data>().cardsEnchantedThisTurn = 0;
        return Task.CompletedTask;
    }
    
    public async Task OnEnchant(PlayerChoiceContext ctx, Player player, CardModel card, CardModel? cardSource)
    {
        if (card == null || player != this.Owner.Player || this.GetInternalData<ByproductPower.Data>().cardsEnchantedThisTurn >= this.Amount)
            return;
        ++this.GetInternalData<ByproductPower.Data>().cardsEnchantedThisTurn;
        await PlayerCmd.GainEnergy((Decimal) 1m, this.Owner.Player);
        await CardPileCmd.Draw(ctx,(Decimal) 1m, this.Owner.Player);
    }
    private class Data
    {
        public int cardsEnchantedThisTurn;
    }
}