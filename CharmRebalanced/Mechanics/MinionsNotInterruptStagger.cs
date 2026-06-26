using TuyenTuyenTuyen.Charms;
using UnityEngine;

namespace TuyenTuyenTuyen.Mechanics {
    internal static class MinionsNotInterruptStagger {
        internal static void Load() {
            On.HutongGames.PlayMaker.Fsm.ProcessEvent += OnFsmProcessEvent;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Fsm.ProcessEvent -= OnFsmProcessEvent;
        }

        private static void OnFsmProcessEvent(On.HutongGames.PlayMaker.Fsm.orig_ProcessEvent orig, HutongGames.PlayMaker.Fsm self, HutongGames.PlayMaker.FsmEvent fsmEvent, HutongGames.PlayMaker.FsmEventData eventData) {
            if (self.ActiveState != null && self.ActiveStateName.Contains("Stun") && fsmEvent.Name == "TOOK DAMAGE") {
                GameObject source = eventData?.SentByFsm?.GameObject;
                string parentName = source?.transform?.parent?.name;

                if (parentName != null && (parentName.StartsWith("Weaverling") || parentName.StartsWith("Grimmball"))) {
                    PlayMakerFSM attackFSM = source.GetComponent<PlayMakerFSM>();
                    if (parentName.StartsWith("Weaverling"))
                        attackFSM.Fsm.SetState("Init");
                    else
                        attackFSM.Fsm.Event("FINISHED");
                    return;
                }
            }
            orig(self, fsmEvent, eventData);
        }
    }
}
