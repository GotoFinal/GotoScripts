using HarmonyLib;
using UnityEditor;

namespace GotoFinal.SceneDirtyPatch.Editor
{
    [InitializeOnLoad]
    public static class HarmonyPatcher
    {
        static HarmonyPatcher()
        {
            var harmony = new Harmony("dev.gotofinal.scene-dirty-patch");
            harmony.PatchAll();
        }
    }
}