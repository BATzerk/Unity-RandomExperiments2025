using UnityEngine;
using System.Collections;


/** All dashed lines, rendered with a MeshRenderer! */
public class DashedLineCube : MonoBehaviour {
	// Components
	[SerializeField] private MeshRenderer meshRenderer=null;
	// Properties
	private Vector3 forward;
	private float dashSize; // how many pixels long each dash is. Pretty simple, if you think about it!
	private float length; // x coords
	private float thickness = 1f; // y coords
	private float depth = 1f; // z coords
	// References
	private Vector3 startPos;
	private Vector3 endPos;

	// Getters / Setters
	public float Length { get { return length; } }
	public Vector3 Forward { get { return forward; } }
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
	/** Use this to scroll OneWayStreets. */
	public float TextureOffsetX {
		get { return meshRenderer.material.mainTextureOffset.x; }
		set { meshRenderer.material.mainTextureOffset = new Vector2 (value, meshRenderer.material.mainTextureOffset.y); }
		// Material.SetTextureOffset("_MainTex", new Vector2(scrollValue, 0));
	}

	public void SetStartAndEndPos (Vector3 _startPos, Vector3 _endPos) {
		startPos = _startPos;
		endPos = _endPos;
		UpdateForwardLengthPosition ();
	}



	// ================================================================
	//  Initialize
	// ================================================================
	public void Initialize (float _dashSize) {
		Initialize (Vector3.zero, Vector3.zero, _dashSize);
	}
	public void Initialize (Vector3 _startPos, Vector3 _endPos, float _dashSize) {
		startPos = _startPos;
		endPos = _endPos;
		dashSize = _dashSize;

		UpdateForwardLengthPosition ();
	}


	// ================================================================
	//  Update Things
	// ================================================================
	private void UpdateForwardLengthPosition() {
		// Update values
		forward = LineUtils.ForwardBetweenPoints(startPos, endPos);
		length = LineUtils.GetLength (startPos, endPos);
		// Transform sprite!
		if (float.IsNaN (endPos.x)) {
			Debug.LogError ("Ahem! A DashedLine's endPos is NaN! (Its startPos is " + startPos + ".)");
		}
		Vector3 centerPos = LineUtils.GetCenterPos (startPos, endPos);
		this.transform.localPosition = new Vector3 (centerPos.x, centerPos.y, this.transform.localPosition.z);
		this.transform.right = forward; // Lol. Sorry. I'm converting this forward back to the space that's relative to the camera.
		ApplyScale ();
	}
	private void ApplyScale () {
		this.transform.localScale = new Vector3 (length, thickness, depth);
		// Set textureScale properly!
		float textureScaleX = 0.5f/dashSize; // I did the math: With a 16-pixel-wide dash (8px filled, 8px transparent), the pixel-perfect factor is 0.5f.
		meshRenderer.material.mainTextureScale = new Vector3(textureScaleX*length, meshRenderer.material.mainTextureScale.y);
	}

	public bool IsVisible {
		get { return meshRenderer.enabled; }
		set { meshRenderer.enabled = value; }
	}
//	public void SetAlpha(float alpha) {
////		Color color = meshRenderer.material.GetColor ("_MainColor");
//		Color color = meshRenderer.material.color;
//		SetColor (new Color (color.r,color.g,color.b, alpha));
//	}
	public void SetColor(Color color) {
		meshRenderer.material.color = color;
//		meshRenderer.material.SetColor ("_MainColor", color);
	}
	public void SetColor(Color color, float alpha) {
		SetColor (new Color (color.r,color.g,color.b, alpha));
	}
	public void SetThicknessAndDepth(float _thickness, float _depth) {
		thickness = _thickness;
		depth = _depth;
		ApplyScale ();
	}


}




