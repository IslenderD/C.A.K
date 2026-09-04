using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySelector : MonoBehaviour
{
    [System.Serializable]
    public class Option
    {
        public Button button;
        public TMP_Text label;
        public string title = "EASY";
        public bool locked = false;
    }

    [Header("Options (Easy / Normal / Hard)")]
    public Option[] options = new Option[3];

    [Header("Colours")]
    public Color selectedColor = new Color(0.44f, 0.89f, 1f);
    public Color normalColor   = Color.white;
    public Color lockedColor   = new Color(1f, 1f, 1f, 0.28f);
    public string lockedSuffix = "  (LOCKED)";

    public int Selected { get; private set; }

    void Start()
    {
        for (int i = 0; i < options.Length; i++)
        {
            int index = i;                       // capture, or every button picks the last one
            if (options[i].button == null) continue;
            options[i].button.onClick.AddListener(() => Select(index));
            options[i].button.interactable = !options[i].locked;
        }

        int saved = PlayerPrefs.GetInt("Difficulty", 0);
        Select(IsSelectable(saved) ? saved : FirstUnlocked());
    }

    public void Select(int index)
    {
        if (!IsSelectable(index)) return;
        Selected = index;
        PlayerPrefs.SetInt("Difficulty", index);
        Refresh();
    }

    public void Unlock(int index)
    {
        if (index < 0 || index >= options.Length) return;
        options[index].locked = false;
        if (options[index].button != null) options[index].button.interactable = true;
        Refresh();
    }

    bool IsSelectable(int i) => i >= 0 && i < options.Length && !options[i].locked;

    int FirstUnlocked()
    {
        for (int i = 0; i < options.Length; i++) if (!options[i].locked) return i;
        return 0;
    }

    void Refresh()
    {
        for (int i = 0; i < options.Length; i++)
        {
            var o = options[i];
            if (o.label == null) continue;
            o.label.text  = o.title + (o.locked ? lockedSuffix : "");
            o.label.color = o.locked ? lockedColor
                          : (i == Selected ? selectedColor : normalColor);
        }
    }
}