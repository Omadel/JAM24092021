using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class Bomb : Explosive {
    [SerializeField] private int strenght = 1;

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
}
