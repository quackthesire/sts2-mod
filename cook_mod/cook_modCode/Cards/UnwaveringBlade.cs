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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models.Enchantments;


namespace cook_mod.cook_modCode.Cards;

[Pool(typeof(TheCookCardPool))]

public class UnwaveringBlade() : CustomCardModel(1, CardType.Attack,
    CardRarity.Uncommon, TargetType.AnyEnemy)
{

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            List<IHoverTip> tips = new List<IHoverTip>();
            tips.AddRange(HoverTipFactory.FromEnchantment<Steady>());
            return tips;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move), new CardsVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull((object)cardPlay.Target, "cardPlay.Target");
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel)this).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_dramatic_stab", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        foreach (CardModel card in PileType.Hand.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>)(card => ModelDb.Enchantment<Steady>().CanEnchant(card) && card.Type != CardType.None)).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.Shuffle).Take<CardModel>(2))
        {
            if (card == null)
                continue;
            else
            {
                CardCmd.Enchant<Steady>(card, 1m);
                await EnchantCmd.OnEnchant(choiceContext, this.Owner, card, (CardModel) this);
            }
        }
    }
    
    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(3m);
    }
}