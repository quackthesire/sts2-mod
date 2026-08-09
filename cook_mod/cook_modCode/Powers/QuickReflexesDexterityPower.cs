using BaseLib.Abstracts;
using BaseLib.Extensions;
using cook_mod.cook_modCode.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace cook_mod.cook_modCode.Powers;

public class QuickReflexesDexterityPower : CustomTemporaryPowerModelWrapper<QuickReflexes, DexterityPower>
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/quick_reflexes_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/quick_reflexes_power.png";
}