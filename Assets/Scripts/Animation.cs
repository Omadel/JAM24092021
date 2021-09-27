using DG.Tweening;
using UnityEngine;

public class Animation : MonoBehaviour {
    [SerializeField] private float animationHeight, animationDuration;

    private void Start() {
        transform.DORotate(new Vector3(0, 360, 0), animationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        transform.DOMoveY(transform.localPosition.y + animationHeight, animationDuration / 3)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void OnDisable() {
        transform.DOComplete();
    }

    private void OnDestroy() {
        transform.DOKill();
    }
}
