using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
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

public class Pufferfish() : FoodCardModel(1, CardType.Skill,
    CardRarity.Token, TargetType.AllEnemies, salty: 1, bitter: 3)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/pufferfish.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PoisonPower>(), HoverTipFactory.FromPower<BleedPower>(), HoverTipFactory.FromPower<Salty>(), HoverTipFactory.FromPower<Bitter>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PoisonPower>(4m), new PowerVar<BleedPower>(3m), new ExhaustiveVar(3m)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (Creature hittableEnemy in this.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<PoisonPower>(choiceContext, hittableEnemy, this.DynamicVars["PoisonPower"].BaseValue,
                this.Owner.Creature, this);
            await PowerCmd.Apply<BleedPower>(choiceContext, hittableEnemy, this.DynamicVars["BleedPower"].BaseValue,
                this.Owner.Creature, this);
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["PoisonPower"].UpgradeValueBy(2m);
        this.DynamicVars["BleedPower"].UpgradeValueBy(1m);
    }
}