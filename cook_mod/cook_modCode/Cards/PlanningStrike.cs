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
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Players;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class PlanningStrike() : CustomCardModel(1, CardType.Attack,
    CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags
    {
        get => new HashSet<CardTag>() { CardTag.Strike };
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Prepare>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move), new CardsVar(3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3").Execute(choiceContext);
        await PrepareCmd.Look(choiceContext, base.Owner, base.DynamicVars.Cards.IntValue, cardPlay);
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
        base.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}