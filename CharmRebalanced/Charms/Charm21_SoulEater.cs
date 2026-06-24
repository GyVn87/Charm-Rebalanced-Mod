using TuyenTuyenTuyen.Mechanics;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm21_SoulEater {
        private static readonly int soulCharge = 4;
        private static readonly int soulReserve = 3;
        internal static readonly float eaterEffectDuration = 3f;

        internal static void Load() {
            ModHooks.SoulGainHook += OnSoulGain;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
        }

        internal static void Unload() {
            ModHooks.SoulGainHook -= OnSoulGain;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
        }

        private static int OnSoulGain(int orig) {
            PlayerData playerData = PlayerData.instance;
            if (playerData.GetBool("equippedCharm_21")) {
                if (playerData.GetInt("MPCharge") < playerData.GetInt("maxMP"))
                    orig = orig - 8 + soulCharge;
                else
                    orig = orig - 6 + soulReserve;
            }
            return orig;
        }   

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            GameObject owner = self.gameObject;
            if (hitInstance.AttackType == AttackTypes.Nail) {
                EaterCurse eaterEffect = owner.GetComponent<EaterCurse>();
                if (eaterEffect != null) {
                    HeroController.instance.SoulGain();
                    eaterEffect.ClearEffect();
                }
            }
            else if (PlayerData.instance.GetBool("equippedCharm_21") && hitInstance.AttackType == AttackTypes.Spell) {
                EaterCurse eaterEffect = owner.GetComponent<EaterCurse>();
                if (eaterEffect != null)
                    eaterEffect.RefreshTimer();
                else 
                    owner.AddComponent<EaterCurse>();
            }
            orig(self, hitInstance);
        }
    }

    public class EaterCurse : TuyenTuyenTuyen.Mechanics.CustomEffect {
        protected override float Duration => Charm21_SoulEater.eaterEffectDuration;
        protected override Color StartColor => new(0.85f, 0.90f, 0.92f, 0.65f);
        protected override Vector3 LocalScale => new(2f, 2f, 2f);
        protected override string Name => "Eater Effect Particle";
    }

    public class EaterCurseCooldown : CustomEffect {
        protected override float Duration => 3f;
        protected override Color StartColor => new(1f, 1f, 1f, 0f);
        protected override Vector3 LocalScale => new(1f, 1f, 1f);
        protected override string Name => "Eater Curse Cooldown";

        EaterCurseCooldown() {
            base.SetEmissionRate(0f);
        }
    }
}
