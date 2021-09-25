using UnityEngine;

public class LockRotation : MonoBehaviour {
    [SerializeField] private Vector3 rotation;

    // Update is called once per frame
    private void Update() {
        transform.rotation = Quaternion.Euler(rotation);
    }
}
