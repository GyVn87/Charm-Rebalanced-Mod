using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using UnityEngine;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm11_Flukenest {
        private static readonly int flukeSpawnLevel1 = 9;
        private static readonly int flukeSpawnLevel1Shaman = 9;
        private static readonly int flukeSpawnLevel2 = 16;
        private static readonly int flukeSpawnLevel2Shaman = 16;

        private static readonly int flukeDamageLevel1 = 4;
        private static readonly int flukeDamageLevel1Shaman = 5;
        private static readonly int flukeDamageLevel2 = 4;
        private static readonly int flukeDamageLevel2Shaman = 5;

        private static readonly FieldInfo hasBursted = typeof(SpellFluke).GetField("hasBursted", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo flukeDamage = typeof(SpellFluke).GetField("damage", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo burstMethod = typeof(SpellFluke).GetMethod("Burst", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static void Load() {
            On.HutongGames.PlayMaker.Actions.FlingObjectsFromGlobalPool.OnEnter += OnFlingObjectsFromGlobalPool_OnEnter;
            On.SpellFluke.OnEnable += ONSFOnEnable;
            On.SpellFluke.DoDamage += ONSFDoDamage;
            IL.SpellFluke.DoDamage += HookAfterApplyingDamage;
            IL.SpellFluke.DoDamage += ForceBurstIfImmune;
        }

        internal static void Unload() {
            On.HutongGames.PlayMaker.Actions.FlingObjectsFromGlobalPool.OnEnter -= OnFlingObjectsFromGlobalPool_OnEnter;
            On.SpellFluke.OnEnable -= ONSFOnEnable;
            On.SpellFluke.DoDamage -= ONSFDoDamage;
            IL.SpellFluke.DoDamage -= HookAfterApplyingDamage;
            IL.SpellFluke.DoDamage -= ForceBurstIfImmune;
        }

        private static void ForceBurstIfImmune(ILContext il)
        {
            ILCursor cursor = new ILCursor(il).Goto(0);

            /*
                // if (component.IsInvincible && obj.tag != "Spell Vulnerable")
	            IL_000f: ldloc.0
	            IL_0010: callvirt instance bool HealthManager::get_IsInvincible()
	            IL_0015: brfalse.s IL_002a

	            IL_0017: ldarg.1
	            IL_0018: callvirt instance string [UnityEngine.CoreModule]UnityEngine.GameObject::get_tag()
	            IL_001d: ldstr "Spell Vulnerable"
	            IL_0022: call bool [netstandard]System.String::op_Inequality(string, string)
	            IL_0027: brfalse.s IL_002a

	            // return;
	            IL_0029: ret
             */

            if (cursor.TryGotoNext(
                MoveType.After,
                i => i.MatchCall(out _),
                i => i.MatchBrfalse(out _),
                i => i.MatchRet()
            ))
            {
                cursor.Index--;
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit(OpCodes.Call, burstMethod);
            }
        }

        private static void ONSFDoDamage(On.SpellFluke.orig_DoDamage orig, SpellFluke self, GameObject obj, int upwardRecursionAmount, bool burst)
        {
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
            bool hasShamanStone = PlayerData.instance.GetBool("equippedCharm_19");
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
            PlayerData PD = PlayerData.instance;
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

        private static void HookAfterApplyingDamage(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);

            /*
                // component.SubtractHealth(damage);
		        IL_0031: ldloc.1
		        IL_0032: ldarg.0
		        IL_0033: ldfld int32 SpellFluke::damage
		        IL_0038: callvirt instance void HealthManager::SubtractHealth(int32)
		        // if (component.hp <= 0)
		        IL_003d: ldloc.1
		        IL_003e: ldfld int32 HealthManager::hp
		        IL_0043: ldc.i4.0
		        IL_0044: bgt.s IL_0058
            */

            if (cursor.TryGotoNext(
                MoveType.Before,
                i => i.MatchLdloc(1),
                i => i.MatchLdfld(out _),
                i => i.MatchLdcI4(0),
                i => i.MatchBgt(out _)  
            )) {
                cursor.Emit(OpCodes.Ldarg_1);
                cursor.EmitDelegate<Action<GameObject>>(AddEaterCurse);
            }
        }

        private static void AddEaterCurse(GameObject gameObject) {
            PlayerData playerData = PlayerData.instance;
            if (!playerData.GetBool("equippedCharm_21"))
                return;

            EaterCurseCooldown cooldown = gameObject.GetComponent<EaterCurseCooldown>();
            if (cooldown == null) {
                gameObject.AddComponent<EaterCurseCooldown>();
                gameObject.AddComponent<EaterCurse>();
            }
        }
    } 
}

