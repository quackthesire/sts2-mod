using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Control;
using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace cook_mod.cook_modCode.Commands;

public static class FlavorCmd
{
    public static async Task ChangeFlavor(PlayerChoiceContext ctx, Player player, CardModel? cardSource, int sweet = 0, int sour = 0, int salty = 0, int bitter = 0, int spicy = 0)
    {
        var flavors = FlavorsModel.Get(player);
        var original = new Flavors();
        original.sweet = flavors.sweet;
        original.sour = flavors.sour;
        original.salty = flavors.salty;
        original.bitter = flavors.bitter;
        original.spicy = flavors.spicy;
        flavors.sweet += sweet;
        flavors.sour += sour;
        flavors.salty += salty;
        flavors.bitter += bitter;
        flavors.spicy += spicy;
        await OnFlavorModified(ctx, player, original, flavors);
    }
    
    public static async Task SetFlavor(PlayerChoiceContext ctx, Player player, CardModel? cardSource, int sweet = 0, int sour = 0, int salty = 0, int bitter = 0, int spicy = 0)
    {
        var flavors = FlavorsModel.Get(player);
        var original = new Flavors();
        original.sweet = flavors.sweet;
        original.sour = flavors.sour;
        original.salty = flavors.salty;
        original.bitter = flavors.bitter;
        original.spicy = flavors.spicy;
        flavors.sweet = sweet;
        flavors.sour = sour;
        flavors.salty = salty;
        flavors.bitter = bitter;
        flavors.spicy = spicy;
        await OnFlavorModified(ctx, player, original, flavors);
    }
    
    public static async Task AddRandomFlavor(PlayerChoiceContext ctx, Player player, CardModel? cardSource, int times = 1)
    {
        var flavors = FlavorsModel.Get(player);
        var original = new Flavors();
        original.sweet = flavors.sweet;
        original.sour = flavors.sour;
        original.salty = flavors.salty;
        original.bitter = flavors.bitter;
        original.spicy = flavors.spicy;
        Rng rng = player.RunState.Rng.Niche;
        for (int i = 0; i < times; i++)
        {
            var value = rng.NextInt(5);
            switch (value)
            {
                case 0:
                    flavors.sweet += 1;
                    break;
                case 1:
                    flavors.sour += 1;
                    break;
                case 2:
                    flavors.salty += 1;
                    break;
                case 3:
                    flavors.bitter += 1;
                    break;
                case 4:
                    flavors.spicy += 1;
                    break;
            }
        }
        await OnFlavorModified(ctx, player, original, flavors);
    }
    
    public static async Task AddRandomGenericFlavor(PlayerChoiceContext ctx, Player player, CardModel? cardSource, int times = 1)
    {
        var flavors = FlavorsModel.Get(player);
        var original = new Flavors();
        original.sweet = flavors.sweet;
        original.sour = flavors.sour;
        original.salty = flavors.salty;
        original.bitter = flavors.bitter;
        original.spicy = flavors.spicy;
        Rng rng = player.RunState.Rng.Niche;
        for (int i = 0; i < times; i++)
        {
            var value = rng.NextInt(4);
            switch (value)
            {
                case 0:
                    flavors.sweet += 1;
                    break;
                case 1:
                    flavors.sour += 1;
                    break;
                case 2:
                    flavors.salty += 1;
                    break;
                case 3:
                    flavors.bitter += 1;
                    break;
            }
        }
        await OnFlavorModified(ctx, player, original, flavors);
    }

    public static async Task OnFlavorModified(PlayerChoiceContext ctx, Player player, Flavors original, Flavors modified)
    {
        SweetCounter._label.Text = modified.sweet.ToString();
        SweetCounter._label.HorizontalAlignment = HorizontalAlignment.Center;
        SweetCounter._label.VerticalAlignment = VerticalAlignment.Center;
        SourCounter._label.Text = modified.sour.ToString();
        SourCounter._label.HorizontalAlignment = HorizontalAlignment.Center;
        SourCounter._label.VerticalAlignment = VerticalAlignment.Center;
        SaltyCounter._label.Text = modified.salty.ToString();
        SaltyCounter._label.HorizontalAlignment = HorizontalAlignment.Center;
        SaltyCounter._label.VerticalAlignment = VerticalAlignment.Center;
        BitterCounter._label.Text = modified.bitter.ToString();
        BitterCounter._label.HorizontalAlignment = HorizontalAlignment.Center;
        BitterCounter._label.VerticalAlignment = VerticalAlignment.Center;
        SpicyCounter._label.Text = modified.spicy.ToString();
        SpicyCounter._label.HorizontalAlignment = HorizontalAlignment.Center;
        SpicyCounter._label.VerticalAlignment = VerticalAlignment.Center;
        if (modified.sweet > 0 || modified.sour > 0 || modified.salty > 0 || modified.bitter > 0 || modified.spicy > 0)
        {
            SweetCounter._label.Visible = true;
            SweetCounter._control.Visible = true;
            SweetCounter._texRect.Visible = true;
            SourCounter._label.Visible = true;
            SourCounter._control.Visible = true;
            SourCounter._texRect.Visible = true;
            SaltyCounter._label.Visible = true;
            SaltyCounter._control.Visible = true;
            SaltyCounter._texRect.Visible = true;
            BitterCounter._label.Visible = true;
            BitterCounter._control.Visible = true;
            BitterCounter._texRect.Visible = true;
            SpicyCounter._label.Visible = true;
            SpicyCounter._control.Visible = true;
            SpicyCounter._texRect.Visible = true;
            CookPile._label.Visible = true;
            CookPile._button.Visible = true;
            CookPile._button.Disabled = false;
            CookPile._player = player;
            FlavorBar._control.Visible = true;
            FlavorBar._player = player;
        }
        await CookHook.OnFlavor(ctx, player, original, modified);
    }
}