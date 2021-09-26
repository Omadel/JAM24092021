using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class Bomb : Explosive {
    [SerializeField] private int strenght = 1;
    [SerializeField] private TMPro.TextMeshProUGUI floatingText;
    private float targetPosition;

    private void Start() {
        targetPosition = floatingText.transform.parent.localPosition.y;

        floatingText.transform.DOScale(0, 0);
        floatingText.transform.parent.DOLocalMove(Vector3.zero, 0, true);
    }

    private void OnEnable() {
        floatingText.transform.parent.forward = Camera.main.transform.forward;
    }

    public override List<Explosive> Explode(int strenght, Explosive explosiveToRemove = null) {
        List<Explosive> explosives = new List<Explosive> {
            this
        };
        foreach(Explosive explosive in connectedExplosives) {
            explosive.RemoveExplosive(this);
        }
        foreach(Explosive explosive in connectedExplosives) {
            explosives.AddRange(explosive.Explode(strenght + this.strenght));
        }
        return explosives;
    }
    public override int DoExplotion(int strenght) {
        floatingText.text = (strenght + this.strenght).ToString();
        floatingText.transform.parent.SetParent(null, true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(floatingText.transform.parent.DOMoveY(targetPosition, .4f)
            .OnStart(() => floatingText.transform.DOScale(1, .4f)));
        sequence.AppendInterval(1f);
        sequence.Append(floatingText.transform.parent.DOMoveY(0, .2f)
            .OnStart(() => floatingText.transform.DOScale(0, .2f))
            .OnComplete(() => {
                floatingText.transform.DOKill();
                floatingText.transform.parent.DOKill();
                GameObject.Destroy(floatingText.transform.parent.gameObject);

            }));
        return base.DoExplotion(strenght + this.strenght);

    }
}
