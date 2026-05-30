using System.Collections.Generic;

namespace TuyenTuyenTuyen.Charms {
    internal static class NewCharmCosts {
        private static readonly Dictionary<string, int> charmCosts = new Dictionary<string, int>() {
            {"charmCost_4", 1},
            {"charmCost_6", 3},
            {"charmCost_8", 1},
            {"charmCost_9", 2},
            {"charmCost_11", 2},
            {"charmCost_15", 1},
            {"charmCost_20", 1},
            {"charmCost_21", 3},
            {"charmCost_27", 2},
            {"charmCost_29", 2},
            {"charmCost_34", 3},
            {"charmCost_38", 2}
        };

        internal static void Load() {
            ModHooks.GetPlayerIntHook += NewCharmCosts.OnGetInt;
        }

        internal static void Unload() {
            ModHooks.GetPlayerIntHook -= NewCharmCosts.OnGetInt;
        }

        internal static int OnGetInt(string name, int orig) {
            if (NewCharmCosts.charmCosts.TryGetValue(name, out int newCost))
                return newCost;
            else if (name == "charmCost_36" && PlayerData.instance.royalCharmState == 3)
                return 3;
            return orig;
        }
    }
}