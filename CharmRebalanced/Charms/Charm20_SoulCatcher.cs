namespace TuyenTuyenTuyen.Charms {
    internal static class Charm20_SoulCatcher {
        private static readonly int soulCharge = 2;
        private static readonly int soulReserve = 2;

        internal static void Load() {
            ModHooks.SoulGainHook += OnSoulGain;
        }

        internal static void Unload() {
            ModHooks.SoulGainHook -= OnSoulGain;
        }

        // Subtracting the vanilla soul gain and adding the new ones.
        // This additive approach allows the mod can work with other mods that affect soul gain.
        private static int OnSoulGain(int orig) {
            PlayerData playerData = PlayerData.instance;
            if (playerData.GetBool("equippedCharm_20")) {
                if (playerData.GetInt("MPCharge") < playerData.GetInt("maxMP"))
                    orig = orig - 3 + soulCharge;
                else
                    orig = orig - 2 + soulReserve;
            }
            return orig;
        }
    }
}
