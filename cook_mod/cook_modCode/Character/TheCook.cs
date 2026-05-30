using System.Runtime.InteropServices;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using cook_mod.cook_modCode.Cards;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Relic;
using Godot;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace cook_mod.cook_modCode.Character;

public class TheCook : PlaceholderCharacterModel
{
    public const string CharacterId = "TheCook";

    public override Color NameColor => StsColors.orange;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 75;
    
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeCook>(),
        ModelDb.Card<StrikeCook>(),
        ModelDb.Card<StrikeCook>(),
        ModelDb.Card<StrikeCook>(),
        ModelDb.Card<DefendCook>(),
        ModelDb.Card<DefendCook>(),
        ModelDb.Card<DefendCook>(),
        ModelDb.Card<DefendCook>(),
        ModelDb.Card<Sample>(),
        ModelDb.Card<PlanningStrike>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Aromatics>()
    ];
    
    public override CardPoolModel CardPool => ModelDb.CardPool<TheCookCardPool>();
    public override RelicPoolModel RelicPool => (RelicPoolModel) ModelDb.RelicPool<TheCookRelicPool>();
    public override PotionPoolModel PotionPool
    {
        get => (PotionPoolModel) ModelDb.PotionPool<RegentPotionPool>();
    }
    
    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets. 
        These are just some of the simplest assets, given some placeholders to differentiate your character with. 
        You don't have to, but you're suggested to rename these images. */
    public override Godot.Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Godot.Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Godot.Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    public override float AttackAnimDelay => 0.15f;
    public override float CastAnimDelay => 0.25f;
    public override List<string> GetArchitectAttackVfx()
    {
        int num = 5;
        List<string> list = new List<string>(num);
        CollectionsMarshal.SetCount<string>(list, num);
        Span<string> span = CollectionsMarshal.AsSpan<string>(list);
        int index1 = 0;
        span[index1] = "vfx/vfx_starry_impact";
        int index2 = index1 + 1;
        span[index2] = "vfx/vfx_attack_blunt";
        int index3 = index2 + 1;
        span[index3] = "vfx/vfx_attack_slash";
        int index4 = index3 + 1;
        span[index4] = "vfx/vfx_heavy_blunt";
        int index5 = index4 + 1;
        span[index5] = "vfx/vfx_attack_lightning";
        return list;
    }
    public override Color EnergyLabelOutlineColor => new Color("784000FF");

    public override Color DialogueColor => new Color("52371D");

    public override VfxColor SpeechBubbleColor => VfxColor.Orange;

    public override Color MapDrawingColor => new Color("935206");

    public override Color RemoteTargetingLineColor => new Color("BFA270FF");

    public override Color RemoteTargetingLineOutline => new Color("784000FF");

    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";
}