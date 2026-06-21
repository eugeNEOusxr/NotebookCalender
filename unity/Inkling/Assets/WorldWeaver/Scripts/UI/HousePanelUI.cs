using System;
using TMPro;
using UnityEngine;
using WorldWeaver.Core;

namespace WorldWeaver.UI
{
    public class HousePanelUI : MonoBehaviour
    {
        public GameObject panelRoot;
        public TextMeshProUGUI hashText;
        public TextMeshProUGUI wordText;
        public TextMeshProUGUI definitionText;
        public TextMeshProUGUI exampleText;
        public TextMeshProUGUI dateText;

        public void Show(WordHouseEntry entry)
        {
            if (panelRoot != null) panelRoot.SetActive(true);
            if (hashText != null) hashText.text = entry.hash ?? $"#{entry.word}";
            if (wordText != null) wordText.text = entry.word;
            if (definitionText != null) definitionText.text = entry.definition;
            if (exampleText != null) exampleText.text = entry.example;
            if (dateText != null)
            {
                if (DateTime.TryParse(entry.addedAt, out var d))
                    dateText.text = d.ToString("MMM d, yyyy");
                else
                    dateText.text = entry.addedAt ?? "—";
            }
        }

        public void Hide()
        {
            if (panelRoot != null) panelRoot.SetActive(false);
        }
    }
}
