using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm10_DefenderCrest {
        private static readonly float weaknessDebuffMultiplier = 1.1f;
        internal static readonly float weaknessDuration = 2f;

        private static readonly int flukeDungLv1Damage = 50;
        private static readonly int flukeDungLv2Damage = 100;
        private static readonly float shamanIncrease = Charm19_ShamanStone.spellDamageIncrease;

        private static readonly float flukeDungDuration = 2.2f; // don't change this

        private static readonly Vector3 knightDungTrailScale = new(1f, 1f, 1f);

        internal static void Load() {
            On.DamageEffectTicker.OnTriggerEnter2D += OnDamageEffectTicker_OnTriggerEnter2D;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.GetOwner.OnEnter += OnGetOwner_OnEnter;
            On.HutongGames.PlayMaker.Actions.CallMethodProper.OnEnter += OnCallMethodProper;
            On.GetHP.OnEnter += OnGetHP_OnEnter;
        }

        internal static void Unload() {
            On.DamageEffectTicker.OnTriggerEnter2D -= OnDamageEffectTicker_OnTriggerEnter2D;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Actions.GetOwner.OnEnter -= OnGetOwner_OnEnter;
            On.HutongGames.PlayMaker.Actions.CallMethodProper.OnEnter -= OnCallMethodProper;
            On.GetHP.OnEnter -= OnGetHP_OnEnter;
        }

        private static void OnDamageEffectTicker_OnTriggerEnter2D(On.DamageEffectTicker.orig_OnTriggerEnter2D orig, DamageEffectTicker self, Collider2D otherCollider) {
            orig(self, otherCollider);
            if (self.extraDamageType == ExtraDamageTypes.Spore)
                return;

            WeaknessDebuff weaknessDebuff = otherCollider.gameObject.GetComponent<WeaknessDebuff>();
            if (weaknessDebuff == null)
                otherCollider.gameObject.AddComponent<WeaknessDebuff>();
            else
                weaknessDebuff.RefreshTimer();
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            WeaknessDebuff debuff = self.gameObject.GetComponent<WeaknessDebuff>();
            if (debuff)
                hitInstance.Multiplier *= weaknessDebuffMultiplier;
            orig(self, hitInstance);
        }

        // Minions' damage: Weaverling, Grimmchild
        private static void OnGetHP_OnEnter(On.GetHP.orig_OnEnter orig, GetHP self) {
            orig(self);
            if (self.Owner.name == "Enemy Damager") {
                WeaknessDebuff debuff = self.target?.GameObject?.Value?.GetComponent<WeaknessDebuff>();
                if (debuff)
                    self.storeValue.Value -= 1;
            }
        }

        private static void OnGetOwner_OnEnter(On.HutongGames.PlayMaker.Actions.GetOwner.orig_OnEnter orig, HutongGames.PlayMaker.Actions.GetOwner self) {
            orig(self);
            if (self.Owner.name.StartsWith("Knight Dung Trail") && self.Fsm.Name == "Control" && self.State.Name == "Init") 
                self.Owner.gameObject.transform.localScale = knightDungTrailScale;
        }

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
    }

    public class WeaknessDebuff : TuyenTuyenTuyen.Mechanics.CustomEffect {
        public override float Duration => Charm10_DefenderCrest.weaknessDuration;
        public override Color StartColor => new(0.08f, 0.02f, 0.1f, 0.8f);
        public override Vector3 LocalScale => new(1.5f, 1.5f, 1.5f);
        public override string Name => "Weakness Debuff Particle";
    }
}
