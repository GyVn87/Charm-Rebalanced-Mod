using HutongGames.PlayMaker;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm12_ThornsOfAgony {
        private static readonly float thornDamageMutiplier = 1f;

        private static GameObject? thornHit = null;
             
        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.SetFsmInt.OnEnter += OnSetFsmInt_OnEnter;
            On.HutongGames.PlayMaker.Actions.FindChild.OnEnter += OnFindChild_OnEnter;
            On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter += OnSendMessage_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetPosition.OnEnter += OnSetPosition_OnEnter;
            On.HeroController.CanCast += OnHCCanCast;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.SetFsmInt.OnEnter -= OnSetFsmInt_OnEnter;
            On.HutongGames.PlayMaker.Actions.FindChild.OnEnter -= OnFindChild_OnEnter;
            On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter -= OnSendMessage_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetPosition.OnEnter -= OnSetPosition_OnEnter;
            On.HeroController.CanCast -= OnHCCanCast;
        }

        private static void OnSetFsmInt_OnEnter(On.HutongGames.PlayMaker.Actions.SetFsmInt.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetFsmInt self) {
            orig(self);
            if (self.Fsm.Name == "set_thorn_damage" && self.State.Name == "Set") {
                GameObject targetGO = self.Fsm.GetOwnerDefaultTarget(self.gameObject);
                if (targetGO == null) return;
                PlayMakerFSM targetFSM = ActionHelpers.GetGameObjectFsm(targetGO, self.fsmName.Value);
                if (targetFSM == null) return;
                FsmInt fsmInt = targetFSM.FsmVariables.GetFsmInt(self.variableName.Value);
                if (fsmInt == null) return;
                fsmInt.Value = Mathf.CeilToInt((float)self.setValue.Value * thornDamageMutiplier);
            }
        }

        private static void OnFindChild_OnEnter(On.HutongGames.PlayMaker.Actions.FindChild.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FindChild self) {
            orig(self);
            if (self.Fsm.Name == "Thorn Counter" && self.State.Name == "Init")
                thornHit = self.storeResult.Value;
        }

        private static void OnSendMessage_OnEnter(On.HutongGames.PlayMaker.Actions.SendMessage.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SendMessage self) {
            if (self.Fsm.Name == "Thorn Counter") {
                string funcName = self.functionCall.FunctionName;
                if (self.State.Name == "Counter Start" || self.State.Name == "Counter End") {
                    if (funcName == "RelinquishControl" || funcName == "AffectedByGravity" || funcName == "RegainControl" || funcName == "AffectedByGravity") {
                        self.Finish();
                        return;
                    }
                }
            }
            orig(self);
        }

        private static void OnSetPosition_OnEnter(On.HutongGames.PlayMaker.Actions.SetPosition.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetPosition self) {
            if (self.Fsm.Name == "Thorn Counter") {
                if (self.State.Name == "Counter Start" || self.State.Name == "Counter") {
                    self.Finish();
                    return;
                }
            }
            orig(self);
        }

        private static bool OnHCCanCast(On.HeroController.orig_CanCast orig, HeroController self) {
            if (thornHit != null && thornHit!.activeInHierarchy)
                return false;
            return orig(self);
        }
    }
}