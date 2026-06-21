using TMPro;
using UnityEngine;
using WorldWeaver.Core;
using WorldWeaver.UI;

namespace WorldWeaver.Spawning
{
    /// <summary>
    /// One word house with TMP hashtag label and tap collider.
    /// </summary>
    public class WordHouse : MonoBehaviour
    {
        public WordHouseEntry entry;
        public TextMeshPro label;
        public Collider pickCollider;

        public void Initialize(SpawnedWordHouse spawn)
        {
            entry = spawn.entry;
            transform.position = spawn.position;
            transform.rotation = Quaternion.Euler(0f, spawn.rotationY, 0f);
            if (label != null)
                label.text = string.IsNullOrEmpty(entry.hash) ? $"#{entry.word}" : entry.hash;
        }

        public void OnTapped(HousePanelUI panel)
        {
            if (panel != null && entry != null)
                panel.Show(entry);
        }
    }
}
