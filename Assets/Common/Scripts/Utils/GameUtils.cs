using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

public static class GameUtils {
    // ----------------------------------------------------------------
    //  Editor and Application
    // ----------------------------------------------------------------
    public static bool IsEditorWindowMaximized() {
        #if UNITY_EDITOR
        var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
        EditorWindow gameWindow = EditorWindow.GetWindow(type);
        return gameWindow!=null && gameWindow.maximized;
        #else
        return false;
        #endif
    }

    public static void SetExpanded(GameObject go, bool expand) {
#if UNITY_EDITOR
        SetExpandedRecursive(go, expand);
        if (expand) {
            for (int i=0; i<go.transform.childCount; i++) {
                SetExpandedRecursive(go.transform.GetChild(i).gameObject, false);
            }
        }
#endif
    }

    public static void SetExpandedRecursive(GameObject go, bool expand) {
#if UNITY_EDITOR
        var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        var methodInfo = type.GetMethod("SetExpandedRecursive", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

        // Get all SceneHierarchyWindow instances
        var windows = Resources.FindObjectsOfTypeAll(type);
        if (windows != null && windows.Length > 0) {
            // Use the first one found — this avoids focusing a new window
            var window = windows[0] as EditorWindow;
            methodInfo.Invoke(window, new object[] { go.GetInstanceID(), expand });
        }
#endif
    }

    /*
    public static void SetExpanded(GameObject go, bool expand) {
        #if UNITY_EDITOR
        //EditorApplication.delayCall += () => {
            SetExpandedRecursive(go, expand);
            // Kinda hackily expand recursively, then un-expand the children.
            if (expand) {
                for (int i=0; i<go.transform.childCount; i++) {
                    SetExpandedRecursive(go.transform.GetChild(i).gameObject, false);
                }
            }
        //};
        #endif
    }
    public static void SetExpandedRecursive(GameObject go, bool expand) {
        #if UNITY_EDITOR
        //EditorApplication.delayCall += () => // NOTE: THIS HAS NO EFFECT?? DELAY the call so we don't risk messing with consuming any keyboard events.
        //{
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
            var methodInfo = type.GetMethod("SetExpandedRecursive");
            EditorWindow window = EditorWindow.GetWindow(type);
            methodInfo.Invoke(window, new object[] { go.GetInstanceID(), expand });
        //};
        #endif
    }
    */




    public static void FocusOnWindow(string window) {
        #if UNITY_EDITOR
        EditorApplication.ExecuteMenuItem("Window/General/" + window);
        #endif
    }
    static public void SetEditorCameraPos(Vector2 pos) {
        #if UNITY_EDITOR
        if (UnityEditor.SceneView.lastActiveSceneView != null) {
            UnityEditor.SceneView.lastActiveSceneView.LookAt(new Vector3(pos.x,pos.y, -10));
        }
        //else { Debug.LogWarning("Can't set editor camera position: UnityEditor.SceneView.lastActiveSceneView is null."); }
        #endif
    }
    public static GameObject CurrSelectedGO() {
        if (UnityEngine.EventSystems.EventSystem.current==null) { return null; }
        return UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
    }
    
    public static void CopyToClipboard(string str) {
        GUIUtility.systemCopyBuffer = str;
    }
    /** Provides the index of which available screen resolution the current Screen.width/Screen.height combo is at. Returns null if there's no perfect fit. */
    public static int GetClosestFitScreenResolutionIndex () {
        for (int i=0; i<Screen.resolutions.Length; i++) {
            // We found a match!?
            if (Screen.width==Screen.resolutions[i].width && Screen.height==Screen.resolutions[i].height) {
                return i;
            }
        }
        return -1; // Hmm, nah, the current resolution doesn't match anything our monitor recommends.
    }
    /** Returns number of seconds elapsed since 1970. */
    public static int GetSecondsSinceEpochStart () {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
    }
    /** Type provided has to be a Component. */
    public static void EditorSelectGameObjectsOfType<T>(Transform parentTF) {
        #if UNITY_EDITOR
        Component[] comps = parentTF.GetComponentsInChildren<T>() as Component[];
        GameObject[] newSelection = new GameObject[comps.Length];
        for (int i=0; i<comps.Length; i++) {
            newSelection[i] = comps[i].gameObject;
        }
        Selection.objects = newSelection;
        #endif
    }
    
    
    
    // ----------------------------------------------------------------
    //  Arrays
    // ----------------------------------------------------------------
    public static bool[] CopyBoolArray(bool[] original) {
        bool[] newArray = new bool[original.Length];
        original.CopyTo(newArray, 0);
        return newArray;
    }
    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n --;
            int k = Random.Range(0, n+1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    // ----------------------------------------------------------------
    //  Colors
    // ----------------------------------------------------------------
    public static bool AreColorsAlmostIdentical(Color colorA, Color colorB) {
        const float thresh = 0.01f;
        return Mathf.Abs(colorA.r-colorB.r) < thresh && Mathf.Abs(colorA.g-colorB.g) < thresh && Mathf.Abs(colorA.b-colorB.b) < thresh && Mathf.Abs(colorA.a-colorB.a) < thresh;
    }




    // ----------------------------------------------------------------
    //  GameObjects
    // ----------------------------------------------------------------
    /** Parents GO to TF, and resets GO's pos, scale, and rotation! */
    public static void ParentAndReset(GameObject go, Transform parentTF) {
        go.transform.SetParent(parentTF);
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;
    }
    public static void MatchTransform(Transform tf, Transform targetTF) {
        tf.SetPositionAndRotation(targetTF.position, targetTF.rotation);
        // Match global scale
        Vector3 targetLossyScale = targetTF.lossyScale;
        if (tf.parent != null) {
            Vector3 parentLossyScale = tf.parent.lossyScale;
            targetLossyScale = new Vector3(
                targetLossyScale.x / parentLossyScale.x,
                targetLossyScale.y / parentLossyScale.y,
                targetLossyScale.z / parentLossyScale.z
            );
        }
        tf.localScale = targetLossyScale;
    }
    public static void MatchPosAndRot(Transform tf, Transform targetTF) {
        tf.SetPositionAndRotation(targetTF.position, targetTF.rotation);
    }
    /// <summary>A cheap and simple helper-function to "un-scale" a child in a parent that's been scaled wonky.</summary>
    public static void SetLocalScaleInverseOfParent(Transform tf, Transform parent=null) {
        if (parent == null) parent = tf.parent;
        if (parent == null) { Debug.LogWarning("SetLocalScaleInverseOfParent: No parent found for " + tf.name); return; }
        tf.localScale = new Vector3(1/parent.localScale.x, 1/parent.localScale.y, 1/parent.localScale.z);
    }

    public static void DestroyAllChildren (Transform parentTF) {
        for (int i=parentTF.childCount-1; i>=0; --i) {
            Transform child = parentTF.GetChild(i);
            Object.Destroy(child.gameObject);
        }
    }
    public static void SetChildrenActive(Transform parent, bool isActive) {
        for (int i= parent.childCount-1; i>=0; --i) {
            parent.GetChild(i).gameObject.SetActive(isActive);
        }
    }
    /** Makes a RectTransform fit to its parent (and STAY flush)! */
    public static void FlushRectTransform(RectTransform rt) {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
    }
    /** Makes the anchor/offset properties of the second RectTransform match those of the first. */
    public static void EchoRectTransformAnchor(RectTransform rtSource, RectTransform rtToChange) {
        rtToChange.anchorMax = rtSource.anchorMax;
        rtToChange.anchorMin = rtSource.anchorMin;
        //rtToChange.offsetMax = rtSource.offsetMax;
        //rtToChange.offsetMin = rtSource.offsetMin;
    }
    /** Local space. Puts tf in center of points, and rotates it to the angle between them. Picture positioning/rotating a broomstick based on the top/bottom points. */
    public static void PositionAndRotate2DTFFrom2Points(Transform tf, Vector2 pointA,Vector2 pointB) {
        tf.localPosition = Vector3.Lerp(pointA, pointB, 0.5f);
        float angle = LineUtils.GetAngle_Degrees(pointA, pointB);
        tf.localEulerAngles = new Vector3(0, 0, angle);
        //tf.localRotation = Quaternion.FromToRotation(pointB, pointA);
    }
    public static void SetGameObjectPivot(Transform tf, Vector3 newGlobalPos) {
        Vector3 offset = newGlobalPos - tf.position;
        tf.position += offset;
        for (int i=0; i<tf.childCount; i++) {
            tf.GetChild(i).position -= offset;
        }
    }
    /** Not SUPER expensive, but it DOES require unparenting and reparenting all children. So be cautious with objects with many children. */
    public static void SetGameObjectPivotExpensive(Transform tf, Quaternion newGlobalRot) {
        SetGameObjectPivotExpensive(tf, tf.position, newGlobalRot);
    }
    /** Not SUPER expensive, but it DOES require unparenting and reparenting all children. So be cautious with objects with many children. */
    public static void SetGameObjectPivotExpensive(Transform tf, Vector3 newGlobalPos, Quaternion newGlobalRot) {
        List<Transform> children = new List<Transform>();
        for (int i=0; i<tf.childCount; i++) {
            children.Add(tf.GetChild(i));
        }
        tf.DetachChildren();
        tf.position = newGlobalPos;
        tf.rotation = newGlobalRot;
        foreach (Transform child in children) {
            child.SetParent(tf);
        }
    }
    /** Will find ACTIVE and INACTIVE children. Recursive search optional, too. */
    public static GameObject FindInactiveChildByName(Transform parent, string name, bool searchRecursively=false) {
        foreach (Transform child in parent) {
            if (child.name == name)
                return child.gameObject;
            if (searchRecursively) {
                var result = FindInactiveChildByName(child, name);
                if (result != null)
                    return result;
            }
        }
        return null;
    }



    // ----------------------------------------------------------------
    //  Sprites and Images
    // ----------------------------------------------------------------
    /** The final alpha will be the provided alpha * the color's base alpha. */
    public static void SetSpriteColor (SpriteRenderer sprite, Color color, float alpha=1) {
        sprite.color = new Color (color.r, color.g, color.b, color.a*alpha);
    }
    /** The final alpha will be the provided alpha * the color's base alpha. */
    public static void SetUIGraphicColor (UnityEngine.UI.Graphic uiGraphic, Color color, float alpha=1) {
        uiGraphic.color = new Color (color.r, color.g, color.b, color.a*alpha);
    }
    /** The sprite's base color alpha is ignored/overridden. */
    public static void SetSpriteAlpha(SpriteRenderer sprite, float alpha) {
        sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, alpha);
    }
    public static void SetTextMeshAlpha(TextMesh textMesh, float alpha) {
        textMesh.color = new Color (textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
    }
    public static void SetUIGraphicAlpha (UnityEngine.UI.Graphic uiGraphic, float alpha) {
        uiGraphic.color = new Color (uiGraphic.color.r, uiGraphic.color.g, uiGraphic.color.b, alpha);
    }
    public static void SetMaterialAlpha(Material m, float alpha) {
        m.color = new Color(m.color.r,m.color.g,m.color.b, alpha);
    }

    static public void SizeRenderer(Renderer renderer, float desiredWidth, float desiredHeight, bool doPreserveRatio = false) {
        if (renderer == null) {
            Debug.LogError("Oops! We've passed in a null Renderer into GameUtils.SizeRenderer.");
            return;
        }
        // Reset my sprite's scale/rotation; find out its neutral size; scale the images based on the neutral size.
        Vector3 pEulerAngles = renderer.transform.localEulerAngles;
        renderer.transform.localScale = Vector2.one;
        renderer.transform.localEulerAngles = Vector3.zero;
        float imgW = renderer.bounds.size.x;
        float imgH = renderer.bounds.size.y;
        if (doPreserveRatio) {
            if (imgW > imgH) {
                desiredHeight *= imgH/imgW;
            }
            else if (imgW < imgH) {
                desiredWidth *= imgW/imgH;
            }
        }
        renderer.transform.localScale = new Vector2(desiredWidth/imgW, desiredHeight/imgH);
        renderer.transform.localEulerAngles = pEulerAngles; // put this back how it was.
    }
    static public void SizeSpriteMask (SpriteMask sm, Vector2 size) { SizeSpriteMask(sm, size.x,size.y); }
    static public void SizeSpriteMask (SpriteMask sm, float desiredWidth,float desiredHeight, bool doPreserveRatio=false) {
        if (sm == null) {
            Debug.LogError("Oops! We've passed in a null SpriteMask into GameUtils.SizeSpriteMask."); return;
        }
        if (sm.sprite == null) {
            Debug.LogError("Oops! We've passed in a SpriteMask with a NULL Sprite into GameUtils.SizeSpriteMask."); return;
        }
        // Reset my sprite's scale; find out its neutral size; scale the images based on the neutral size.
        sm.transform.localScale = Vector2.one;
        float imgW = sm.sprite.bounds.size.x;
        float imgH = sm.sprite.bounds.size.y;
        if (doPreserveRatio) {
            if (imgW > imgH) {
                desiredHeight *= imgH/imgW;
            }
            else if (imgW < imgH) {
                desiredWidth *= imgW/imgH;
            }
        }
        sm.transform.localScale = new Vector2(desiredWidth/imgW, desiredHeight/imgH);
    }
    static public void SizeSpriteRenderer (SpriteRenderer sr, float widthAndHeight) {
        SizeSpriteRenderer(sr, widthAndHeight,widthAndHeight);
    }
    static public void SizeSpriteRenderer (SpriteRenderer sr, Vector2 size) {
        SizeSpriteRenderer(sr, size.x,size.y);
    }
    static public void SizeSpriteRenderer (SpriteRenderer sr, float desiredWidth,float desiredHeight, bool doPreserveRatio=false) {
        if (sr == null) {
            Debug.LogError("Oops! We've passed in a null SpriteRenderer into GameUtils.SizeSpriteRenderer."); return;
        }
        if (sr.sprite == null) {
            Debug.LogError("Oops! We've passed in a SpriteRenderer with a NULL Sprite into GameUtils.SizeSpriteRenderer."); return;
        }
        // Reset my sprite's scale; find out its neutral size; scale the images based on the neutral size.
        float localScaleZ = sr.transform.localScale.z;
        sr.transform.localScale = Vector2.one;
        float imgW = sr.sprite.bounds.size.x;
        float imgH = sr.sprite.bounds.size.y;
        if (doPreserveRatio) {
            if (imgW > imgH) {
                desiredHeight *= imgH/imgW;
            }
            else if (imgW < imgH) {
                desiredWidth *= imgW/imgH;
            }
        }
        sr.transform.localScale = new Vector3(desiredWidth/imgW, desiredHeight/imgH, localScaleZ);
    }
    static public void SizeUIGraphic (UnityEngine.UI.Graphic uiGraphic, Vector2 size) { SizeUIGraphic(uiGraphic, size.x,size.y); }
    static public void SizeUIGraphic (UnityEngine.UI.Graphic uiGraphic, float desiredWidth,float desiredHeight) {
        uiGraphic.rectTransform.sizeDelta = new Vector2 (desiredWidth, desiredHeight);
    }
    static public void SizeUIGraphicX(UnityEngine.UI.Graphic uiGraphic, float desiredWidth) {
        uiGraphic.rectTransform.sizeDelta = new Vector2(desiredWidth, uiGraphic.rectTransform.sizeDelta.y);
    }
    static public void SizeUIGraphicY(UnityEngine.UI.Graphic uiGraphic, float desiredHeight) {
        uiGraphic.rectTransform.sizeDelta = new Vector2(uiGraphic.rectTransform.sizeDelta.x, desiredHeight);
    }




    // ----------------------------------------------------------------
    //  Particle Systems
    // ----------------------------------------------------------------
    public static void SetParticleSystemEmissionEnabled (ParticleSystem ps, bool isEnabled) {
        ParticleSystem.EmissionModule m = ps.emission;
        m.enabled = isEnabled;
    }
    public static void SetParticleSystemEmissionRateOverTime(ParticleSystem ps, float rate) {
        ParticleSystem.EmissionModule m = ps.emission;
        m.rateOverTime = rate;
    }
    public static void SetParticleSystemEmissionRateOverTime(ParticleSystem ps, float rateMin, float rateMax) {
        if (ps == null) return; // Safety check.
        var emission = ps.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rateMin, rateMax);
    }
    public static void SetParticleSystemStartColor(ParticleSystem ps, Color _color) {
        ParticleSystem.MainModule mainModule = ps.main;
        ParticleSystem.MinMaxGradient newColor = new ParticleSystem.MinMaxGradient(_color);
        mainModule.startColor = newColor;
    }
    public static void SetParticleSystemStartColor(ParticleSystem ps, Color colorA,Color colorB) {
        var main = ps.main;
        main.startColor = new ParticleSystem.MinMaxGradient(colorA, colorB);
    }
    public static void SetParticleSystemShapeRadius (ParticleSystem particleSystem, float radius) {
        ParticleSystem.ShapeModule m;
        m = particleSystem.shape;
        m.radius = radius;
    }
    public static void SetParticleSystemStartSize(ParticleSystem particleSystem, float value) {
        ParticleSystem.MainModule main = particleSystem.main;
        main.startSize = value;
    }
    public static void SetParticleSystemStartSpeed(ParticleSystem particleSystem, float value) {
        ParticleSystem.MainModule main = particleSystem.main;
        main.startSpeed = value;
    }
    public static void SetParticleSystemStartSpeed(ParticleSystem particleSystem, float minSpeed, float maxSpeed) {
        ParticleSystem.MainModule main = particleSystem.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(minSpeed, maxSpeed);
    }
    public static void SetParticleSystemLoop(ParticleSystem particleSystem, bool isLooping) {
        ParticleSystem.MainModule main = particleSystem.main;
        main.loop = isLooping;
    }
    public static void SetParticleSystemCollision(ParticleSystem ps, bool isCollision) {
        ParticleSystem.CollisionModule m = ps.collision;
        m.enabled = isCollision;
    }
    public static void AddParticleVel(ParticleSystem ps, Vector3 velDelta) {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int count = ps.GetParticles(particles);
        for (int i = 0; i<count; i++)
            particles[i].velocity += velDelta;
        ps.SetParticles(particles, count);
    }


}




