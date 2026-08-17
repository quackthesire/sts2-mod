using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Cards;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Foods;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace cook_mod.cook_modCode.Relic;
[Pool(typeof(TheCookRelicPool))]

public class Lunchbox : CustomRelicModel
{
    protected override string BigIconPath => "res://cook_mod/lunchbox.png";
    
    public override string PackedIconPath => "res://cook_mod/lunchbox.png";
    
    protected override string PackedIconOutlinePath => "res://cook_mod/lunchbox.png";
    
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber > 1)
            return;
        IEnumerable<CardModel> allCards = ModelDb.CardPool<TokenCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint);
        List<CardModel> allFoods = new List<CardModel>();
        foreach (CardModel card in allCards)
        {
            if (card.Rarity == CardRarity.Token && card is FoodCardModel && !(card is Skip))
                allFoods.Add(card);
        }
        allFoods = CardFactory.GetDistinctForCombat(player, allFoods, 1, player.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) allFoods, PileType.Hand, this.Owner);
    }
}