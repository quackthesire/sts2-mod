using BaseLib.Abstracts;
using BaseLib.Utils;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
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

public class HerbalExtract : CustomPotionModel
{
    public sealed override string CustomPackedImagePath => "res://cook_mod/herbal_extract.png";
    public override PotionRarity Rarity => PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;
    
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CustomKeywords.Generic_Flavor), HoverTipFactory.FromPower<Spicy>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Flavors", 2m)];
    
    protected override async Task OnUse(PlayerChoiceContext ctx, Creature? target)
    {
        if (target?.Player == null) return;
        await FlavorCmd.AddRandomGenericFlavor(ctx, this.Owner, null, this.DynamicVars["Flavors"].IntValue);
        await FlavorCmd.ChangeFlavor(ctx, this.Owner, null, spicy: 1);
    }
}