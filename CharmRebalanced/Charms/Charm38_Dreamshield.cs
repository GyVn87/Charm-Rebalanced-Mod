using HutongGames.PlayMaker;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm38_Dreamshield {
        private static readonly float shieldDamageRatio = 0.5f; // to Nail damage

        private static GameObject? newOrbitShield = null;
        private static GameObject? newShield = null;
        private static PlayMakerFSM? shieldHitFSM = null;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter += OnSpawnObjectFromGlobalPool_OnEnter;
            On.HutongGames.PlayMaker.Actions.SendEventByName.OnEnter += OnSendEventByName_OnEnter;
            On.HutongGames.PlayMaker.Actions.GetPlayerDataInt.OnEnter += OnGetPlayerDataInt_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter -= OnSpawnObjectFromGlobalPool_OnEnter;
            On.HutongGames.PlayMaker.Actions.SendEventByName.OnEnter -= OnSendEventByName_OnEnter;
            On.HutongGames.PlayMaker.Actions.GetPlayerDataInt.OnEnter -= OnGetPlayerDataInt_OnEnter;
        }

        private static void OnSpawnObjectFromGlobalPool_OnEnter(On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool self) {
            orig(self);
            string? gameObjectName = self.gameObject.Value?.name;
            if (self.Fsm.Name == "Spawn Orbit Shield" && gameObjectName == "Orbit Shield") {
                GameObject vanillaShield = self.storeObject.Value;
                newOrbitShield = Object.Instantiate<GameObject>(vanillaShield);
                newOrbitShield.name = "Extra Orbit Shield";
                newOrbitShield.AddComponent<CheckOverlap>();
            }
        }

        private static void OnSendEventByName_OnEnter(On.HutongGames.PlayMaker.Actions.SendEventByName.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SendEventByName self) {
            orig(self);
            if (self.Fsm.Name == "Spawn Orbit Shield" && self.State.Name == "Send Slash Event") {
                if (newOrbitShield == null)
                    newOrbitShield = GameObject.Find("Extra Orbit Shield");
                if (newShield == null)
                    newShield = newOrbitShield.transform.Find("Shield").gameObject;
                if (shieldHitFSM == null)
                    shieldHitFSM = ActionHelpers.GetGameObjectFsm(newShield, "Shield Hit");
                shieldHitFSM.SendEvent(self.sendEvent.Value);
            }
        }

        private static void OnGetPlayerDataInt_OnEnter(On.HutongGames.PlayMaker.Actions.GetPlayerDataInt.orig_OnEnter orig, HutongGames.PlayMaker.Actions.GetPlayerDataInt self) {
            orig(self);
            if (self.Fsm.Name == "Shield Hit" && self.State.Name == "Init")
                self.storeValue.Value = Mathf.CeilToInt(self.storeValue.Value * shieldDamageRatio);
        }
    }

    internal class CheckOverlap : MonoBehaviour {
        private int delayedFrames = 5;
        private void LateUpdate() {
            if (delayedFrames > 0)
                delayedFrames--;
            else {
                ApplyOffset();
                Destroy(this);
            }
        }

        void ApplyOffset() {
            Vector3 rotation = new(0, 0, 180.0f);
            transform.Rotate(rotation);
        }
    }
}
