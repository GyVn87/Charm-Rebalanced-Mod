using HutongGames.PlayMaker;
using UnityEngine;


namespace TuyenTuyenTuyen.Charms {
    internal static class Charm39_Weaversong {
        private static readonly float baseSpeedMultiplier = 1.25f;
        private static readonly float weaverlingDamageRatio = 0.33f; // to nail damage
        private static readonly int soulGainOnHit = 2;
        private static readonly int soulGainGrubsong = 3;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.OnEnter += Charm39_Weaversong.OnPlayerDataBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.CallMethodProper.OnEnter += Charm39_Weaversong.OnCallMethodProper_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetFloatValue.OnEnter += OnSetFloatValue_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.OnEnter -= Charm39_Weaversong.OnPlayerDataBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.CallMethodProper.OnEnter -= Charm39_Weaversong.OnCallMethodProper_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetFloatValue.OnEnter -= OnSetFloatValue_OnEnter;
        }

        private static void OnPlayerDataBoolTest_OnEnter(On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.orig_OnEnter orig, HutongGames.PlayMaker.Actions.PlayerDataBoolTest self) {
            if (self.Owner.name != "Enemy Damager" || self.Fsm.Name != "Attack" || self.State.Name != "Grubsong") {
                orig(self);
                return;
            }
            FsmEvent isFalseEventDefault = self.isFalse;
            self.isFalse = null;
            orig(self);
            self.isFalse = isFalseEventDefault;

            SetWeaverlingDamage(self);
        }

        private static void OnCallMethodProper_OnEnter(On.HutongGames.PlayMaker.Actions.CallMethodProper.orig_OnEnter orig, HutongGames.PlayMaker.Actions.CallMethodProper self) {
            if (self.Owner.name != "Enemy Damager" || self.Fsm.Name != "Attack" || self.State.Name != "Grubsong") {
                orig(self);
                return;
            }
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            int soulGainDefault = (int)self.parameters[0].GetValue();
            if (PD.GetBool("equippedCharm_3"))
                self.parameters[0].SetValue(soulGainGrubsong);
            else
                self.parameters[0].SetValue(soulGainOnHit);
            orig(self);
            self.parameters[0].SetValue(soulGainDefault);
        }

        private static void SetWeaverlingDamage(HutongGames.PlayMaker.Actions.PlayerDataBoolTest self) {
            int nailDamage = CharmRebalanced.LoadedInstance.PD.GetInt("nailDamage");
            self.Fsm.Variables.GetFsmInt("Damage").Value = Mathf.CeilToInt((float)nailDamage * weaverlingDamageRatio);
        }

        private static void OnSetFloatValue_OnEnter(On.HutongGames.PlayMaker.Actions.SetFloatValue.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetFloatValue self) {
            if (self.Owner.name.StartsWith("Weaverling") && self.State.Name == "Sprintmaster") {
                int actionIndex = System.Array.IndexOf(self.State.Actions, self);
                if (actionIndex == 0)
                    self.floatValue.Value = baseSpeedMultiplier;
            }
            orig(self);
        }
    }
}
