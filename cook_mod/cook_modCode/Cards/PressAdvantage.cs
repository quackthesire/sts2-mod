using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
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


namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class PressAdvantage() : CustomCardModel(1, CardType.Attack,
    CardRarity.Common, TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal
    {
        get
        {
            int count = PileType.Hand.GetPile(this.Owner).Cards.Count;
            CardPile pile = this.Pile;
            if ((pile != null ? (pile.Type == PileType.Hand ? 1 : 0) : 0) != 0)
                --count;
            int times = 0;
            if (count >= 6)
                times++;
            if (count >= 8)
                times++;
            return times > 0;
        }
    }
    
    private const string _calculatedHitsKey = "CalculatedHits";
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        (DynamicVar)new DamageVar(7m, ValueProp.Move),
        (DynamicVar)new CalculationBaseVar(1m),
        (DynamicVar)new CalculationExtraVar(1m),
        (DynamicVar)new CalculatedVar("CalculatedHits").WithMultiplier(
            (Func<CardModel, Creature, Decimal>)((card, _) =>
            {
                int count = PileType.Hand.GetPile(card.Owner).Cards.Count;
                CardPile pile = card.Pile;
                if ((pile != null ? (pile.Type == PileType.Hand ? 1 : 0) : 0) != 0)
                    --count;
                int times = 0;
                if (count >= 6)
                    times++;
                if (count >= 8)
                    times++;
                return (Decimal) times;
            }))
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount((int) ((CalculatedVar) this.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard((CardModel) this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3").Execute(choiceContext);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(2m);
    }
}