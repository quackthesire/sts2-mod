using BaseLib.Abstracts;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Cards;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace cook_mod.cook_modCode.Character;

public class TheCookCardPool : CustomCardPoolModel
{
    public override string Title => TheCook.CharacterId; //This is not a display name.


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 1f; //Hue; changes the color.
    public override float S => 1f; //Saturation
    public override float V => 1f; //Brightness
    
    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load CharMod/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override string EnergyColorName => "regent";

    public override string CardFrameMaterialPath => "card_frame_orange";

    public override Color DeckEntryCardColor => new Color("E36600");

    public override Color EnergyOutlineColor => new Color("803D0E");

    public override bool IsColorless => false;
}