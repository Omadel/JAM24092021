using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bomb))]
public class NewBehaviourScript : EtienneEditor.Editor<Bomb> {
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    private static void DrawHandles(Bomb bomb, GizmoType gizmoType) {
        Handles.DrawWireArc(bomb.transform.position, Vector3.up, Vector3.forward, 360f, bomb.Range / 2f);

        foreach(var connectedBomb in bomb.ConnectedBombs) {
            Handles.DrawLine(bomb.transform.position, connectedBomb.transform.position);
        }
    }

    public override void OnInspectorGUI() {
        Target.GetComponent<SphereCollider>().radius = Target.Range / 2f;
        base.OnInspectorGUI();
    }
}
