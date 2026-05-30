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

public class Brine() : CustomCardModel(1, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Sour>(), HoverTipFactory.FromPower<Salty>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4m, ValueProp.Move), new RepeatVar(2), new DynamicVar("Sour", 1), new DynamicVar("Salty", 1)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(base.DynamicVars.Repeat.IntValue).FromCard((CardModel) this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3").Execute(choiceContext);
        await FlavorCmd.ChangeFlavor(choiceContext, base.Owner, this, sour: base.DynamicVars["Sour"].IntValue, salty: base.DynamicVars["Salty"].IntValue);
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}