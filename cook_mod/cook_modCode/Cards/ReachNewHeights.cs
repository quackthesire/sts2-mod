using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
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

public class ReachNewHeights() : CustomCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Flavor>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Flavors", 1)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Flavors flavors = FlavorsModel.Get(base.Owner);
        if (flavors != null && flavors.sweet > 0 && flavors.sour > 0 && flavors.salty > 0 && flavors.bitter > 0 && flavors.spicy > 0)
            await FlavorCmd.ChangeFlavor(choiceContext, base.Owner, this, sweet: base.DynamicVars["Flavors"].IntValue, sour: base.DynamicVars["Flavors"].IntValue, salty: base.DynamicVars["Flavors"].IntValue, bitter: base.DynamicVars["Flavors"].IntValue, spicy: base.DynamicVars["Flavors"].IntValue);
    }
    
    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Retain);
    }
}