using UnityEngine;

namespace TuyenTuyenTuyen.Mechanics {
    public abstract class CustomEffect : MonoBehaviour {
        private float mTimer = 0f;
        private GameObject? mEffectParticle = null;
        private GameObject? mParticlePrefab = null;
        private ParticleSystem? mParticleSystem = null;

        protected abstract float Duration { get; }
        protected abstract Color StartColor { get; }
        protected abstract Vector3 LocalScale { get; }
        protected abstract string Name { get; }
        private float EmissionRate = 80f;
        private float StartSize = 1f;

        private void Awake() {
            mTimer = Duration;

            if (mParticlePrefab == null)
                SetPrefab();

            if (mParticlePrefab) {
                mEffectParticle = Object.Instantiate<GameObject>(mParticlePrefab!, transform);
                mEffectParticle.SetActive(true);
                mEffectParticle.name = Name;
                mEffectParticle.transform.localScale = LocalScale;

                mParticleSystem = mEffectParticle.GetComponent<ParticleSystem>();
                var emission = mParticleSystem.emission;
                emission.rateOverTimeMultiplier = EmissionRate;
                emission.enabled = true;

                var main = mParticleSystem.main;
                main.startColor = StartColor;
                main.startSize = StartSize;
                main.stopAction = ParticleSystemStopAction.Destroy;

                mParticleSystem.Clear();
                mParticleSystem.Play();
            }
            else {
                CharmRebalanced.LoadedInstance?.LogWarn("CustomEffect.mParticlePrefab is NULL");
            }
        }

        protected GameObject? GetPrefab() => mParticlePrefab;


        protected void SetEmissionRate(float rate) {
            EmissionRate = rate;
        }

        protected void SetStartSize(float size) {
            StartSize = size;
        }

        public void SetTimer(float time) {
            mTimer = time;
        }

        private void SetPrefab() {
            Transform KnighTransform = HeroController.instance.transform;
            mParticlePrefab = KnighTransform.Find("Charm Effects/Dung/Particle 1").gameObject;
        }

        public void RefreshTimer() {
            mTimer = Duration;
        }

        private void OnTimeOut() {
            ClearEffect();
        }

        public void ClearEffect() {
            OnClearEffect();
            mEffectParticle?.transform?.SetParent(null);
            mParticleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Object.Destroy(this);
        }

        protected void DestroyParticle() {
            Object.Destroy(mEffectParticle);
        }

        protected virtual void OnUpdate() {}

        protected virtual void OnClearEffect() {}

        private void Update() {
            OnUpdate();
            mTimer -= Time.deltaTime;
            if (mTimer < 0f) {
                OnTimeOut();
            }
        }
    }
}
