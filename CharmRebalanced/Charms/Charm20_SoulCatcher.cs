namespace TuyenTuyenTuyen.Charms {
    internal static class Charm20_SoulCatcher {
        private static readonly int soulCharge = 3;
        private static readonly int soulReserve = 2;

        internal static void Load() {
            ModHooks.SoulGainHook += Charm20_SoulCatcher.OnSoulGain;
        }

        internal static void Unload() {
            ModHooks.SoulGainHook -= Charm20_SoulCatcher.OnSoulGain;
        }

        // Subtracting the vanilla soul gain and adding the new ones.
        // This additive approach allows the mod can work with other mods that affect soul gain.
        private static int OnSoulGain(int orig) {
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            if (PD.GetBool("equippedCharm_20")) {
                if (PD.GetInt("MPCharge") < PD.GetInt("maxMP"))
                    orig = orig - 3 + soulCharge;
                else
                    orig = orig - 2 + soulReserve;
            }
            return orig;
        }
    }
}
