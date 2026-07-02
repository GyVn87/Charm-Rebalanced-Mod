using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using TuyenTuyenTuyen.Mechanics;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm40_CarefreeMelody {
        private static readonly float hitBlockCooldown = 15f;

        private static float timer = 0f;
        private static GameObject? carefreeMelodyIcon = null;
        private static int lastHitDamage = 0;
        private static int GetLastHitDamage() => lastHitDamage;

        internal static void Load() {
            IL.HeroController.TakeDamage += BeforeCarefreeShieldCheck;
            On.PlayerData.MaxHealth += OnPDMaxHealth;
            On.HUDCamera.OnEnable += OnHudCameraOnEnable;
            ModHooks.HeroUpdateHook += OnHeroUpdate;
        }

        internal static void Unload() {
            IL.HeroController.TakeDamage -= BeforeCarefreeShieldCheck;
            On.PlayerData.MaxHealth -= OnPDMaxHealth;
            On.HUDCamera.OnEnable -= OnHudCameraOnEnable;
            ModHooks.HeroUpdateHook -= OnHeroUpdate;
        }

        private static void BeforeCarefreeShieldCheck(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);

            /*
                // if (flag)
	            IL_0180: ldloc.3
	            IL_0181: brfalse.s IL_019d

	            // hitsSinceShielded = 0;
	            IL_0183: ldarg.0
	            IL_0184: ldc.i4.0
	            IL_0185: stfld int32 HeroController::hitsSinceShielded
            */

            ILLabel? blockEffectLabel = null;
            if (cursor.TryGotoNext(
                MoveType.Before,
                i => i.MatchLdarg(0),
                i => i.MatchLdcI4(0),
                i => i.MatchStfld<HeroController>("hitsSinceShielded")
            )) {
                blockEffectLabel = cursor.MarkLabel();
            }

            /*
                // bool flag = false;
	            IL_008a: callvirt instance void VibrationMixer::StopAllEmissionsWithTag(string)

	            IL_008f: ldc.i4.0
	            IL_0090: stloc.3
	            // if (carefreeShieldEquipped && hazardType == 1)
	            IL_0091: ldarg.0
	            IL_0092: ldfld bool HeroController::carefreeShieldEquipped
	            IL_0097: brfalse IL_01ab

             */

            cursor.Goto(0);
            if (cursor.TryGotoNext(
                MoveType.After,
                i => i.MatchCallvirt<VibrationMixer>("StopAllEmissionsWithTag"),
                i => i.MatchLdcI4(0),
                i => i.MatchStloc(3)
            )) {
                cursor.Emit(OpCodes.Ldarg_S, (byte)3);
                cursor.EmitDelegate<Func<int, bool>>(NewCarefreeMelodyMechanic);
                cursor.Emit(OpCodes.Brtrue, blockEffectLabel);

                ILLabel? label = null;
                if (cursor.TryGotoNext(
                    MoveType.After,
                    i => i.MatchBrfalse(out label)
                )) {
                    cursor.Emit(OpCodes.Ldarg_S, (byte)3);
                    cursor.EmitDelegate<Func<int>>(GetLastHitDamage);
                    cursor.Emit(OpCodes.Add);
                    cursor.Emit(OpCodes.Starg_S, (byte)3);
                    cursor.EmitDelegate<Action>(ResetTimer);
                    cursor.Emit(OpCodes.Br, label);
                }
            }
        }

        private static bool NewCarefreeMelodyMechanic(int damageAmount) {
            HeroController controller = HeroController.instance;
            if (damageAmount < 100 && controller.carefreeShieldEquipped && Time.time > timer) {
                timer = Time.time + hitBlockCooldown;
                lastHitDamage = damageAmount;
                carefreeMelodyIcon?.SetActive(true);
                return true;
            }
            return false;
        }

        private static void OnPDMaxHealth(On.PlayerData.orig_MaxHealth orig, PlayerData self) {
            orig(self);
            ResetTimer();
        }

        private static void ResetTimer() {
            timer = 0f;
            carefreeMelodyIcon?.SetActive(false);
        }

        private static void OnHeroUpdate() {
            if (HeroController.instance.carefreeShieldEquipped && Time.time > timer)
                carefreeMelodyIcon?.SetActive(false);
        }

        private static void OnHudCameraOnEnable(On.HUDCamera.orig_OnEnable orig, HUDCamera self) {
            orig(self);
            Transform extrasTransform = GameCameras.instance.hudCanvas.transform.Find("Extras");
            carefreeMelodyIcon = new GameObject("Carefree Melody Icon");           
            carefreeMelodyIcon.transform.SetParent(extrasTransform, false);
            carefreeMelodyIcon.SetActive(false);
            carefreeMelodyIcon.layer = extrasTransform.gameObject.layer;

            carefreeMelodyIcon.transform.localPosition = new(-2.5f, -2f, 0f);
            carefreeMelodyIcon.transform.localScale = new(0.6f, 0.6f, 0.6f);

            SpriteRenderer spriteRenderer = carefreeMelodyIcon.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Utilities.LoadSprite("Carefree_Melody_On_Trigger");
            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.renderingLayerMask = 1;
        }
    }
}
