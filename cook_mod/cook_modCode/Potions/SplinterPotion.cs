using BaseLib.Abstracts;
using BaseLib.Utils;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace cook_mod.cook_modCode.Potions;

[Pool(typeof(TheCookPotionPool))]

public class SplinterPotion : CustomPotionModel
{
    public sealed override string CustomPackedImagePath => "res://cook_mod/splinter_potion.png";

    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AllEnemies;
    
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<BleedPower>(4m)];
    
    protected override async Task OnUse(PlayerChoiceContext ctx, Creature? target)
    {
        IReadOnlyList<Creature> targets = this.Owner.Creature.CombatState.HittableEnemies;
        foreach (Creature target1 in (IEnumerable<Creature>) targets)
        {
            NCombatRoom instance = NCombatRoom.Instance;
            if (instance != null)
                instance.CombatVfxContainer.AddChildSafely((Node) NSmokePuffVfx.Create(target1, NSmokePuffVfx.SmokePuffColor.Green));
        }
        await PowerCmd.Apply<BleedPower>(ctx, (IEnumerable<Creature>) targets, (Decimal) this.DynamicVars["BleedPower"].IntValue, this.Owner.Creature, (CardModel) null);
        targets = (IReadOnlyList<Creature>) null;
    }
}