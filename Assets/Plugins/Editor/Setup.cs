using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Mono.Cecil;
using System.IO;

[InitializeOnLoad]
public class Setup
{
    static Setup()
    {
        LogHandler.Setup();
        Settings.ReloadBuildSettings();

        Debug.Log("Rewriting Dll");

        string assemblyPath = Path.GetFullPath(Application.dataPath + "\\Dlls\\BBI.Unity.Game.dll");
        string cleanPath = Path.GetFullPath(Application.dataPath + "\\Dlls\\BBI.Unity.Game.dll.clean");

        AssemblyResolver.ResolveDirectories = new string[]
        {
            Path.GetFullPath(Path.Combine(Application.dataPath, "Dlls")),
            Path.Combine(Settings.buildSettings.ShipbreakerPath, "Shipbreaker_Data", "Managed")
        };

        string[] typesToModify = File.ReadAllLines(Path.Combine(Settings.buildSettings.ShipbreakerPath, "BepInEx", "patchers", "ModdedShipLoaderPatcher", "TypesToModify.txt"));

        AssemblyDefinition readOnlyAssembly = AssemblyDefinition.ReadAssembly(assemblyPath);
        var typeAssetTypes = readOnlyAssembly.MainModule.Types.Where(t => typesToModify.Contains(t.Name));

        var fields = new List<FieldDefinition>();

        foreach(var typeDefinition in typeAssetTypes)
        {
            var foundFieldAssetBasis = typeDefinition.Fields.Where(field => field.Name == "AssetBasis").FirstOrDefault();
            var foundFieldAssetCloneRef = typeDefinition.Fields.Where(field => field.Name == "AssetCloneRef").FirstOrDefault();
            if(foundFieldAssetBasis != null && foundFieldAssetCloneRef != null)
            {
                fields.Add(foundFieldAssetBasis);
            }
        }

        readOnlyAssembly.Dispose();

        if(fields.Count != typesToModify.Length)
        {
            Debug.Log("Some fields missing, adding");

            if(File.Exists(cleanPath))
            {
                File.Copy(cleanPath, assemblyPath, true);
            }
            else
            {
                File.Copy(assemblyPath, cleanPath);
            }

            var rp = new ReaderParameters();
            rp.ReadingMode = ReadingMode.Immediate;
            rp.ReadWrite = true;
            rp.InMemory = true;
            rp.AssemblyResolver = new AssemblyResolver();
            AssemblyDefinition writingAssembly = AssemblyDefinition.ReadAssembly(assemblyPath, rp);

            typeAssetTypes = writingAssembly.MainModule.Types.Where(t => typesToModify.Contains(t.Name));

            foreach(var typeDefinition in typeAssetTypes)
            {
                typeDefinition.Fields.Insert(0, new FieldDefinition("AssetBasis", FieldAttributes.Public, writingAssembly.MainModule.ImportReference(typeof(string))));
                typeDefinition.Fields.Insert(0, new FieldDefinition("AssetCloneRef", FieldAttributes.Public, writingAssembly.MainModule.ImportReference(typeof(string))));
            }

            writingAssembly.Write(assemblyPath);
        }
        else
        {
            Debug.Log("Fields found");
        }
    }
}

public class AssemblyResolver : IAssemblyResolver
{
    public static string[] ResolveDirectories { get; set; } = {};

    public void Dispose()
    {
        // throw new System.NotImplementedException();
    }

    public AssemblyDefinition Resolve(AssemblyNameReference reference)
    {
        foreach (var directory in ResolveDirectories)
        {
            var potentialDirectories = new List<string> { directory };

            potentialDirectories.AddRange(Directory.GetDirectories(directory, "*", SearchOption.AllDirectories));

            var potentialFiles = potentialDirectories.Select(x => Path.Combine(x, $"{reference.Name}.dll"))
                                                     .Concat(potentialDirectories.Select(
                                                                 x => Path.Combine(x, $"{reference.Name}.exe")));

            foreach (string path in potentialFiles)
            {
                if (!File.Exists(path))
                    continue;

                var assembly = AssemblyDefinition.ReadAssembly(path, new ReaderParameters(ReadingMode.Deferred));

                if (assembly.Name.Name == reference.Name)
                    return assembly;

                assembly.Dispose();
            }
        }

        return null;
    }

    public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
    {
        return Resolve(name);
    }
}