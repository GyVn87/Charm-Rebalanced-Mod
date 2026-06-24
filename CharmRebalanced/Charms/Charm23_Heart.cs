using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm23_Heart {
        private static readonly float masksIncreaseRatio = 0.34f;

        internal static void Load() {
            On.HUDCamera.OnEnable += OnHCOnEnable;
            ModHooks.CharmUpdateHook += OnCharmUpdate;
        }

        internal static void Unload() {
            On.HUDCamera.OnEnable -= OnHCOnEnable;
            ModHooks.CharmUpdateHook -= OnCharmUpdate;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            if (data.GetBool("equippedCharm_23") && !data.GetBool("brokenCharm_23"))
                data.SetInt("maxHealth", Mathf.FloorToInt((float)data.GetInt("maxHealthBase") * (1f +masksIncreaseRatio)));
            else
                data.SetInt("maxHealth", data.GetInt("maxHealthBase"));
            controller.MaxHealth();
        }

        private static void OnHCOnEnable(On.HUDCamera.orig_OnEnable orig, HUDCamera self) {
            orig(self);
            Transform healthTransf = GameCameras.instance.hudCanvas.transform.Find("Health");
            GameObject health11 = healthTransf.Find("Health 11").gameObject;

            // the game only has 11 Health game objects, so we need to instantiate a new one
            GameObject health12 = GameObject.Instantiate<GameObject>(health11, healthTransf);
            health12.name = "Health 12";
            PlayMakerFSM FSM = health12.LocateMyFSM("health_display");
            FSM.FsmVariables.GetFsmInt("Health Number").Value = 12;
            Vector3 health11Position = health11.transform.localPosition;
            health12.transform.localPosition = new Vector3(health11Position.x + 0.94f, health11Position.y, health11Position.z);
        }
    }
}
