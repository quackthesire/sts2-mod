using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Powers;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TokenCardPool))]
public sealed class Knife : TokenCardModel
{
	public Knife() : base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
	{
		WithDamage(6, 2);
		WithPower<BleedPower>(2, 1);
		WithKeywords(CardKeyword.Retain, CardKeyword.Exhaust);
	}
	
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CommonActions.CardAttack(this, cardPlay)
			.WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3")
			.Execute(choiceContext);
		await CommonActions.Apply<BleedPower>(choiceContext, cardPlay.Target, this);
	}
}