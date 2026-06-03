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

public class Calamari() : CustomCardModel(0, CardType.Skill,
    CardRarity.Rare, TargetType.Self)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.AddRange(HoverTipFactory.FromEnchantment<Inky>());
            return tips;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> cards = (await CardSelectCmd.FromHand(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0, this.DynamicVars.Cards.IntValue), (Func<CardModel, bool>)(card => ModelDb.Enchantment<Inky>().CanEnchant(card) && card.Type == CardType.Attack), this)).ToList();
        if (cards == null)
        {
            cards = (List<CardModel>)null;
        }
        else
        {
            foreach (CardModel card in cards)
            {
                CardCmd.Enchant<Inky>(card, 1m);
                await EnchantCmd.OnEnchant(choiceContext, this.Owner, card, (CardModel) this);
            }
            cards = (List<CardModel>)null;
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}