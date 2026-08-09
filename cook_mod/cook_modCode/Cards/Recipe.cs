using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Powers;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TokenCardPool))]
public sealed class Recipe : TokenCardModel
{
	public sealed override string CustomPortraitPath => "res://cook_mod/recipe.png";
	public Recipe() : base(0, CardType.Skill, CardRarity.Token, TargetType.None)
	{
		WithCards(2, 1);
		WithKeywords(CardKeyword.Retain, CardKeyword.Exhaust);
	}

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CommonActions.Draw(this, choiceContext);
	}
}