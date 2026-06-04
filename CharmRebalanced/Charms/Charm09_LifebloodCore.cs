using System.Collections;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm09_LifebloodCore {
        private static readonly int blueHealthIncreases = 0;
        private static readonly int totalDamageTakenToGainLifeblood = 4;

        private static int damageTakenCounter = 0;
        private static PlayMakerFSM blueHealthControlFSM = null;


        internal static void Load() {
            On.PlayerData.UpdateBlueHealth += Charm09_LifebloodCore.OnUpdateBlueHealth;
            On.PlayerData.MaxHealth += OnPDMaxHealth;
            ModHooks.TakeHealthHook += OnTakeHealth;
        }

        internal static void Unload() {
            On.PlayerData.UpdateBlueHealth -= Charm09_LifebloodCore.OnUpdateBlueHealth;
            On.PlayerData.MaxHealth -= OnPDMaxHealth;
            ModHooks.TakeHealthHook -= OnTakeHealth;
        }

        private static void OnUpdateBlueHealth(On.PlayerData.orig_UpdateBlueHealth orig, PlayerData self) {
            orig(self);
            if (self.GetBool("equippedCharm_9"))
                self.SetInt("healthBlue", self.GetInt("healthBlue") - 4 + blueHealthIncreases);
        }

        private static int OnTakeHealth(int orig) {
            if (orig > 100)
                return orig;

            PlayerData PD = PlayerData.instance;
            if (PD.GetBool("equippedCharm_9")) {
                damageTakenCounter += orig;
                GameManager.instance.StartCoroutine(GrantLifeblood(PD));
            }
            return orig;
        }

        private static IEnumerator GrantLifeblood(PlayerData data) {
            yield return null;

            if (blueHealthControlFSM == null) {
                GameObject healthGameObject = GameCameras.instance.transform.Find("HudCamera/Hud Canvas/Health").gameObject;
                blueHealthControlFSM = healthGameObject.LocateMyFSM("Blue Health Control");
            }

            int lifebloodMasksGain = damageTakenCounter / totalDamageTakenToGainLifeblood;
            for (int i = 1; i <= lifebloodMasksGain; i++)
                blueHealthControlFSM.SendEvent("ADD BLUE HEALTH");
            damageTakenCounter %= totalDamageTakenToGainLifeblood;
        }

        private static void OnPDMaxHealth(On.PlayerData.orig_MaxHealth orig, PlayerData self) {
            orig(self);
            damageTakenCounter = 0;
        }
    }
}
