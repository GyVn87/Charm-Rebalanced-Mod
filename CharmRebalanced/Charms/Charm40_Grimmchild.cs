using TuyenTuyenTuyen.Mechanics;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm40_Grimmchild {
        private static readonly float multipleShotsSpread = 30f;

        private static readonly float newSpread = 0f; // the lower, the more accurate Grimmchild's shot is
        private static readonly int lv2Damage = 5;
        private static readonly int lv3Damage = 8;
        private static readonly int lv4Damage = 11;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.FireAtTarget.OnEnter += Charm40_Grimmchild.OnFireAtTarget_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetIntValue.OnEnter += Charm40_Grimmchild.OnSetIntValue_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.FireAtTarget.OnEnter -= Charm40_Grimmchild.OnFireAtTarget_OnEnter;
            On.HutongGames.PlayMaker.Actions.SetIntValue.OnEnter -= Charm40_Grimmchild.OnSetIntValue_OnEnter;
        }

        private static void OnFireAtTarget_OnEnter(On.HutongGames.PlayMaker.Actions.FireAtTarget.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FireAtTarget self) {
            string ownerName = self.Owner?.name;
            if (!ownerName.StartsWith("Grimmchild") || self.Fsm.Name != "Control" || self.State.Name != "Shoot") {
                orig(self);
                return;
            }
            self.spread.Value = newSpread;
            orig(self);

            float angle = Utilities.GetAngleBetween2Object(self.Owner, self.target.Value);
            angle -= multipleShotsSpread;
            GameObject fireball = GameObject.Instantiate<GameObject>(self.gameObject.GameObject.Value);
            Utilities.SetVelocityAsAngle(fireball, angle, self.speed.Value);

            angle += multipleShotsSpread * 2;
            fireball = GameObject.Instantiate<GameObject>(self.gameObject.GameObject.Value);
            Utilities.SetVelocityAsAngle(fireball, angle, self.speed.Value);
        }

        private static void OnSetIntValue_OnEnter(On.HutongGames.PlayMaker.Actions.SetIntValue.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetIntValue self) {
            orig(self);

            string ownerName = self.Owner?.name;
            string stateName = self.State.Name;
            if (!ownerName.StartsWith("Grimmchild") || self.Fsm.Name != "Control" || !stateName.StartsWith("Level "))
                return;

            if (stateName == "Level 2")
                self.intVariable.Value = lv2Damage;
            else if (stateName == "Level 3")
                self.intVariable.Value = lv3Damage;
            else if (stateName == "Level 4")
                self.intVariable.Value = lv4Damage;
        }
    }
}
