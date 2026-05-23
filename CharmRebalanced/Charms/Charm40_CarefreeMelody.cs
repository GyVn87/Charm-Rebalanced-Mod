using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm40_CarefreeMelody {
        private static readonly float hitBlockCooldown = 20f;

        private static float timer = 0f;

        internal static void Load() {
            IL.HeroController.TakeDamage += BeforeCarefreeShieldCheck;
        }

        internal static void Unload() {
            IL.HeroController.TakeDamage -= BeforeCarefreeShieldCheck;
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

            ILLabel blockEffectLabel = null;
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
                cursor.EmitDelegate<Func<bool>>(NewCarefreeMelodyMechanic);
                cursor.Emit(OpCodes.Brtrue, blockEffectLabel);

                ILLabel label = null;
                if (cursor.TryGotoNext(
                    MoveType.After,
                    i => i.MatchBrfalse(out label)
                )) {
                    cursor.Emit(OpCodes.Ldarg_S, (byte)3);
                    cursor.Emit(OpCodes.Ldc_I4, 2);
                    cursor.Emit(OpCodes.Mul);
                    cursor.Emit(OpCodes.Starg_S, (byte)3);
                    cursor.Emit(OpCodes.Br, label);
                }
            }
        }

        private static bool NewCarefreeMelodyMechanic() {
            HeroController controller = HeroController.instance;
            if (controller.carefreeShieldEquipped && Time.time > timer) {
                timer = Time.time + hitBlockCooldown;
                return true;
            }
            return false;
        }
    }
}
