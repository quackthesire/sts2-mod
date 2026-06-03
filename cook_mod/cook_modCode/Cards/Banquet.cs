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
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;


namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class Banquet() : CustomCardModel(3, CardType.Attack,
    CardRarity.Rare, TargetType.AllEnemies), IOnEnchant
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(20m, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3").Execute(choiceContext);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(5m);
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(this.Owner.Creature))
            return Task.CompletedTask;
        this.EnergyCost.SetThisCombat(3);
        foreach (CardModel card in PileType.Hand.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (card.Enchantment != null && card != this)
                this.EnergyCost.AddThisCombat(-1);
        }
        GD.Print("Turn Start!" +  this.EnergyCost.Canonical);
        return Task.CompletedTask;
    }

    public override Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Owner != this.Owner || card.Enchantment == null || card == this || card.Pile == null || !(card.Pile.Type is PileType.Hand) || (oldPileType is PileType.Hand))
            return Task.CompletedTask;
        this.EnergyCost.AddThisCombat(-1);
        GD.Print("Pile Change!" + this.EnergyCost.Canonical);
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != this.Owner || cardPlay.Card.Enchantment == null || cardPlay.Card == this || cardPlay.Card.Keywords.Contains(CardKeyword.Exhaust))
            return Task.CompletedTask;
        this.EnergyCost.AddThisCombat(1);
        GD.Print("Card Play!" +  this.EnergyCost.Canonical);
        return Task.CompletedTask;
    }

    public override Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != this.Owner || card.Enchantment == null || card == this)
            return Task.CompletedTask;
        this.EnergyCost.AddThisCombat(1);
        GD.Print("Card Discarded!" + this.EnergyCost.Canonical);
        return Task.CompletedTask;
    }

    public override Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner != this.Owner || card.Enchantment == null || card == this)
            return Task.CompletedTask;
        this.EnergyCost.AddThisCombat(1);
        GD.Print("Card Exhausted!" + this.EnergyCost.Canonical);
        return Task.CompletedTask;
    }

    public Task OnEnchant(PlayerChoiceContext ctx, Player player, CardModel card, CardModel? cardSource)
    {
        if (card.Owner != this.Owner || card.Enchantment == null || card == this)
            return Task.CompletedTask;
        this.EnergyCost.AddThisCombat(-1);
        GD.Print("Card Enchanted!" + this.EnergyCost.Canonical);
        return Task.CompletedTask;
    }
    
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || this.IsClone)
            return Task.CompletedTask;
        foreach (CardModel cardInHand in PileType.Hand.GetPile(this.Owner).Cards.ToList<CardModel>())
        {
            if (cardInHand.Enchantment != null && cardInHand != this)
                this.EnergyCost.AddThisCombat(-1);
        }
        return Task.CompletedTask;
    }
}