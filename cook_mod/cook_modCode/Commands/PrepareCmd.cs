using BaseLib.Extensions;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Relic;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Commands;

public static class PrepareCmd
{
    public static async Task Look(PlayerChoiceContext choiceContext, Player player, int amount, CardPlay? cardPlay)
    {

        if (amount <= 0) return;

        if (player.GetRelic<Almanac>() != null) amount += 2;

        var drawPile = PileType.Draw.GetPile(player);
        var cardsToPrepare = drawPile.Cards.Take(amount).ToList();

        if (cardsToPrepare.Count == 0) return;
        LocString locstring = new LocString("combat_messages", "PREPARE-LOOK");
        var prefs = new CardSelectorPrefs(
            locstring,
            0,
            cardsToPrepare.Count
        );

        await CardSelectCmd.FromSimpleGrid(choiceContext, cardsToPrepare, player, prefs);
        await CookHook.OnPrepared(choiceContext, player, amount, 0, cardPlay);
    }
    
    public static async Task Discard(PlayerChoiceContext choiceContext, Player player, int amount, int minDiscard, int maxDiscard, CardPlay? cardPlay)
    {

        if (amount <= 0) return;
        
        if (player.GetRelic<Almanac>() != null)
        {
            if (minDiscard == 0 && maxDiscard == amount)
                maxDiscard += 2;
            amount += 2;
        }

        var drawPile = PileType.Draw.GetPile(player);
        var cardsToPrepare = drawPile.Cards.Take(amount).ToList();

        if (maxDiscard > cardsToPrepare.Count)
            maxDiscard = cardsToPrepare.Count;
        
        if (cardsToPrepare.Count == 0) return;
        LocString locstring = new LocString("combat_messages", "PREPARE-DISCARD-FORCED");
        if (minDiscard != maxDiscard)
            locstring = new LocString("combat_messages", "PREPARE-DISCARD-FREE");
        var prefs = new CardSelectorPrefs(
            locstring,
            minDiscard,
            maxDiscard
        );

        var cardsToDiscard = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            cardsToPrepare,
            player,
            prefs
        )).ToList();
        foreach (var card in cardsToDiscard)
        {
            await CardCmd.Discard(choiceContext, card);
            PrepareModel.Add(player, 1);
        }
        await CookHook.OnPrepared(choiceContext, player, amount, cardsToDiscard.Count, cardPlay);
    }
    
    public static async Task Play(PlayerChoiceContext choiceContext, Player player, int amount, int minPlay, int maxPlay, int times, CardPlay? cardPlay)
    {

        if (amount <= 0) return;
        
        if (player.GetRelic<Almanac>() != null)
        {
            if (minPlay == 0 && maxPlay == amount)
                maxPlay += 2;
            amount += 2;
        }

        var drawPile = PileType.Draw.GetPile(player);
        var cardsToPrepare = drawPile.Cards.Take(amount).ToList();

        if (maxPlay > cardsToPrepare.Count)
            maxPlay = cardsToPrepare.Count;
        
        if (cardsToPrepare.Count == 0) return;
        LocString locstring = new LocString("combat_messages", "PREPARE-PLAY-FORCED");
        if (minPlay != maxPlay)
            locstring = new LocString("combat_messages", "PREPARE-PLAY-FREE");
        var prefs = new CardSelectorPrefs(
            locstring,
            minPlay,
            maxPlay
        );

        var cardsToPlay = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            cardsToPrepare,
            player,
            prefs
        )).ToList();
        foreach (var card in cardsToPlay)
        {
            for (int i = 0; i < times; ++i)
                await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
            PrepareModel.Add(player, 1);
        }
        await CookHook.OnPrepared(choiceContext, player, amount, cardsToPlay.Count, cardPlay);
    }
    
    public static async Task Exhaust(PlayerChoiceContext choiceContext, Player player, int amount, int minExhaust, int maxExhaust, CardPlay? cardPlay)
    {

        if (amount <= 0) return;
        
        if (player.GetRelic<Almanac>() != null)
        {
            if (minExhaust == 0 && maxExhaust == amount)
                maxExhaust += 2;
            amount += 2;
        }

        var drawPile = PileType.Draw.GetPile(player);
        var cardsToPrepare = drawPile.Cards.Take(amount).ToList();

        if (maxExhaust > cardsToPrepare.Count)
            maxExhaust = cardsToPrepare.Count;
        
        if (cardsToPrepare.Count == 0) return;
        LocString locstring = new LocString("combat_messages", "PREPARE-EXHAUST-FORCED");
        if (minExhaust != maxExhaust)
            locstring = new LocString("combat_messages", "PREPARE-EXHAUST-FREE");
        var prefs = new CardSelectorPrefs(
            locstring,
            minExhaust,
            maxExhaust
        );

        var cardsToExhaust = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            cardsToPrepare,
            player,
            prefs
        )).ToList();
        foreach (var card in cardsToExhaust)
        {
            await CardCmd.Exhaust(choiceContext, card);
            PrepareModel.Add(player, 1);
        }
        await CookHook.OnPrepared(choiceContext, player, amount, cardsToExhaust.Count, cardPlay);
    }
    
    public static async Task PutIntoHand(PlayerChoiceContext choiceContext, Player player, int amount, int minCard, int maxCard, CardPlay? cardPlay)
    {

        if (amount <= 0) return;
        
        if (player.GetRelic<Almanac>() != null)
        {
            if (minCard == 0 && maxCard == amount)
                maxCard += 2;
            amount += 2;
        }

        var drawPile = PileType.Draw.GetPile(player);
        var cardsToPrepare = drawPile.Cards.Take(amount).ToList();
        
        if (maxCard > cardsToPrepare.Count)
            maxCard = cardsToPrepare.Count;

        if (cardsToPrepare.Count == 0) return;
        LocString locstring = new LocString("combat_messages", "PREPARE-HAND-FORCED");
        if (minCard != maxCard)
            locstring = new LocString("combat_messages", "PREPARE-HAND-FREE");
        var prefs = new CardSelectorPrefs(
            locstring,
            minCard,
            maxCard
        );

        var cardsToPutIntoHand = (await CardSelectCmd.FromSimpleGrid(
            choiceContext,
            cardsToPrepare,
            player,
            prefs
        )).ToList();
        foreach (var card in cardsToPutIntoHand)
        {
            await CardPileCmd.Add(card, PileType.Hand);
            PrepareModel.Add(player, 1);
        }
        await CookHook.OnPrepared(choiceContext, player, amount, cardsToPutIntoHand.Count, cardPlay);
    }
}