using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
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
using cook_mod.cook_modCode.Cards;
using cook_mod.cook_modCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace cook_mod.cook_modCode.Foods;

[Pool(typeof(TokenCardPool))]

public class Durian() : FoodCardModel(0, CardType.Skill,
    CardRarity.Token, TargetType.Self, sweet: 2, bitter: 2)
{
    public sealed override string CustomPortraitPath => "res://cook_mod/durian.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CustomKeywords.Prepare), HoverTipFactory.FromPower<Sweet>(), HoverTipFactory.FromPower<Bitter>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), new ExhaustiveVar(3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PrepareCmd.Discard(choiceContext, this.Owner, this.DynamicVars.Cards.IntValue, 0, -1, cardPlay);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Cards.UpgradeValueBy(1m);
    }
}