using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class Bomber : Etienne.Singleton<Bomber> {
    [SerializeField] private Button bomberButton, explosionButton;
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Color connectedColor = Color.white;
    [SerializeField] private int maxBombCount;
    [SerializeField] private StarManager starManager;
    [SerializeField] private Menu menu;
    [SerializeField] private Etienne.Sound winSound, dropSound;
    [SerializeField] private Etienne.Cue popCue;

    private Color? baseColor = null;
    private Material connectedBombMaterial;
    private Bomb connectedBomb;
    private GameObject currentBomb;
    private Rigidbody currentBombRB;
    private Collider[] currentBombColliders;
    private BombCounter bombCounter;
    private int currentBombCount, friendSavedCount;

    internal void Pickup() {
        currentBombCount++;
        bombCounter.ChangeCounterText(currentBombCount.ToString());

        bomberButton.interactable = true;
        bombCounter.gameObject.SetActive(true);
        Etienne.AudioManager.Play(popCue);
    }

    protected override void Awake() {
        base.Awake();
        if(bomberButton != null) {
            EventTrigger trigger = bomberButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry pointerDown = new EventTrigger.Entry {
                eventID = EventTriggerType.PointerDown
            };
            pointerDown.callback.AddListener(_ => SpawnBomb());
            trigger.triggers.Add(pointerDown);
            bomberButton.onClick.AddListener(DropBomb);
            bombCounter = bomberButton.GetComponentInChildren<BombCounter>();
        }
        if(explosionButton != null) {
            explosionButton.onClick.AddListener(Explosion);
        }
        currentBombCount = maxBombCount;
        bombCounter.ChangeCounterText(currentBombCount.ToString());
        bomberButton.interactable = true;
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
        if(explosionButton != null) {
            explosionButton.interactable = false;
        }
    }

    private void SpawnBomb() {
        if(bomberButton.interactable) {
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
            currentBombCount--;
            bombCounter.ChangeCounterText(currentBombCount.ToString());
            Etienne.AudioManager.Play(popCue);
        }
    }

    private void DropBomb() {
        currentBomb.transform.DOComplete();
        currentBomb.transform.parent = null;
        currentBomb = null;
        currentBombRB.isKinematic = false;
        currentBombRB = null;
        Etienne.AudioManager.Play(dropSound);
        foreach(Collider collider in currentBombColliders) {
            collider.enabled = true;
        }
        currentBombColliders = null;
        if(currentBombCount <= 0) {
            bomberButton.interactable = false;
            bombCounter.gameObject.SetActive(false);
        }
    }

    private void Explosion() {
        if(connectedBomb == null) {
            return;
        }
        StartCoroutine(Explosions(connectedBomb.Explode(0)));
        connectedBomb = null;
    }


    public IEnumerator Explosions(List<Explosive> explosives) {
        Camera camera = Camera.main;
        Vector3 baseLocalPosition = camera.transform.localPosition;
        float baseZ = baseLocalPosition.z;
        camera.transform.DOLocalMoveZ(baseZ * 1.4f, .4f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(.4f);
        float zoffset = transform.position.z - camera.transform.position.z;
        explosives = explosives.Distinct().ToList();
        int strenght = 0;
        foreach(Explosive explosive in explosives) {
            Vector3 newPosition = new Vector3(
                explosive.transform.position.x,
                camera.transform.position.y,
                explosive.transform.position.z - zoffset
                );
            camera.transform.DOMove(newPosition, .2f).SetEase(Ease.Linear);
            strenght = explosive.DoExplotion(strenght);
            yield return new WaitForSeconds(.3f);
        }
        camera.transform.DOLocalMove(baseLocalPosition, .4f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(1f);
        CheckWin();

    }

    private void CheckWin() {
        if(friendSavedCount < 3 && currentBombCount > 0) {
            return;
        }
        menu.Win(friendSavedCount);
        Etienne.AudioManager.Play(winSound);
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

    public void SaveAFriend(Vector3 worldPosition) {
        friendSavedCount++;
        starManager.SaveAFriend(friendSavedCount - 1, worldPosition);
    }
}
