namespace TuyenTuyenTuyen.Charms {
    internal static class Charm36_Kingsoul {
        private static readonly float soulGainRate = 1.5f;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.Wait.OnEnter += OnWait_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.Wait.OnEnter -= OnWait_OnEnter;
        }

        private static void OnWait_OnEnter(On.HutongGames.PlayMaker.Actions.Wait.orig_OnEnter orig, HutongGames.PlayMaker.Actions.Wait self) {
            if (self.Fsm.Name == "White Charm" && self.State.Name == "Wait")
                self.time.Value = soulGainRate;
            orig(self);
        }
    }
}
