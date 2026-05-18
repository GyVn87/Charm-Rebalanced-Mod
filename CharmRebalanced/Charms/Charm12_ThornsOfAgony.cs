using HutongGames.PlayMaker;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm12_ThornsOfAgony {
        private static readonly float thornDamageMutiplier = 1f;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.SetFsmInt.OnEnter += Charm12_ThornsOfAgony.OnSetFsmInt_OnEnter;
            On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter += Charm12_ThornsOfAgony.OnSendMessage_OnEnter;
            On.HutongGames.PlayMaker.Actions.Wait.OnEnter += Charm12_ThornsOfAgony.OnWait_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetPosition.OnEnter += Charm12_ThornsOfAgony.OnSetPosition_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.SetFsmInt.OnEnter -= Charm12_ThornsOfAgony.OnSetFsmInt_OnEnter;
            On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter -= Charm12_ThornsOfAgony.OnSendMessage_OnEnter;
            On.HutongGames.PlayMaker.Actions.Wait.OnEnter -= Charm12_ThornsOfAgony.OnWait_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetPosition.OnEnter -= Charm12_ThornsOfAgony.OnSetPosition_OnEnter;
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

        private static void OnSendMessage_OnEnter(On.HutongGames.PlayMaker.Actions.SendMessage.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SendMessage self) {
            if (self.Fsm.Name == "Thorn Counter" && self.State.Name == "Counter Start") {
                string funcName = self.functionCall.FunctionName;
                if (funcName == "RelinquishControl" || funcName == "AffectedByGravity") {
                    self.Finish();
                    return;
                }
            }
            orig(self);
        }

        private static void OnWait_OnEnter(On.HutongGames.PlayMaker.Actions.Wait.orig_OnEnter orig, HutongGames.PlayMaker.Actions.Wait self) {
            if (self.Fsm.Name == "Thorn Counter" && self.State.Name == "Counter") {
                HeroController Knight = HeroController.instance;
                Knight.AffectedByGravity(true);
                Knight.RegainControl();
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
    }
}