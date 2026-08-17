using BaseLib.Abstracts;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Powers;
public class LeechPower : CustomPowerModel, IOnBleed
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/leech_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/leech_power.png";
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>(), HoverTipFactory.FromKeyword(CustomKeywords.Flavor)];
    
    public override int DisplayAmount => 2 - this.GetInternalData<LeechPower.Data>().bleedTimes % 2;
    
    protected override object InitInternalData() => (object) new LeechPower.Data();

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    
    public async Task OnBleed(PlayerChoiceContext ctx, Player player, int amount, int changed, CardModel? cardSource, Creature target)
    {
        if (this.Owner.Player == null || player != this.Owner.Player || amount <= 0)
            return;
        LeechPower.Data data;
        data = this.GetInternalData<LeechPower.Data>();
        data.bleedTimes ++;
        int triggers = data.bleedTimes / 2 - data.triggerCount;
        if (triggers > 0)
        {
            this.Flash();
            await FlavorCmd.AddRandomFlavor(ctx, this.Owner.Player, null, this.Amount * triggers);
            data.triggerCount += triggers;
        }
        this.InvokeDisplayAmountChanged();
        data = (LeechPower.Data) null;
    }
    
    private class Data
    {
        public int bleedTimes;
        public int triggerCount;
    }
}