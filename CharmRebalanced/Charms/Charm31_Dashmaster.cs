using System.Reflection;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm31_Dashmaster {
        private static readonly float dashSpeed = 20.0f;
        private static readonly float dashSpeedIncrease = 1f;
        private static readonly float dashSpeedMaster = dashSpeed * dashSpeedIncrease;

        private static readonly float shadowDashCooldown = 1.5f;
        private static readonly float shadowDashCooldownDecrease = 0.8f;
        private static readonly float shadowDashCooldownMaster = shadowDashCooldown * shadowDashCooldownDecrease;

        private static readonly float dashCooldown = 0.3f;
        private static readonly float cooldownDecreaseOnDash = 0f;

        private static InputHandler? inputHandler = null;

        private static readonly FieldInfo inputHandlerField = typeof(HeroController).GetField("inputHandler", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo airDashed = typeof(HeroController).GetField("airDashed", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo dashCooldownTimer = typeof(HeroController).GetField("dashCooldownTimer", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo shadowDashTimer = typeof(HeroController).GetField("shadowDashTimer", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static void Load() {
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            On.HeroController.HeroDash += OnHeroController_HeroDash;
            On.HeroController.CanDash += OnHeroController_CanDash;
        }

        internal static void Unload() {
            ModHooks.CharmUpdateHook -= OnCharmUpdate;
            On.HeroController.HeroDash -= OnHeroController_HeroDash;
            On.HeroController.CanDash -= OnHeroController_CanDash;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            controller.DASH_COOLDOWN_CH = dashCooldown;
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
            PlayerData playerData = PlayerData.instance;
            if (playerData.GetBool("equippedCharm_31") && playerData.hasShadowDash && !self.cState.shadowDashing)
                shadowDashTimer.SetValue(HeroController.instance, (float)shadowDashTimer.GetValue(HeroController.instance) - cooldownDecreaseOnDash);
            if (self.dashingDown) {
                dashCooldownTimer.SetValue(self, 0);
                airDashed.SetValue(self, false);
            }
        }

        private static bool OnHeroController_CanDash(On.HeroController.orig_CanDash orig, HeroController self) {
            if (inputHandler == null)
                inputHandler = (InputHandler)inputHandlerField.GetValue(self);
            HeroActions inputActions = inputHandler.inputActions;
            if (inputActions.down.IsPressed && !self.cState.onGround && PlayerData.instance.GetBool("equippedCharm_31") && !inputActions.left.IsPressed && !inputActions.right.IsPressed)
                return true;
            return orig(self);
        }
    }
}
