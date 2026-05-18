using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm15_HeavyBlow {
        private static readonly float damageMultiplier = 1.2f;
        private static readonly int stunHit = 3;

        private static PlayerData PD => CharmRebalanced.LoadedInstance.PD;
        private static float stunDamageTimer = 0f;
        private static readonly float eventDelay = 1f;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.FloatMultiply.OnEnter += OnFloatMutiply_OnEnter;
            On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.OnEnter += OnPlayerDataBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.TakeDamage.OnEnter += OnTakeDamage_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.FloatMultiply.OnEnter -= OnFloatMutiply_OnEnter;
            On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.OnEnter -= OnPlayerDataBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.TakeDamage.OnEnter -= OnTakeDamage_OnEnter;
        }

        private static void OnFloatMutiply_OnEnter(On.HutongGames.PlayMaker.Actions.FloatMultiply.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FloatMultiply self) {
            orig(self);
            if (self.Fsm.Name == "nailart_damage" && self.State.Name == "Init") {
                if (PD.GetBool("equippedCharm_15"))
                    self.floatVariable.Value *= damageMultiplier;
            }
        }

        private static void OnPlayerDataBoolTest_OnEnter(On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.orig_OnEnter orig, HutongGames.PlayMaker.Actions.PlayerDataBoolTest self) {
            if (self.Owner.name == "Charm Effects" && self.Fsm.Name == "Enemy Recoil Up" && self.State.Name == "Check") {
                self.Event(self.isFalse);
                return;
            }
            orig(self);
        }

        private static void OnTakeDamage_OnEnter(On.HutongGames.PlayMaker.Actions.TakeDamage.orig_OnEnter orig, HutongGames.PlayMaker.Actions.TakeDamage self) {
            orig(self);
            if (!PD.GetBool("equippedCharm_15"))
                return;
            if (self.Owner.name == "Dash Slash" || self.Owner.name == "Great Slash") {
                if (self.Fsm.Name == "damages_enemy") {
                    if (Time.time < stunDamageTimer)
                        return;
                    stunDamageTimer = Time.time + eventDelay;
                    for (int i = 1; i <= (stunHit - 1); i++)
                        self.Fsm.BroadcastEventToGameObject(self.Target.Value, "STUN DAMAGE", false);
                }
            }
        }
    }
}
