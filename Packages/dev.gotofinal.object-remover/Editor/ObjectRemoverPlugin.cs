using System.Collections.Generic;
using nadena.dev.ndmf;
using UnityEngine;

[assembly: ExportsPlugin(
    typeof(GotoFinal.ObjectRemover.Editor.ObjectRemoverPlugin)
)]

namespace GotoFinal.ObjectRemover.Editor
{
    public class ObjectRemoverPlugin : Plugin<ObjectRemoverPlugin>
    {
        protected override void Configure()
        {
            InPhase(BuildPhase.Resolving)
                .BeforePlugin("dev.hai-vr.starmesh.ExecuteOperators")
                .BeforePlugin("com.anatawa12.avatar-optimizer")
                .BeforePlugin("nadena.dev.ndmf.InternalPasses").Run(ObjectRemoverPluginPass.Instance);
        }
    }

    class ObjectRemoverPluginPass : Pass<ObjectRemoverPluginPass>
    {
        protected override void Execute(BuildContext context)
        {
            var removers = context.AvatarRootObject.GetComponentsInChildren<ObjectRemover>(true);
            var objectsToRemove = new List<GameObject>();
            foreach (var remover in removers)
            {
                objectsToRemove.AddRange(remover.objectsToRemove);
                Object.DestroyImmediate(remover);
            }

            foreach (var toRemove in objectsToRemove) Object.DestroyImmediate(toRemove);
        }
    }
}