using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm05_BaldurShell {
        private static readonly int maximumBlockerHits = 4;
        private static readonly int focusTimesNeededToRestore = 2;
        private static readonly int brokenStage1 = 3; // Equal to when vanilla Baldur Shell takes 1 hit
        private static readonly int brokenStage2 = 2; // Equal to when vanilla Baldur Shell takes 2 hits

        private static PlayMakerFSM blockerShieldFSM = null;
        private static int focusCounter = 0;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.IntSwitch.OnEnter += Charm05_BaldurShell.OnIntSwitch_OnEnter;
            On.PlayerData.MaxHealth += Charm05_BaldurShell.OnPDMaxHealth;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent += OnFsmProcessEvent;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.IntSwitch.OnEnter -= Charm05_BaldurShell.OnIntSwitch_OnEnter;
            On.PlayerData.MaxHealth -= Charm05_BaldurShell.OnPDMaxHealth;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent -= OnFsmProcessEvent;
        }

        private static void OnFsmProcessEvent(On.HutongGames.PlayMaker.Fsm.orig_ProcessEvent orig, HutongGames.PlayMaker.Fsm self, HutongGames.PlayMaker.FsmEvent fsmEvent, HutongGames.PlayMaker.FsmEventData eventData) {
            orig(self, fsmEvent, eventData);
            if (self.Name != "Spell Control" || fsmEvent.Name != "FOCUS COMPLETED")
                return;

            PlayerData playerData = CharmRebalanced.LoadedInstance.PD;
            if (!playerData.GetBool("equippedCharm_5") || playerData.blockerHits >= maximumBlockerHits)
                return;

            if (blockerShieldFSM == null) {
                GameObject Knight = CharmRebalanced.LoadedInstance.Knight;
                blockerShieldFSM = Knight.transform.Find("Charm Effects").Find("Blocker Shield").GetComponent<PlayMakerFSM>();
            }

            focusCounter += (playerData.GetBool("equippedCharm_34") ? 2 : 1);
            if (focusCounter < focusTimesNeededToRestore)
                return;
            focusCounter = 0;
            playerData.SetInt("blockerHits", Math.Min(maximumBlockerHits, playerData.blockerHits + 1));

            if (playerData.blockerHits >= maximumBlockerHits) {
                blockerShieldFSM.Fsm.SetState("HUD Icon Up");
                blockerShieldFSM.Fsm.SetState("Focus End");
            }
            else {
                playerData.blockerHits++; // Since blockerHits will be decremented by 1 in "Blocker Hit" state, so this line will be netraulized
                blockerShieldFSM.Fsm.SetState("Blocker Hit");
            }
        }

        /// <summary>
        /// override IntSwitch.OnEnter() method of state "Blocker Hit" of FSM Blocker Shield
        /// </summary>
        private static void OnIntSwitch_OnEnter(On.HutongGames.PlayMaker.Actions.IntSwitch.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntSwitch self) {
            if (self.State.Name != "Blocker Hit" || self.Owner.name != "Blocker Shield") {
                orig(self);
                return;
            }
            int blockHits = self.intVariable.Value;
            // update the texture of the Baldur Shell icon on the up left of the screen
            if (blockHits >= brokenStage1)
                self.Fsm.Event("3"); // STAGE 1
            else if (blockHits >= brokenStage2)
                self.Fsm.Event("2"); // STAGE 2
            else
                self.Fsm.Event("1"); // STAGE 3
        }

        private static void OnPDMaxHealth(On.PlayerData.orig_MaxHealth orig, PlayerData self) {
            orig(self);
            self.SetInt("blockerHits", maximumBlockerHits);
        }
    }
}
