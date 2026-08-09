using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace cook_mod.cook_modCode.Powers;
public class GrowingGashPower : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/growing_gash_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/growing_gash_power.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>()];
    
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (participants.Contains(this.Owner))
        {
            Flash();
            Creature creature = this.Owner.Player.RunState.Rng.CombatTargets.NextItem(this.CombatState.HittableEnemies);
            if (creature != null)
            {
                await PowerCmd.Apply<BleedPower>(new ThrowingPlayerChoiceContext(), creature, this.Amount, this.Owner, null);
            }
        }
    }
}