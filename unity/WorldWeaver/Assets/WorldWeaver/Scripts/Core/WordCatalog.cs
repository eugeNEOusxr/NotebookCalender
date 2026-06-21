using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldWeaver.Core
{
    [Serializable]
    public class WordHouseEntry
    {
        public string id;
        public string word;
        public string hash;
        public string definition;
        public string example;
        public string addedAt;
    }

    [Serializable]
    public class SpawnSettings
    {
        public int gridCols = 4;
        public float blockSpacing = 5.5f;
        public float streetWidth = 3.2f;
    }

    [Serializable]
    public class NeighborhoodCatalog
    {
        public int version = 1;
        public string districtName = "Inkling Lane";
        public SpawnSettings spawn = new SpawnSettings();
        public List<WordHouseEntry> words = new List<WordHouseEntry>();
    }

    [Serializable]
    public class SpawnedWordHouse
    {
        public WordHouseEntry entry;
        public Vector3 position;
        public float rotationY;
    }

    public static class WordCatalog
    {
        public static NeighborhoodCatalog Load(TextAsset json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));
            return JsonUtility.FromJson<NeighborhoodCatalog>(json.text);
        }

        public static List<SpawnedWordHouse> SpawnPositions(NeighborhoodCatalog catalog)
        {
            var result = new List<SpawnedWordHouse>();
            if (catalog?.words == null) return result;

            int cols = catalog.spawn?.gridCols ?? 4;
            float spacing = catalog.spawn?.blockSpacing ?? 5.5f;
            float street = catalog.spawn?.streetWidth ?? 3.2f;
            float half = (cols - 1) * spacing * 0.5f;

            for (int i = 0; i < catalog.words.Count; i++)
            {
                int col = i % cols;
                int row = i / cols;
                float side = row % 2 == 0 ? 1f : -1f;
                float x = col * spacing - half;
                float z = row * spacing * 0.85f + side * (street * 0.5f + 1.8f);
                result.Add(new SpawnedWordHouse
                {
                    entry = catalog.words[i],
                    position = new Vector3(x, 0f, z),
                    rotationY = side > 0 ? 180f : 0f
                });
            }

            return result;
        }
    }
}
