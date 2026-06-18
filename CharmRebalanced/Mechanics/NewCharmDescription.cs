using System.Collections.Generic;
using System.Xml.Linq;
using TuyenTuyenTuyen.Charms;

namespace TuyenTuyenTuyen.Mechanics {
    internal static class NewCharmDescription {
        private static readonly Dictionary<string, string> charmDescriptions = new Dictionary<string, string>() {
            {"CHARM_DESC_5", "Protects its bearer with a hard shell while focusing SOUL.<br><br>The shell is not indestructible and will shatter if it absorbs too much damage. The bearer can recover the shell by successfully focusing SOUL."},
            {"CHARM_DESC_9", "Contains a living core that bleeds precious lifeblood.<br><br>After taking some damage, the bearer will gain a coating of lifeblood that protects from a humble amount of damage."},
            {"CHARM_DESC_10", "Unique charm bestowed by the King of Hallownest to his most loyal knight. Scratched and dirty, but still cared for.<br><br>Causes the bearer to emit a heroic odour, making enemies suffer more damage from all sources."},
            {"CHARM_DESC_15", "Formed from the nails of fallen warriors.<br><br>Increases the force of the bearer's nail, making Nail Arts deal more damage and stagger bosses more easily."},
            {"CHARM_DESC_21", "Forgotten shaman artifact, used to draw SOUL from still-living creatures.<br><br>Spells inflict the Eater Curse on enemies. Striking them with the nail greatly increases the amount of SOUL gained on that hit."},
            {"CHARM_DESC_22", "After the bearer successfully focuses SOUL, it summons hatchlings to protect them.<br><br>The hatchlings have no desire to eat or live, and will sacrifice themselves to protect their parent."},
            {"CHARM_DESC_24", "Causes the bearer to find more Geo when defeating enemies or completing Trials.<br><br>This charm is fragile, and will break if its bearer is killed."},
            {"CHARM_DESC_24_G", "Causes the bearer to find more Geo when defeating enemies or completing Trials.<br><br>This charm is unbreakable."},
            {"CHARM_DESC_24_BROKEN", "Causes the bearer to find more Geo when defeating enemies or completing Trials.<br><br>This charm has broken, and the power inside has been silenced. It can not be equipped."},
            {"CHARM_DESC_35", "Contains the gratitude of grubs who will move to the next stage of their lives. Imbues weapons with a holy strength.<br><br>When the bearer is at high health, they will fire beams of white-hot energy from their nail."},
            {"CHARM_DESC_38", "Defensive charm once wielded by a tribe that could shape dreams.<br><br>Conjures two shields that follow the bearer and attempt to protect them."}
        };

        internal static void Load() {
            ModHooks.LanguageGetHook += OnGetLanguage;
        }

        internal static void Unload() {
            ModHooks.LanguageGetHook -= OnGetLanguage;
        }

        private static string OnGetLanguage(string key, string sheetTitle, string orig) {
            if (charmDescriptions.TryGetValue(key, out string newDescription))
                return newDescription;
            return orig;
        }
    }
}
