namespace TuyenTuyenTuyen.Charms {
    internal static class Charm36_Kingsoul {
        private static readonly float soulGainRate = 2f;
        private static readonly int soulGain = 5;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.Wait.OnEnter += OnWait_OnEnter;
            On.HutongGames.PlayMaker.Actions.SendMessageV2.OnUpdate += OnSendMessageV2_OnUpdate;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.Wait.OnEnter -= OnWait_OnEnter;
            On.HutongGames.PlayMaker.Actions.SendMessageV2.OnUpdate -= OnSendMessageV2_OnUpdate;
        }

        private static void OnWait_OnEnter(On.HutongGames.PlayMaker.Actions.Wait.orig_OnEnter orig, HutongGames.PlayMaker.Actions.Wait self) {
            if (self.Fsm.Name == "White Charm" && self.State.Name == "Wait")
                self.time.Value = soulGainRate;
            orig(self);
        }

        private static void OnSendMessageV2_OnUpdate(On.HutongGames.PlayMaker.Actions.SendMessageV2.orig_OnUpdate orig, HutongGames.PlayMaker.Actions.SendMessageV2 self) {
            if (self.Fsm.Name == "White Charm" && self.State.Name == "Soul UP")
                self.functionCall.IntParameter.Value = soulGain;
            orig(self);
        }
    }
}
