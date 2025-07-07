using UnityEngine;
using System.Collections;


/** Like a GradientSpriteLine, but ALWAYS fades to clear! So no second color/sprite. Slightly optimized so we don't have to create an extra sprite. */
public class GradientClearSpriteLine : SpriteLine {

	override protected void Awake() {
        base.Awake();

        Sprite gradientSprite = Resources.Load<Sprite>(CommonPaths.WhiteGradientSprite);
        sprite.sprite = gradientSprite;
    }

}




