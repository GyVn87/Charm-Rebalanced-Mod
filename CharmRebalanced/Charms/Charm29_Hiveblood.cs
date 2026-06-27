using System.Collections;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm29_Hiveblood {
        private static readonly float newRecoverTime = 5f;

        private static GameObject? recoveryBlob = null;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.FindChild.OnEnter += OnFindChild_OnEnter;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent += OnFsmProcessEvent;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.FindChild.OnEnter -= OnFindChild_OnEnter;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent -= OnFsmProcessEvent;
        }

        private static void OnFindChild_OnEnter(On.HutongGames.PlayMaker.Actions.FindChild.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FindChild self) {
            orig(self);
            if (self.Owner.name == "Health" && self.Fsm.Name == "Hive Health Regen" && self.State.Name == "Init") {
                self.Fsm.Variables.GetFsmFloat("Recover Time").Value = newRecoverTime;
            }
        }

        private static void OnFsmProcessEvent(On.HutongGames.PlayMaker.Fsm.orig_ProcessEvent orig, HutongGames.PlayMaker.Fsm self, HutongGames.PlayMaker.FsmEvent fsmEvent, HutongGames.PlayMaker.FsmEventData eventData) {
            if (self.Name == "Hive Health Regen" && self.ActiveStateName.StartsWith("Recover") && (fsmEvent.Name == "HERO HEALED" || fsmEvent.Name == "HERO HEALED FULL")) {
                if (recoveryBlob == null)
                    recoveryBlob = self.Variables.FindFsmGameObject("Recovery Blob").Value;

                PlayerData playerData = PlayerData.instance;
                int health = playerData.GetInt("health");

                if (health >= PlayerData.instance.CurrentMaxHealth) {
                    orig(self, fsmEvent, eventData);
                    return;
                }
                    
                float newX = (float)health * 0.94f - 10.32f;
                Vector3 newLocalPosition = recoveryBlob.transform.localPosition;
                newLocalPosition.x = newX;
                recoveryBlob.transform.localPosition = newLocalPosition;
                GameManager.instance.StartCoroutine(DelayedUpdateBlob());
                return;
            }
            orig(self, fsmEvent, eventData);
        }

        private static IEnumerator DelayedUpdateBlob() {
            yield return new WaitForSeconds(0.5f);

            int health = PlayerData.instance.GetInt("health");

            Transform healthTransf = GameCameras.instance.hudCanvas.transform.Find("Health");
            GameObject currentHealthObject = healthTransf.Find($"Health {health}").gameObject;
            PlayMakerFSM healthDisplayFSM = currentHealthObject.GetComponent<PlayMakerFSM>();
            healthDisplayFSM.SetState("Heal?");

            float newX = (float)health * 0.94f - 10.32f;
            Vector3 newLocalPosition = recoveryBlob!.transform.localPosition;
            newLocalPosition.x = newX;
            recoveryBlob.transform.localPosition = newLocalPosition;
        }
    }
}
