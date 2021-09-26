using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class StarManager : MonoBehaviour {
    [SerializeField] private Image[] stars;
    [SerializeField] private Etienne.Cue cue;

    private void Start() {
        foreach(Image star in stars) {
            star.enabled = false;
        }
    }

    public void SaveAFriend(int index, Vector3 worldPosition) {
        Image star = stars[index];
        star.enabled = true;
        Vector3 targetPosition = star.transform.position;
        Vector3 startPosition = Camera.main.WorldToScreenPoint(worldPosition);

        star.transform.DOMove(startPosition, 0);
        star.transform.DOMove(targetPosition, 1).OnComplete(() => Etienne.AudioManager.Play(cue));
        star.transform.DOScale(0, 0);
        star.transform.DOScale(1, .4f);

    }
}
