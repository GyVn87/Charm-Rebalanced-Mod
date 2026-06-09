using HutongGames.PlayMaker;
using System;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm37_Sprintmaster {
        private static readonly float runSpeed = 8.3f;
        private static readonly float speedIncrease = 1.25f;
        private static readonly float runSpeedMaster = runSpeed * speedIncrease;

        private static readonly float speedComboIncrease = 1.4f;
        private static readonly float runSpeedCombo = runSpeed * speedComboIncrease;

        private static readonly float swimSpeedBase = 5f;
        private static readonly float swimSpeedMaster = 10f;

        internal static void Load() {
            ModHooks.CharmUpdateHook += Charm37_Sprintmaster.OnCharmUpdate;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent += OnFsmProcessEvent;
        }

        internal static void Unload() {
            ModHooks.CharmUpdateHook -= Charm37_Sprintmaster.OnCharmUpdate;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent -= OnFsmProcessEvent;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            if (data.GetBool("equippedCharm_37")) {
                if (data.GetBool("equippedCharm_31"))
                    controller.RUN_SPEED = runSpeedCombo;
                else
                    controller.RUN_SPEED = runSpeedMaster;
            }
            else {
                controller.RUN_SPEED = runSpeed;
            }
            controller.RUN_SPEED_CH = runSpeedMaster;
            controller.RUN_SPEED_CH_COMBO = runSpeedCombo;
        }

        private static void OnFsmProcessEvent(On.HutongGames.PlayMaker.Fsm.orig_ProcessEvent orig, HutongGames.PlayMaker.Fsm self, HutongGames.PlayMaker.FsmEvent fsmEvent, HutongGames.PlayMaker.FsmEventData eventData) {
            orig(self, fsmEvent, eventData);
            if (self.Name != "Surface Water" || fsmEvent.Name != "SURFACE ENTER")
                return;
            FsmFloat swimSpeedFsm = self.Variables.FindFsmFloat("Swim Speed");
            FsmFloat swimSpeedNegFsm = self.Variables.FindFsmFloat("Swim Speed neg");
            if (PlayerData.instance.GetBool("equippedCharm_37")) {
                swimSpeedFsm.Value = swimSpeedMaster;
                swimSpeedNegFsm.Value = -swimSpeedMaster;
            }
            else {
                swimSpeedFsm.Value = swimSpeedBase;
                swimSpeedNegFsm.Value = -swimSpeedBase;
            }
        }
    }
}
