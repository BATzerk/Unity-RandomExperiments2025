using UnityEngine;
using System.Collections;


/** A lightweight alternative to LineRenderer. */
public class SpriteLine : MonoBehaviour {
	// Components
	[SerializeField] protected SpriteRenderer sprite=null; // the actual line sprite thingy
	// Properties
	private Vector3 right;
	private float length;
	private float thickness = 1f;
	// References
	//	private Line line;
	private Vector3 startPos;
	private Vector3 endPos;

	// Getters
	public Vector3 Right { get { return right; } }
	public float Length { get { return length; } }
	public Vector3 StartPos {
		get { return startPos; }
		set {
			if (startPos == value) { return; }
			startPos = value;
			UpdateAngleLengthPosition ();
		}
	}
	public Vector3 EndPos {
		get { return endPos; }
		set {
			if (endPos == value) { return; }
			endPos = value;
			UpdateAngleLengthPosition ();
		}
	}

	public void SetStartAndEndPos (Vector3 _startPos, Vector3 _endPos) {
		startPos = _startPos;
		endPos = _endPos;
		UpdateAngleLengthPosition ();
	}



	// ----------------------------------------------------------------
	//  Initialize
	// ----------------------------------------------------------------
	virtual protected void Awake () {
        // Make my SpriteRenderer if it doesn't exist!
        if (sprite == null) {
            sprite = this.gameObject.AddComponent<SpriteRenderer>();
            Texture2D squareTexture = Resources.Load(CommonPaths.tex_WhiteSquare) as Texture2D;
            sprite.sprite = Sprite.Create(squareTexture, new Rect(0, 0, squareTexture.width, squareTexture.height), new Vector2(0.5f, 0.5f));
        }
    }
    public void Initialize () {
		Initialize (Vector3.zero, Vector3.zero);
	}
	public void Initialize (Vector3 _startPos, Vector3 _endPos) {
		startPos = _startPos;
		endPos = _endPos;

		UpdateAngleLengthPosition ();
	}


	// ----------------------------------------------------------------
	//  Update Things
	// ----------------------------------------------------------------
	private void UpdateAngleLengthPosition() {
		// Update values
		right = LineUtils.ForwardBetweenPoints(startPos, endPos);
		length = LineUtils.GetLength (startPos, endPos);
		// Transform sprite!
		if (float.IsNaN (endPos.x)) {
			Debug.LogError ("Ahem! A SpriteLine's endPos is NaN! (Its startPos is " + startPos + ".)");
		}
		this.transform.localPosition = LineUtils.GetCenterPos(startPos, endPos);
		this.transform.right = right;
		GameUtils.SizeSpriteRenderer(sprite, length, thickness);
	}

	public bool IsVisible {
		get { return sprite.enabled; }
		set {
			sprite.enabled = value;
		}
	}


	public void SetAlpha(float alpha) {
		GameUtils.SetSpriteAlpha (sprite, alpha);
	}
	public void SetColor(Color color) {
		sprite.color = color;
	}
	public void SetSortingOrder(int sortingOrder) {
		sprite.sortingOrder = sortingOrder;
	}
	public void SetThickness(float _thickness) {
		thickness = _thickness;
		GameUtils.SizeSpriteRenderer(sprite, length, thickness);
	}
	/** Replace my default, solid square sprite with a different sprite! Basically used for gradient lines. */
	public void SetSourceSprite(Sprite _sourceSprite) {
		sprite.sprite = _sourceSprite;
		UpdateAngleLengthPosition();
	}
	public void MakeSourceSpriteGradientSprite() {
		Sprite sprite = Resources.Load<Sprite>(CommonPaths.WhiteGradientSprite);
		if (sprite == null) {
			Debug.LogError("No gradient sprite found at path: " + CommonPaths.WhiteGradientSprite);
			return;
		}
		SetSourceSprite(sprite);
	}


}




