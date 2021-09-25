using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bomber : MonoBehaviour {
    [SerializeField] private Button bomberButton, explosionButton;
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Color connectedColor = Color.white;

    private Color? baseColor = null;
    private Material connectedBombMaterial;
    private Bomb connectedBomb;
    private GameObject currentBomb;
    private Rigidbody currentBombRB;
    private Collider[] currentBombColliders;

    private void Awake() {
        if(bomberButton != null) {
            EventTrigger trigger = bomberButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry pointerDown = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerDown
            };
            pointerDown.callback.AddListener(_ => SpawnBomb());
            trigger.triggers.Add(pointerDown);
            bomberButton.onClick.AddListener(DropBomb);
        }
        if(explosionButton != null) {
            explosionButton.onClick.AddListener(Explosion);
        }
    }

    private void OnEnable() {
        if(bomberButton != null) {
            bomberButton.interactable = true;
        }
    }

    private void OnDisable() {
        if(bomberButton != null) {
            bomberButton.interactable = false;
        }
        explosionButton.interactable = false;
    }

    private void SpawnBomb() {
        Debug.Log("Spawn");
        currentBomb = GameObject.Instantiate(spawnPrefab, transform.position, transform.GetChild(0).rotation, spawnPoint.parent);
        currentBomb.transform.DOLocalMove(spawnPoint.localPosition, .2f);
        currentBomb.transform.DOScale(0, 0);
        currentBomb.transform.DOScale(1, .2f).SetEase(Ease.OutBounce);
        currentBombRB = currentBomb.GetComponentInChildren<Rigidbody>();
        currentBombRB.isKinematic = true;
        currentBombColliders = currentBomb.GetComponentsInChildren<Collider>();
        foreach(Collider collider in currentBombColliders) {
            collider.enabled = false;
        }
    }

    private void DropBomb() {
        Debug.Log("Drop");
        currentBomb.transform.DOComplete();
        currentBomb.transform.parent = null;
        currentBomb = null;
        currentBombRB.isKinematic = false;
        currentBombRB = null;

        foreach(Collider collider in currentBombColliders) {
            collider.enabled = true;
        }
        currentBombColliders = null;
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
                SetConnectedBomb(bomb);
            } else {
                if(Vector3.Distance(transform.position, bomb.transform.position) < Vector3.Distance(transform.position, connectedBomb.transform.position)) {
                    SetConnectedBomb(bomb);
                }
            }
        }
    }

    private void SetConnectedBomb(Bomb bomb) {
        if(connectedBomb != null) {
            RemoveConnectedBomb();
        }
        connectedBomb = bomb;
        connectedBombMaterial = connectedBomb.transform.GetChild(2).GetComponent<MeshRenderer>().material;
        baseColor ??= connectedBombMaterial.color;
        connectedBombMaterial.DOColor(connectedColor, .2f);
        explosionButton.interactable = true;
    }

    private void RemoveConnectedBomb() {
        connectedBombMaterial.DOColor(baseColor.Value, .2f);
        connectedBomb = null;
        connectedBombMaterial = null;
        explosionButton.interactable = false;
    }

    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent(out Bomb bomb)) {
            if(connectedBomb == null) {
                return;
            }
            RemoveConnectedBomb();
        }
    }

    private void OnDrawGizmos() {
        if(connectedBomb == null) {
            return;
        }
        Gizmos.DrawLine(transform.position, connectedBomb.transform.position);
    }
}
