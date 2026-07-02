using HutongGames.PlayMaker;

namespace TuyenTuyenTuyen.FSM {
    public class CheckEquippedCustomCharm : FsmStateAction {
        public string customCharmName;
        public string isTrueEventName;
        public string isFalseEventName;
        public FsmEvent? isTrue = null;
        public FsmEvent? isFalse = null;

        public CheckEquippedCustomCharm(string charmName, string isTrueEvent, string isFalseEvent) {
            customCharmName = charmName;
            isTrueEventName = isTrueEvent;
            isFalseEventName = isFalseEvent;
        }

        public override void OnEnter() {
            GetEvent();
            var customCharms = CharmRebalanced.LoadedInstance!.CustomCharms;
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
