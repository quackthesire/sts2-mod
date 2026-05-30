using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;

namespace cook_mod.cook_modCode.Keywords;
public sealed class Flavor : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
}