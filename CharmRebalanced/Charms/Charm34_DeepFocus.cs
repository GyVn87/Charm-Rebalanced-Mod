using UnityEngine.Assertions.Must;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm34_DeepFocus {
        private static readonly float deepFocusSpeedIncreases = 1.5f;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.FloatMultiply.OnEnter += Charm34_DeepFocus.OnFloatMutiply_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.FloatMultiply.OnEnter -= Charm34_DeepFocus.OnFloatMutiply_OnEnter;
        }

        private static void OnFloatMutiply_OnEnter(On.HutongGames.PlayMaker.Actions.FloatMultiply.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FloatMultiply self) {
            orig(self);
            if (self.Fsm.Name == "Spell Control" && self.State.Name == "Deep Focus Speed")
                self.floatVariable.Value = self.floatVariable.Value / self.multiplyBy.Value * deepFocusSpeedIncreases;
        }
    }
}
