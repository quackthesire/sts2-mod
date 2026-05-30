using BaseLib.Abstracts;
using cook_mod.cook_modCode.Commands;
using cook_mod.cook_modCode.Control;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace cook_mod.cook_modCode.Abstract;

public abstract class FoodCardModel (int canonicalEnergyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    bool shouldShowInCardLibrary = true,
    int sweet = 0,
    int sour = 0,
    int salty = 0,
    int bitter = 0,
    int spicy = 0)
    : CustomCardModel(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    public int sweet = sweet;
    public int sour = sour;
    public int salty = salty;
    public int bitter = bitter;
    public int spicy = spicy;
}