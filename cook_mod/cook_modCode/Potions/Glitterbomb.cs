using BaseLib.Abstracts;
using BaseLib.Utils;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
using cook_mod.cook_modCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace cook_mod.cook_modCode.Potions;

[Pool(typeof(TheCookPotionPool))]

public class Glitterbomb : CustomPotionModel
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    
    public override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.AddRange(HoverTipFactory.FromEnchantment<Glam>());
            return tips;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    
    protected override async Task OnUse(PlayerChoiceContext ctx, Creature? target)
    {
        if (target?.Player == null) return;
        List<CardModel> cards = (await CardSelectCmd.FromHand(ctx, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, this.DynamicVars.Cards.IntValue), (Func<CardModel, bool>)(card => ModelDb.Enchantment<Glam>().CanEnchant(card) && card.Type != CardType.None), this)).ToList();
        if (cards == null)
        {
            cards = (List<CardModel>)null;
        }
        else
        {
            foreach (CardModel card in cards)
            {
                CardCmd.Enchant<Glam>(card, 1m);
                await EnchantCmd.OnEnchant(ctx, this.Owner, card, (CardModel) null);
            }
            cards = (List<CardModel>)null;
        }
    }
}