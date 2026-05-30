using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Foods;
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
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Unlocks;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TokenCardPool))]

public class Cook() : CustomCardModel(0, CardType.None,
    CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<CardModel> allCards = ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint);
        List<CardModel> allFoods = new List<CardModel>();
        CardModel skip = null;
        foreach (CardModel card in allCards)
        {
            if (card.Rarity == CardRarity.Token && card is FoodCardModel && !(card is Skip))
                allFoods.Add(card);
            else if (card is Skip)
                skip = card;
        }
        List<CardModel> generateable = new List<CardModel>();
        List<CardModel> notGenerateable = new List<CardModel>();
        foreach (CardModel card in allFoods)
        {
            bool canCreate = true;
            if (FlavorsModel.Get((base.Owner)).sweet < ((FoodCardModel) card).sweet)
                canCreate = false;
            if (FlavorsModel.Get((base.Owner)).sour < ((FoodCardModel) card).sour)
                canCreate = false;
            if (FlavorsModel.Get((base.Owner)).salty < ((FoodCardModel) card).salty)
                canCreate = false;
            if (FlavorsModel.Get((base.Owner)).bitter < ((FoodCardModel) card).bitter)
                canCreate = false;
            if (FlavorsModel.Get((base.Owner)).spicy < ((FoodCardModel) card).spicy)
                canCreate = false;
            if (canCreate)
            {
                generateable.Add(card);
            }
            else
            {
                notGenerateable.Add(card);
            }
        }
        generateable = CardFactory.GetDistinctForCombat(base.Owner, generateable, generateable.Count(), base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        generateable = generateable.OrderBy(card => card.Title).ToList();
        if (base.Owner.HasPower<MasterChefPower>())
            CardCmd.Upgrade(generateable, CardPreviewStyle.GridLayout);
        notGenerateable = CardFactory.GetDistinctForCombat(base.Owner, notGenerateable, notGenerateable.Count(), base.Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        notGenerateable = notGenerateable.OrderBy(card => card.Title).ToList();
        if (base.Owner.HasPower<MasterChefPower>())
            CardCmd.Upgrade(notGenerateable, CardPreviewStyle.GridLayout);
        await CardCmd.AfflictAndPreview<Bound>((IEnumerable<CardModel>) notGenerateable, 1m, CardPreviewStyle.None);
        allFoods.Clear();
        if (skip != null)
            allFoods.Add(skip);
        allFoods.AddRange(generateable);
        allFoods.AddRange(notGenerateable);
        CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("cards", "COOK_MOD-COOK.selectionScreenPrompt"), 1);
        foreach (CardModel card in await CardSelectCmd.FromSimpleGrid(choiceContext, (IReadOnlyList<CardModel>) allFoods, this.Owner, prefs))
        {
            bool canCreate = true;
            if (FlavorsModel.Get((base.Owner)).sweet < ((FoodCardModel) card).sweet)
                canCreate = false;
            if (FlavorsModel.Get((base.Owner)).sour < ((FoodCardModel) card).sour)
                canCreate = false;
            if (FlavorsModel.Get((base.Owner)).salty < ((FoodCardModel) card).salty)
                canCreate = false;
            if (FlavorsModel.Get((base.Owner)).bitter < ((FoodCardModel) card).bitter)
                canCreate = false;
            if (FlavorsModel.Get((base.Owner)).spicy < ((FoodCardModel) card).spicy)
                canCreate = false;
            if (canCreate)
            {
                if (!(card is Skip))
                {
                    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner);
                    await FlavorCmd.ChangeFlavor(choiceContext, base.Owner, this, sweet: -((FoodCardModel)card).sweet,
                    sour: -((FoodCardModel)card).sour, salty: -((FoodCardModel)card).salty,
                    bitter: -((FoodCardModel)card).bitter,
                    spicy: -((FoodCardModel)card).spicy);
                }
            }
            else
            {
                LocString playerDialogueLine = new LocString("combat_messages", "NOT_ENOUGH_FLAVORS");
                if (playerDialogueLine != null)
                {
                    NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NThoughtBubbleVfx.Create(playerDialogueLine.GetFormattedText(), card.Owner.Creature, 1.0));
                }
            }
        }
    }
    
    protected override PileType GetResultPileTypeForCardPlay()
    {
        return PileType.Hand;
    }
}