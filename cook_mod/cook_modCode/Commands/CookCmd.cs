using BaseLib.Extensions;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Control;
using cook_mod.cook_modCode.Foods;
using cook_mod.cook_modCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;

namespace cook_mod.cook_modCode.Commands;

public static class CookCmd
{
    public static async Task Cook(PlayerChoiceContext choiceContext, Player player, CardModel? cardSource)
    {
        IEnumerable<CardModel> allCards = ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint);
        List<CardModel> allFoods = new List<CardModel>();
        List<CardModel> skip = new List<CardModel>();
        foreach (CardModel card in allCards)
        {
            if (card.Rarity == CardRarity.Token && card is FoodCardModel && !(card is Skip))
                allFoods.Add(card);
            else if (card is Skip)
                skip.Add(card);
        }
        List<CardModel> generateable = new List<CardModel>();
        List<CardModel> notGenerateable = new List<CardModel>();
        foreach (CardModel card in allFoods)
        {
            bool canCreate = true;
            if (FlavorsModel.Get((player)).sweet < ((FoodCardModel) card).sweet)
                canCreate = false;
            if (FlavorsModel.Get((player)).sour < ((FoodCardModel) card).sour)
                canCreate = false;
            if (FlavorsModel.Get((player)).salty < ((FoodCardModel) card).salty)
                canCreate = false;
            if (FlavorsModel.Get((player)).bitter < ((FoodCardModel) card).bitter)
                canCreate = false;
            if (FlavorsModel.Get((player)).spicy < ((FoodCardModel) card).spicy)
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
        generateable = CardFactory.GetDistinctForCombat(player, generateable, generateable.Count(), player.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        generateable = generateable.OrderBy(card => card.Title).ToList();
        if (player.HasPower<MasterChefPower>())
            CardCmd.Upgrade(generateable, CardPreviewStyle.GridLayout);
        notGenerateable = CardFactory.GetDistinctForCombat(player, notGenerateable, notGenerateable.Count(), player.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        notGenerateable = notGenerateable.OrderBy(card => card.Title).ToList();
        if (player.HasPower<MasterChefPower>())
            CardCmd.Upgrade(notGenerateable, CardPreviewStyle.GridLayout);
        skip = CardFactory.GetDistinctForCombat(player, skip, skip.Count(), player.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        await CardCmd.AfflictAndPreview<Bound>((IEnumerable<CardModel>) notGenerateable, 1m, CardPreviewStyle.None);
        allFoods.Clear();
        if (skip.Count > 0)
            allFoods.AddRange(skip);
        allFoods.AddRange(generateable);
        allFoods.AddRange(notGenerateable);
        CardSelectorPrefs prefs = new CardSelectorPrefs(new LocString("cards", "COOK_MOD-COOK.selectionScreenPrompt"), 1);
        foreach (CardModel card in await CardSelectCmd.FromSimpleGrid(choiceContext, (IReadOnlyList<CardModel>) allFoods, player, prefs))
        {
            bool canCreate = true;
            if (FlavorsModel.Get((player)).sweet < ((FoodCardModel) card).sweet)
                canCreate = false;
            if (FlavorsModel.Get((player)).sour < ((FoodCardModel) card).sour)
                canCreate = false;
            if (FlavorsModel.Get((player)).salty < ((FoodCardModel) card).salty)
                canCreate = false;
            if (FlavorsModel.Get((player)).bitter < ((FoodCardModel) card).bitter)
                canCreate = false;
            if (FlavorsModel.Get((player)).spicy < ((FoodCardModel) card).spicy)
                canCreate = false;
            if (canCreate)
            {
                if (!(card is Skip))
                {
                    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
                    await FlavorCmd.ChangeFlavor(choiceContext, player, cardSource, sweet: -((FoodCardModel)card).sweet,
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
}