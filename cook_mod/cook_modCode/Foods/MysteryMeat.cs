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


namespace cook_mod.cook_modCode.Foods;

[Pool(typeof(TokenCardPool))]

public class MysteryMeat() : FoodCardModel(0, CardType.Skill,
    CardRarity.Token, TargetType.Self, sweet: 1, sour: 1, salty: 1, bitter: 1, spicy: 1)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Sweet>(), HoverTipFactory.FromPower<Sour>(), HoverTipFactory.FromPower<Salty>(), HoverTipFactory.FromPower<Bitter>(), HoverTipFactory.FromPower<Spicy>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (CardModel card in CardFactory.GetDistinctForCombat(base.Owner, base.Owner.Character.CardPool.GetUnlockedCards(base.Owner.UnlockState, base.RunState.CardMultiplayerConstraint), base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration))
        {
            card.SetToFreeThisTurn();
            CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, base.Owner);
        }
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(1);
    }
}