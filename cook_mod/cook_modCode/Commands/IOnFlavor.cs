using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Commands;

public interface IOnFlavor
{
    Task OnFlavor(PlayerChoiceContext ctx, Player player, Flavors original, Flavors modified);
}