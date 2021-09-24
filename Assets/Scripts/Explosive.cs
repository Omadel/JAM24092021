using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Explosive : MonoBehaviour {
    public float Range => range;
    [SerializeField] protected float range;

    public List<Explosive> ConnectedExplosives => connectedExplosives;
    [SerializeField] protected List<Explosive> connectedExplosives;

    protected virtual void Update() {
        connectedExplosives = connectedExplosives.Where(x => x != null).ToList();
    }
    protected virtual void OnTriggerStay(Collider other) {
        if(other.TryGetComponent(out Explosive explosive)) {
            if(connectedExplosives.Contains(explosive)) {
                return;
            }
            connectedExplosives.Add(explosive);
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if(other.TryGetComponent(out Explosive explosive)) {
            if(connectedExplosives.Contains(explosive)) {
                connectedExplosives.Remove(explosive);
                return;
            }
        }
    }

    public abstract void Explode(int strenght,Explosive explosiveToRemove = null);
}
