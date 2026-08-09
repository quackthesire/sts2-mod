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

public class SteadyHand() : CustomCardModel(0, CardType.Skill,
    CardRarity.Common, TargetType.Self)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/steady_hand.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.AddRange(HoverTipFactory.FromEnchantment<Steady>());
            return tips;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (this.IsUpgraded)
            await CardPileCmd.Draw(choiceContext, 1m, this.Owner);
        CardModel card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, this.DynamicVars.Cards.IntValue), (Func<CardModel, bool>)(card => ModelDb.Enchantment<Steady>().CanEnchant(card) && card.Type != CardType.None && card.Enchantment == null), this)).FirstOrDefault<CardModel>();
        if (card == null)
        {
            card = (CardModel)null;
        }
        else
        {
            CardCmd.Enchant<Steady>(card, 1m);
            await EnchantCmd.OnEnchant(choiceContext, this.Owner, card, (CardModel) this);
            card = (CardModel)null;
        }
    }
}