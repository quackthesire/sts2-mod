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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class BladeCache() : CustomCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/blade_cache.png";

    private bool Upgrade()
    {
        if (this.CombatState == null)
            return false;
            
        bool upgraded = true;
        
        foreach (Creature hittableEnemy in this.CombatState.HittableEnemies)
        {
            if (!hittableEnemy.HasPower<BleedPower>())
            {
                upgraded = false;
            }
        }

        return upgraded;
    }
    
    protected override bool ShouldGlowGoldInternal => Upgrade();
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Knife>(Upgrade()), HoverTipFactory.FromPower<BleedPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), new DynamicVar("Knives", 3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreateCard.GiveCards<Knife>(Owner, this.DynamicVars["Knives"].IntValue, PileType.Hand, upgraded: Upgrade());
    }
    
    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}