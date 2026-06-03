using BaseLib.Abstracts;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Control;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace cook_mod.cook_modCode.Abstract;

public abstract class RotCardModel (int canonicalEnergyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    bool shouldShowInCardLibrary = true)
    : CustomCardModel(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    
}