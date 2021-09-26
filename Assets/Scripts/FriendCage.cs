using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FriendCage : Destructible {
    public override int DoExplotion(int strenght) {
        Bomber.Instance.SaveAFriend(transform.position);
        return base.DoExplotion(strenght);
    }
}
