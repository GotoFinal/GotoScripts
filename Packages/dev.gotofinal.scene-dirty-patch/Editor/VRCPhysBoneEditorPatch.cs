using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEditor;
using UnityEngine;

namespace GotoFinal.SceneDirtyPatch.Editor
{
    [HarmonyPatch(typeof(VRC.SDK3.Dynamics.PhysBone.VRCPhysBoneEditor))]
    [HarmonyPatch("OnInspectorGUI")]
    public static class VRCPhysBoneEditorPatch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> RemoveSetDirtyCall(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo setDirtyMethod = AccessTools.Method(typeof(EditorUtility), nameof(EditorUtility.SetDirty), new[] { typeof(Object) });
            MethodInfo skipMethod = AccessTools.Method(typeof(VRCPhysBoneEditorPatch), nameof(DontSetDirty), new[] { typeof(Object) });

            foreach (var code in instructions)
            {
                if (code.opcode == OpCodes.Call && Equals(code.operand, setDirtyMethod))
                    code.operand = skipMethod;
                yield return code;
            }

            Debug.Log("VRCPhysBoneEditorPatch done.");
        }

        public static void DontSetDirty(UnityEngine.Object obj)
        {
        }
    }
}