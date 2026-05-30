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

public class FlavorBomb() : CustomCardModel(0, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Flavor>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Flavors", 4m)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await FlavorCmd.AddRandomFlavor(choiceContext, base.Owner, this, base.DynamicVars["Flavors"].IntValue);
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars["Flavors"].UpgradeValueBy(2m);
    }
}