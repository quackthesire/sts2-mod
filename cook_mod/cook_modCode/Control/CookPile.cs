using System.Collections.Generic;
using System.Reflection;
using BaseLib.Utils;
using cook_mod.cook_modCode.Commands;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace cook_mod.cook_modCode.Control;

public partial class CookPile : Button
{
    public static Player _player;
    public static Label _label;
    public static Button _button;
    public static TextureRect _texRect;
    
    public static AddedNode<NEnergyCounter, CookPile> Node = new((energy) => {
        var button = new CookPile
        {
            Position = new Vector2(35, -50),
            Text = "+",
            Size = new Vector2(50, 50),
            MouseFilter = MouseFilterEnum.Stop
        };
        
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
        button.AddChild(texRect);
        var label = new Label { Text = "0" };
        label.SetAnchorsAndOffsetsPreset(LayoutPreset.Center);
        button.AddChild(label);
        _label = label;
        _button = button;
        _texRect = texRect;

        button.MouseEntered += () =>
        {
            if (_player == null)
                return;
            NHoverTipSet tip = NHoverTipSet.CreateAndShow((Godot.Control) energy, (IHoverTip)new HoverTip(new LocString("static_hover_tips", "COOK_PILE.title"), new LocString("static_hover_tips", "COOK_PILE.description")));
            if (tip != null)
                tip.GlobalPosition = energy.GlobalPosition + new Vector2(-70f, -200f);
        };
        
        button.MouseExited += () =>
        {
            if (_player == null)
                return;
            NHoverTipSet.Remove((Godot.Control) energy);
        };

        button.Pressed += () =>
        {
            if (_player == null)
                return;
            CookCmd.Cook(new BlockingPlayerChoiceContext(), _player, null);
        };

        return button;
    });
}