using System.Reflection;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm15_HeavyBlow {
        private static readonly int stunHit = 3;
        private static readonly float nailArtDamageIncrease = 1.2f;

        private static readonly FieldInfo stunControlFSM = typeof(HealthManager).GetField("stunControlFSM", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.OnEnter += OnPlayerDataBoolTest_OnEnter;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.FloatMultiply.OnEnter += OnFloatMutiply_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.OnEnter -= OnPlayerDataBoolTest_OnEnter;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.FloatMultiply.OnEnter -= OnFloatMutiply_OnEnter;
        }

        private static void OnPlayerDataBoolTest_OnEnter(On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.orig_OnEnter orig, HutongGames.PlayMaker.Actions.PlayerDataBoolTest self) {
            if (self.Owner.name == "Charm Effects" && self.Fsm.Name == "Enemy Recoil Up" && self.State.Name == "Check") {
                self.Event(self.isFalse);
                return;
            }
            orig(self);
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            orig(self, hitInstance);
            if (!PlayerData.instance.GetBool("equippedCharm_15")) return;

            string ownerName = hitInstance.Source.name;
            if (ownerName != "Dash Slash" && ownerName != "Great Slash") return;

            PlayMakerFSM stunControl = (PlayMakerFSM)stunControlFSM.GetValue(self);
            if (stunControl) {
                for (int i = 1; i <= stunHit - 1; i++)
                    stunControl.Fsm.Event("STUN DAMAGE");
            }
        }

        private static void OnFloatMutiply_OnEnter(On.HutongGames.PlayMaker.Actions.FloatMultiply.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FloatMultiply self) {
            orig(self);
            if (self.Fsm.Name == "nailart_damage" && self.State.Name == "Init") {
                if (PlayerData.instance.GetBool("equippedCharm_15"))
                    self.floatVariable.Value *= nailArtDamageIncrease;
            }
        }
    }
}
