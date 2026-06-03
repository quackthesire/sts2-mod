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

public class CrystallineGrid() : CustomCardModel(1, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Sweet>(), HoverTipFactory.FromPower<Salty>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Sweet", 1), new DynamicVar("Salty", 3)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await FlavorCmd.ChangeFlavor(choiceContext, this.Owner, this, sweet: this.DynamicVars["Sweet"].IntValue, salty: this.DynamicVars["Salty"].IntValue);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["Sweet"].UpgradeValueBy(1m);
    }
}