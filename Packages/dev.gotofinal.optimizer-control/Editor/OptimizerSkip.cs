using System;
using System.Collections.Generic;
using System.Reflection;
using GotoFinal.OptimizerControl.Editor;
using nadena.dev.ndmf;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[assembly: ExportsPlugin(
    typeof(OptimizerSkip)
)]

namespace GotoFinal.OptimizerControl.Editor
{
    public class OptimizerSkip : Plugin<OptimizerSkip>
    {
        private const string SkipKey = "SkipOptimizingOnPlayMode";
        private const string ForceOppositeKey = "SkipOptimizingOnPlayModeForceOpposite";
        private const string BaseMenuPath = "Tools/GotTools/";
        private const string MenuPath = BaseMenuPath + "Skip Optimizing on play mode";
        internal static readonly List<string> KnownOptimizers = new() { "Anatawa12.AvatarOptimizer.TraceAndOptimize", "d4rkAvatarOptimizer" };
        internal static List<Type> optimizers = new();

        internal static bool ForceOpposite
        {
            get => EditorPrefs.GetBool(ForceOppositeKey, false);
            set => EditorPrefs.SetBool(ForceOppositeKey, value);
        }

        protected override void Configure()
        {
            if (optimizers.Count == 0) FindOptimizers();
            InPhase(BuildPhase.Resolving)
                .BeforePlugin("dev.hai-vr.starmesh.ExecuteOperators")
                .BeforePlugin("com.anatawa12.avatar-optimizer")
                .BeforePlugin("nadena.dev.ndmf.InternalPasses").Run(OptiDisablerPass.Instance);
        }

        private void FindOptimizers()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = GetTypes(assembly);
                foreach (var type in types)
                {
                    if (!KnownOptimizers.Contains(type.FullName)) continue;
                    optimizers.Add(type);
                }
            }
        }

        private Type[] GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types;
            }
        }

        internal static bool ShouldSkipOptimizer()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode) return false;
            bool setting = EditorPrefs.GetBool(SkipKey, true);
            bool opposite = ForceOpposite;
            bool result = setting ? !opposite : opposite;
            Debug.Log("ShouldSkipOptimizer: " + result + ", setting: " + setting + ", forceOpposite: " + opposite);
            return result;
        }

        [MenuItem(MenuPath, false, 1)]
        private static void ToggleSkipOptimizing()
        {
            bool current = EditorPrefs.GetBool(SkipKey, true);
            EditorPrefs.SetBool(SkipKey, !current);
        }

        [MenuItem(MenuPath, true)]
        private static bool ToggleSkipOptimizingValidate()
        {
            Menu.SetChecked(MenuPath, EditorPrefs.GetBool(SkipKey, true));
            return true;
        }

        [MenuItem(BaseMenuPath + "Play With Optimizing", false, 2)]
        private static void PlayWithOptimizing()
        {
            if (EditorPrefs.GetBool(SkipKey, true)) ForceOpposite = true;
            EditorApplication.isPlaying = true;
        }

        [MenuItem(BaseMenuPath + "Play Without Optimizing", false, 3)]
        private static void PlayWithoutOptimizing()
        {
            if (!EditorPrefs.GetBool(SkipKey, true)) ForceOpposite = true;
            EditorApplication.isPlaying = true;
        }
    }

    class OptiDisablerPass : Pass<OptiDisablerPass>
    {
        protected override void Execute(BuildContext context)
        {
            try
            {
                if (!OptimizerSkip.ShouldSkipOptimizer()) return;
            }
            finally
            {
                OptimizerSkip.ForceOpposite = false;
            }

            var optimizersToDisable = new List<Type>(OptimizerSkip.optimizers);

            var controls = context.AvatarRootObject.GetComponentsInChildren<OptimizerControl>( true);
            foreach (var control in controls)
            {
                if (control.entries == null) continue;
                foreach (var controlEntry in control.entries)
                {
                    if (!controlEntry.gameObject) continue;
                    if (!controlEntry.filterEnabled)
                    {
                        Object.DestroyImmediate(controlEntry.gameObject);
                        continue;
                    }
                    foreach (var componentEntry in controlEntry.components)
                    {
                        if (!componentEntry.component) continue;
                        // if someone on purpose re-added one of components, then allow it
                        if (!componentEntry.removeOnFastBuild && optimizersToDisable.Contains(componentEntry.component.GetType()))
                        {
                            optimizersToDisable.Remove(componentEntry.component.GetType());
                            continue;
                        }
                        if (!componentEntry.removeOnFastBuild) continue;
                        Object.DestroyImmediate(componentEntry.component);
                    }
                }
            }

            foreach (var type in optimizersToDisable)
            {
                var optimizers = context.AvatarRootObject.GetComponentsInChildren(type, true);
                foreach (var optimizer in optimizers)
                {
                    Object.DestroyImmediate(optimizer);
                }
            }
        }
    }
}