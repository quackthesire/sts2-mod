using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace cook_mod.cook_modCode.Powers;

public class SugarRushPower : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/sugar_rush_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/sugar_rush_power.png";

    private int _cardsLeft;
    
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override int DisplayAmount => this._cardsLeft;

    public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        this._cardsLeft = this.Amount;
        this.InvokeDisplayAmountChanged();
    }

    public override async Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target,
        Creature? applier,
        CardModel? cardSource)
    {
        if (!(power is SugarRushPower) || target != this.Owner)
            return;
        this._cardsLeft += (int) amount;
        this.InvokeDisplayAmountChanged();
    }

    public override bool ShouldPlay(CardModel card, AutoPlayType _)
    {
        return card.Owner.Creature != this.Owner || this._cardsLeft > 0;
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != this.Owner.Player)
            return Task.CompletedTask;
        --this._cardsLeft;
        this.InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains<Creature>(this.Owner))
            return;
        await PowerCmd.Remove((PowerModel) this);
    }
}