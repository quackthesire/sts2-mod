using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
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
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;


namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class Rot() : CustomCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<VulnerablePower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1m), new PowerVar<VulnerablePower>(1m)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> allCards = ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint);
        List<CardModel> rotCards = new List<CardModel>();
        foreach (CardModel card in allCards)
        {
            if (card.Rarity == CardRarity.Token && card is RotCardModel)
                rotCards.Add(card);
        }
        rotCards = CardFactory.GetDistinctForCombat(this.Owner, rotCards, rotCards.Count(), this.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        rotCards = rotCards.OrderByDescending(card => card.Title).ToList();
        if (this.IsUpgraded)
            CardCmd.Upgrade(rotCards, CardPreviewStyle.GridLayout);
        CardModel chosen = await CardSelectCmd.FromChooseACardScreen(choiceContext, (IReadOnlyList<CardModel>) rotCards, this.Owner, false);
        if (chosen == null)
            return;
        if (chosen is Weak)
            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, this.DynamicVars["WeakPower"].BaseValue, this.Owner.Creature, (CardModel) this);
        if (chosen is Vulnerable)
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, this.DynamicVars["VulnerablePower"].BaseValue, this.Owner.Creature, (CardModel) this);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["WeakPower"].UpgradeValueBy(1m);
        this.DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
    }
}