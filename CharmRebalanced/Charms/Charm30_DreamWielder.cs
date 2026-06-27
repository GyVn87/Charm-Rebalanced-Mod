using MonoMod.Cil;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm30_DreamWielder {
        private static readonly int soulGainNormal = 33;
        private static readonly int soulGainWielder = 66;

        internal static void Load() {
            IL.EnemyDreamnailReaction.RecieveDreamImpact += ChangeDreamNailSoulGain;
        }

        internal static void Unload() {
            IL.EnemyDreamnailReaction.RecieveDreamImpact -= ChangeDreamNailSoulGain;
        }

        private static void ChangeDreamNailSoulGain(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);

            /*
                // int amount = (GameManager.instance.playerData.GetBool("equippedCharm_30") ? 66 : 33);
	            IL_0012: call class GameManager GameManager::get_instance()
	            IL_0017: ldfld class PlayerData GameManager::playerData
	            IL_001c: ldstr "equippedCharm_30"
	            IL_0021: callvirt instance bool PlayerData::GetBool(string)
	            IL_0026: brtrue.s IL_002c

	            IL_0028: ldc.i4.s 33
	            IL_002a: br.s IL_002e

	            // HeroController.instance.AddMPCharge(amount);
	            IL_002c: ldc.i4.s 66

	            IL_002e: stloc.2
	            IL_002f: call class HeroController HeroController::get_instance()
	            IL_0034: ldloc.2
	            IL_0035: callvirt instance void HeroController::AddMPCharge(int32)
            */

            int soulGain = -1;
            while (cursor.TryGotoNext(
                MoveType.Before,
                i => i.MatchLdcI4(out soulGain)
            )) {
                switch (soulGain) {
                    case 33:
                        cursor.Next.Operand = soulGainNormal;
                        break;
                    case 66:
                        cursor.Next.Operand = soulGainWielder;
                        break;
                }
                cursor.Index++;
            }
        }
    }
}
