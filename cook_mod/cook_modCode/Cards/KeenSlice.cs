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

public class KeenSlice() : CustomCardModel(1, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.AddRange(HoverTipFactory.FromEnchantment<Sharp>(this.DynamicVars["Sharp"].IntValue));
            return tips;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move), new DynamicVar("Sharp", 3m)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3").Execute(choiceContext);
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
        this.DynamicVars["Sharp"].UpgradeValueBy(2m);
    }
}