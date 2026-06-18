using GlobalEnums;
using IL;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using On;
using System;
using System.Collections;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm35_GrubberflyElegy {
        private static int baseSoulGain = 4;
        private static int catcherSoulGain = 1;
        private static int eaterSoulGain = 2;

        internal static void Load() {
            ModHooks.AfterAttackHook += OnAfterAttack;
            IL.HealthManager.TakeDamage += RemoveBeamStagger;
            On.PlayerData.MaxHealth += OnPDMaxHealth;
            On.HeroController.TakeDamage += OnHeroController_TakeDamage;
            On.HeroController.AddHealth += OnHeroController_AddHealth;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
        }

        internal static void Unload() {
            ModHooks.AfterAttackHook -= OnAfterAttack;
            IL.HealthManager.TakeDamage -= RemoveBeamStagger;
            On.PlayerData.MaxHealth -= OnPDMaxHealth;
            On.HeroController.TakeDamage -= OnHeroController_TakeDamage;
            On.HeroController.AddHealth -= OnHeroController_AddHealth;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
        }

        private static void OnAfterAttack(AttackDirection attackDir) {
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            HeroController Controller = HeroController.instance;
            GameObject Knight = Controller.gameObject;

            if (!PD.GetBool("equippedCharm_35") || Controller.cState.wallSliding)
                return;
            if (!CanCastBeam())
                return;

            GameObject grubberFlyBeam;
            float MANTIS_CHARM_SCALE = 1.35f;
            switch (attackDir) {
                case AttackDirection.normal:
                    if (Knight.transform.localScale.x < 0f) grubberFlyBeam = Controller.grubberFlyBeamPrefabR.Spawn(Knight.transform.position);
                    else grubberFlyBeam = Controller.grubberFlyBeamPrefabL.Spawn(Knight.transform.position);

                    if (PD.GetBool("equippedCharm_13")) grubberFlyBeam.transform.SetScaleY(MANTIS_CHARM_SCALE);
                    else grubberFlyBeam.transform.SetScaleY(1f);
                    break;
                case AttackDirection.upward:
                    grubberFlyBeam = Controller.grubberFlyBeamPrefabU.Spawn(Knight.transform.position);
                    grubberFlyBeam.transform.SetScaleY(Knight.transform.localScale.x);
                    grubberFlyBeam.transform.localEulerAngles = new Vector3(0f, 0f, 270f);
                    if (PD.GetBool("equippedCharm_13"))
                        grubberFlyBeam.transform.SetScaleY(grubberFlyBeam.transform.localScale.y * MANTIS_CHARM_SCALE);
                    break;
                case AttackDirection.downward:
                    grubberFlyBeam = Controller.grubberFlyBeamPrefabD.Spawn(Knight.transform.position);
                    grubberFlyBeam.transform.SetScaleY(Knight.transform.localScale.x);
                    grubberFlyBeam.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
                    if (PD.GetBool("equippedCharm_13"))
                        grubberFlyBeam.transform.SetScaleY(grubberFlyBeam.transform.localScale.y * MANTIS_CHARM_SCALE);
                    break;
            }
        }

        private static bool CanCastBeam() {
            PlayerData playerData = PlayerData.instance;
            int currentHP = playerData.GetInt("health");

            if (currentHP == 1) 
                return false;
            if (!BossSequenceController.BoundShell && currentHP >= playerData.CurrentMaxHealth)
                return false;
            if (BossSequenceController.BoundShell && currentHP > playerData.CurrentMaxHealth)
                return false;
            if (playerData.GetInt("beamDamage") <= 1)
                return false;
            return true;
        }

        private static int NewBeamDamage() {
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            float strengthIncrease = (PD.GetBool("equippedCharm_25") ? Charm25_Strength.strengthMutiplier : 1f);
            int nailDamge = PD.GetInt("nailDamage");
            int currentHP = PD.GetInt("health");
            int currentMaxHP = PD.CurrentMaxHealth;
            if (currentHP > currentMaxHP)
                currentHP = currentMaxHP;

            int newBeamDamage;
            if (BossSequenceController.BoundShell)
                newBeamDamage = Mathf.CeilToInt(Mathf.Pow((float)(currentHP - 1) / (float)(currentMaxHP + 1), 2f) * 1.5f * strengthIncrease * nailDamge);
            else
                newBeamDamage = Mathf.CeilToInt(Mathf.Pow((float)(currentHP - 1) / (float)(currentMaxHP + 1), 2f) * strengthIncrease * nailDamge);
            return newBeamDamage;
        }

        private static int BaseBeamDamage() {
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            float strengthIncrease = (PD.GetBool("equippedCharm_25") ? Charm25_Strength.strengthMutiplier : 1f);
            int nailDamge = PD.GetInt("nailDamage");
            int currentMaxHP = PD.CurrentMaxHealth;

            int newBeamDamage;
            if (BossSequenceController.BoundShell)
                newBeamDamage = Mathf.CeilToInt(Mathf.Pow((float)(currentMaxHP - 1) / (float)(currentMaxHP + 1), 2f) * 1.5f * strengthIncrease * nailDamge);
            else
                newBeamDamage = Mathf.CeilToInt(Mathf.Pow((float)(currentMaxHP - 1) / (float)(currentMaxHP + 1), 2f) * strengthIncrease * nailDamge);
            return newBeamDamage;
        }

        private static void RemoveBeamStagger(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            /*
                // if (hp > 0)
                IL_0365: ldarg.0

                IL_0366: ldfld int32 HealthManager::hp
                IL_036b: ldc.i4.0

                IL_036c: ble.s IL_0398

                // NonFatalHit(hitInstance.IgnoreInvulnerable);
            */
            if (cursor.TryGotoNext(
                MoveType.After,
                i => i.MatchLdarg(0),
                i => i.MatchLdfld(out var value1),
                i => i.MatchLdcI4(0),
                i => i.MatchBle(out ILLabel value2)
            )) {
                cursor.Emit(OpCodes.Ldarg, 1);
                cursor.EmitDelegate<Func<HitInstance, bool>>(IsBeam);

                /*
                    // if ((bool)stunControlFSM)
                    IL_037a: ldarg.0
                    IL_037b: ldfld class [PlayMaker]PlayMakerFSM HealthManager::stunControlFSM
                    IL_0380: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Implicit(class [UnityEngine.CoreModule]UnityEngine.Object)
                    IL_0385: brfalse.s IL_03bc
                */

                ILLabel label = null;
                ILCursor cursorClone = cursor.Clone();
                if (cursorClone.TryGotoNext(
                    i => i.MatchBrfalse(out label)
                )) {
                    cursor.Emit(OpCodes.Brtrue, label);
                }
            }
        }

        private static bool IsBeam(HitInstance hitInstance) {
            if (hitInstance.AttackType == AttackTypes.NailBeam)
                return true;
            return false;
        }

        private static void OnPDMaxHealth(On.PlayerData.orig_MaxHealth orig, PlayerData self) {
            orig(self);
            GameManager.instance.StartCoroutine(SetBaseBeamDamageDelayed());
        }

        private static void OnHeroController_TakeDamage(On.HeroController.orig_TakeDamage orig, HeroController self, GameObject go, CollisionSide damageSide, int damageAmount, int hazardType) { 
            orig(self, go, damageSide, damageAmount, hazardType);
            PlayerData playerData = PlayerData.instance;
            int currentHP = playerData.GetInt("health");
            if (currentHP == 1 && playerData.GetBool("equippedCharm_6"))
                playerData.SetInt("beamDamage", BaseBeamDamage());
            else
                playerData.SetInt("beamDamage", NewBeamDamage());
        }

        // for some reason, the beams deal triple the damage to Pure Vessel
        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            if (hitInstance.AttackType == AttackTypes.NailBeam && self.gameObject.name == "HK Prime")
                hitInstance.Multiplier *= 0.33f;
            SoulGainedOnBeam();
            orig(self, hitInstance);
        }

        private static void OnHeroController_AddHealth(On.HeroController.orig_AddHealth orig, HeroController self, int amount) {
            orig(self, amount);
            PlayerData.instance.SetInt("beamDamage", NewBeamDamage());
        }

        private static IEnumerator SetBaseBeamDamageDelayed() {
            yield return null;
            PlayerData.instance.SetInt("beamDamage", BaseBeamDamage());
        }

        private static void SoulGainedOnBeam() {
            PlayerData playerData = PlayerData.instance;
            int total = baseSoulGain;
            total += (playerData.GetBool("equippedCharm_20") ? catcherSoulGain : 0);  // Soul Catcher
            total += (playerData.GetBool("equippedCharm_21") ? eaterSoulGain : 0);    // Soul Eater
            HeroController.instance.AddMPCharge(total);
        }
    }
}
