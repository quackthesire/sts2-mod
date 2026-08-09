using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
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

public class SoySauce() : FoodCardModel(0, CardType.Skill,
    CardRarity.Token, TargetType.Self, salty: 5)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/soy_sauce.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.Add(HoverTipFactory.FromPower<Salty>());
            tips.AddRange(HoverTipFactory.FromEnchantment<Adroit>(this.DynamicVars["Adroit"].IntValue));
            return tips;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Adroit", 3m), new ExhaustiveVar(3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card = (await CardSelectCmd.FromHand(choiceContext, this.Owner, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1), (Func<CardModel, bool>)(card => ModelDb.Enchantment<Adroit>().CanEnchant(card) && card.Type != CardType.None && card.Enchantment == null), this)).FirstOrDefault<CardModel>();
        if (card == null)
        {
            card = (CardModel)null;
        }
        else
        {
            CardCmd.Enchant<Adroit>(card, this.DynamicVars["Adroit"].IntValue);
            await EnchantCmd.OnEnchant(choiceContext, this.Owner, card, (CardModel) this);
            card = (CardModel)null;
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["Adroit"].UpgradeValueBy(1m);
    }
}