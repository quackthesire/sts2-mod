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
using cook_mod.cook_modCode.Cards;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class GrowingGash() : CustomCardModel(1, CardType.Power,
    CardRarity.Uncommon, TargetType.Self)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<GrowingGashPower>(2m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<GrowingGashPower>(choiceContext, base.Owner.Creature, base.DynamicVars["GrowingGashPower"].BaseValue, base.Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars["GrowingGashPower"].UpgradeValueBy(1m);
    }
}