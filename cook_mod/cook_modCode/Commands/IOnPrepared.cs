using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace cook_mod.cook_modCode.Commands;

public interface IOnPrepared
{
    Task OnPrepared(PlayerChoiceContext ctx, Player player, int amount, int selected, CardPlay? cardPlay);
}