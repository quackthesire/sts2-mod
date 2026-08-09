using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
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
using cook_mod.cook_modCode.Cards;
using cook_mod.cook_modCode.Keywords;
using Godot;
using MegaCrit.Sts2.Core.Models.Powers;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class Caffeination() : CustomCardModel(1, CardType.Power,
    CardRarity.Uncommon, TargetType.Self)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/caffeination.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>(), this.EnergyHoverTip];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2m), new PowerVar<VulnerablePower>(2m), new EnergyVar(1)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WeakPower>(choiceContext, this.Owner.Creature, this.DynamicVars["WeakPower"].BaseValue, this.Owner.Creature, this);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, this.Owner.Creature, this.DynamicVars["VulnerablePower"].BaseValue, this.Owner.Creature, this);
        await PowerCmd.Apply<CaffeinationPower>(choiceContext, this.Owner.Creature, this.DynamicVars.Energy.BaseValue, this.Owner.Creature, this);
        GD.Print(this.Owner.Creature.GetPower<CaffeinationPower>().SmartDescription);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["WeakPower"].UpgradeValueBy(-1m);
        this.DynamicVars["VulnerablePower"].UpgradeValueBy(-1m);
    }
}