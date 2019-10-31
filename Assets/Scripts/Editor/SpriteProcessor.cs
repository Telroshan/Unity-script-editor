using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Editor
{
    public class SpriteProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            List<string> paths = importedAssets.Concat(movedAssets).ToList();
            foreach (string importedAsset in paths)
            {
                if (importedAsset.Split('/').Any(x => x == "Sprites"))
                {
                    TextureImporter textureImporter = (TextureImporter) AssetImporter.GetAtPath(importedAsset);

                    if (textureImporter)
                    {
                        textureImporter.textureType = TextureImporterType.Sprite;
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }
    }
}