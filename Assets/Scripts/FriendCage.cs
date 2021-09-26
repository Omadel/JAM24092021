using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FriendCage : Explosive {
    [SerializeField] private int hp = 3;
    [SerializeField] private TMPro.TextMeshProUGUI floatingText;

    private void Awake() {
        floatingText.text = hp.ToString();
    }

    public override List<Explosive> Explode(int strenght, Explosive explosiveToRemove = null) {
        if(strenght >= hp) {
            return new List<Explosive>() { this };
        }
        return new List<Explosive>();
    }

    public override int DoExplotion(int strenght) {
        Bomber.Instance.SaveAFriend(transform.position);
        return base.DoExplotion(strenght);
    }
}
