using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Cards;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace cook_mod.cook_modCode.Relic;
[Pool(typeof(TheCookRelicPool))]
public class Aromatics : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars
    {
        get
        {
            return (IEnumerable<DynamicVar>) [(DynamicVar) new CardsVar(1)];
        }
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> items = new List<IHoverTip>();
            items.Add(HoverTipFactory.FromPower<GenericFlavor>());
            items.AddRange(HoverTipFactory.FromCardWithCardHoverTips<Cook>());
            return (IEnumerable<IHoverTip>) items;
        }
    }

    public override async Task BeforeHandDraw(
        Player player,
        PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        Aromatics aromatics = this;
        if (player != aromatics.Owner || aromatics.Owner.PlayerCombatState.TurnNumber != 1)
            return;
        List<CardModel> cards = new List<CardModel>();
        for (int index = 0; index < aromatics.DynamicVars.Cards.IntValue; ++index)
            cards.Add((CardModel) aromatics.Owner.Creature.CombatState.CreateCard<Cook>(aromatics.Owner));
        IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) cards, PileType.Hand, aromatics.Owner);
        await FlavorCmd.AddRandomGenericFlavor(choiceContext, player, null, 3);
    }
    
    public override RelicModel? GetUpgradeReplacement()
    {
        return ModelDb.Relic<FlavorBase>();
    }
}