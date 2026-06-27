namespace TuyenTuyenTuyen.Charms {
    internal static class Charm16_SharpShadow {
        private static readonly float shadowDashSpeedIncrease = 1.4f;
        private static readonly float shadowDashDamageSharp = 2f;
        private static readonly float shadowDashDamageMaster = 2.5f;
        private static readonly float nailBindingPenalty = 0.75f;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.FloatMultiplyV2.OnEnter += OnFloatMultiplyV2_OnEnter;
            On.HutongGames.PlayMaker.Actions.ConvertFloatToInt.OnEnter += OnConvertFloatToInt_OnEnter;
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.FloatMultiplyV2.OnEnter -= OnFloatMultiplyV2_OnEnter;
            On.HutongGames.PlayMaker.Actions.ConvertFloatToInt.OnEnter -= OnConvertFloatToInt_OnEnter;
            ModHooks.CharmUpdateHook -= OnCharmUpdate;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            if (data.GetBool("equippedCharm_16"))
                controller.DASH_SPEED_SHARP = controller.DASH_SPEED * shadowDashSpeedIncrease;
            else
                controller.DASH_SPEED_SHARP = controller.DASH_SPEED;
        }

        private static void OnFloatMultiplyV2_OnEnter(On.HutongGames.PlayMaker.Actions.FloatMultiplyV2.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FloatMultiplyV2 self) {
            orig(self);
            if (self.Fsm.Name == "Set Sharp Shadow Damage" && self.State.Name == "Master")
                self.floatVariable.Value = self.floatVariable.Value / self.multiplyBy.Value * shadowDashDamageMaster;
        }

        private static void OnConvertFloatToInt_OnEnter(On.HutongGames.PlayMaker.Actions.ConvertFloatToInt.orig_OnEnter orig, HutongGames.PlayMaker.Actions.ConvertFloatToInt self) {
            if (self.Fsm.Name == "Set Sharp Shadow Damage" && self.State.Name == "Set") {
                if (!PlayerData.instance.GetBool("equippedCharm_31"))
                    self.floatVariable.Value *= shadowDashDamageSharp;
            }
            orig(self);
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            if (BossSequenceController.BoundNail && hitInstance.AttackType == AttackTypes.SharpShadow)
                hitInstance.Multiplier *= nailBindingPenalty;
            orig(self, hitInstance);
        }
    }
}
