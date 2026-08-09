using System.Collections.Generic;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace cook_mod.cook_modCode.Control;

public partial class FlavorBar : Godot.Control
{
    public static Player _player;
    public static Godot.Control _control;

    public static AddedNode<NEnergyCounter, FlavorBar> Node = new((energy) =>
    {
        var control = new FlavorBar();
        
        control.Size = new(250, 50);
        control.Position = new(-65, 100);
        
        _control = control;
        
        control.MouseEntered += () =>
        {
            if (_player == null)
                return;
            NHoverTipSet tip = NHoverTipSet.CreateAndShow((Godot.Control) energy, (IHoverTip)new HoverTip(new LocString("static_hover_tips", "FLAVOR_BAR.title"), new LocString("static_hover_tips", "FLAVOR_BAR.description")));
            if (tip != null)
                tip.GlobalPosition = energy.GlobalPosition + new Vector2(-70f, -150f);
        };
        
        control.MouseExited += () =>
        {
            if (_player == null)
                return;
            NHoverTipSet.Remove((Godot.Control) energy);
        };

        return control;
    });
}