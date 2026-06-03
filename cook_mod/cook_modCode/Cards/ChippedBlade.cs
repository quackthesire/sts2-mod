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
using MegaCrit.Sts2.Core.Saves.Runs;

namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class ChippedBlade() : CustomCardModel(1, CardType.Attack,
    CardRarity.Rare, TargetType.AnyEnemy)
{
    private const string _decreaseKey = "Decrease";
    private int _currentDamage = 40;
    private int _decreasedDamage;
    
    [SavedProperty]
    public int CurrentDamage
    {
        get => this._currentDamage;
        set
        {
            this.AssertMutable();
            this._currentDamage = value;
            this.DynamicVars.Damage.BaseValue = (Decimal) this._currentDamage;
        }
    }
    
    [SavedProperty]
    public int DecreasedDamage
    {
        get => this._decreasedDamage;
        set
        {
            this.AssertMutable();
            this._decreasedDamage = value;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(this.CurrentDamage, ValueProp.Move), new DynamicVar("Decrease", 2m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab", null, "blunt_attack.mp3").Execute(choiceContext);
        int intValue = this.DynamicVars["Decrease"].IntValue;
        this.DebuffFromPlay(intValue);
        if (!(this.DeckVersion is ChippedBlade deckVersion))
            return;
        deckVersion.DebuffFromPlay(intValue);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(10m);
        this.UpdateDamage();
    }
    
    protected override void AfterDowngraded() => this.UpdateDamage();

    private void DebuffFromPlay(int reducedDamage)
    {
        this.DecreasedDamage += reducedDamage;
        this.UpdateDamage();
    }

    private void UpdateDamage() => this.CurrentDamage = (this.IsUpgraded ? 50 : 40) - this.DecreasedDamage;
}