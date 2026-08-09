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
using MegaCrit.Sts2.Core.Models.Enchantments;


namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class RushOrder() : CustomCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/rush_order.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.AddRange(HoverTipFactory.FromEnchantment<Swift>(this.DynamicVars["Swift"].IntValue));
            return tips;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Swift", 2m), new CardsVar(1)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card = (await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner)).FirstOrDefault<CardModel>();
        if (card == null || !ModelDb.Enchantment<Swift>().CanEnchant(card) || card.Type == CardType.None || card.Enchantment != null)
        {
            card = (CardModel)null;
        }
        else
        {
            CardCmd.Enchant<Swift>(card, this.DynamicVars["Swift"].IntValue);
            await EnchantCmd.OnEnchant(choiceContext, this.Owner, card, (CardModel) this);
            card = (CardModel)null;
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["Swift"].UpgradeValueBy(1m);
    }
}