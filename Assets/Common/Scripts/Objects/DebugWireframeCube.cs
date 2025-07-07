using UnityEngine;

/// <summary>
/// Put this DIRECTLY ON a BoxCollider GameObject! (Or parented to it.)
/// </summary>
public class DebugWireframeCube : MonoBehaviour {
    public Color wireColor = Color.green; // Color for the wireframe
    private Material lineMaterial;

    void OnEnable() {
        CreateLineMaterial();
    }

    void CreateLineMaterial() {
        if (!lineMaterial) {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader) {
                hideFlags = HideFlags.HideAndDontSave
            };
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void OnRenderObject() {
        if (!lineMaterial) return;

        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        GL.Color(wireColor);

        DrawWireCube(Vector3.zero, Vector3.one);// transform.localScale); NOTE: If we're parented (which we should be), then keep our scale at 1.

        GL.End();
        GL.PopMatrix();
    }

    void DrawWireCube(Vector3 center, Vector3 size) {
        Vector3[] points = new Vector3[8];
        Vector3 half = size * 0.5f;

        points[0] = center + new Vector3(-half.x, -half.y, -half.z);
        points[1] = center + new Vector3(half.x, -half.y, -half.z);
        points[2] = center + new Vector3(half.x, -half.y, half.z);
        points[3] = center + new Vector3(-half.x, -half.y, half.z);
        points[4] = center + new Vector3(-half.x, half.y, -half.z);
        points[5] = center + new Vector3(half.x, half.y, -half.z);
        points[6] = center + new Vector3(half.x, half.y, half.z);
        points[7] = center + new Vector3(-half.x, half.y, half.z);

        int[] edges = {
            0,1, 1,2, 2,3, 3,0,
            4,5, 5,6, 6,7, 7,4,
            0,4, 1,5, 2,6, 3,7
        };

        for (int i = 0; i < edges.Length; i += 2) {
            GL.Vertex(points[edges[i]]);
            GL.Vertex(points[edges[i + 1]]);
        }
    }
}
