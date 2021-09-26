using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FriendCage : Destructible {
    [SerializeField] private Color rangeColor = Color.blue;
    private void Start() {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        renderers[renderers.Length - 1].material.color = rangeColor;
    }
    public override int DoExplotion(int strenght) {
        Bomber.Instance.SaveAFriend(transform.position);
        return base.DoExplotion(strenght);
    }
}
