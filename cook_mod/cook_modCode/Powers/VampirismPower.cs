using BaseLib.Abstracts;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Powers;
public class VampirismPower : CustomPowerModel, IOnBleed
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override int DisplayAmount => this.Amount - this.GetInternalData<VampirismPower.Data>().bleedApplied % this.Amount;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>(), HoverTipFactory.FromPower<Flavor>()];
    
    protected override object InitInternalData() => (object) new VampirismPower.Data();

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    public async Task OnBleed(PlayerChoiceContext ctx, Player player, int amount, int changed, CardModel? cardSource)
    {
        if (this.Owner.Player == null || player != this.Owner.Player || amount <= 0)
            return;
        VampirismPower.Data data;
        data = this.GetInternalData<VampirismPower.Data>();
        data.bleedApplied ++;
        int triggers = data.bleedApplied / this.Amount - data.triggerCount;
        if (triggers > 0)
        {
            this.Flash();
            await FlavorCmd.AddRandomFlavor(ctx, this.Owner.Player, null, 1);
            data.triggerCount += triggers;
        }
        this.InvokeDisplayAmountChanged();
        data = (VampirismPower.Data) null;
    }
    
    private class Data
    {
        public int bleedApplied;
        public int triggerCount;
    }
}