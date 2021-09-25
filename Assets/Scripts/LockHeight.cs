using UnityEngine;

public class LockHeight : MonoBehaviour {
    [SerializeField] private float height;

    private void LateUpdate() {
        transform.position = new Vector3(transform.root.position.x, height, transform.root.position.z);
    }
}
