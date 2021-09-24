using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FriendCage : Explosive
{
    [SerializeField] private int hp=3;
    public override void Explode(int strenght, Explosive explosiveToRemove = null) {
        if(strenght>=hp) {
            GameObject.Destroy(gameObject);
        }
    }
}
