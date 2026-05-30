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
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace cook_mod.cook_modCode.Powers;
public class DrawBloodPower : CustomPowerModel, IOnBleed
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override int DisplayAmount => 2 - this.GetInternalData<DrawBloodPower.Data>().bleedTimes % 2;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            return (IEnumerable<IHoverTip>) [HoverTipFactory.FromPower<BleedPower>(), HoverTipFactory.FromPower<Flavor>()];
        }
    }
    
    public async Task OnBleed(PlayerChoiceContext ctx, Player player, int amount, int changed, CardModel?  cardSource)
    {
        if (base.Owner.Player != null && player == base.Owner.Player)
        {
            DrawBloodPower drawBloodPower = this;
            DrawBloodPower.Data data;
            if (amount <= 0)
            {
                data = (DrawBloodPower.Data) null;
            }
            else
            {
                data = drawBloodPower.GetInternalData<DrawBloodPower.Data>();
                data.bleedTimes += 1;
                int triggers = data.bleedTimes / 2 - data.triggerCount;
                if (triggers > 0)
                {
                    drawBloodPower.Flash();
                    await FlavorCmd.AddRandomFlavor(ctx, base.Owner.Player, null, base.Amount);
                    data.triggerCount += triggers;
                }
                drawBloodPower.InvokeDisplayAmountChanged();
                data = (DrawBloodPower.Data) null;
            }
        }
    }
    
    protected override object InitInternalData() => (object) new DrawBloodPower.Data();

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    private class Data
    {
        public int bleedTimes;
        public int triggerCount;
    }
}