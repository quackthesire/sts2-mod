using BaseLib.Abstracts;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Control;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace cook_mod.cook_modCode.Abstract;

public class FlavorControl() : CustomSingletonModel (true, false)
{
    
    public override Task BeforeCombatStart()
    {
        var state = CombatManager.Instance.DebugOnlyGetState();
        if (state == null) return Task.CompletedTask;
        foreach (var player in state.Players)
        {
            if (player.Character is TheCook)
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
            }
            else
            {
                SweetCounter._label.Visible = false;
                SweetCounter._control.Visible = false;
                SweetCounter._texRect.Visible = false;
                SourCounter._label.Visible = false;
                SourCounter._control.Visible = false;
                SourCounter._texRect.Visible = false;
                SaltyCounter._label.Visible = false;
                SaltyCounter._control.Visible = false;
                SaltyCounter._texRect.Visible = false;
                BitterCounter._label.Visible = false;
                BitterCounter._control.Visible = false;
                BitterCounter._texRect.Visible = false;
                SpicyCounter._label.Visible = false;
                SpicyCounter._control.Visible = false;
                SpicyCounter._texRect.Visible = false;
            }
        }

        return Task.CompletedTask;
    }
}