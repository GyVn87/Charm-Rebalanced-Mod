using SFCore;
using System.Collections.Generic;
using System.Reflection;
using TuyenTuyenTuyen.Charms;
using TuyenTuyenTuyen.CustomCharms;
using TuyenTuyenTuyen.Mechanics;
using UnityEngine;

namespace TuyenTuyenTuyen {
	public class CharmSettings {
		public Dictionary<string, EasyCharmState> Settings;
	}

	public class CharmRebalanced : Mod, ITogglableMod, ILocalSettings<CharmSettings> {
		public static CharmRebalanced LoadedInstance { get; set; }
		/// <summary>
		/// Using expression bodied property ensures that everytime we use pd,
		/// it will recalculate the value which helps us get the most up-to-date data.
		/// </summary>
		public PlayerData PD => PlayerData.instance;
        public GameObject Knight => HeroController.instance.gameObject;
		public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();
		public CharmSettings LocalCharmSettings = new CharmSettings();
		public Dictionary<string, EasyCharm> CustomCharms = new Dictionary<string, EasyCharm> {
			{"Kingsoul", new KingsoulClone()}
		};

        public void OnLoadLocal(CharmSettings s) {
			LocalCharmSettings = s;
			if (LocalCharmSettings.Settings != null) {
				foreach (var kvp in LocalCharmSettings.Settings) {
					if (CustomCharms.TryGetValue(kvp.Key, out EasyCharm m))
						m.RestoreCharmState(kvp.Value);
				}
			}
        }

        public CharmSettings OnSaveLocal() {
			LocalCharmSettings.Settings = new Dictionary<string, EasyCharmState>();
			foreach (var kvp in CustomCharms) 
				LocalCharmSettings.Settings[kvp.Key] = kvp.Value.GetCharmState();
			return LocalCharmSettings;
        }

        public override void Initialize() {
			if (CharmRebalanced.LoadedInstance != null) return;
			CharmRebalanced.LoadedInstance = this;

			KingsoulClone.Load();

			Charm31_Dashmaster.Load();  // has to be called before Sharp Shadow'
			Charm03_GrubSong.Load();
			Charm04_StalwartShell.Load();
			Charm05_BaldurShell.Load();
			Charm06_FuryOfTheFallen.Load();
			Charm09_LifebloodCore.Load();
			Charm10_DefenderCrest.Load();
			Charm11_Flukenest.Load();
			Charm12_ThornsOfAgony.Load();
			Charm13_18_MarkOfPride_Longnail.Load();
			Charm15_HeavyBlow.Load();
			Charm16_SharpShadow.Load();
			Charm17_SporeShroom.Load();
			Charm19_ShamanStone.Load();
			Charm20_SoulCatcher.Load();
			Charm21_SoulEater.Load();
			Charm22_GlowingWomb.Load();
			Charm23_Heart.Load();  // has to be called before Joni's Blessing
			Charm24_Greed.Load();
			Charm25_Strength.Load();
			Charm27_JoniBlessing.Load();
			Charm29_Hiveblood.Load();  
			Charm30_DreamWielder.Load();
			Charm32_QuickSlash.Load();
			Charm34_DeepFocus.Load();
			Charm35_GrubberflyElegy.Load();
			Charm36_Kingsoul.Load();
			Charm37_Sprintmaster.Load();
			Charm38_Dreamshield.Load();
			Charm39_Weaversong.Load(); 
			Charm40_Grimmchild.Load();
			Charm40_CarefreeMelody.Load();
			NewCharmCosts.Load();
			Focus.Load();
			ExtraDamage.Load();
		}

		public void Unload() {
			CharmRebalanced.LoadedInstance = null;
			if (HeroController.instance != null)
				RevertChanges();

            KingsoulClone.Unload();

            Charm31_Dashmaster.Unload();
			Charm03_GrubSong.Unload();
			Charm04_StalwartShell.Unload();
			Charm05_BaldurShell.Unload();
			Charm06_FuryOfTheFallen.Unload();
			Charm09_LifebloodCore.Unload();
			Charm10_DefenderCrest.Unload();
			Charm11_Flukenest.Unload();
			Charm12_ThornsOfAgony.Unload();
			Charm13_18_MarkOfPride_Longnail.Unload();
			Charm15_HeavyBlow.Unload();
			Charm16_SharpShadow.Unload();
			Charm17_SporeShroom.Unload();
			Charm19_ShamanStone.Unload();
			Charm20_SoulCatcher.Unload();
			Charm21_SoulEater.Unload();
			Charm22_GlowingWomb.Unload();
			Charm23_Heart.Unload();
			Charm24_Greed.Unload();
			Charm25_Strength.Unload();
			Charm27_JoniBlessing.Unload();
			Charm29_Hiveblood.Unload();
			Charm30_DreamWielder.Unload();
			Charm32_QuickSlash.Unload();
			Charm34_DeepFocus.Unload();
			Charm35_GrubberflyElegy.Unload();
			Charm36_Kingsoul.Unload();
			Charm37_Sprintmaster.Unload();
			Charm38_Dreamshield.Unload();
			Charm39_Weaversong.Unload();
			Charm40_Grimmchild.Unload();
			Charm40_CarefreeMelody.Unload();
			NewCharmCosts.Unload();
			Focus.Unload();
			ExtraDamage.Unload();
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
        }
    }
}
