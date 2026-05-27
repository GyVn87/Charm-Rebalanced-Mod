using System.Reflection;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm11_Flukenest {
        private static readonly int flukeSpawnLevel1 = 9;
        private static readonly int flukeSpawnLevel1Shaman = 10;
        private static readonly int flukeSpawnLevel2 = 16;
        private static readonly int flukeSpawnLevel2Shaman = 15;

        private static readonly int flukeDamageLevel1 = 4;
        private static readonly int flukeDamageLevel1Shaman = 5;
        private static readonly int flukeDamageLevel2 = 4;
        private static readonly int flukeDamageLevel2Shaman = 6;

        private static readonly FieldInfo hasBursted = typeof(SpellFluke).GetField("hasBursted", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo flukeDamage = typeof(SpellFluke).GetField("damage", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static void Load() {
            On.SpellFluke.DoDamage += Charm11_Flukenest.ONSFDoDamage;
            On.HutongGames.PlayMaker.Actions.FlingObjectsFromGlobalPool.OnEnter += Charm11_Flukenest.OnFlingObjectsFromGlobalPool_OnEnter;
            On.SpellFluke.OnEnable += Charm11_Flukenest.ONSFOnEnable;
        }

        internal static void Unload() {
            On.SpellFluke.DoDamage -= Charm11_Flukenest.ONSFDoDamage;
            On.HutongGames.PlayMaker.Actions.FlingObjectsFromGlobalPool.OnEnter -= Charm11_Flukenest.OnFlingObjectsFromGlobalPool_OnEnter;
            On.SpellFluke.OnEnable -= Charm11_Flukenest.ONSFOnEnable;
        }

        // Somehow this solves the "Flukenest multihits" bug
        private static void ONSFDoDamage(On.SpellFluke.orig_DoDamage orig, SpellFluke self, GameObject obj, int upwardRecursionAmount, bool burst) {
            bool alreadyBursted = (bool)hasBursted.GetValue(self);
            if (alreadyBursted)
                return;
            orig(self, obj, upwardRecursionAmount, burst);
            if (burst)
                hasBursted.SetValue(self, true); 
        }

        private static void OnFlingObjectsFromGlobalPool_OnEnter(On.HutongGames.PlayMaker.Actions.FlingObjectsFromGlobalPool.orig_OnEnter orig, HutongGames.PlayMaker.Actions.FlingObjectsFromGlobalPool self) {
            if (self.Fsm.Name != "Fireball Cast" || self.State.Name != "Flukes") {
                orig(self);
                return;
            }

            int defaultSpawn = self.spawnMin.Value;
            bool hasShamanStone = CharmRebalanced.LoadedInstance.PD.GetBool("equippedCharm_19");
            int currentSpawn = defaultSpawn;
            string ownerName = self.Owner.name;
            if (ownerName.StartsWith("Fireball Top")) {
                if (hasShamanStone)
                    currentSpawn = flukeSpawnLevel1Shaman;
                else
                    currentSpawn = flukeSpawnLevel1;
            }
            else if (ownerName.StartsWith("Fireball2 Top")) {
                if (hasShamanStone)
                    currentSpawn = flukeSpawnLevel2Shaman;
                else
                    currentSpawn = flukeSpawnLevel2;
            }
            self.spawnMin.Value = currentSpawn;
            self.spawnMax.Value = currentSpawn;

            orig(self);

            self.spawnMin.Value = defaultSpawn;
            self.spawnMax.Value = defaultSpawn;
        }

        private static void ONSFOnEnable(On.SpellFluke.orig_OnEnable orig, SpellFluke self) {
            PlayerData PD = CharmRebalanced.LoadedInstance.PD;
            bool hasShamanStone = PD.GetBool("equippedCharm_19");
            int fireballLevel = PD.GetInt("fireballLevel");
            int newDamage = (int)flukeDamage.GetValue(self);

            orig(self);

            switch (fireballLevel) {
                case 1:
                    if (hasShamanStone)
                        newDamage = flukeDamageLevel1Shaman;
                    else
                        newDamage = flukeDamageLevel1;
                    break;
                case 2:
                    if (hasShamanStone)
                        newDamage = flukeDamageLevel2Shaman;
                    else
                        newDamage = flukeDamageLevel2;
                    break;
            }
            flukeDamage.SetValue(self, newDamage);
        }
    }
}

