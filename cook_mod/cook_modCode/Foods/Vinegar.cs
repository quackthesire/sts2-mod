using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
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
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace cook_mod.cook_modCode.Foods;

[Pool(typeof(TokenCardPool))]

public class Vinegar() : FoodCardModel(0, CardType.Skill,
    CardRarity.Token, TargetType.Self, sour: 4)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.Add(HoverTipFactory.FromPower<Sour>());
            tips.AddRange(HoverTipFactory.FromEnchantment<Sharp>(this.DynamicVars["Sharp"].IntValue));
            return tips;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Sharp", 7m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1), (Func<CardModel, bool>)(card => ModelDb.Enchantment<Sharp>().CanEnchant(card) && card.Type != CardType.None), this)).FirstOrDefault<CardModel>();
        if (card == null)
        {
            card = (CardModel)null;
        }
        else
        {
            CardCmd.Enchant<Sharp>(card, this.DynamicVars["Sharp"].IntValue);
            await EnchantCmd.OnEnchant(choiceContext, this.Owner, card, (CardModel) this);
            card = (CardModel)null;
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["Sharp"].UpgradeValueBy(3m);
    }
}