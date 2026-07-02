using SFCore;
using UnityEngine;

namespace TuyenTuyenTuyen.CustomCharms {
    internal class GrimmTroupe : EasyCharm {
        internal static GrimmTroupe? Instance { get; private set; }
        protected override int GetCharmCost() => (PlayerData.instance.GetInt("grimmChildLevel") == 5 ? 2 : 3);
        protected override string GetDescription() => (PlayerData.instance.GetInt("grimmChildLevel") == 5 ? grimmchildDescription : carefreeMelodyDescription);
        protected override string GetName() => (PlayerData.instance.GetInt("grimmChildLevel") == 5 ? "Grimmchild" : "Carefree Melody");
        protected override Sprite GetSpriteInternal() => BlankSprite();
        private readonly string carefreeMelodyDescription = "Token commemorating the start of a friendship.<br><br>Contains a song of protection that absorbs the first incoming blow, delaying the damage until the song fades.";
        private readonly string grimmchildDescription = "Worn by those who take part in the Grimm Troupe's Ritual.";

        public GrimmTroupe() {
            Instance = this;
        }

        private static Sprite BlankSprite() {
            Texture2D texture = new(1, 1);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        }

        internal static void Load() {
            On.CharmIconList.GetSprite += OnCILGetSprite;
            ModHooks.CharmUpdateHook += OnCharmUpdate;
            On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.OnEnter += OnPlayerDataBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.GetPlayerDataInt.OnEnter += OnGetPlayerDataInt_OnEnter;
        }

        internal static void Unload() {
            On.CharmIconList.GetSprite -= OnCILGetSprite;
            ModHooks.CharmUpdateHook -= OnCharmUpdate;
            On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.OnEnter -= OnPlayerDataBoolTest_OnEnter;
            On.HutongGames.PlayMaker.Actions.GetPlayerDataInt.OnEnter -= OnGetPlayerDataInt_OnEnter;
        }

        private static Sprite OnCILGetSprite(On.CharmIconList.orig_GetSprite orig, CharmIconList self, int id) {
            if (id == Instance?.Id) {
                if (PlayerData.instance.GetInt("grimmChildLevel") == 5)
                    return CharmIconList.Instance.grimmchildLevel4;
                else
                    return CharmIconList.Instance.nymmCharm;
            }
            return orig(self, id);
        }

        private static void OnCharmUpdate(PlayerData data, HeroController controller) {
            if (data.GetInt("grimmChildLevel") >= 4)
                Instance!.GotCharm = true;
            else
                Instance!.GotCharm = false;

            if (Instance.IsEquipped && data.GetInt("grimmChildLevel") == 4)
                HeroController.instance.carefreeShieldEquipped = true;
        }

        private static void OnPlayerDataBoolTest_OnEnter(On.HutongGames.PlayMaker.Actions.PlayerDataBoolTest.orig_OnEnter orig, HutongGames.PlayMaker.Actions.PlayerDataBoolTest self) {
            if (self.Fsm.Name == "Spawn Grimmchild" && self.State.Name == "Check") {
                PlayerData playerData = PlayerData.instance;
                if (self.boolName.Value == "destroyedNightmareLantern") {
                    if (Instance!.IsEquipped && playerData.GetInt("grimmChildLevel") == 5)
                        self.Fsm.Event("EQUIPPED");
                    else
                        self.Finish();
                }
                else if (self.boolName.Value == "equippedCharm_40") {
                    if (playerData.GetBool("equippedCharm_40") && playerData.GetInt("grimmChildLevel") != 5)
                        self.Fsm.Event("EQUIPPED");
                    else
                        self.Fsm.Event("UNEQUIPPED");
                }
                return;
            }
            orig(self);
        }

        private static void OnGetPlayerDataInt_OnEnter(On.HutongGames.PlayMaker.Actions.GetPlayerDataInt.orig_OnEnter orig, HutongGames.PlayMaker.Actions.GetPlayerDataInt self) {
            orig(self);
            if (self.Owner != null && self.Owner.name.StartsWith("Grimmchild") && self.Fsm.Name == "Control" && self.State.Name == "Init") {
                if (PlayerData.instance.GetInt("grimmChildLevel") == 5)
                    self.storeValue.Value = 4;
            }
        }
    }
}
