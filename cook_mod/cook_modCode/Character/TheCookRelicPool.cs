using BaseLib.Abstracts;
using cook_mod.cook_modCode.Relic;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Character;

public class TheCookRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => StsColors.quarterTransparentWhite;
    
    protected override IEnumerable<RelicModel> GenerateAllRelics()
    {
        return new RelicModel[9]
        {
            (RelicModel) ModelDb.Relic<Almanac>(),
            (RelicModel) ModelDb.Relic<Aromatics>(),
            (RelicModel) ModelDb.Relic<BottledWind>(),
            (RelicModel) ModelDb.Relic<CastIronPan>(),
            (RelicModel) ModelDb.Relic<CuttingBoard>(),
            (RelicModel) ModelDb.Relic<FermentationBarrel>(),
            (RelicModel) ModelDb.Relic<FlavorBase>(),
            (RelicModel) ModelDb.Relic<Lunchbox>(),
            (RelicModel) ModelDb.Relic<SerratedKnife>(),
        };
    }
}