using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FriendCage : Destructible {
    [SerializeField] private Color rangeColor = Color.blue;
    [SerializeField] private MeshRenderer rangeRenderer;
    private void Start() {
        rangeRenderer.material.color = rangeColor;
    }
    public override int DoExplotion(int strenght) {
        Bomber.Instance.SaveAFriend(transform.position);
        return base.DoExplotion(strenght);
    }
}
