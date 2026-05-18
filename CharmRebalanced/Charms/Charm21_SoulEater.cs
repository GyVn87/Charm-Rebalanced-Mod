using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm21_SoulEater {
        private static readonly int soulCharge = 6;
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
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            if (PD.GetBool("equippedCharm_21")) {
                if (PD.GetInt("MPCharge") < PD.GetInt("maxMP"))
                    orig = orig - 8 + soulCharge;
                else
                    orig = orig - 6 + soulReserve;
            }
            return orig;
        }   

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            GameObject owner = self.gameObject;
            if (hitInstance.AttackType == AttackTypes.Nail) {
                EaterEffect eaterEffect = owner.GetComponent<EaterEffect>();
                if (eaterEffect != null) {
                    HeroController.instance.SoulGain();
                    eaterEffect.Destroy();
                }
            }
            else if (PlayerData.instance.GetBool("equippedCharm_21") && hitInstance.AttackType == AttackTypes.Spell) {
                EaterEffect eaterEffect = owner.GetComponent<EaterEffect>();
                if (eaterEffect != null)
                    eaterEffect.RefreshTimer();
                else 
                    owner.AddComponent<EaterEffect>();
            }
            orig(self, hitInstance);
        }
    }

    public class EaterEffect : MonoBehaviour {
        private float timer = 0f;
        private readonly float duration = Charm21_SoulEater.eaterEffectDuration;
        private GameObject eaterParticle = null;
        private static GameObject particlePrefab = null;

        public void Awake() {
            timer = duration;
            if (particlePrefab == null) {
                Transform KnighTransform = HeroController.instance.transform;
                particlePrefab = KnighTransform.Find("Charm Effects/Dung/Particle 1").gameObject;
            }
            eaterParticle = Object.Instantiate<GameObject>(particlePrefab, transform);
            eaterParticle.name = "Eater Effect Particle";
            eaterParticle.SetActive(true);
            ParticleSystem ps = eaterParticle.GetComponent<ParticleSystem>();
            var emission = ps.emission;
            emission.enabled = true;
            var main = ps.main;
            main.startColor = new Color(0.85f, 0.90f, 0.92f, 0.65f);
            eaterParticle.transform.localScale = new Vector3(2f, 2f, 2f);
            ps.Clear();
            ps.Play();
        }

        public void RefreshTimer() {
            timer = duration;
        }

        public void Destroy() {
            Object.Destroy(eaterParticle);
            Object.Destroy(this);
        }

        private void Update() {
            timer -= Time.deltaTime;
            if (timer < 0f) {
                Object.Destroy(eaterParticle);
                Object.Destroy(this);
            }
        }
    }
}
