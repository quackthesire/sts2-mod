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
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;


namespace cook_mod.cook_modCode.Foods;

[Pool(typeof(TokenCardPool))]

public class Sauerkraut() : FoodCardModel(0, CardType.Skill,
    CardRarity.Token, TargetType.AllEnemies, sour: 3, salty: 1)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SauerkrautPower>(), HoverTipFactory.FromPower<Sour>(), HoverTipFactory.FromPower<Salty>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SauerkrautPower>(8m)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<SauerkrautPower>(choiceContext, hittableEnemy, this.DynamicVars["SauerkrautPower"].BaseValue, this.Owner.Creature, (CardModel) this);
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["SauerkrautPower"].UpgradeValueBy(3m);
    }
}