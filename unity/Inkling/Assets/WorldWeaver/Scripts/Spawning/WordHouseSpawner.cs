using System.Collections.Generic;
using UnityEngine;
using WorldWeaver.Core;
using WorldWeaver.UI;

namespace WorldWeaver.Spawning
{
    public class WordHouseSpawner : MonoBehaviour
    {
        public GameObject housePrefab;
        public Transform housesRoot;
        public HousePanelUI panel;

        readonly List<WordHouse> _houses = new List<WordHouse>();

        public IReadOnlyList<WordHouse> Houses => _houses;

        public void SpawnAll(List<SpawnedWordHouse> spawns)
        {
            if (housesRoot == null) housesRoot = transform;
            foreach (var s in spawns)
            {
                var go = Instantiate(housePrefab, housesRoot);
                var wh = go.GetComponent<WordHouse>();
                if (wh == null) wh = go.AddComponent<WordHouse>();
                wh.Initialize(s);
                _houses.Add(wh);
            }
        }

        public void TryPick(Ray ray, Camera cam)
        {
            if (!Physics.Raycast(ray, out var hit, 80f)) return;
            var wh = hit.collider.GetComponentInParent<WordHouse>();
            wh?.OnTapped(panel);
        }
    }
}
