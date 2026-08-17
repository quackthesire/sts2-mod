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

public class Corrode() : CustomCardModel(2, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    
    public sealed override string CustomPortraitPath => "res://cook_mod/corrode.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CustomKeywords.Prepare), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14m, ValueProp.Move), new CardsVar(5), new DynamicVar("Exhaust", 3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3").Execute(choiceContext);
        await PrepareCmd.Exhaust(choiceContext, this.Owner, this.DynamicVars.Cards.IntValue, 0, this.DynamicVars["Exhaust"].IntValue, cardPlay);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Cards.UpgradeValueBy(2m);
        this.DynamicVars["Exhaust"].UpgradeValueBy(1m);
    }
}