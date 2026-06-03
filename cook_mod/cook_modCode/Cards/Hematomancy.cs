using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Powers;
using MegaCrit.Sts2.Core.Models.Enchantments;


namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class Hematomancy() : CustomCardModel(0, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>(), this.EnergyHoverTip];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BleedPower>(3m), new EnergyVar(2)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
        VfxCmd.PlayOnCreatureCenter(this.Owner.Creature, "vfx/vfx_bloody_impact");
        await PowerCmd.Apply<BleedPower>(choiceContext, this.Owner.Creature, this.DynamicVars["BleedPower"].IntValue, this.Owner.Creature, this);
        await PlayerCmd.GainEnergy(this.DynamicVars.Energy.BaseValue, this.Owner);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Energy.UpgradeValueBy(1m);
    }
}