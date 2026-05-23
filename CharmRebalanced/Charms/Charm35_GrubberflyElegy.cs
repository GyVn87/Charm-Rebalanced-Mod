using GlobalEnums;
using IL;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm35_GrubberflyElegy {
        internal static void Load() {
            ModHooks.AfterAttackHook += OnAfterAttack;
            IL.HealthManager.TakeDamage += RemoveBeamStagger;
        }

        internal static void Unload() {
            ModHooks.AfterAttackHook -= OnAfterAttack;
            IL.HealthManager.TakeDamage -= RemoveBeamStagger;
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
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            int currentHP = PD.GetInt("health");

            if (currentHP == 1) {
                if (PD.GetBool("equippedCharm_6"))
                    PD.SetInt("beamDamage", BaseBeamDamage());
                return false;
            }

            int beamDamage = NewBeamDamage();
            PD.SetInt("beamDamage", beamDamage);

            if (!BossSequenceController.BoundShell && currentHP >= PD.CurrentMaxHealth)
                return false;
            if (BossSequenceController.BoundShell && currentHP > PD.CurrentMaxHealth)
                return false;
            if (beamDamage <= 1)
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
    }
}
