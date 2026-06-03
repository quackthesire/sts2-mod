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
using cook_mod.cook_modCode.Cards;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class TripleStab() : CustomCardModel(2, CardType.Attack,
    CardRarity.Rare, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3m, ValueProp.Move), new PowerVar<BleedPower>(1m), new RepeatVar(3)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        for (int i = 0; i < this.DynamicVars.Repeat.IntValue; i++)
        {
            await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3")
                .Execute(choiceContext);
            await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target, this.DynamicVars["BleedPower"].BaseValue, this.Owner.Creature, this);
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["BleedPower"].UpgradeValueBy(1m);
    }
}