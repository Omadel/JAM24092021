using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Destructible : Explosive {
    [SerializeField] protected int hp;
    [SerializeField] protected TMPro.TextMeshProUGUI floatingText;

    private void Awake() {
        floatingText.text = hp.ToString();
    }

    public override List<Explosive> Explode(int strenght, Explosive explosiveToRemove = null) {
        if(strenght >= hp) {
            return new List<Explosive>() { this };
        }
        return new List<Explosive>();
    }
}
