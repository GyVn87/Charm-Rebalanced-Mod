using System.Reflection;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm31_Dashmaster {
        private static readonly float dashSpeed = 20.0f;
        private static readonly float dashSpeedIncrease = 1f;
        private static readonly float dashSpeedMaster = dashSpeed * dashSpeedIncrease;

        private static readonly float shadowDashCooldown = 1.5f;
        private static readonly float shadowDashCooldownDecrease = 1f;
        private static readonly float shadowDashCooldownMaster = shadowDashCooldown * shadowDashCooldownDecrease;

        private static readonly float cooldownDecreaseOnDash = 0.15f;

        private static readonly FieldInfo shadowDashTimer = typeof(HeroController).GetField("shadowDashTimer", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static void Load() {
            ModHooks.CharmUpdateHook += Charm31_Dashmaster.OnCharmUpdate;
            On.HeroController.HeroDash += OnHeroController_HeroDash;
        }

        internal static void Unload() {
            ModHooks.CharmUpdateHook -= Charm31_Dashmaster.OnCharmUpdate;
            On.HeroController.HeroDash -= OnHeroController_HeroDash;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            controller.DASH_COOLDOWN_CH = 0.35f;
            if (data.GetBool("equippedCharm_31")) {
                controller.DASH_SPEED = dashSpeedMaster;
                controller.SHADOW_DASH_COOLDOWN = shadowDashCooldownMaster;
            }
            else {
                controller.DASH_SPEED = dashSpeed;
                controller.SHADOW_DASH_COOLDOWN = shadowDashCooldown;
            }
        }

        private static void OnHeroController_HeroDash(On.HeroController.orig_HeroDash orig, HeroController self) {
            orig(self);
            if (PlayerData.instance.GetBool("equippedCharm_31") && PlayerData.instance.hasShadowDash && !self.cState.shadowDashing)
                shadowDashTimer.SetValue(HeroController.instance, (float)shadowDashTimer.GetValue(HeroController.instance) - cooldownDecreaseOnDash);
        }
    }
}
