using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace cook_mod.cook_modCode.Keywords;
public sealed class Sour : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/sour.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/sour.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
}