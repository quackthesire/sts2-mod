using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
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
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;


namespace cook_mod.cook_modCode.Foods;

[Pool(typeof(TokenCardPool))]

public class Radicchio() : FoodCardModel(3, CardType.Skill,
    CardRarity.Token, TargetType.Self, bitter: 4, spicy: 2)
{
    private bool _hasExtraTurn;
    private bool _paelsEyeWasAlreadyUsed;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Bitter>(), HoverTipFactory.FromPower<Spicy>()];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return _hasExtraTurn && player == Owner;
    }

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        _hasExtraTurn = true;

        var paelsEye = Owner.Relics.OfType<PaelsEye>().FirstOrDefault();
        if (paelsEye != null)
            _paelsEyeWasAlreadyUsed = Traverse.Create(paelsEye).Field("_usedThisCombat").GetValue<bool>();

        // End your turn
        PlayerCmd.EndTurn(Owner, false);
        return Task.CompletedTask;
    }

    public override Task AfterTakingExtraTurn(Player player)
    {
        if (player != Owner) return Task.CompletedTask;
        if (!_hasExtraTurn) return Task.CompletedTask; // Not our extra turn, don't touch PaelsEye

        _hasExtraTurn = false;

        if (_paelsEyeWasAlreadyUsed) return Task.CompletedTask;
        var paelsEye = player.Relics.OfType<PaelsEye>().FirstOrDefault();
        if (paelsEye == null) return Task.CompletedTask;
        Traverse.Create(paelsEye).Field("_usedThisCombat").SetValue(false);

        return Task.CompletedTask;
    }
    
    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}