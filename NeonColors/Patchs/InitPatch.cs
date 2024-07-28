using HarmonyLib;
using System.Collections.Generic;

namespace NeonColors.Patchs
{
    [HarmonyPatch(typeof(VRRig), "InitializeNoobMaterial")]
    [HarmonyWrapSafe]
    internal class InitializeNoobMaterialPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (i >= 44 && i <= 58)
                {
                    continue;
                }
                yield return codes[i];
            }
        }
    }

    [HarmonyPatch(typeof(VRRig), "InitializeNoobMaterialLocal")]
    [HarmonyWrapSafe]
    internal class InitializeNoobMaterialLocalPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                if (i >= 12 && i <= 32)
                {
                    continue;
                }
                yield return codes[i];
            }
        }
    }
}
