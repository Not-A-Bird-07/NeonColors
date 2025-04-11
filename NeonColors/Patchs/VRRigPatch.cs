using System.Collections.Generic;
using HarmonyLib;

namespace NeonColors.Patchs
{
    [HarmonyPatch(typeof(VRRig)), HarmonyWrapSafe]
    internal class VRRigPatch
    {
        /*public static void Hook()
        {
            IL.VRRig.InitializeNoobMaterial += InitializeNoobMaterialILHook;
            IL.VRRig.InitializeNoobMaterialLocal += InitializeNoobMaterialLocalILHook;
        }

        private static void InitializeNoobMaterialLocalILHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            Func<Instruction, bool>[] instructions =//ldloca.s 
            [
                instruction => instruction.OpCode == OpCodes.Ldloca_S,
                instruction => instruction.OpCode == OpCodes.Ldloc_0,
                instruction => instruction.OpCode == OpCodes.Ldfld && ((FieldReference)instruction.Operand).Name == "r",
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 0.0f,
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 1.0f,
                instruction => instruction.OpCode == OpCodes.Call && ((MethodReference)instruction.Operand).Name == "Clamp",
                instruction => instruction.OpCode == OpCodes.Stfld && ((FieldReference)instruction.Operand).Name == "r",
                
                instruction => instruction.OpCode == OpCodes.Ldloca_S,
                instruction => instruction.OpCode == OpCodes.Ldloc_0,
                instruction => instruction.OpCode == OpCodes.Ldfld && ((FieldReference)instruction.Operand).Name == "g",
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 0.0f,
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 1.0f,
                instruction => instruction.OpCode == OpCodes.Call && ((MethodReference)instruction.Operand).Name == "Clamp",
                instruction => instruction.OpCode == OpCodes.Stfld && ((FieldReference)instruction.Operand).Name == "g",
                
                instruction => instruction.OpCode == OpCodes.Ldloca_S,
                instruction => instruction.OpCode == OpCodes.Ldloc_0,
                instruction => instruction.OpCode == OpCodes.Ldfld && ((FieldReference)instruction.Operand).Name == "b",
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 0.0f,
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 1.0f,
                instruction => instruction.OpCode == OpCodes.Call && ((MethodReference)instruction.Operand).Name == "Clamp",
                instruction => instruction.OpCode == OpCodes.Stfld && ((FieldReference)instruction.Operand).Name == "b"
            ];
            
            if (c.TryFindNext(out ILCursor[] cursors, instructions))
            {
                foreach (var cursor in cursors)
                {
                    Debug.Log($"Removing OpCode: {cursor}");
                    c.Goto(cursor.Index);
                    c.Remove();
                    //cursor.Remove();
                }
            }
        }

        private static void InitializeNoobMaterialILHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            
            Func<Instruction, bool>[] instructions =
            [
                instruction => instruction.OpCode == OpCodes.Ldarg && (int)instruction.Operand == 3, 
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 0.0f,
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 1.0f,
                instruction => instruction.OpCode == OpCodes.Call && (MethodInfo)instruction.Operand == typeof(GorillaExtensions.GTExt).GetMethod("ClampSafe"),
                instruction => instruction.OpCode == OpCodes.Starg_S && (int)instruction.Operand == 3,
                
                instruction => instruction.OpCode == OpCodes.Ldarg && (int)instruction.Operand == 1, 
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 0.0f,
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 1.0f,
                instruction => instruction.OpCode == OpCodes.Call && (MethodInfo)instruction.Operand == typeof(GorillaExtensions.GTExt).GetMethod("ClampSafe"),
                instruction => instruction.OpCode == OpCodes.Starg_S && (int)instruction.Operand == 3,
                
                instruction => instruction.OpCode == OpCodes.Ldarg && (int)instruction.Operand == 2, 
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 0.0f,
                instruction => instruction.OpCode == OpCodes.Ldc_R4 && (float)instruction.Operand == 1.0f,
                instruction => instruction.OpCode == OpCodes.Call && (MethodInfo)instruction.Operand == typeof(GorillaExtensions.GTExt).GetMethod("ClampSafe"),
                instruction => instruction.OpCode == OpCodes.Starg_S && (int)instruction.Operand == 3
            ];

            if (c.TryFindNext(out ILCursor[] cursors, instructions))
            {
                foreach (var cursor in cursors)
                {
                    Debug.Log($"Removing OpCode: {cursor} in InitializeNoobMaterialILHook");
                    cursor.Remove();
                }
            }
        }*/

        [HarmonyPatch("InitializeNoobMaterial"), HarmonyTranspiler]
        static IEnumerable<CodeInstruction> InitializeNoobMaterialTranspiler(IEnumerable<CodeInstruction> instructions)
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
        [HarmonyPatch("InitializeNoobMaterialLocal"), HarmonyTranspiler]
        static IEnumerable<CodeInstruction> InitializeNoobMaterialLocalTranspiler(IEnumerable<CodeInstruction> instructions)
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