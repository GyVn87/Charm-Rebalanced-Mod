using System.Reflection;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm26_NailmasterGlory {
        private static readonly float nailArtChargeTimeMaster = 0.9f;

        internal static void Load() {
            On.HeroController.Awake += OnHCAwake;
        }

        internal static void Unload() {
            On.HeroController.Awake -= OnHCAwake;
        }
        private static void OnHCAwake(On.HeroController.orig_Awake orig, HeroController self) {
            orig(self);
            self.NAIL_CHARGE_TIME_CHARM = nailArtChargeTimeMaster;
        }
    }
}
