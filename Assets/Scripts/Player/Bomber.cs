using UnityEngine;
using UnityEngine.InputSystem;

public class Bomber : MonoBehaviour {
    [SerializeField] private InputActionReference bomberInput;
    [SerializeField] private GameObject spawnPrefab;
    private Bomb connectedBomb;
    private GameObject currentBomb;
    private Rigidbody currentBombRB;
    private Collider currentBombCollider;

    private void Awake() {
        bomberInput.action.performed += _ => SpawnBomb();
        bomberInput.action.canceled += _ => DropBomb();
    }

    private void OnEnable() {
        bomberInput.action.Enable();
    }

    private void SpawnBomb() {
        Debug.Log("Spawn");
        currentBomb = GameObject.Instantiate(spawnPrefab, transform.position + transform.forward + Vector3.up, transform.rotation, transform.GetChild(0));
        currentBombRB = currentBomb.GetComponentInChildren<Rigidbody>();
        currentBombRB.isKinematic = true;
        currentBombCollider = currentBomb.GetComponentInChildren<Collider>();
        currentBombCollider.enabled = false;
    }

    private void DropBomb() {
        Debug.Log("Drop");
        currentBomb.transform.parent = null;
        currentBomb = null;
        currentBombRB.isKinematic = false;
        currentBombRB = null;
        currentBombCollider.enabled = true;
        currentBombCollider = null;
    }

    private void OnTriggerEnter(Collider other) {
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
        if(connectedBomb==null) {
            return;
        }
        Gizmos.DrawLine(transform.position, connectedBomb.transform.position);
    }
}
