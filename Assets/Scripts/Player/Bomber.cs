using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bomber : MonoBehaviour {
    [SerializeField] private InputActionReference bomberInput, explosionInput;
    [SerializeField] private GameObject spawnPrefab;
    private Bomb connectedBomb;
    private GameObject currentBomb;
    private Rigidbody currentBombRB;
    private Collider currentBombCollider;
    [SerializeField] private Transform spawnPoint;

    private void Awake() {
        bomberInput.action.performed += _ => SpawnBomb();
        bomberInput.action.canceled += _ => DropBomb();
        explosionInput.action.performed += _ => Explosion();
    }

    private void OnEnable() {
        bomberInput.action.Enable();
        explosionInput.action.Enable();
    }

    private void SpawnBomb() {
        Debug.Log("Spawn");
        currentBomb = GameObject.Instantiate(spawnPrefab, transform.position, transform.GetChild(0).rotation, spawnPoint);
        currentBomb.transform.DOMove(spawnPoint.position, .2f);
        currentBomb.transform.DOScale(0, 0);
        currentBomb.transform.DOScale(1, .2f);
        currentBombRB = currentBomb.GetComponentInChildren<Rigidbody>();
        currentBombRB.isKinematic = true;
        currentBombCollider = currentBomb.GetComponentInChildren<Collider>();
        currentBombCollider.enabled = false;
    }

    private void DropBomb() {
        Debug.Log("Drop");
        currentBomb.transform.DOComplete();
        currentBomb.transform.parent = null;
        currentBomb = null;
        currentBombRB.isKinematic = false;
        currentBombRB = null;
        currentBombCollider.enabled = true;
        currentBombCollider = null;
    }

    private void Explosion() {
        if(connectedBomb == null) {
            return;
        }
        List<Explosive> uuu = connectedBomb.Explode(0);
        uuu = uuu.Distinct().ToList();
        foreach(Explosive u in uuu) {
            Debug.Log(u, u);
            u.DoExplotion();
        }
        connectedBomb = null;
    }


    public IEnumerator Explosions(
        List<Explosive> explosives) {
        foreach(Explosive explosive in explosives) {
            explosive.DoExplotion();
            yield return new WaitForSeconds(.3f);
        }
    }


    private void OnTriggerStay(Collider other) {
        if(other.TryGetComponent(out Bomb bomb)) {
            if(connectedBomb == null) {
                connectedBomb = bomb;
            } else {
                if(Vector3.Distance(transform.position, bomb.transform.position) < Vector3.Distance(transform.position, connectedBomb.transform.position)) {
                    connectedBomb = bomb;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent(out Bomb bomb)) {
            if(connectedBomb == null) {
                return;
            }
            connectedBomb = null;
        }
    }

    private void OnDrawGizmos() {
        if(connectedBomb == null) {
            return;
        }
        Gizmos.DrawLine(transform.position, connectedBomb.transform.position);
    }
}
