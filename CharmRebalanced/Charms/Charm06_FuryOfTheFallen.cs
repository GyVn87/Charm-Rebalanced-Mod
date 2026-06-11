using GlobalEnums;
using IL.InControl.NativeDeviceProfiles;
using System;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm06_FuryOfTheFallen {
        private static float furyExtraMultiplier = 0f;

        internal static void Load() {
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.BoolTest.OnEnter += OnBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetFsmFloat.OnEnter += OnSetFsmFloat_OnEnter;
            On.HeroController.TakeDamage += OnHeroController_TakeDamage;
            On.HeroController.AddHealth += OnHeroController_AddHealth;
        }

        internal static void Unload() {
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.BoolTest.OnEnter -= OnBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetFsmFloat.OnEnter -= OnSetFsmFloat_OnEnter;
            On.HeroController.TakeDamage -= OnHeroController_TakeDamage;
            On.HeroController.AddHealth -= OnHeroController_AddHealth;
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            PlayerData PD = PlayerData.instance;
            int currentHealth = PD.GetInt("health");
            int maxHealth = PD.CurrentMaxHealth;
            if (hitInstance.AttackType != AttackTypes.Nail || !PD.GetBool("equippedCharm_6") || PD.GetBool("equippedCharm_27") || currentHealth >= maxHealth) {
                orig(self, hitInstance);
                return;
            }

            if (furyExtraMultiplier > 0.05f)
                hitInstance.Multiplier *= (1f + furyExtraMultiplier);
            orig(self, hitInstance);
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

        private static void UpdateFuryMultiplier() {
            PlayerData PD = PlayerData.instance;
            int currentHealth = PD.GetInt("health");
            int maxHealth = PD.CurrentMaxHealth;

            if (BossSequenceController.BoundShell)
                furyExtraMultiplier = Mathf.Pow((float)(currentHealth - maxHealth) / (float)(maxHealth + 1), 2f) * 1.5f;
            else
                furyExtraMultiplier = Mathf.Pow((float)(currentHealth - maxHealth) / (float)(maxHealth + 1), 2f);
        }

        private static void OnHeroController_TakeDamage(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType) {
            orig(self, go, damageSide, damageAmount, hazardType);
            UpdateFuryMultiplier();
        }

        private static void OnHeroController_AddHealth(On.HeroController.orig_AddHealth orig, HeroController self, int amount) {
            orig(self, amount);
            UpdateFuryMultiplier();
        }
    }
}
