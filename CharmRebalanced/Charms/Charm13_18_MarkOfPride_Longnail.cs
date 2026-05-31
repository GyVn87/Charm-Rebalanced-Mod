using MonoMod.Cil;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm13_18_MarkOfPride_Longnail {
        private static readonly float comboRange = 1.4f;
        private static readonly float mantisRange = 1.25f;
        private static readonly float longnailRange = 1.15f;

        private static readonly Vector3 origCycloneScale = new Vector3(1.4403f, 1.9107f, 1.3863f); // don't change
        private static readonly Vector3 origNailArtScale = new Vector3(1.3162f, 1.3162f, 1.3162f); // don't change

        internal static void Load() {
            IL.NailSlash.StartSlash += NewRangeBuff;
            On.HutongGames.PlayMaker.Actions.SetScale.OnEnter += OnSetScale_OnEnter;
            ModHooks.CharmUpdateHook += OnCharmUpdate;
        }

        internal static void Unload() {
            IL.NailSlash.StartSlash -= NewRangeBuff;
            On.HutongGames.PlayMaker.Actions.SetScale.OnEnter -= OnSetScale_OnEnter;
            ModHooks.CharmUpdateHook -= OnCharmUpdate;
        }

        private static void NewRangeBuff(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);

            float rangeBuff = 0;
            while (cursor.TryGotoNext(
                MoveType.Before,
                i => i.MatchLdcR4(out rangeBuff)
            )) {
                if (rangeBuff == 1.4f)
                    cursor.Next.Operand = comboRange;
                else if (rangeBuff == 1.25f)
                    cursor.Next.Operand = mantisRange;
                else if (rangeBuff == 1.15f)
                    cursor.Next.Operand = longnailRange;

                cursor.Index++;
            }
        }

        private static void OnSetScale_OnEnter(On.HutongGames.PlayMaker.Actions.SetScale.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetScale self) {
            string ownerName = self.Owner.name;
            if (ownerName == "Dash Slash" || ownerName == "Great Slash") {
                if (self.State.Name == "Deactivate") {
                    self.Finish();
                    return;
                }
                if (self.State.Name == "Init") {
                    float rangeMultiplier = GetRangeMultiplier();
                    self.x.Value = origNailArtScale.x * rangeMultiplier;
                    self.y.Value = origNailArtScale.y * rangeMultiplier;
                    self.z.Value = origNailArtScale.z * rangeMultiplier;
                }
            }

            orig(self);
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            Transform cycloneTransform = HeroController.instance.transform.Find("Attacks/Cyclone Slash");
            cycloneTransform.localScale = origCycloneScale * GetRangeMultiplier();

            Transform dashSlashTransform = HeroController.instance.transform.Find("Attacks/Dash Slash");
            dashSlashTransform.localScale = origNailArtScale * GetRangeMultiplier();
        }

        private static float GetRangeMultiplier() {
            float rangeMultiplier = 1f;
            PlayerData PD = PlayerData.instance;
            if (PD.GetBool("equippedCharm_13") && PD.GetBool("equippedCharm_18"))
                rangeMultiplier = comboRange;
            else if (PD.GetBool("equippedCharm_13"))
                rangeMultiplier = mantisRange;
            else if (PD.GetBool("equippedCharm_18"))
                rangeMultiplier = longnailRange;
            return rangeMultiplier;
        }
    }
}
