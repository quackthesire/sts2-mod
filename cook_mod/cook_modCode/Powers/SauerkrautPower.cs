using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Foods;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace cook_mod.cook_modCode.Powers;

public class SauerkrautPower : CustomTemporaryPowerModelWrapper<Sauerkraut, StrengthPower>
{
    
    public sealed override string CustomPackedIconPath => "res://cook_mod/sauerkraut_power.png";

    public sealed override string CustomBigIconPath => "res://cook_mod/sauerkraut_power.png";
}