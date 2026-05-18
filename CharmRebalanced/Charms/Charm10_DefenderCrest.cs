using GlobalEnums;
using TuyenTuyenTuyen.Mechanics;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm10_DefenderCrest {
        private static readonly float weaknessDebuffMultiplier = 1.1f;
        private static readonly float weaknessDebuffSpellMultiplier = 1.15f;
        internal static readonly float weaknessDuration = 2f;

        private static readonly int flukeDungLv1Damage = 50;
        private static readonly int flukeDungLv2Damage = 100;
        private static readonly float shamanIncrease = Charm19_ShamanStone.spellDamageIncrease;

        //private static readonly float knightDungTrailDuration = 2f;
        //private static readonly float flukeDungLV1Duration = 3f;
        //private static readonly float flukeDungLV2Duration = 4f;
        private static readonly float flukeDungDuration = 2.2f; // don't change this

        private static readonly Vector3 knightDungTrailScale = new(1f, 1f, 1f);

        internal static void Load() {
            On.DamageEffectTicker.OnTriggerEnter2D += OnDamageEffectTicker_OnTriggerEnter2D;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.GetOwner.OnEnter += OnGetOwner_OnEnter;
            //On.HutongGames.PlayMaker.Actions.Wait.OnEnter += OnWait_OnEnter;
            On.HutongGames.PlayMaker.Actions.CallMethodProper.OnEnter += OnCallMethodProper;
        }

        internal static void Unload() {
            On.DamageEffectTicker.OnTriggerEnter2D -= OnDamageEffectTicker_OnTriggerEnter2D;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.GetOwner.OnEnter -= OnGetOwner_OnEnter;
            //On.HutongGames.PlayMaker.Actions.Wait.OnEnter -= OnWait_OnEnter;
            On.HutongGames.PlayMaker.Actions.CallMethodProper.OnEnter -= OnCallMethodProper;
        }

        private static void OnDamageEffectTicker_OnTriggerEnter2D(On.DamageEffectTicker.orig_OnTriggerEnter2D orig, DamageEffectTicker self, Collider2D otherCollider) {
            orig(self, otherCollider);
            WeaknessDebuff weaknessDebuff = otherCollider.gameObject.GetComponent<WeaknessDebuff>();
            if (weaknessDebuff == null)
                otherCollider.gameObject.AddComponent<WeaknessDebuff>();
            else
                weaknessDebuff.RefreshTimer();
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            WeaknessDebuff debuff = self.gameObject.GetComponent<WeaknessDebuff>();
            if (debuff != null) {
                if (hitInstance.AttackType == AttackTypes.Spell)
                    hitInstance.Multiplier *= weaknessDebuffSpellMultiplier;
                else
                    hitInstance.Multiplier *= weaknessDebuffMultiplier;
            }
            orig(self, hitInstance);
        }

        private static void OnGetOwner_OnEnter(On.HutongGames.PlayMaker.Actions.GetOwner.orig_OnEnter orig, HutongGames.PlayMaker.Actions.GetOwner self) {
            orig(self);
            if (self.Owner.name.StartsWith("Knight Dung Trail") && self.Fsm.Name == "Control" && self.State.Name == "Init") 
                self.Owner.gameObject.transform.localScale = knightDungTrailScale;
        }

        //private static void OnWait_OnEnter(On.HutongGames.PlayMaker.Actions.Wait.orig_OnEnter orig, HutongGames.PlayMaker.Actions.Wait self) {
        //    if (self.Owner.name.StartsWith("Knight Dung Trail") && self.Fsm.Name == "Control" && self.State.Name == "Wait")
        //        self.time.Value = knightDungTrailDuration;
        //    else if (self.Owner.name == "Knight Dung Cloud" && self.Fsm.Name == "Control" && self.State.Name == "Collider On")
        //        SpellFlukeCloudDuration(self);
        //    else if (self.Owner.name.StartsWith("Spell Fluke Dung Lv") && self.Fsm.Name == "Control" && self.State.Name == "Blow")
        //        GiantFlukeDuration(self);
        //    orig(self);
        //}

        private static void OnCallMethodProper(On.HutongGames.PlayMaker.Actions.CallMethodProper.orig_OnEnter orig, HutongGames.PlayMaker.Actions.CallMethodProper self) {
            orig(self);
            string ownerName = self.Owner.name;
            if (ownerName.StartsWith("Spell Fluke Dung Lv1"))
                SpellFlukeDungLV1(self);
            else if (ownerName.StartsWith("Spell Fluke Dung Lv2"))
                SpellFlukeDungLV2(self);
        }

        private static void SpellFlukeDungLV1(HutongGames.PlayMaker.Actions.CallMethodProper self) {
            DamageEffectTicker damageEffect = self.gameObject.GameObject.Value.GetComponent<DamageEffectTicker>();
            if (damageEffect == null) return;
            damageEffect.extraDamageType = ExtraDamageTypes.Spore;
            int extraDamage = ExtraDamageable.GetDamageOfType(damageEffect.extraDamageType);
            if (self.State.Name == "Normal") 
                damageEffect.SetDamageInterval(1f / ((float)flukeDungLv1Damage / flukeDungDuration / (float)extraDamage));
            else if (self.State.Name == "Spell Up")
                damageEffect.SetDamageInterval(1f / ((float)flukeDungLv1Damage * shamanIncrease / flukeDungDuration / (float)extraDamage));
        }

        private static void SpellFlukeDungLV2(HutongGames.PlayMaker.Actions.CallMethodProper self) {
            DamageEffectTicker damageEffect = self.gameObject.GameObject.Value.GetComponent<DamageEffectTicker>();
            if (damageEffect == null) return;
            damageEffect.extraDamageType = ExtraDamageTypes.Dung2;
            int extraDamage = ExtraDamageable.GetDamageOfType(damageEffect.extraDamageType);
            if (self.State.Name == "Normal")
                damageEffect.SetDamageInterval(1f / ((float)flukeDungLv2Damage / flukeDungDuration / (float)extraDamage));
            else if (self.State.Name == "Spell Up")
                damageEffect.SetDamageInterval(1f / ((float)flukeDungLv2Damage * shamanIncrease / flukeDungDuration / (float)extraDamage));
        }

        //private static void SpellFlukeCloudDuration(HutongGames.PlayMaker.Actions.Wait self) {
        //    string parentName = self.Owner.transform?.parent?.name;
        //    if (parentName.StartsWith("Spell Fluke Dung Lv1"))
        //        self.time.Value = flukeDungLV1Duration;
        //    else if (parentName.StartsWith("Spell Fluke Dung Lv2"))
        //        self.time.Value = flukeDungLV2Duration;
        //}

        //private static void GiantFlukeDuration(HutongGames.PlayMaker.Actions.Wait self) {
        //    string ownerName = self.Owner.name;
        //    if (ownerName.StartsWith("Spell Fluke Dung Lv1"))
        //        self.time.Value = flukeDungLV1Duration;
        //    else if (ownerName.StartsWith("Spell Fluke Dung Lv2"))
        //        self.time.Value = flukeDungLV2Duration;
        //}
    }

    public class WeaknessDebuff : MonoBehaviour {
        private float timer = 0f;
        private readonly float duration = Charm10_DefenderCrest.weaknessDuration;
        private GameObject debuffParticle = null;
        private static GameObject particlePrefab = null;

        public void Awake() {
            timer = duration;
            if (particlePrefab == null) {
                Transform KnighTransform = HeroController.instance.transform;
                particlePrefab = KnighTransform.Find("Charm Effects/Dung/Particle 1").gameObject;
            }
            debuffParticle = Object.Instantiate<GameObject>(particlePrefab, transform);
            debuffParticle.SetActive(true);
            debuffParticle.name = "Weakness Debuff Particle";
            ParticleSystem ps = debuffParticle.GetComponent<ParticleSystem>();
            var emission = ps.emission;
            emission.enabled = true;
            var main = ps.main;
            main.startColor = new Color(0.08f, 0.02f, 0.1f, 0.8f);
            debuffParticle.transform.localScale = new Vector3(2f, 2f, 2f);
            ps.Clear();
            ps.Play();
        }

        public void RefreshTimer() {
            timer = duration;   
        }

        private void Update() {
            timer -= Time.deltaTime;
            if (timer < 0f) {
                Object.Destroy(debuffParticle);
                Object.Destroy(this);
            }
        }
    }
}
