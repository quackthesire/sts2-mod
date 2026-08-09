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

public partial class CookPile : TextureButton
{
    public static Player _player;
    public static Label _label;
    public static TextureButton _button;
    
    public static AddedNode<NEnergyCounter, CookPile> Node = new((energy) => {
        var tex = ResourceLoader.Load<Texture2D>("res://cook_mod/menu.png");
        var button = new CookPile
        {
            Position = new Vector2(15, -100),
            Size = new Vector2(100, 100),
            MouseFilter = MouseFilterEnum.Stop,
            FocusMode = FocusModeEnum.None,

            TextureNormal = tex,
            TextureHover = tex,
            TexturePressed = tex,
        };
        button.StretchMode = TextureButton.StretchModeEnum.Scale;
        button.AddThemeStyleboxOverride("focus", new StyleBoxEmpty());
        var label = new Label
        {
            Text = "0"
        };
        label.AddThemeColorOverride("font_color", Colors.Black);
        label.SetAnchorsAndOffsetsPreset(LayoutPreset.Center);
        button.AddChild(label);
        _label = label;
        _button = button;

        button.MouseEntered += () =>
        {
            if (_player == null)
                return;
            NHoverTipSet tip = NHoverTipSet.CreateAndShow((Godot.Control) energy, (IHoverTip)new HoverTip(new LocString("static_hover_tips", "COOK_PILE.title"), new LocString("static_hover_tips", "COOK_PILE.description")));
            if (tip != null)
                tip.GlobalPosition = energy.GlobalPosition + new Vector2(-70f, -250f);
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