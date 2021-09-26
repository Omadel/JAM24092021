using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    [SerializeField] private Image blackFade;
    [SerializeField] private Image[] stars;
    [SerializeField] private GameObject menu;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private string[] messages;
    [SerializeField] private Etienne.Sound starSound, applauseSound;

    private void Awake() {
        blackFade.DOFade(0, 0);
        menu.SetActive(false);
        foreach(Image star in stars) {
            star.transform.DOScale(0, 0);
        }
    }
    [ContextMenu("Win")]
    internal void Win(int friendSavedCount = 3) {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(blackFade.DOFade(1, .4f).OnComplete(() => menu.SetActive(true)));
        sequence.Append(blackFade.DOFade(0, .4f));
        for(int i = 0; i < friendSavedCount; i++) {
            sequence.Append(stars[i].transform.DOScale(1, .4f)
                .SetEase(Ease.OutBounce)
                .OnStart(() => Etienne.AudioManager.Play(starSound))
                .OnComplete(() => {
                    if(friendSavedCount >= 3) {
                        Etienne.AudioManager.Play(applauseSound);
                    }
                }));
        }
        text.text = messages[friendSavedCount];
    }
}
