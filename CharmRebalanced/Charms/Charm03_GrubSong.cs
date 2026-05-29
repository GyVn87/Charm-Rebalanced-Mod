namespace TuyenTuyenTuyen.Charms {
    internal static class Charm03_GrubSong {
        private static readonly int soulGain = 12;
        private static readonly int soulGainCombo = 20;

        internal static void Load() {
            On.HeroController.Awake += OnHCAwake;
            ModHooks.TakeHealthHook += OnTakeHealth;
        }

        internal static void Unload() {
            On.HeroController.Awake -= OnHCAwake;
            ModHooks.TakeHealthHook -= OnTakeHealth;
        }

        private static void OnHCAwake(On.HeroController.orig_Awake orig, HeroController self) {
            orig(self);
            self.GRUB_SOUL_MP = soulGain;
            self.GRUB_SOUL_MP_COMBO = soulGainCombo;
        }

        private static int OnTakeHealth(int orig) {
            // Checking if the damage amount is within a reasonable range.
            // This ensures player don't suddenly gain massive pool of soul when get killed by Radiant bossess.
            if (orig < 2 || orig > 100)
                return orig;
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            if (PD.GetBool("equippedCharm_3")) {
                if (PD.GetBool("equippedCharm_35")) 
                    HeroController.instance.AddMPCharge(soulGainCombo * (orig - 1));
                else 
                    HeroController.instance.AddMPCharge(soulGain * (orig - 1));
            }
            return orig;
        }
    }
}