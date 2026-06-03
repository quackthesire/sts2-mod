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

public class Cranberry() : FoodCardModel(1, CardType.Skill,
    CardRarity.Token, TargetType.AnyEnemy, sweet: 1, sour: 3, bitter: 1)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<Sweet>(), HoverTipFactory.FromPower<Sour>(), HoverTipFactory.FromPower<Bitter>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(3m)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, this.DynamicVars["WeakPower"].BaseValue, this.Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["WeakPower"].UpgradeValueBy(1m);
    }
}