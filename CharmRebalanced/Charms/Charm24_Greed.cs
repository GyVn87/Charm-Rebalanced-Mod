using MonoMod.Cil;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm24_Greed {
        private static readonly Dictionary<string, int> trialPrize = new Dictionary<string, int>() {
            {"colosseumBronzeCompleted", 1000},
            {"colosseumSilverCompleted", 2000},
            {"colosseumGoldCompleted",   3000},
        };

        private static readonly Dictionary<int, float> geoDropIncrease = new Dictionary<int, float>() {
            {1, 0.6f},  // small Geo drop
            {2, 0.35f},  // medium Geo drop
            {3, 0.25f},  // large Geo drop
        };

        private static readonly float trialRewardIncrease = 4f / 3f;
        private static readonly int unbreakableGreedGeoPrice = 4000;

        internal static void Load() {
            On.HutongGames.PlayMaker.Fsm.DoTransition += Charm24_Greed.OnFsmDoTransition;
            IL.HealthManager.Die += ChangeGreedGeoDropIncrease;
            On.HutongGames.PlayMaker.Actions.ConvertStringToInt.OnEnter += OnConvertStringToInt_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Fsm.DoTransition -= Charm24_Greed.OnFsmDoTransition;
            IL.HealthManager.Die -= ChangeGreedGeoDropIncrease;
            On.HutongGames.PlayMaker.Actions.ConvertStringToInt.OnEnter -= OnConvertStringToInt_OnEnter;
        }

        private static void OnConvertStringToInt_OnEnter(On.HutongGames.PlayMaker.Actions.ConvertStringToInt.orig_OnEnter orig, HutongGames.PlayMaker.Actions.ConvertStringToInt self) {
            orig(self);
            if (self.Fsm.Name == "Conversation Control" && self.State.Name == "Swallowed Greed")
                self.intVariable.Value = unbreakableGreedGeoPrice;
        }

        private static bool OnFsmDoTransition(On.HutongGames.PlayMaker.Fsm.orig_DoTransition orig, HutongGames.PlayMaker.Fsm self, HutongGames.PlayMaker.FsmTransition transition, bool isGlobal) {
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            if (self.Name == "Geo Pool" && transition.EventName == "GIVE GEO") {
                float trialPrizeMutiplier = (PD.GetBool("equippedCharm_24") && !PD.GetBool("brokenCharm_24") ? trialRewardIncrease : 1.0f);
                string trialTier = self.Variables.GetFsmString("Completion PD Bool").Value;
                if (trialPrize.TryGetValue(trialTier, out int newPrize))
                    self.Variables.GetFsmInt("Starting Pool").Value = Mathf.CeilToInt((float)newPrize * trialPrizeMutiplier);
            }
            return orig(self, transition, isGlobal);
        }

        private static void ChangeGreedGeoDropIncrease(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);

            /*
                IL_025b: call class GameManager GameManager::get_instance()
	            IL_0260: ldfld class PlayerData GameManager::playerData
	            IL_0265: ldstr "brokenCharm_24"
	            IL_026a: callvirt instance bool PlayerData::GetBool(string)
	            IL_026f: brtrue.s IL_02ad
            */

            if (cursor.TryGotoNext(
                MoveType.After,
                i => i.MatchCall(out _),
                i => i.MatchLdfld(out _),
                i => i.MatchLdstr("brokenCharm_24"),
                i => i.MatchCallvirt(out _),
                i => i.MatchBrtrue(out _)
            )) {
                for (int counter = 1; counter <= 3; counter++) {
                    if (cursor.TryGotoNext(
                        MoveType.Before,
                        i => i.MatchLdcR4(0.2f)
                    )) {
                        cursor.Next.Operand = geoDropIncrease[counter];
                        cursor.Index++;
                    }
                }
            }
        }
    }
}
