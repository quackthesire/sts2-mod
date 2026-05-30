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
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;


namespace cook_mod.cook_modCode.Foods;

[Pool(typeof(TokenCardPool))]

public class GreenTea() : FoodCardModel(1, CardType.Skill,
    CardRarity.Token, TargetType.Self, bitter: 4)
{
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Bitter>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardModel card = CombatManager.Instance.History.CardPlaysFinished.Last().CardPlay.Card;
        await CardCmd.AutoPlay(choiceContext, card.CreateDupe(), (Creature) null);
    }
    
    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}