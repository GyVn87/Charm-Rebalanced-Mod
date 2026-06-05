using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using SFCore;
using SFCore.Utils;
using TuyenTuyenTuyen.Charms;
using TuyenTuyenTuyen.Mechanics;
using UnityEngine;

namespace TuyenTuyenTuyen.CustomCharms {
    internal class KingsoulClone : EasyCharm {
        internal static KingsoulClone Instance { get; set; }

        public KingsoulClone() {
            Instance = this;
        }

        protected override int GetCharmCost() => 2;

        protected override string GetDescription() => """
            Holy charm symbolising a union between higher beings. The bearer will slowly absorb the limitless Soul contained within.

            Opens the way to a birthplace.
            """;

        protected override string GetName() => "Kingsoul";

        protected override Sprite GetSpriteInternal() => Utilities.LoadSprite("Kingsoul");

        internal static void Load() {
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            On.HeroController.Awake += OnHCAwake;
        }

        internal static void Unload() {
            ModHooks.CharmUpdateHook -= OnCharmUpdate;
            On.HeroController.Awake -= OnHCAwake;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            if (Instance == null)
                return;
            if (data.GetBool("gotCharm_36") && data.GetInt("royalCharmState") == 4)
                Instance.GotCharm = true;
            else
                Instance.GotCharm = false;
        }

        private static void OnHCAwake(On.HeroController.orig_Awake orig, HeroController self) {
            orig(self);
            AddCheckKingsoulClone();
        }

        private static void AddCheckKingsoulClone() {
            CheckEquippedCustomCharm newAction = new(Instance.GetName(), "ACTIVE", "");
            GameObject charmEffects = HeroController.instance.transform.Find("Charm Effects").gameObject;
            var FSM = charmEffects.LocateMyFSM("White Charm");
            FSM.GetState("Check").InsertAction(newAction, 0);
        }
    }

    public class CheckEquippedCustomCharm : FsmStateAction {
        public string customCharmName;
        public string isTrueEventName;
        public string isFalseEventName;
        public FsmEvent isTrue;
        public FsmEvent isFalse;

        public CheckEquippedCustomCharm(string charmName, string isTrueEvent, string isFalseEvent) {
            customCharmName = charmName;
            isTrueEventName = isTrueEvent;
            isFalseEventName = isFalseEvent;
        }

        public override void OnEnter() {
            GetEvent();
            var customCharms = CharmRebalanced.LoadedInstance.CustomCharms;
            bool boolTest = false;
            if (customCharms.TryGetValue(customCharmName, out var charm))
                boolTest = charm.IsEquipped;
            if (boolTest)
                base.Fsm.Event(isTrue);
            else
                base.Fsm.Event(isFalse);
            Finish();
        }

        private void GetEvent() {
            if (!string.IsNullOrEmpty(isTrueEventName))
                isTrue = FsmEvent.GetFsmEvent(isTrueEventName);
            if (!string.IsNullOrEmpty(isFalseEventName))
                isFalse = FsmEvent.GetFsmEvent(isFalseEventName);
        }
    }
}
