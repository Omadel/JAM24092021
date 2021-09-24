using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Bomb : MonoBehaviour {
    public float Range => range;
    [SerializeField] private float range;

    public List<Bomb> ConnectedBombs => connectedBombs;
    [SerializeField] private List<Bomb> connectedBombs;

    [SerializeField] private GameObject explosionEffect;

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent(out Bomb bomb)) {
            if(connectedBombs.Contains(bomb)) {
                return;
            }
            connectedBombs.Add(bomb);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent(out Bomb bomb)) {
            if(connectedBombs.Contains(bomb)) {
                connectedBombs.Remove(bomb);
                return;
            }
        }
    }

    [ContextMenu("Explode")]
    public void Explode() {
        Debug.Log("Explosion");
        GameObject.Destroy(GameObject.Instantiate(explosionEffect, transform.position, Quaternion.identity), 2);
        GameObject.Destroy(gameObject);
    }
}
