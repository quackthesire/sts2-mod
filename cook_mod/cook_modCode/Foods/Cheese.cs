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

public class Cheese() : FoodCardModel(3, CardType.Attack,
    CardRarity.Token, TargetType.AllEnemies, sour: 1, salty: 3)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Sour>(), HoverTipFactory.FromPower<Salty>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(60m, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).TargetingAllOpponents(base.CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
    }
    
    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(15m);
    }
}