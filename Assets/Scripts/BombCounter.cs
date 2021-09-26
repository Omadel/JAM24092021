using DG.Tweening;
using UnityEngine;

public class BombCounter : MonoBehaviour {
    [SerializeField] private TMPro.TextMeshProUGUI text;
    //[SerializeField] private Button bomberButton;
    //[SerializeField] private Image targetGraphic;
    //[SerializeField] private Sprite highlightedSprite, pressedSprite, selectedSprite, disabledSprite;

    public void ChangeCounterText(string text) {
        this.text.text = text;
        this.text.transform.DOPunchScale(Vector3.one * 1.1f, .2f);
    }

    private void OnDisable() {
        text.transform.DOKill();
    }
}
