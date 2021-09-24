using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class Bomb : Explosive {

    [SerializeField] private GameObject explosionEffect;



    [ContextMenu("Explode")]
    public override void Explode(int strenght, Explosive explosiveToRemove = null) {
        if(connectedExplosives != null) {
            if(explosiveToRemove != null && connectedExplosives.Contains(explosiveToRemove)) {
                connectedExplosives.Remove(explosiveToRemove);
            }
            StartCoroutine(ExplotionCoroutine(strenght));
            return;
        }
        GameObject.Destroy(GameObject.Instantiate(explosionEffect, transform.position, Quaternion.identity), 2);
        GameObject.Destroy(gameObject);
    }


    public IEnumerator ExplotionCoroutine(int strenght) {
        foreach(Explosive toExplode in connectedExplosives) {
            yield return new WaitForSeconds(.4f);
            if(toExplode == null) {
                connectedExplosives.Remove(toExplode);
            } else {
                toExplode.Explode(strenght+1, this);
            }
        }
        GameObject.Destroy(GameObject.Instantiate(explosionEffect, transform.position, Quaternion.identity), 2);
        GameObject.Destroy(gameObject);
    }

}
