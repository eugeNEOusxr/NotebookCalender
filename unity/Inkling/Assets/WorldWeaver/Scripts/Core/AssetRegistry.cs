using System.Collections.Generic;
using UnityEngine;

namespace WorldWeaver.Core
{
    public enum AssetSource
    {
        Builtin,
        Gltf,
        Glb,
        Fbx
    }

    public enum AssetCategory
    {
        House,
        Street,
        Decor,
        Prop
    }

    [System.Serializable]
    public class WorldWeaverAssetDef
    {
        public string id;
        public string label;
        public AssetSource source = AssetSource.Builtin;
        public AssetCategory category = AssetCategory.Prop;
        public GameObject prefab;
        public float scale = 1f;
    }

    /// <summary>
    /// Central registry for builtin meshes and imported Blender/Meshy prefabs.
    /// </summary>
    [CreateAssetMenu(fileName = "AssetRegistry", menuName = "WorldWeaver/Asset Registry")]
    public class AssetRegistry : ScriptableObject
    {
        public List<WorldWeaverAssetDef> assets = new List<WorldWeaverAssetDef>();

        public void RegisterExternal(GameObject prefab, string id, string label, AssetSource source, AssetCategory category)
        {
            assets.Add(new WorldWeaverAssetDef
            {
                id = id,
                label = label,
                source = source,
                category = category,
                prefab = prefab,
                scale = 1f
            });
        }

        public WorldWeaverAssetDef Find(string id)
        {
            return assets.Find(a => a.id == id);
        }
    }
}
