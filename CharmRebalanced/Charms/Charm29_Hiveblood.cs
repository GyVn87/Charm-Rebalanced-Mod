namespace TuyenTuyenTuyen.Charms {
    internal static class Charm29_Hiveblood {
        private static readonly float newRecoverTime = 6f;
        private static PlayMakerFSM FSM;

        internal static void Load() {
            ModHooks.HeroUpdateHook += Charm29_Hiveblood.OnHeroUpdate;
            On.HutongGames.PlayMaker.Actions.FindChild.OnEnter += OnFindChild_OnEnter;
        }

        internal static void Unload() {
            ModHooks.HeroUpdateHook -= Charm29_Hiveblood.OnHeroUpdate;
            On.HutongGames.PlayMaker.Actions.FindChild.OnEnter -= OnFindChild_OnEnter;
        }

        private static void OnHeroUpdate() {
            if (FSM == null)
                FSM = GameCameras.instance.transform.Find("HudCamera/Hud Canvas/Health").gameObject.LocateMyFSM("Hive Health Regen");
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            if ((FSM.ActiveStateName == "Idle") && (PD.GetInt("health") < PD.CurrentMaxHealth))
                FSM.SendEvent("DAMAGE TAKEN");
        }

        private static void OnFindChild_OnEnter(On.HutongGames.PlayMaker.Actions.FindChild.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FindChild self) {
            orig(self);
            if (self.Owner.name == "Health" && self.Fsm.Name == "Hive Health Regen" && self.State.Name == "Init") {
                self.Fsm.Variables.GetFsmFloat("Recover Time").Value = newRecoverTime;
            } 
        }
    }
}
