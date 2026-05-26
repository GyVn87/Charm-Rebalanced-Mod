namespace TuyenTuyenTuyen.Charms {
    internal static class Charm25_Strength {
        internal static readonly float strengthMutiplier = 1.3f;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.FloatMultiply.OnEnter += OnFloatMutiply_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.FloatMultiply.OnEnter -= OnFloatMutiply_OnEnter;
        }

        private static void OnFloatMutiply_OnEnter(On.HutongGames.PlayMaker.Actions.FloatMultiply.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FloatMultiply self) {
            orig(self);
            if (self.Fsm.Name == "Set Slash Damage" && self.State.Name == "Glass Attack Modifier")
                self.floatVariable.Value = self.floatVariable.Value / self.multiplyBy.Value * strengthMutiplier;
            else if (self.Fsm.Name == "nailart_damage" && self.State.Name == "Init") {
                if (PlayerData.instance.GetBool("equippedCharm_25"))
                    self.floatVariable.Value *= strengthMutiplier;
            }
        }
    }
}
