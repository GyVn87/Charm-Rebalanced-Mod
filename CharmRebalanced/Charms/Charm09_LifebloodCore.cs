namespace TuyenTuyenTuyen.Charms {
    internal static class Charm09_LifebloodCore {
        private static readonly int blueHealthIncreases = 4;

        internal static void Load() {
            On.PlayerData.UpdateBlueHealth += Charm09_LifebloodCore.OnUpdateBlueHealth;
        }

        internal static void Unload() {
            On.PlayerData.UpdateBlueHealth -= Charm09_LifebloodCore.OnUpdateBlueHealth;
        }

        private static void OnUpdateBlueHealth(On.PlayerData.orig_UpdateBlueHealth orig, PlayerData self) {
            orig(self);
            if (self.GetBool("equippedCharm_9"))
                self.SetInt("healthBlue", self.GetInt("healthBlue") - 4 + blueHealthIncreases);
        }
    }
}
