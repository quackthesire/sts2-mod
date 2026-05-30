using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Powers;
public class SadisticNaturePower : CustomPowerModel, IOnBleed
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>()];
    
    public async Task OnBleed(PlayerChoiceContext ctx, Player player, int amount, int changed, CardModel?  cardSource)
    {
        if (base.Owner.Player == null || player != base.Owner.Player)
            return;

        await CreatureCmd.GainBlock(base.Owner, changed * base.Amount / 100, ValueProp.Unpowered, null);
    }
}