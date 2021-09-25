using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Explosive), true)]
public class ExplosiveEditor : EtienneEditor.Editor<Explosive> {
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    private static void DrawHandles(Explosive explosive, GizmoType gizmoType) {
        Handles.DrawWireArc(explosive.transform.position, Vector3.up, Vector3.forward, 360f, explosive.Range / 2f);

        foreach(Explosive connectedExplosive in explosive.ConnectedExplosives) {
            if(explosive != null && connectedExplosive != null) {
                Handles.DrawLine(explosive.transform.position, connectedExplosive.transform.position);
            }
        }
    }

    public override void OnInspectorGUI() {
        if(Target.TryGetComponent(out SphereCollider sphereCollider)) {
            sphereCollider.radius = Target.Range / 2f;
        }
        Target.transform.GetChild(2).localScale = 1.07f * Target.Range * Vector3.one;
        base.OnInspectorGUI();
    }
}
