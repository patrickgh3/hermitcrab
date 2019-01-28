using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    [SerializeField] Image bar;
    [SerializeField] Image barBacking;
    [SerializeField] Text text;
    [SerializeField] Text instructionsText;
    float maxWidth;

    float flashT;
    bool flashing = false;

    void Awake() {
        maxWidth = bar.rectTransform.sizeDelta.x;
    }

    public void SetSize(float size) {
        bar.rectTransform.sizeDelta = new Vector2(maxWidth * size, bar.rectTransform.sizeDelta.y);
    }

    public void SetText(string t) {
        text.text = t;
    }

    public void SetFlash(bool f) {
        if (flashing != f) {
            flashT = 0;
        }
        flashing = f;
    }

    public void HideInstructions() {
        instructionsText.enabled = false;
    }

    void Update() {
        flashT += Time.deltaTime * 2f;

        bool visible = true;
        if (flashing && flashT % 1 < 0.5f) visible = false;

        text.enabled = visible;
    }
}
