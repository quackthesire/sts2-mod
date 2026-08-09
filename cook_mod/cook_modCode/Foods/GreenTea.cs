using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using cook_mod.cook_modCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;


namespace cook_mod.cook_modCode.Foods;

[Pool(typeof(TokenCardPool))]

public class GreenTea() : FoodCardModel(1, CardType.Skill,
    CardRarity.Token, TargetType.Self, bitter: 5)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/green_tea.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Bitter>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ExhaustiveVar(3m)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var lastCardPlay = CombatManager.Instance.History.CardPlaysFinished
            .Reverse()
            .FirstOrDefault(play =>
            {
                CardModel card = play.CardPlay.Card;
                return (card.Type == CardType.Attack || card.Type == CardType.Skill) && ! (card is GreenTea) && play.HappenedThisTurn(this.CombatState);
            });

        if (lastCardPlay != null)
        {
            CardModel card = lastCardPlay.CardPlay.Card;
            await CardCmd.AutoPlay(choiceContext, card.CreateDupe(this.Owner), null);
        }
    }
    
    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}