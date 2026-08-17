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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Players;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class Garnish() : CustomCardModel(2, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/garnish.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m, ValueProp.Move), new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3").Execute(choiceContext);
        await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(2m);
        this.DynamicVars.Cards.UpgradeValueBy(1m);
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || this.IsClone || CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.CardPlay.Card.Keywords.Contains(CardKeyword.Retain) && e.CardPlay.Player == this.Owner && e.HappenedThisTurn(this.CombatState))) == 0)
            return Task.CompletedTask;
        this.EnergyCost.SetThisTurnOrUntilPlayed(0);
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!cardPlay.Card.Keywords.Contains(CardKeyword.Retain) || cardPlay.Card.Owner != this.Owner)
            return Task.CompletedTask;
        this.EnergyCost.SetThisTurnOrUntilPlayed(0);
        return Task.CompletedTask;
    }
}