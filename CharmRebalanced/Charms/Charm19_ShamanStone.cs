using HutongGames.PlayMaker;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm19_ShamanStone {
        private static readonly int vengefulSpiritDamage = 15;
        private static readonly int shadeSoulDamage = 30;
        private static readonly int howlingWraithsDamage = 13;
        private static readonly int abyssShriekDamage = 20;
        private static readonly int diveDamage = 15;
        private static readonly int shockwaveDiveDamage = 20;
        private static readonly int shockwaveDarkDamage = 30;
        private static readonly int megaDarkDamage = 15;

        internal static readonly float fireballDamageIncrease = 1.3f;
        internal static readonly float spellDamageIncrease = 1.4f;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.SetFsmInt.OnEnter += Charm19_ShamanStone.OnSetFsmInt_OnEnter;
            On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter += OnIntCompare_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.SetFsmInt.OnEnter -= Charm19_ShamanStone.OnSetFsmInt_OnEnter;
            On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter -= OnIntCompare_OnEnter;
        }

        private static void OnSetFsmInt_OnEnter(On.HutongGames.PlayMaker.Actions.SetFsmInt.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetFsmInt self) {
            orig(self);
            if (self.State.Name != "Set Damage") 
                return;

            int newDamage = self.setValue.Value;
            if (self.Fsm.Name == "Fireball Control")
                newDamage = NewFireballDamage(self);
            else if (self.Fsm.Name == "Set Damage") {
                string ownerName = self.Owner?.name;
                string parentName = self.Owner?.transform?.parent?.name;

                if (parentName == "Scr Heads")
                    newDamage = NewHowlingWraithsDamage(self);
                else if (parentName == "Scr Heads 2")
                    newDamage = NewAbyssShriekDamage(self);
                else if (ownerName == "Q Fall Damage")
                    newDamage = NewDiveDamage(self);
                else if (parentName == "Q Slam")
                    newDamage = NewShockwaveDiveDamage(self);
                else if (parentName == "Q Slam 2")
                    newDamage = NewShockwaveDarkDamage(self);
            }
            GameObject targetGO = self.Fsm.GetOwnerDefaultTarget(self.gameObject);
            if (targetGO == null) return;
            PlayMakerFSM targetFSM = ActionHelpers.GetGameObjectFsm(targetGO, self.fsmName.Value);
            if (targetFSM == null) return;
            FsmInt fsmInt = targetFSM.FsmVariables.GetFsmInt(self.variableName.Value);
            if (fsmInt == null) return;
            fsmInt.Value = newDamage;
        }

        private static int NewFireballDamage(HutongGames.PlayMaker.Actions.SetFsmInt self) {
            FsmInt setValue = self.setValue;
            int newDamage = setValue.Value;
            switch (setValue.Value) {
                case 15:  // Vengeful Spirit 
                    newDamage = vengefulSpiritDamage;
                    break;
                case 20:  // Vengeful Spirit + Shaman Stone
                    newDamage = Mathf.CeilToInt((float)vengefulSpiritDamage * fireballDamageIncrease);
                    break;
                case 30:  // Shade Soul
                    newDamage = shadeSoulDamage;
                    break;
                case 40:  // Shade Soul + Shaman Stone
                    newDamage = Mathf.CeilToInt((float)shadeSoulDamage * fireballDamageIncrease);
                    break;
                default:
                    break;
            }
            return newDamage;
        }

        private static int NewHowlingWraithsDamage(HutongGames.PlayMaker.Actions.SetFsmInt self) {
            FsmInt setValue = self.setValue;
            int newDamage = setValue.Value;
            switch (setValue.Value) {
                case 13:  // Howling Wraiths
                    newDamage = howlingWraithsDamage;
                    break;
                case 20:  // Howling Wraiths + Shaman Stone
                    newDamage = Mathf.CeilToInt((float)howlingWraithsDamage * spellDamageIncrease);
                    break;
                default:
                    break;
            }
            return newDamage;
        }

        private static int NewAbyssShriekDamage(HutongGames.PlayMaker.Actions.SetFsmInt self) {
            FsmInt setValue = self.setValue;
            int newDamage = setValue.Value;
            switch (setValue.Value) {
                case 20:  // Abyss Shriek
                    newDamage = abyssShriekDamage;
                    break;
                case 30:  // Abyss Shriek + Shaman Stone
                    newDamage = Mathf.CeilToInt((float)abyssShriekDamage * spellDamageIncrease);
                    break;
                default:
                    break;
            }
            return newDamage;
        }

        private static int NewDiveDamage(HutongGames.PlayMaker.Actions.SetFsmInt self) {
            FsmInt setValue = self.setValue;
            int newDamage = setValue.Value;
            switch (setValue.Value) {
                case 15:  // Dive
                    newDamage = diveDamage;
                    break;
                case 23:  // Dive + Shaman Stone
                    newDamage = Mathf.CeilToInt((float)diveDamage * spellDamageIncrease);
                    break;
                default:
                    break;
            }
            return newDamage;
        }

        private static int NewShockwaveDiveDamage(HutongGames.PlayMaker.Actions.SetFsmInt self) {
            FsmInt setValue = self.setValue;
            int newDamage = setValue.Value;
            switch (setValue.Value) {
                case 20:  // Shockwave of Desolate Dive
                    newDamage = shockwaveDiveDamage;
                    break;
                case 30:  // Shockwave of Desolate Dive + Shaman Stone
                    newDamage = Mathf.CeilToInt((float)shockwaveDiveDamage * spellDamageIncrease);
                    break;
                default:
                    break;
            }
            return newDamage;
        }

        private static int NewShockwaveDarkDamage(HutongGames.PlayMaker.Actions.SetFsmInt self) {
            FsmInt setValue = self.setValue;
            int newDamage = setValue.Value;
            switch (setValue.Value) {
                case 30:  // Shockwave of Descending Dark
                    newDamage = shockwaveDarkDamage;
                    break;
                case 50:  // Shockwave of Descending Dark + Shaman Stone
                    newDamage = Mathf.CeilToInt((float)shockwaveDarkDamage * spellDamageIncrease);
                    break;
                default:
                    break;
            }
            return newDamage;
        }

        private static void OnIntCompare_OnEnter(On.HutongGames.PlayMaker.Actions.IntCompare.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntCompare self) {
            orig(self);
            string parentName = self.Owner?.transform?.parent?.name;
            if (parentName == "Q Mega" && self.State.Name == "Send Event") {
                if (PlayerData.instance.GetBool("equippedCharm_19"))
                    self.integer1.Value = Mathf.CeilToInt((float)megaDarkDamage * spellDamageIncrease);
                else
                    self.integer1.Value = megaDarkDamage;
            }
        }
    }
}
