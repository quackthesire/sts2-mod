using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Helpers;

namespace cook_mod.cook_modCode.Character;

public class TheCookPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => StsColors.orange;
}