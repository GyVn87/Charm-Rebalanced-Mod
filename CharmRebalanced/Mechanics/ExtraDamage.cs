namespace TuyenTuyenTuyen.Mechanics {
    internal static class ExtraDamage {
        internal static void Load() {
            On.ExtraDamageable.GetDamageOfType += OnExtraDamageable_GetDamageOfType;
        }

        internal static void Unload() {
            On.ExtraDamageable.GetDamageOfType -= OnExtraDamageable_GetDamageOfType;
        }

        private static int OnExtraDamageable_GetDamageOfType(On.ExtraDamageable.orig_GetDamageOfType orig, ExtraDamageTypes extraDamageTypes) {
            return extraDamageTypes switch {
                ExtraDamageTypes.Dung => 1,
                ExtraDamageTypes.Spore => 2,
                ExtraDamageTypes.Dung2 => 3,
                _ => 999,
            };
        }
    }
}
