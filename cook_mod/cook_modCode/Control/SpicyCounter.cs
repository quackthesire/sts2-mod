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

public partial class SpicyCounter : Godot.Control
{
    public static Label _label;
    public static Godot.Control _control;
    public static TextureRect _texRect;

    public static AddedNode<NEnergyCounter, SpicyCounter> Node = new((energy) =>
    {
        //would probably suggest loading from scene rather than this manual setup
        var control = new SpicyCounter();
        
        var tex = ResourceLoader.Load<Texture2D>("res://cook_mod/mod_image.png");
        
        var size = tex.GetSize();
        var texRect = new TextureRect();
        texRect.Name = tex.ResourcePath;
        texRect.Size = new(50, 50);
        texRect.Texture = tex;
        texRect.PivotOffset = size / 2f;
        texRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
        texRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
        texRect.MouseFilter = MouseFilterEnum.Ignore;
        
        control.Size = new(50, 50);
        control.Position = new(135, 100);
        control.AddChild(texRect);
        
        var label = new Label { Text = "0" };
        label.SetAnchorsAndOffsetsPreset(LayoutPreset.Center);
        control.AddChild(label);
        _label = label;
        _control = control;
        _texRect = texRect;
        
        return control;
    });
}