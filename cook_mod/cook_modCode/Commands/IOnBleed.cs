using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace cook_mod.cook_modCode.Commands;

public interface IOnBleed
{
    Task OnBleed(PlayerChoiceContext ctx, Player player, int amount, int changed, CardModel? cardSource);
}