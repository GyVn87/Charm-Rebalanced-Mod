using UnityEngine;

namespace TuyenTuyenTuyen.Mechanics {
    internal static class Focus {
        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter += OnIntCompare_OnEnter;
            On.HealthManager.TakeDamage += OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent += OnFsmProcessEvent;
            ModHooks.AfterTakeDamageHook += OnAfterTakeDamage;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.IntCompare.OnEnter -= OnIntCompare_OnEnter;
            On.HealthManager.TakeDamage -= OnHealthManager_TakeDamage;
            On.HutongGames.PlayMaker.Fsm.ProcessEvent -= OnFsmProcessEvent;
            ModHooks.AfterTakeDamageHook -= OnAfterTakeDamage;
        }

        private static void OnIntCompare_OnEnter(On.HutongGames.PlayMaker.Actions.IntCompare.orig_OnEnter orig, HutongGames.PlayMaker.Actions.IntCompare self) {
            if (self.Fsm.Name == "Spell Control" && self.State.Name.StartsWith("Full HP?") && self.integer1.Name == "HP") {
                self.Finish();
                return;
            }
            orig(self);
        }

        private static void OnFsmProcessEvent(On.HutongGames.PlayMaker.Fsm.orig_ProcessEvent orig, HutongGames.PlayMaker.Fsm self, HutongGames.PlayMaker.FsmEvent fsmEvent, HutongGames.PlayMaker.FsmEventData eventData) {
            orig(self, fsmEvent, eventData);

            if (self.Name != "Spell Control" || fsmEvent.Name != "FOCUS COMPLETED")
                return;

            PlayerData PD = PlayerData.instance;
            if (!PD.GetBool("gotCharm_36") || PD.GetInt("royalCharmState") != 4)
                return;

            DarkOverflow overflow = HeroController.instance.gameObject.GetComponent<DarkOverflow>();
            if (overflow)
                overflow.ClearEffect();

            bool deepFocus = PD.GetBool("equippedCharm_34");
            int healedMasks = (deepFocus ? 2 : 1);
            int currentHP = PD.GetInt("health");
            int maxHP = PD.CurrentMaxHealth;
            bool isFullHealth = (currentHP + healedMasks > maxHP);

            overflow = HeroController.instance.gameObject.AddComponent<DarkOverflow>();
            overflow.SetUpDamageMultiplier(isFullHealth, deepFocus);
        }

        private static void OnHealthManager_TakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance) {
            PlayerData PD = PlayerData.instance;
            if (!PD.GetBool("gotCharm_36") || PD.GetInt("royalCharmState") != 4) {
                orig(self, hitInstance);
                return;
            }

            switch (hitInstance.AttackType) {
                case AttackTypes.Nail:
                case AttackTypes.Spell:
                case AttackTypes.SharpShadow:
                case AttackTypes.NailBeam:
                    break;
                default:
                    orig(self, hitInstance);
                    return;
            }

            string ownerName = hitInstance.Source.name;
            bool nailArt = (ownerName == "Dash Slash" || ownerName == "Great Slash");

            DarkOverflow overflow = HeroController.instance.gameObject.GetComponent<DarkOverflow>();
            if (overflow) 
                hitInstance.Multiplier *= overflow.GetMultiplier(nailArt);

            orig(self, hitInstance);
        }

        private static int OnAfterTakeDamage(int hazardType, int damageAmount) {
            if (damageAmount > 0) {
                DarkOverflow overflow = HeroController.instance.gameObject.GetComponent<DarkOverflow>();
                if (overflow)
                    overflow.ClearEffect();
            }
            return damageAmount;
        }
    }

    public class DarkOverflow : MonoBehaviour {
        private static readonly float duration = 4f;
        private static readonly float normalIncrease = 0.5f;
        private static readonly float fullHealthIncrease = 0.7f;
        private static readonly float deepFocusMultiplier = 1.25f;
        private static readonly float nailArtPenalty = 1f;

        private float damageMultiplier = 1f;

        private float mTimer = 0f;
        private static readonly float mFadeOutDuration = 0.5f;
        private float mFadeOutTimer = 0f;
        private static GameObject? mPrefab = null;
        private GameObject? mEffect = null;
        private tk2dSprite? tk2dComponent = null;

        public void SetUpDamageMultiplier(bool fullHealth, bool deepFocus) {
            float multiplier;
            if (fullHealth)
                multiplier = fullHealthIncrease;
            else
                multiplier = normalIncrease;

            if (deepFocus)
                multiplier *= deepFocusMultiplier;

            damageMultiplier = 1f + multiplier;
        }

        public float GetMultiplier(bool nailArt) {
            if (nailArt)
                return 1f + (damageMultiplier - 1f) * nailArtPenalty;
            return damageMultiplier;
        }

        private static void GetPrefab() {
            Transform KnighTransform = HeroController.instance.transform;
            mPrefab = KnighTransform.Find("Spells/Q Trail 2").gameObject;
        }

        public void ClearEffect() {
            Object.Destroy(mEffect);
            Object.Destroy(this);
        }

        private void Awake() {
            mFadeOutTimer = mFadeOutDuration;
            mTimer = duration;

            if (mPrefab == null)
                GetPrefab();

            Transform KnightTransform = HeroController.instance.transform;
            mEffect = Object.Instantiate(mPrefab, KnightTransform);
            mEffect?.SetActive(true);
            mEffect!.name = "Dark Overflow Effect";
            mEffect!.transform.localPosition = new Vector3(0.0933f, 1.4524f, -0.001f);

            tk2dComponent = mEffect.GetComponent<tk2dSprite>();
            tk2dComponent.color = new Color(0.4385f, 0.2906f, 1f, 0.5f);
        }

        private void Update() {
            mTimer -= Time.deltaTime;
            if (mTimer < mFadeOutDuration) {
                mFadeOutTimer -= Time.deltaTime;
                Color color = tk2dComponent!.color;
                tk2dComponent.color = new Color(color.r, color.g, color.b, mFadeOutTimer / mFadeOutDuration * 0.5f);
            }
            if (mTimer < 0f)
                ClearEffect();
        }
    }
}
