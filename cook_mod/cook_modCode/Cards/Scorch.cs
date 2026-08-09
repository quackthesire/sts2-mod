using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
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


namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class Scorch() : CustomCardModel(1, CardType.Power,
    CardRarity.Rare, TargetType.Self)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/scorch.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Spicy>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Spicy", 6), new PowerVar<ScorchPower>(5m)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await FlavorCmd.ChangeFlavor(choiceContext, this.Owner, this, spicy: this.DynamicVars["Spicy"].IntValue);
        await PowerCmd.Apply<ScorchPower>(choiceContext, this.Owner.Creature, this.DynamicVars["ScorchPower"].BaseValue, this.Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["ScorchPower"].UpgradeValueBy(-2m);
    }
}