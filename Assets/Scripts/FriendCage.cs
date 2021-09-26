using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FriendCage : Explosive {
    [SerializeField] private int hp = 3;
    public override List<Explosive> Explode(int strenght, Explosive explosiveToRemove = null) {
        if(strenght >= hp) {
            return new List<Explosive>() { this };
        }
        return new List<Explosive>();
    }

    public override void DoExplotion() {
        Bomber.Instance.SaveAFriend(transform.position);
        base.DoExplotion();
    }
}
