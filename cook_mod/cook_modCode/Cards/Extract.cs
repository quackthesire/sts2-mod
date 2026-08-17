using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
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

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class Extract() : CustomCardModel(1, CardType.Skill,
    CardRarity.Uncommon, TargetType.Self), IOnPrepared
{
    public sealed override string CustomPortraitPath => "res://cook_mod/extract.png";
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CustomKeywords.Prepare), HoverTipFactory.FromKeyword(CustomKeywords.Generic_Flavor)];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PrepareCmd.Discard(choiceContext, this.Owner, this.DynamicVars.Cards.IntValue, 0, -1, cardPlay);
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Cards.UpgradeValueBy(1m);
    }
    
    public async Task OnPrepared(PlayerChoiceContext ctx, Player player, int amount, int selected, CardPlay?  cardPlay)
    {
        if (player != this.Owner || cardPlay == null || cardPlay.Card != this)
            return;
        await FlavorCmd.AddRandomGenericFlavor(ctx, this.Owner, this, selected);
    }
}