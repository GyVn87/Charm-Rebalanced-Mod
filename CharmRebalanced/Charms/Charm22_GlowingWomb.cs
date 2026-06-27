using System;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm22_GlowingWomb {
        private static readonly int normalMaximumHatchlings = 6;
        private static readonly int deepMaximumHatchling = 8;
        private static readonly int hatchlingSpawnFocus = 3;
        private static readonly int hatchlingSpawnFocusDeep = 4;
        private static readonly float hatchlingDamageRatio = 2f / 3f; // to Nail damage

        private static int MaximumHatchlings => (PlayerData.instance.GetBool("equippedCharm_34") ? deepMaximumHatchling : normalMaximumHatchlings);
        private static GameObject? knightHatchlingPrefab = null;

        internal static void Load() {
            On.KnightHatchling.Awake += OnKnightHatchling_OnAwake;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent += OnFsmProcessEvent;
            On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter += OnSendMessage_OnEnter;
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter += OnSpawnObjectFromGlobalPool_OnEnter;
        }

        internal static void Unload() {
            On.KnightHatchling.Awake -= OnKnightHatchling_OnAwake;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent -= OnFsmProcessEvent;
            On.HutongGames.PlayMaker.Actions.SendMessage.OnEnter -= OnSendMessage_OnEnter;
            On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.OnEnter -= OnSpawnObjectFromGlobalPool_OnEnter;
        }

        private static void OnKnightHatchling_OnAwake(On.KnightHatchling.orig_Awake orig, KnightHatchling self) {
            orig(self);
            int nailDamage = PlayerData.instance.GetInt("nailDamage");
            self.normalDetails.damage = Mathf.CeilToInt((float)nailDamage * hatchlingDamageRatio);
            self.dungDetails.damage = Mathf.CeilToInt((float)nailDamage * hatchlingDamageRatio);
        }

        private static void OnFsmProcessEvent(On.HutongGames.PlayMaker.Fsm.orig_ProcessEvent orig, HutongGames.PlayMaker.Fsm self, HutongGames.PlayMaker.FsmEvent fsmEvent, HutongGames.PlayMaker.FsmEventData eventData) {
            orig(self, fsmEvent, eventData);
            if (self.Name != "Spell Control" || fsmEvent.Name != "FOCUS COMPLETED")
                return;
            if (PlayerData.instance.GetBool("equippedCharm_22")) {
                GameObject[] hatchlingSpawn = GameObject.FindGameObjectsWithTag("Knight Hatchling");
                int hatchlingNum = ((hatchlingSpawn != null) ? hatchlingSpawn.Length : 0);
                if (hatchlingNum < MaximumHatchlings) {
                    PlayerData PD = PlayerData.instance;
                    GameObject Knight = HeroController.instance.gameObject;
                    if (PD.GetBool("equippedCharm_34")) // Deep Focus
                        SpawnHatchling(Knight, Math.Min(MaximumHatchlings - hatchlingNum, hatchlingSpawnFocusDeep));
                    else
                        SpawnHatchling(Knight, Math.Min(MaximumHatchlings - hatchlingNum, hatchlingSpawnFocus));
                }
            }
        }

        private static void OnSendMessage_OnEnter(On.HutongGames.PlayMaker.Actions.SendMessage.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SendMessage self) {
            if (self.Fsm.Name == "Hatchling Spawn" && self.State.Name == "Hatch") {
                self.Finish();
                return;
            }
            orig(self);
        }

        private static void OnSpawnObjectFromGlobalPool_OnEnter(On.HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SpawnObjectFromGlobalPool self) {
            if (self.Fsm.Name == "Hatchling Spawn" && self.State.Name == "Hatch") {
                knightHatchlingPrefab = self.gameObject.Value;
                self.Fsm.Event("FINISHED");
                return;
            }
            orig(self);
        }

        private static void SpawnHatchling(GameObject spawnPoint, int number) {
            if (knightHatchlingPrefab == null)
                return;
            for (int i = 1; i <= number; i++)
                knightHatchlingPrefab.Spawn(spawnPoint.transform.position, spawnPoint.transform.rotation);
        }

        // no longer be used... maybe
        private static void OnSetVelocity2d_OnEnter(On.HutongGames.PlayMaker.Actions.SetVelocity2d.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetVelocity2d self) {
            orig(self);
            if (self.Fsm.Name == "Spell Control" && self.State.Name == "Spell End") {
                if (PlayerData.instance.GetBool("equippedCharm_22")) {
                    GameObject[] hatchlingSpawn = GameObject.FindGameObjectsWithTag("Knight Hatchling");
                    int hatchlingNum = ((hatchlingSpawn != null) ? hatchlingSpawn.Length : 0);
                    if (hatchlingNum < MaximumHatchlings) {
                        GameObject Knight = HeroController.instance.gameObject;
                        SpawnHatchling(Knight, 1);
                    }
                }
            }
        }
    }
}
