using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
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
public class AcquiredTastePower : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/acquired_taste_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/acquired_taste_power.png";
    
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(CardKeyword.Retain)
    ];

    private List<CardModel> alreadyChecked = new List<CardModel>();

    public override Task AfterFlush(PlayerChoiceContext choiceContext, Player player,
        IReadOnlyCollection<CardModel> flushedCards,
        IReadOnlyCollection<CardModel> retainedCards)
    {
        if (player.Creature != Owner) return Task.CompletedTask;
        foreach (var card in retainedCards)
        {
            if (!(alreadyChecked.Contains(card)))
                card.EnergyCost.AddThisCombat(-Amount);
        }
        return Task.CompletedTask;
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != Owner.Side || Owner.Player == null) return;
        if (Owner.GetPower<RetainHandPower>() == null) return;
        foreach (var card in PileType.Hand.GetPile(Owner.Player).Cards)
        {
            card.EnergyCost.AddThisCombat(-Amount);
            alreadyChecked.Add(card);
        }
        await Task.CompletedTask;
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        alreadyChecked.Clear();
    }
}