using UnityEngine;

/// <summary>
/// Bottom-left aligned!
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class WireframeCube : MonoBehaviour {
    // Components
    private LineRenderer lineRenderer;
    // Properties
    public Vector3 size = new Vector3(1, 1, 1); // Again, I'm all bottom-left aligned.
    private Vector3[] vertices = new Vector3[8];
    private Vector3[] points = new Vector3[18];
    private int[] path = { 0, 1, 2, 3, 7, 6, 5, 4, 0, 1, 5, 6, 2, 3, 7, 4, 0, 3 }; // Hamiltonian path.


    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points.Length;
        lineRenderer.loop = false;
        lineRenderer.useWorldSpace = false; // we be local, baby. So you can position/rotate us.

        UpdateWireframe();
    }


    // ----------------------------------------------------------------
    //  Show / Hide
    // ----------------------------------------------------------------
    public void SetVisible(bool _isVisible) {
        this.gameObject.SetActive(_isVisible);
    }


    // ----------------------------------------------------------------
    //  Set Pos/Size
    // ----------------------------------------------------------------
    public void SetPosLocal(Vector3 _pos) {
        this.transform.localPosition = _pos;
    }
    public void SetSize(Vector3 _size) {
        size = _size;
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(size.x, 0, 0);
        vertices[2] = new Vector3(size.x, size.y, 0);
        vertices[3] = new Vector3(0, size.y, 0);
        vertices[4] = new Vector3(0, 0, size.z);
        vertices[5] = new Vector3(size.x, 0, size.z);
        vertices[6] = new Vector3(size.x, size.y, size.z);
        vertices[7] = new Vector3(0, size.y, size.z);
        UpdateWireframe();
    }
    public void SetPosLocalAndSize(Vector3 _pos, Vector3 _size) {
        SetPosLocal(_pos);
        SetSize(_size);
    }


    // ----------------------------------------------------------------
    //  Applying Size (Updating Wireframe)
    // ----------------------------------------------------------------
    private void UpdateWireframe() {
        for (int i=0; i<path.Length; i++) {
            points[i] = vertices[path[i]];
        }
        lineRenderer.SetPositions(points);
    }

}
