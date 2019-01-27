using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField] Image bar;
    [SerializeField] Image barBacking;
    [SerializeField] Text text;
    float maxWidth;

    void Awake() {
        maxWidth = bar.rectTransform.sizeDelta.x;
    }

    public void SetSize(float size) {
        bar.rectTransform.sizeDelta = new Vector2(maxWidth * size, bar.rectTransform.sizeDelta.y);
    }

    public void SetText(string t) {
        text.text = t;
    }
}
