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

        private static readonly float trialRewardIncrease = 1.25f;
        private static readonly float geoGainIncrease = 1.5f;
        private static readonly float vanillaGreedIncrease = 1.2f;

        private static readonly FieldInfo smallGeoField = typeof(HealthManager).GetField("smallGeoDrops", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo mediumGeoField = typeof(HealthManager).GetField("mediumGeoDrops", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo largeGeoField = typeof(HealthManager).GetField("largeGeoDrops", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static void Load() {
            On.HutongGames.PlayMaker.Fsm.DoTransition += Charm24_Greed.OnFsmDoTransition;
            On.HealthManager.Die += Charm24_Greed.OnHMDie;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Fsm.DoTransition -= Charm24_Greed.OnFsmDoTransition;
            On.HealthManager.Die -= Charm24_Greed.OnHMDie;
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

        private static void OnHMDie(On.HealthManager.orig_Die orig, HealthManager self, float? attackDirection, AttackTypes attackType, bool ignoreEvasion) {
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            if (PD.GetBool("equippedCharm_24") && !PD.GetBool("brokenCharm_24")) {
                int smallGeoDrops = (int)smallGeoField.GetValue(self);
                smallGeoField.SetValue(self, Mathf.CeilToInt((float)smallGeoDrops * geoGainIncrease / vanillaGreedIncrease));
                int mediumGeoDrops = (int)mediumGeoField.GetValue(self);
                mediumGeoField.SetValue(self, Mathf.CeilToInt((float)mediumGeoDrops * geoGainIncrease / vanillaGreedIncrease));
                int largeGeoDrops = (int)largeGeoField.GetValue(self);
                largeGeoField.SetValue(self, Mathf.CeilToInt((float)largeGeoDrops * geoGainIncrease / vanillaGreedIncrease));
            }
            orig(self, attackDirection, attackType, ignoreEvasion);
        }
    }
}
