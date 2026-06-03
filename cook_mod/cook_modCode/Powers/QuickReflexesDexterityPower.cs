using cook_mod.cook_modCode.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace cook_mod.cook_modCode.Powers;

public class QuickReflexesDexterityPower : TemporaryDexterityPower
{
    public override AbstractModel OriginModel => (AbstractModel) ModelDb.Card<QuickReflexes>();
}
