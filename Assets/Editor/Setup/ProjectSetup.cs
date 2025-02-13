using BepInEx.AssemblyPublicizer;
using HarmonyLib;
using System;
using System.Configuration.Assemblies;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Unity.SharpZipLib.Zip;
using UnityEditor;
using UnityEngine;


namespace OwlcatModification.Editor.Setup
{
    public static class ProjectSetup
    {
        public const string WotrDirectoryKey = "wotr_directory";

        [MenuItem("Modification Tools/Setup project", false, -1000)]
        public static void Setup()
        {
            try
            {
                EditorUtility.DisplayProgressBar("Setup project", "", 0);

                string wotrDirectory = EditorUtility.OpenFolderPanel(
                    "Pathfinder: Wrath of the Righteous folder", EditorPrefs.GetString(WotrDirectoryKey, ""), "");
                if (!Directory.Exists(wotrDirectory))
                {
                    throw new Exception("WotR folder is missing!");
                }

                EditorPrefs.SetString(WotrDirectoryKey, wotrDirectory);
                SetupAssemblies(wotrDirectory);

                File.Copy(
                    Path.Combine(wotrDirectory, "Bundles/utility_shaders"),
                    "Assets/RenderPipeline/utility_shaders",
                    true);
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Error!", $"{e.Message}\n\n{e.StackTrace}", "Close");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static void SetupAssemblies(string wotrDirectory)
        {
            string[] skipAssemblies = {
                "mscorlib.dll",
                "Unity.ScriptableBuildPipeline.dll",
                "Owlcat.SharedTypes.dll",
                "UnityEngine.UI.dll",
                "Unity.Burst.dll",
                "Unity.Burst.Unsafe.dll",
                "Unity.Mathematics.dll",
                "Unity.TextMeshPro.dll",
                "Owlcat.Runtime.Visual.dll",
                "0Harmony.dll"
            };

            bool SkipAssembly(string filename)
            {
                return
                    skipAssemblies.Contains(filename) ||
                    filename.StartsWith("System") ||
                    (!filename.EndsWith(".dll") && !filename.EndsWith(".pdb")) ||
                    (filename.StartsWith("UnityEngine") && !filename.StartsWith("UnityEngine.UI"));
            }

            string[] publicize =
            {
                "Assembly-CSharp.dll",
                "Owlcat.Runtime.Core.dll",
                "Owlcat.Runtime.UI.dll"
            };

            const string targetAssembliesDirectory = "Assets/PathfinderAssemblies";
            Directory.CreateDirectory(targetAssembliesDirectory);

            string assembliesDirectory = Path.Combine(wotrDirectory, "Wrath_Data/Managed");

            int newAssemblies = 0;

            foreach (string assemblyPath in Directory.GetFiles(assembliesDirectory))
            {
                string filename = Path.GetFileName(assemblyPath);

                if (SkipAssembly(filename))
                    continue;

                if (!File.Exists(Path.Combine(targetAssembliesDirectory, filename)))
                {
                    if (publicize.Contains(filename))
                    {
                        AssemblyPublicizerOptions options = new AssemblyPublicizerOptions()
                        {
                            IncludeOriginalAttributesAttribute = false,
                        };
                        AssemblyPublicizer.Publicize(assemblyPath, Path.Combine(targetAssembliesDirectory, filename), options);
                    }
                    else
                        File.Copy(assemblyPath, Path.Combine(targetAssembliesDirectory, filename), true);
                    newAssemblies++;
                }
            }

            const string harmonyZip = "Assets/Editor/Setup/Harmony-1.3.1.1.zip";
            UnzipFile(harmonyZip, targetAssembliesDirectory);

            if (newAssemblies > 0)
                AssetDatabase.Refresh();
        }

        private static void UnzipFile(string zipPath, string extractPath)
        {
            using (FileStream fs = File.OpenRead(zipPath))
            using (ZipInputStream zipStream = new ZipInputStream(fs))
            {
                ZipEntry entry;
                while ((entry = zipStream.GetNextEntry()) != null)
                {
                    string entryPath = Path.Combine(extractPath, entry.Name);
                    if (entry.IsDirectory)
                    {
                        Directory.CreateDirectory(entryPath);
                        continue;
                    }

                    using (FileStream output = File.Create(entryPath))
                    {
                        zipStream.CopyTo(output);
                    }
                }
            }
            Debug.Log($"Extracted to: {extractPath}");
        }
    }
}