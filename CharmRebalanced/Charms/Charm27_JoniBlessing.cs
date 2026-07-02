using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm27_JoniBlessing {
        private static readonly float masksIncreases = 1.5f;

        internal static void Load() {
            ModHooks.CharmUpdateHook += OnCharmUpdate;
        }

        internal static void Unload() {
            ModHooks.CharmUpdateHook -= OnCharmUpdate;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            if (data.GetBool("equippedCharm_27")) {
                int maxHealth = data.GetInt("maxHealthBase");
                if (data.GetBool("equippedCharm_23") && !data.GetBool("brokenCharm_23"))
                    maxHealth += 2;
                data.SetInt("joniHealthBlue", Mathf.FloorToInt((float)maxHealth * masksIncreases));
            }
        }
    }
}
