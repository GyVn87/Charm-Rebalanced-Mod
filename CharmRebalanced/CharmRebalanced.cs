using TuyenTuyenTuyen.Charms;

namespace TuyenTuyenTuyen {
	public class CharmRebalanced : Mod, ITogglableMod {
		public static CharmRebalanced? LoadedInstance { get; set; }
		public override string GetVersion() => "2.0.2.0";

        public override void Initialize() {
			if (CharmRebalanced.LoadedInstance != null) return;
			CharmRebalanced.LoadedInstance = this;

			Charm03_GrubSong.Load();
		}

		public void Unload() {
			CharmRebalanced.LoadedInstance = null;
			if (HeroController.instance != null)
				RevertChanges();

			Charm03_GrubSong.Unload();
		}

		private void RevertChanges() {
			HeroController HC = HeroController.instance;
			HC.GRUB_SOUL_MP = 15;
			HC.GRUB_SOUL_MP_COMBO = 25;
			HC.RUN_SPEED = 8.3f;
			HC.RUN_SPEED_CH = 10.0f;
			HC.RUN_SPEED_CH_COMBO = 11.5f;
			HC.DASH_SPEED = 20.0f;
			HC.DASH_COOLDOWN_CH = 0.4f;
			HC.DASH_SPEED_SHARP = 28.0f;
			HC.SHADOW_DASH_COOLDOWN = 1.5f;
			HC.ATTACK_COOLDOWN_TIME = 0.41f;
			HC.ATTACK_COOLDOWN_TIME_CH = 0.25f;
			HC.ATTACK_DURATION = 0.35f;
			HC.ATTACK_DURATION_CH = 0.28f;
			HC.INVUL_TIME_STAL = 1.75f;
			HC.RECOIL_DURATION_STAL = 0.08f;
			HC.NAIL_CHARGE_TIME_CHARM = 0.75f;
        }
    }
}
