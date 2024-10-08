using Kingmaker.Modding;
using System.Collections.Generic;
using UnityEngine;

namespace OwlcatModification.Editor
{
    [CreateAssetMenu(menuName = "Blueprints' Patches")]
    public class BlueprintPatches : ScriptableObject
    {
        public List<OwlcatModificationSettings.BlueprintPatch> Entries =
            new List<OwlcatModificationSettings.BlueprintPatch>();
    }
}