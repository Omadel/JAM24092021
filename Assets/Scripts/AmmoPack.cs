using DG.Tweening;
using UnityEngine;

public class AmmoPack : MonoBehaviour {
    [SerializeField] private float animationHeight, animationDuration;
    private Transform mesh;

    private void Start() {
        mesh = transform.GetChild(0);
        mesh.DORotate(new Vector3(0, 360, 0), animationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        mesh.DOMoveY(transform.localPosition.y + animationHeight, animationDuration / 3)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void OnDestroy() {
        mesh.DOKill();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject == Bomber.Instance.gameObject) {
            Pickup();
        }
    }

    private void Pickup() {
        mesh.DOComplete();
        mesh.parent = Bomber.Instance.transform;
        GetComponent<SphereCollider>().enabled = false;
        mesh.DOLocalMove(Vector3.up, .4f);
        mesh.DOScale(0, .4f)
            .OnComplete(() => {
                Bomber.Instance.Pickup();
                mesh.DOKill();
                GameObject.Destroy(mesh.gameObject);
                GameObject.Destroy(gameObject);
            })
            ;
    }
}
