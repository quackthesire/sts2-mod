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

public class Sample() : CustomCardModel(1, CardType.Skill,
    CardRarity.Basic, TargetType.Self)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<GenericFlavor>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Flavors", 2m)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await FlavorCmd.AddRandomGenericFlavor(choiceContext, base.Owner, this, base.DynamicVars["Flavors"].IntValue);
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars["Flavors"].UpgradeValueBy(1m);
    }
}