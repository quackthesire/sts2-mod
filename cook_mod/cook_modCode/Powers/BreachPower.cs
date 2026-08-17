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
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace cook_mod.cook_modCode.Powers;
public class BreachPower : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/breach_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/breach_power.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block), HoverTipFactory.FromPower<WeakPower>()];
    
    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
    {
        if (this.Amount <= 0)
            return;
        foreach (Creature hittableEnemy in (IEnumerable<Creature>)this.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(new BlockingPlayerChoiceContext(), hittableEnemy, this.Amount, this.Owner, null);
        }
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (this.Owner.IsAlive) RemoveInternal();
        await Task.CompletedTask;
    }
}