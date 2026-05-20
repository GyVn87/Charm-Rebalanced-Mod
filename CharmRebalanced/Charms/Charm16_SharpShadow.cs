using UnityEngine;
using UnityEngine.Audio;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm16_SharpShadow {
        private static readonly float shadowDashSpeedIncrease = 1.3f;
        private static readonly float shadowDashDamageSharp = 0.5f;
        private static readonly float shadowDashDamageMaster = 1f;
        internal static readonly float shadowMarkBurstMultiplier = 1.5f;
        internal static readonly float shadowMarkDuration = 3f;

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.FloatMultiplyV2.OnEnter += Charm16_SharpShadow.OnFloatMultiplyV2_OnEnter;
            On.HutongGames.PlayMaker.Actions.ConvertFloatToInt.OnEnter += Charm16_SharpShadow.OnConvertFloatToInt_OnEnter;
            ModHooks.CharmUpdateHook += Charm16_SharpShadow.OnCharmUpdate;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.FloatMultiplyV2.OnEnter -= Charm16_SharpShadow.OnFloatMultiplyV2_OnEnter;
            On.HutongGames.PlayMaker.Actions.ConvertFloatToInt.OnEnter -= Charm16_SharpShadow.OnConvertFloatToInt_OnEnter;
            ModHooks.CharmUpdateHook -= Charm16_SharpShadow.OnCharmUpdate;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            if (data.GetBool("equippedCharm_16"))
                controller.DASH_SPEED_SHARP = controller.DASH_SPEED * shadowDashSpeedIncrease;
            else
                controller.DASH_SPEED_SHARP = controller.DASH_SPEED;
        }

        private static void OnFloatMultiplyV2_OnEnter(On.HutongGames.PlayMaker.Actions.FloatMultiplyV2.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FloatMultiplyV2 self) {
            orig(self);
            if (self.Fsm.Name == "Set Sharp Shadow Damage" && self.State.Name == "Master")
                self.floatVariable.Value = self.floatVariable.Value / self.multiplyBy.Value * shadowDashDamageMaster;
        }

        private static void OnConvertFloatToInt_OnEnter(On.HutongGames.PlayMaker.Actions.ConvertFloatToInt.orig_OnEnter orig, HutongGames.PlayMaker.Actions.ConvertFloatToInt self) {
            if (self.Fsm.Name == "Set Sharp Shadow Damage" && self.State.Name == "Set") {
                if (!CharmRebalanced.LoadedInstance.PD.GetBool("equippedCharm_31"))
                    self.floatVariable.Value *= shadowDashDamageSharp;
            }
            orig(self);
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            if (!PlayerData.instance.GetBool("equippedCharm_16")) {
                orig(self, hitInstance);
                return;
            }

            GameObject owner = self.gameObject;
            ShadowMark shadowMark = owner.GetComponent<ShadowMark>();
            if (hitInstance.AttackType != AttackTypes.SharpShadow) {
                if (shadowMark) {
                    CharmRebalanced.LoadedInstance.Log("worked"); // debug
                    shadowMark.Burst();
                    if (hitInstance.AttackType == AttackTypes.Spell)
                        shadowMark.SetTimer(0.5f);
                    else
                        shadowMark.ClearEffect();
                    hitInstance.Multiplier *= shadowMarkBurstMultiplier;
                }
            }
            else {   
                if (shadowMark)
                    shadowMark.RefreshTimer();
                else
                    owner.AddComponent<ShadowMark>();
            }
            orig(self, hitInstance);
        }
    }

    public class ShadowMark : TuyenTuyenTuyen.Mechanics.CustomEffect {
        public override float Duration => Charm16_SharpShadow.shadowMarkDuration;
        public override Color StartColor => new(0.15f, 0.02f, 0.25f, 0.85f); // dark violet
        public override Vector3 LocalScale => new(2f, 2f, 2f);
        public override string Name => "Shadow Mark Particle";

        private static AudioClip screamClip = null;
        private static AudioMixerGroup mixerGroup = null;

        private bool hasBursted = false;

        public void Burst() {
            if (hasBursted) return;
            hasBursted = true;

            ScreamSound();

            GameObject prefab = GetPrefab();
            GameObject burstEffect = Object.Instantiate<GameObject>(prefab, transform);
            burstEffect.SetActive(true);
            burstEffect.name = "Shadow Mark Burst";
            burstEffect.transform.localScale = LocalScale;

            ParticleSystem ps = burstEffect.GetComponent<ParticleSystem>();
            var emission = ps.emission;
            emission.rateOverTimeMultiplier = 0f;
            emission.rateOverTime = 0f;
            emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 100, 120) });
            emission.enabled = true;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;

            var main = ps.main;
            main.loop = false;
            main.startColor = new Color(0.0f, 0.0f, 0.0f, 1f); // black
            main.startSpeed = 8f;
            main.startSize = 1f;
            main.startLifetime = 0.5f;
            main.stopAction = ParticleSystemStopAction.Destroy;

            ParticleSystemRenderer psRenderer = burstEffect.GetComponent<ParticleSystemRenderer>();
            psRenderer.sortingOrder = 2;
            psRenderer.sortingLayerName = "Enemies";

            ps.Clear();
            ps.Play();
        }

        private void ScreamSound() {
            if (screamClip == null) {
                AudioClip[] clips = Resources.FindObjectsOfTypeAll<AudioClip>();
                foreach (AudioClip clip in clips) {
                    if (clip != null && clip.name == "hollow_shade_startle") {
                        screamClip = clip;
                        break;
                    }
                }
            }
            if (mixerGroup == null) {
                AudioMixerGroup[] mixerGroups = Resources.FindObjectsOfTypeAll<AudioMixerGroup>();
                foreach (AudioMixerGroup group in mixerGroups) {
                    if (group != null && group.name == "Actors" && group.audioMixer.name == "Actors") {
                        mixerGroup = group;
                        break;
                    }
                }
            }

            for (int i = 1; i <= 5; i++) {
                GameObject audioPlayer = new GameObject("Scream Audio Player");
                audioPlayer.transform.position = transform.position;
                AudioSource source = audioPlayer.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = mixerGroup;
                source.clip = screamClip;
                source.pitch = 1f;
                source.volume = 1f;
                source.spatialBlend = 0f;
                source.panStereo = 0f;
                source.Play();
                Object.Destroy(audioPlayer, screamClip.length);
            }
        }
    }
}
