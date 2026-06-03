using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using cook_mod.cook_modCode.Abstract;
using cook_mod.cook_modCode.Cards;
using cook_mod.cook_modCode.Character;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Keywords;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace cook_mod.cook_modCode.Relic;
[Pool(typeof(TheCookRelicPool))]

public class BottledWind : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> list = new List<IHoverTip>();
            list.AddRange(HoverTipFactory.FromEnchantment<Swift>(this.DynamicVars["Swift"].IntValue));
            list.AddRange(HoverTipFactory.FromEnchantment<Sown>());
            return list;
        }
    }
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Swift", 2m)];
    
    public override async Task AfterObtained()
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1);
        foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<Swift>(), this.DynamicVars["Swift"].IntValue, prefs))
        {
            CardCmd.Enchant<Swift>(card, this.DynamicVars["Swift"].IntValue);
            NCardEnchantVfx child = NCardEnchantVfx.Create(card);
            if (child != null)
            {
                NRun instance = NRun.Instance;
                if (instance != null)
                    instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) child);
            }
        }
        foreach (CardModel card in await CardSelectCmd.FromDeckForEnchantment(this.Owner, (EnchantmentModel) ModelDb.Enchantment<Sown>(), 1, prefs))
        {
            CardCmd.Enchant<Sown>(card, 1m);
            NCardEnchantVfx child = NCardEnchantVfx.Create(card);
            if (child != null)
            {
                NRun instance = NRun.Instance;
                if (instance != null)
                    instance.GlobalUi.CardPreviewContainer.AddChildSafely((Node) child);
            }
        }
    }
}