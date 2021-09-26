using UnityEngine;

public class LockHeight : MonoBehaviour {
    [SerializeField] private float height;

    private void LateUpdate() {
        transform.localPosition = new Vector3(0, height, 0);
    }
}
