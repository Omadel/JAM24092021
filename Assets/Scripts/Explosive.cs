using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(LineRenderer))]
public abstract class Explosive : MonoBehaviour {
    public float Range => range;
    [SerializeField] protected float range;

    public List<Explosive> ConnectedExplosives => connectedExplosives;
    [SerializeField] protected List<Explosive> connectedExplosives;
    public void RemoveExplosive(Explosive explosive) {
        if(connectedExplosives.Contains(explosive)) {
            connectedExplosives.Remove(explosive);
        }
    }
    private List<Vector3> points = new List<Vector3>();

    [SerializeField] private LineRenderer lineRenderer;

    protected virtual void Update() {
        connectedExplosives = connectedExplosives.Where(x => x != null).ToList();
        points.Clear();
        foreach(Explosive connectedExplosive in ConnectedExplosives) {
            if(connectedExplosive != null) {
                points.Add(transform.position);
                points.Add(connectedExplosive.transform.position);
            }
        }
        points.Add(transform.position);
        if(points.Count > 1) {
            for(int i = 0; i < points.Count; i++) {
                points[i] = new Vector3(points[i].x, 0, points[i].z);
            }
            lineRenderer.enabled = true;
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        } else {
            lineRenderer.enabled = false;

        }
    }
    protected virtual void OnTriggerStay(Collider other) {
        if(other.TryGetComponent(out Explosive explosive)) {
            if(connectedExplosives.Contains(explosive)) {
                return;
            }
            connectedExplosives.Add(explosive);
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if(other.TryGetComponent(out Explosive explosive)) {
            if(connectedExplosives.Contains(explosive)) {
                connectedExplosives.Remove(explosive);
                return;
            }
        }
    }

    public abstract List<Explosive> Explode(int strenght, Explosive explosiveToRemove = null);

    public virtual void DoExplotion() {
        GameObject.Destroy(gameObject);
    }
}
