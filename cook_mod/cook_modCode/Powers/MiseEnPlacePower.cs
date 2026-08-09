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
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace cook_mod.cook_modCode.Powers;
public class MiseEnPlacePower : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/mise_en_place_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/mise_en_place_power.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.AddRange(HoverTipFactory.FromEnchantment<Adroit>(this.Amount));
            return tips;
        }
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != this.Owner.Player)
            return;
        CardModel card = (await CardSelectCmd.FromHand(choiceContext, this.Owner.Player, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1), (Func<CardModel, bool>)(card => ModelDb.Enchantment<Adroit>().CanEnchant(card) && card.Type != CardType.None && card.Enchantment == null), this)).FirstOrDefault<CardModel>();
        if (card == null)
        {
            card = (CardModel)null;
        }
        else
        {
            CardCmd.Enchant<Adroit>(card, this.Amount);
            await EnchantCmd.OnEnchant(choiceContext, player, card, null);
            card = (CardModel)null;
        }
    }
}