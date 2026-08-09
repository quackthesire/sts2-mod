using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace cook_mod.cook_modCode.Keywords;
public sealed class Spicy : CustomPowerModel
{
    public sealed override string CustomPackedIconPath => "res://cook_mod/spicy.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/spicy.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
}