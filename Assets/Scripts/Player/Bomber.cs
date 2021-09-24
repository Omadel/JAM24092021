using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Bomber : MonoBehaviour {
    [SerializeField] private InputActionReference bomberInput;
    [SerializeField] private GameObject spawnPrefab;
    GameObject currentBomb;
    Rigidbody currentBombRB;
    Collider currentBombCollider;

    private void Awake() {
            bomberInput.action.performed+=_=>SpawnBomb();
            bomberInput.action.canceled+=_=>DropBomb();
    }

    private void OnEnable() {
        bomberInput.action.Enable();
    }

    void SpawnBomb(){
        Debug.Log("Spawn");
        currentBomb = GameObject.Instantiate(spawnPrefab, transform.position+ transform.forward, Quaternion.identity, this.transform);
        currentBombRB = currentBomb.GetComponentInChildren<Rigidbody>();
        currentBombRB.isKinematic = true;
        currentBombCollider = currentBomb.GetComponentInChildren<Collider>();
        //currentBombCollider.enabled = false;
    }

    void DropBomb() {
        Debug.Log("Drop");
        currentBomb.transform.parent = null;
        currentBomb = null;
        currentBombRB.isKinematic = false;
        currentBombRB = null;
        currentBombCollider.enabled = true;
        currentBombCollider=null;
    }
}
