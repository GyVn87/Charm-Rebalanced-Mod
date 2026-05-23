using System.Collections;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm04_StalwartShell {
        private static readonly float recoilStalDuration = 0.08f;
        private static readonly float invulStalTime = 2f;
        private static readonly float damagePenalty = 0.25f;

        internal static void Load() {
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
            On.HeroController.CanFocus += OnHeroController_CanFocus;
        }

        internal static void Unload() {
            ModHooks.CharmUpdateHook -= OnCharmUpdate;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
            On.HeroController.CanFocus -= OnHeroController_CanFocus;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            controller.RECOIL_DURATION_STAL = recoilStalDuration;
            controller.INVUL_TIME_STAL = invulStalTime;
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            HeroController controller = HeroController.instance;
            PlayerData PD = PlayerData.instance;
            if (controller.cState.invulnerable && PD.GetBool("equippedCharm_4") && hitInstance.Source.transform?.parent?.name != "Thorn Hit")
                hitInstance.Multiplier *= damagePenalty;
            orig(self, hitInstance);
        }

        private static bool OnHeroController_CanFocus(On.HeroController.orig_CanFocus orig, HeroController self) {
            PlayerData PD = PlayerData.instance;
            HeroController controller = HeroController.instance;
            if (PD.GetBool("equippedCharm_4") && controller.cState.invulnerable)
                return false;
            return orig(self);
        }
    }
}