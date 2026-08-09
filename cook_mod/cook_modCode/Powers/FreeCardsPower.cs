using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace cook_mod.cook_modCode.Powers;
public class FreeCardsPower : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/free_cards_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/free_cards_power.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override bool TryModifyEnergyCostInCombatLate(
        CardModel card,
        Decimal originalCost,
        out Decimal modifiedCost)
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != this.Owner)
            return false;
        PileType? type = card.Pile?.Type;
        bool flag;
        if (type.HasValue)
        {
            switch (type.GetValueOrDefault())
            {
                case PileType.Hand:
                case PileType.Play:
                    flag = true;
                    goto label_6;
            }
        }
        flag = false;
        label_6:
        if (!flag)
            return false;
        modifiedCost = 0M;
        return true;
    }

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        FreeCardsPower power = this;
        if (cardPlay.Card.Owner.Creature != power.Owner)
            return;
        PileType? type = cardPlay.Card.Pile?.Type;
        bool flag;
        if (type.HasValue)
        {
            switch (type.GetValueOrDefault())
            {
                case PileType.Hand:
                case PileType.Play:
                    flag = true;
                    goto label_6;
            }
        }
        flag = false;
        label_6:
        if (!flag)
            return;
        await PowerCmd.Decrement((PowerModel) power);
    }
}