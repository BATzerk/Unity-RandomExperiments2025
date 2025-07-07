using UnityEngine;
using System.Collections;


/** For when you need a line that's actually a cube. */
public class CubeLine : MonoBehaviour {
	public const float EmissionRatio = 0.35f; // when we set my color, EmissionColor is set to that color, scaled by this value.
	// Components
	[SerializeField] private MeshRenderer meshRenderer=null; // the actual line cube thingy
	// Properties
	private Vector3 forward;
	private float length; // x coords
	private float thickness = 1f; // y coords
	private float depth = 1f; // z coords
	// References
	private Vector3 startPos;
	private Vector3 endPos;

	// Getters
	public Vector3 Forward { get { return forward; } }
	public float Length { get { return length; } }
	public Material Material { get { return meshRenderer.material; } }
	public Vector3 StartPos {
		get { return startPos; }
		set {
			if (startPos == value) { return; }
			startPos = value;
			UpdateForwardLengthPosition ();
		}
	}
	public Vector3 EndPos {
		get { return endPos; }
		set {
			if (endPos == value) { return; }
			endPos = value;
			UpdateForwardLengthPosition ();
		}
	}

	public void SetStartAndEndPos (Vector3 _startPos, Vector3 _endPos) {
		startPos = _startPos;
		endPos = _endPos;
		UpdateForwardLengthPosition ();
	}



	// ----------------------------------------------------------------
	//  Initialize
	// ----------------------------------------------------------------
    public void Initialize () {
		Initialize (Vector3.zero, Vector3.zero);
	}
	public void Initialize (Vector3 _startPos, Vector3 _endPos) {
		startPos = _startPos;
		endPos = _endPos;

		UpdateForwardLengthPosition ();
	}


	// ----------------------------------------------------------------
	//  Update Things
	// ----------------------------------------------------------------
	private void UpdateForwardLengthPosition() {
		// Update values
		forward = LineUtils.ForwardBetweenPoints(startPos, endPos);
		length = LineUtils.GetLength(startPos, endPos);
		// Transform sprite!
		if (float.IsNaN(endPos.x)) {
			Debug.LogError("Ahem! A SpriteLine's endPos is NaN! (Its startPos is " + startPos + ".)");
		}
		this.transform.localPosition = LineUtils.GetCenterPos(startPos, endPos);
		this.transform.right = forward; // Lol. Sorry. I'm converting this forward back to the space that's relative to the camera.
		ApplyScale();
	}
	private void ApplyScale() {
		// If one of our values would make us invisible, then make us COMPLETELY invisible. So we don't get weird, black, flat squares.
		if (length == 0 || thickness == 0 || depth == 0) {
			meshRenderer.transform.localScale = Vector3.zero;
		}
		else {
            meshRenderer.transform.localScale = new Vector3(length, thickness, depth);
            //meshRenderer.transform.localScale = new Vector3(depth, thickness, length);
		}
	}


	public bool IsVisible {
		get { return meshRenderer.enabled; }
		set { meshRenderer.enabled = value; }
	}
	public void SetAlpha(float alpha) {
        //		Color color = meshRenderer.material.GetColor ("_MainColor");
        Color color = meshRenderer.material.color;
        SetColor(new Color(color.r, color.g, color.b, alpha));
    }
    public void SetColor(Color color) {
		Color emissionColor = new Color(color.r*EmissionRatio, color.g*EmissionRatio, color.b*EmissionRatio, color.a);
		meshRenderer.material.color = color;
		meshRenderer.material.SetColor("_BaseColor", color);
		meshRenderer.material.SetColor("_EmissionColor", emissionColor);
	}
	public void SetThicknessAndDepth(float _thickness, float _depth) {
		thickness = _thickness;
		depth = _depth;
		ApplyScale();
	}

	public void SetMesh(Mesh _mesh) {
		meshRenderer.GetComponent<MeshFilter>().mesh = _mesh;
	}
	public void SetMaterial(Material _mat) {
		meshRenderer.material = _mat;
		//UpdateAngleLengthPosition();
	}
	public void MakeMaterialGradient() {
		Material mat = Resources.Load<Material>(CommonPaths.mat_WhiteGradient);
		if (mat == null) {
			Debug.LogError("No gradient material found at path: " + CommonPaths.mat_WhiteGradient);
			return;
		}
		SetMaterial(mat);
	}


}




