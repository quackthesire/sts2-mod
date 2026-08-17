using BaseLib.Abstracts;
using cook_mod.cook_modCode.Potions;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace cook_mod.cook_modCode.Character;

public class TheCookPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => StsColors.quarterTransparentWhite;
    
    protected override IEnumerable<PotionModel> GenerateAllPotions()
    {
        return new PotionModel[3]
        {
            (PotionModel) ModelDb.Potion<Glitterbomb>(),
            (PotionModel) ModelDb.Potion<HerbalExtract>(),
            (PotionModel) ModelDb.Potion<SplinterPotion>(),
        };
    }
}