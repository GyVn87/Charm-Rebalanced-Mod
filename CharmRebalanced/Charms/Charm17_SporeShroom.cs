namespace TuyenTuyenTuyen.Charms {
    internal static class Charm17_SporeShroom {
        private static readonly float sporeDamageMultiplier = 3f; // to Nail damage

        private static readonly float sporeDuration = 4.1f; // don't change this

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.GetOwner.OnEnter += OnGetOwner_OnEnter;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.GetOwner.OnEnter -= OnGetOwner_OnEnter;
        }

        private static void OnGetOwner_OnEnter(On.HutongGames.PlayMaker.Actions.GetOwner.orig_OnEnter orig, HutongGames.PlayMaker.Actions.GetOwner self) {
            orig(self);
            if (self.Owner.name.StartsWith("Knight Spore Cloud") && self.Fsm.Name == "Control" && self.State.Name == "Init")
                KnightSporeCloud(self);
            else if (self.Owner.name.StartsWith("Knight Dung Cloud") && self.Fsm.Name == "Control" && self.State.Name == "Init")
                KnightSporeDungCloud(self);
        }

        private static void KnightSporeCloud(HutongGames.PlayMaker.Actions.GetOwner self) {
            DamageEffectTicker damageEffect = self.Owner.gameObject.GetComponent<DamageEffectTicker>();
            if (damageEffect == null) return;
            PlayerData PD = PlayerData.instance;
            int nailDamage = PD.GetInt("nailDamage");
            float shamanStone = (PD.GetBool("equippedCharm_19") ? Charm19_ShamanStone.spellDamageIncrease : 1f);
            int extraDamage = ExtraDamageable.GetDamageOfType(damageEffect.extraDamageType);
            float interval = 1f / (nailDamage * sporeDamageMultiplier * shamanStone / sporeDuration / (float)extraDamage);
            damageEffect.SetDamageInterval(interval);
        }

        private static void KnightSporeDungCloud(HutongGames.PlayMaker.Actions.GetOwner self) {
            DamageEffectTicker damageEffect = self.Owner.gameObject.GetComponent<DamageEffectTicker>();
            if (damageEffect == null) return;
            PlayerData PD = PlayerData.instance;
            int nailDamage = PD.GetInt("nailDamage");
            float shamanStone = (PD.GetBool("equippedCharm_19") ? Charm19_ShamanStone.spellDamageIncrease : 1f);
            int extraDamage = ExtraDamageable.GetDamageOfType(damageEffect.extraDamageType);
            float interval = 1f / (nailDamage * sporeDamageMultiplier * shamanStone / sporeDuration / (float)extraDamage);
            damageEffect.SetDamageInterval(interval);
        }
    }
}
