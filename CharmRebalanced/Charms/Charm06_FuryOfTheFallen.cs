using IL.InControl.NativeDeviceProfiles;
using System;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm06_FuryOfTheFallen {
        internal static void Load() {
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.BoolTest.OnEnter += OnBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetFsmFloat.OnEnter += OnSetFsmFloat_OnEnter;
        }

        internal static void Unload() {
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.BoolTest.OnEnter -= OnBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetFsmFloat.OnEnter -= OnSetFsmFloat_OnEnter;
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            PlayerData PD = PlayerData.instance;
            int currentHealth = PD.GetInt("health");
            int maxHealth = PD.CurrentMaxHealth;
            if (hitInstance.AttackType != AttackTypes.Nail || !PD.GetBool("equippedCharm_6") || PD.GetBool("equippedCharm_27") || currentHealth >= maxHealth) {
                orig(self, hitInstance);
                return;
            }
            float furyMultiplier = 0f;
            if (BossSequenceController.BoundShell)
                furyMultiplier = Mathf.Pow((float)(currentHealth - maxHealth) / (float)(maxHealth + 1), 2f) * 1.5f;
            else 
                furyMultiplier = Mathf.Pow((float)(currentHealth - maxHealth) / (float)(maxHealth + 1), 2f);
            if (furyMultiplier > 0.05f) {
                hitInstance.Multiplier *= (1f + furyMultiplier);
                orig(self, hitInstance);
            }
        }

        private static void OnBoolTest_OnEnter(On.HutongGames.PlayMaker.Actions.BoolTest.orig_OnEnter orig, HutongGames.PlayMaker.Actions.BoolTest self) {
            if (self.Fsm.Name == "nailart_damage" && self.State.Name == "Fury?") {
                self.Fsm.Event("FINISHED");
                return;
            }
            orig(self);
        }

        private static void OnSetFsmFloat_OnEnter(On.HutongGames.PlayMaker.Actions.SetFsmFloat.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetFsmFloat self) {
            if (self.Owner.name == "Charm Effects" && self.Fsm.Name == "Fury" && self.State.Name == "Activate") {
                self.Finish();
                return;
            }
            orig(self);
        }
    }
}
