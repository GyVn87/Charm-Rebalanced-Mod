using MonoMod.Cil;

namespace TuyenTuyenTuyen.Charms {
    internal static class Charm13_18_MarkOfPride_Longnail {
        private static readonly float comboRange = 1.32f;
        private static readonly float mantisRange= 1.2f;
        private static readonly float longnailRange = 1.12f;

        internal static void Load() {
            IL.NailSlash.StartSlash += NewRangeBuff;
        }

        internal static void Unload() {
            IL.NailSlash.StartSlash -= NewRangeBuff;
        }

        private static void NewRangeBuff(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);

            float rangeBuff = 0;
            while (cursor.TryGotoNext(
                MoveType.Before,
                i => i.MatchLdcR4(out rangeBuff)
            )) {
                if (rangeBuff == 1.4f)
                    cursor.Next.Operand = comboRange;
                else if (rangeBuff == 1.25f)
                    cursor.Next.Operand = mantisRange;
                else if (rangeBuff == 1.15f)
                    cursor.Next.Operand = longnailRange;

                cursor.Index++;
            }
        }
    }
}
