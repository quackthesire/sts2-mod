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

public class BatchPrep() : CustomCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    
    private const string _calculatedEnergyKey = "CalculatedEnergy";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [this.EnergyHoverTip];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        (DynamicVar)new EnergyVar(1),
        (DynamicVar)new CalculationBaseVar(0m),
        (DynamicVar)new CalculationExtraVar(1m),
        (DynamicVar)new CalculatedVar("CalculatedEnergy").WithMultiplier(
            (Func<CardModel, Creature, Decimal>)((card, _) =>
            {
                int count = PileType.Hand.GetPile(card.Owner).Cards.Count;
                CardPile pile = card.Pile;
                if ((pile != null ? (pile.Type == PileType.Hand ? 1 : 0) : 0) != 0)
                    --count;
                count /= 2;
                return (Decimal) count;
            }))
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy((int) ((CalculatedVar) this.DynamicVars["CalculatedEnergy"]).Calculate(this.Owner.Creature), this.Owner);
    }
    
    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}