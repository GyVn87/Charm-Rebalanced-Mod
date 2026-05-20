using TuyenTuyenTuyen.Charms;
using UnityEngine;

namespace TuyenTuyenTuyen.Mechanics {
    public abstract class CustomEffect : MonoBehaviour {
        private float mTimer = 0f;
        private GameObject mEffectParticle = null;
        private GameObject mParticlePrefab = null;
        private ParticleSystem mParticleSystem = null;

        public abstract float Duration { get; }
        
        public abstract Color StartColor { get; }
        public abstract Vector3 LocalScale { get; }
        public abstract string Name { get; }
        public float EmissionRate = 80f;
        public float StartSize = 1f;

        private void Awake() {
            mTimer = Duration;

            if (mParticlePrefab == null)
                SetPrefab();

            mEffectParticle = Object.Instantiate<GameObject>(mParticlePrefab, transform);
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

        protected GameObject GetPrefab() => mParticlePrefab;


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
            mEffectParticle.transform.SetParent(null);
            mParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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
