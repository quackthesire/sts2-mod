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
using MegaCrit.Sts2.Core.Models.Powers;


namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class SquirtOfLemon() : CustomCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.AllEnemies)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/squirt_of_lemon.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>(), HoverTipFactory.FromPower<Sour>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(1m), new DynamicVar("Sour", 2)];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
        {
            await PowerCmd.Apply<WeakPower>(choiceContext, hittableEnemy, this.DynamicVars["WeakPower"].IntValue, this.Owner.Creature, (CardModel) this);
        }
        await FlavorCmd.ChangeFlavor(choiceContext, this.Owner, this, sour: this.DynamicVars["Sour"].IntValue);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars["Sour"].UpgradeValueBy(1m);
    }
}