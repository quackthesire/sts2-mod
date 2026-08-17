using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Cards;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Deprecated;
using cook_mod.cook_modCode.Foods;
using cook_mod.cook_modCode.Keywords;
using cook_mod.cook_modCode.Powers;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace cook_mod.cook_modCode.Relic;
[Pool(typeof(TheCookRelicPool))]

public class SerratedKnife : CustomRelicModel, IOnBleed
{
  protected override string BigIconPath => "res://cook_mod/serrated_knife.png";
    
  public override string PackedIconPath => "res://cook_mod/serrated_knife.png";
  
  protected override string PackedIconOutlinePath => "res://cook_mod/serrated_knife.png";
  
  public override RelicRarity Rarity => RelicRarity.Shop;

  private bool _isActivating;
  private int _bleedApplied;

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get
    {
      return !this.IsActivating ? this.BleedApplied % this.DynamicVars["Bleed"].IntValue : this.DynamicVars["Bleed"].IntValue;
    }
  }

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.UpdateDisplay();
    }
  }

  [SavedProperty]
  public int BleedApplied
  {
    get => this._bleedApplied;
    set
    {
      this.AssertMutable();
      this._bleedApplied = value;
      this.UpdateDisplay();
    }
  }

  private void UpdateDisplay()
  {
    if (this.IsActivating)
      this.Status = RelicStatus.Normal;
    else
      this.Status = this.BleedApplied == this.DynamicVars["Bleed"].IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<BleedPower>()];

  protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Bleed", 3m), new PowerVar<BleedPower>(2m)];

  public async Task OnBleed(PlayerChoiceContext ctx, Player player, int amount, int changed, CardModel? cardSource, Creature target)
  {
    if (this.Owner == null || player != this.Owner || cardSource is Fish)
      return;
    this.BleedApplied++;
    if (this.BleedApplied >= this.DynamicVars["Bleed"].IntValue)
    {
      TaskHelper.RunSafely(this.DoActivateVisuals());
      foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.Owner.Creature.CombatState.HittableEnemies)
      {
        await PowerCmd.Apply<BleedPower>(ctx, hittableEnemy, this.DynamicVars["BleedPower"].BaseValue, this.Owner.Creature, this.Owner.Creature.CombatState.CreateCard<Fish>(this.Owner));
      }
      this.BleedApplied %= this.DynamicVars["Bleed"].IntValue;
    }
    this.InvokeDisplayAmountChanged();
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}