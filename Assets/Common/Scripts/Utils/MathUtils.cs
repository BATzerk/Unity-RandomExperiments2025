using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MathUtils {
    // ----------------------------------------------------------------
    //  Bools and Ints
    // ----------------------------------------------------------------
    public static bool RandomBool() { return Random.Range(0,1f) < 0.5f; }

    public static bool AreFloatsEqual(float a, float b, float threshold=0.001f) {
        return Mathf.Abs(a-b) < threshold;
    }

    static public bool IsSameSign(float a,float b) { return a*b >= 0; }
    static public bool IsSameSign(double a,double b) { return a*b >= 0; }
    static public int Sign(float value,bool doAllow0 = true) {
        if (value < 0) return -1;
        if (value > 0) return 1;
        if (doAllow0) return 0;
        return 1; // We can specify to prevent returning 0. Very useful for any variable with "dir" in the name.
    }

    /* Ugly code, but surprisingly faster than alternatives. */
    static public int NumDigits(float n) {
        n = Mathf.Abs(n);
        if (n < 10) return 1;
        if (n < 100) return 2;
        if (n < 1000) return 3;
        if (n < 10000) return 4;
        if (n < 100000) return 5;
        if (n < 1000000) return 6;
        if (n < 10000000) return 7;
        if (n < 100000000) return 8;
        if (n < 1000000000) return 9;
        if (n < 10000000000) return 10;
        if (n < 100000000000) return 11;
        if (n < 1000000000000) return 12;
        if (n < 10000000000000) return 13;
        if (n < 100000000000000) return 14;
        if (n < 1000000000000000) return 15;
        if (n < 10000000000000000) return 16;
        if (n < 100000000000000000) return 17;
        if (n < 1000000000000000000) return 18;
        Debug.LogWarning("Too many digits passed into NumDigits.");
        return 19;
    }
    /// Will turn numbers like 0.00008279 to a nice, fair 0!
    static public float RoundTo2DPs(float _value) {
        return Mathf.Round(_value * 100f) / 100f;
    }
    /// Will turn numbers like 0.00008279 to a nice, fair 0!
    static public float RoundTo3DPs(float _value) {
        return Mathf.Round(_value * 1000f) / 1000f;
    }
    /// Will turn numbers like 0.00008279 to a nice, fair 0!
    static public float RoundTo4DPs(float _value) {
        return Mathf.Round(_value * 10000f) / 10000f;
    }
    static public float RoundTo5DPs(float _value) { return Mathf.Round(_value * 100000f) / 100000f; }
    static public Vector3 RoundTo3DPs(Vector3 vector) {
        return new Vector3(RoundTo3DPs(vector.x), RoundTo3DPs(vector.y), RoundTo3DPs(vector.z));
    }

    /// Maps Cos from (-1 to 1) to (0 to 1); also offsets so 0 returns 1.
    static public float Cos01(float val) { return (1-Mathf.Sin(val)) * 0.5f; }
    /// Maps Sin from (-1 to 1) to (0 to 1); also offsets so 0 returns 0.
    static public float Sin01(float val) { return (1-Mathf.Cos(val)) * 0.5f; }
    /// Maps Sin from (-1 to 1) to (a to b).
    static public float SinRange(float a,float b,float val) { return Mathf.Lerp(a,b,Sin01(val)); }

    /// For 2D grids. Converts col/row to fit into a 1D array.
    public static int GridIndex2Dto1D(int col,int row,int numCols) { return col + row*numCols; }
    /// For 2D grids. Converts 1D-array index to col/row.
    public static Vector2Int GridIndex1Dto2D(int index,int numCols) { return new Vector2Int(index%numCols,Mathf.FloorToInt(index/(float)numCols)); }

    /// E.g. if arrayLength is 4, we may return 2,0,3,1.
    public static int[] GetShuffledIntArray(int arrayLength) {
        int[] array = new int[arrayLength];
        for (int i = 0; i<arrayLength; i++) { array[i] = i; }
        return GetShuffledIntArray(array);
    }
    public static int[] GetShuffledIntArray(int[] originalArray) {
        int[] shuffledArray = new int[originalArray.Length];
        for (int i = 0; i<shuffledArray.Length; i++) { shuffledArray[i] = originalArray[i]; }
        for (int i = 0; i<shuffledArray.Length; i++) {
            int randIndex = Random.Range(0, shuffledArray.Length);
            int valA = shuffledArray[i];
            int valB = shuffledArray[randIndex];
            shuffledArray[i] = valB;
            shuffledArray[randIndex] = valA;
        }
        return shuffledArray;
    }
    public static float Map(float s, float a1, float a2, float b1, float b2) {
        s = a1<a2 ? Mathf.Clamp(s, a1, a2) : Mathf.Clamp(s, a2, a1); // account for a1 maybe being larger than a2.
        float val = b1 + (s-a1)*(b2-b1)/(a2-a1);
        return val;//Mathf.Clamp(val, b1, b2);
    }
    public static float MapUnclamped(float s, float a1, float a2, float b1, float b2) {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }


    /// <summary>
    /// Use for EASING functions that aren't on FixedUpdate!
    /// Example: temperature = (targetTemp - temperature) * RatePerTick(0.1f, TickInterval);
    /// </summary>
    public static float RatePerTick(float ratePerSecond, float tickInterval) {
        return 1f - Mathf.Pow(1f - ratePerSecond, tickInterval);
    }
    ///// <summary>Uses Time.deltaTime!</summary>
    //public static float EaseTowards(float current, float target, float speed) {
    //    return Mathf.Lerp(current, target, 1f - Mathf.Exp(-speed * Time.deltaTime));
    //}
    ///// <summary>Uses Time.deltaTime!</summary>
    //public static float EaseTowards(float current, float target, float ratePerSecond) {
    //    float t = 1f - Mathf.Pow(1f - ratePerSecond, Time.deltaTime);
    //    return Mathf.Lerp(current, target, t);
    //}


    public static float CelsiusToFahrenheit(float celsius) {
        return (celsius * 9f / 5f) + 32f;
    }




    // ----------------------------------------------------------------
    //  Vectors, Angles
    // ----------------------------------------------------------------
    public static readonly Vector2 Vector2NaN = new Vector2(float.NaN, float.NaN);
    public static readonly Vector3 Vector3NaN = new Vector3(float.NaN, float.NaN, float.NaN);
    public static bool IsVector2NaN(Vector2 vector) { return float.IsNaN(vector.x); }
    public static bool IsVector3NaN(Vector3 vector) { return float.IsNaN(vector.x); }
    public static Vector2 Abs(Vector2 v) {
        return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }
    public static Vector2 Round(Vector2 v) {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }
    public static Vector3 Abs(Vector3 v) {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }
    public static Vector3 Round(Vector3 v) {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }
    public static bool AreVectorsAboutEqual(Vector3 v0, Vector3 v1) {
        float t = 0.001f; // Threshold.
        return Mathf.Abs(v0.x-v1.x)<t && Mathf.Abs(v0.y-v1.y)<t && Mathf.Abs(v0.z-v1.z)<t;
    }

    /**
     * -1 to 1.
     * 1 if the exact same direction. 0 if perpendicular. -1 if exact opposite direction.
     * This simply returns the dot product of the two vectors, normalized.
     * Examples:
        1,0,0  and -1,0,0  = -1
        1,-1,0 and -1,1,0  = -1
        1,0,0  and  1,0,0  = 1
        1,-1,1 and  1,-1,1 = 1
        1,0,0  and  0,0,0  = 0
        1,0,0  and  0,1,0  = 0
     */
    static public float VectorAlignment(Vector3 v1, Vector3 v2) {
        return Vector3.Dot(v1.normalized, v2.normalized);
    }
    static public float VectorAlignmentAbs(Vector3 v1, Vector3 v2) {
        return Mathf.Abs(VectorAlignment(v1, v2));
    }
    static public float GetAngleRad(Vector2 vector) { return -Mathf.Atan2 (vector.y,vector.x); } //return Mathf.Atan2(-vector.x, vector.y);
    static public float GetAngleDeg(Vector2 vector) { return GetAngleRad(vector) * Mathf.Rad2Deg; }
    static public float GetAngleDiffDeg(Vector2 vectorA, Vector2 vectorB) {
        return GetAngleDiffDeg(GetAngleDeg(vectorA), GetAngleDeg(vectorB));
    }
	static public float GetAngleDiffRad(Vector2 vectorA, Vector2 vectorB) {
        return GetAngleDiffRad(GetAngleRad(vectorA), GetAngleRad(vectorB));
    }
    static public float GetAngleDiffDeg(float angleA,float angleB) {
        // Keep both angles between -180 and 180.
        float difference = angleA - angleB;
		if (difference < -180) difference += 360;
		else if (difference > 180) difference -= 360;
		return difference;
	}
	static public float GetAngleDiffRad(float angleA, float angleB) {
		// Keep both angles between -PI and PI.
		float difference = angleA - angleB;
		if (difference < -Mathf.PI) difference += Mathf.PI*2;
		else if (difference > Mathf.PI) difference -= Mathf.PI*2;
		return difference;
    }
    /** If provided 90 and -85, this will return -5 as the difference. Useful to determine parallel-ness. */
    static public float GetDifferenceBetweenAngles180Insensitive(float angleA, float angleB) {
        float difference = GetAngleDiffDeg(angleA, angleB);
        if (difference < -90) difference += 180;
        else if (difference > 90) difference -= 180;
        return difference;
    }

    public static Vector2 GetRotatedVector2Rad(Vector2 v,float radians) {
        return GetRotatedVector2Deg(v,radians*Mathf.Rad2Deg);
    }
    public static Vector2 GetRotatedVector2Deg(Vector2 v,float degrees) {
        return Quaternion.Euler(0,0,degrees) * v;
    }
    public static Vector3 GetRotatedVector3Deg(Vector3 v,float degrees) {
        return Quaternion.Euler(0,0,degrees) * v;
    }
    /** 0 is UP, PI is LEFT. */
    public static Vector2 GetVectorFromRad(float radians) {
        return new Vector2(Mathf.Sin(-radians), Mathf.Cos(-radians));
    }
    /** 0 is UP, 90 degrees is LEFT. */
    public static Vector2 GetVectorFromDeg(float degrees) { return GetVectorFromRad(degrees*Mathf.Deg2Rad); }
    /** Returns a normalized vector with the provided angle. */
    public static Vector2 GetVector2FromAngle_Radians(float radians) {
        return new Vector2(Mathf.Cos(radians), -Mathf.Sin(radians));
    }


    public static Vector3 ConstantSlerp(Vector3 from,Vector3 to,float angle) {
        float value = Mathf.Min (1, angle / Vector3.Angle(from, to));
        return Vector3.Slerp(from,to,value);
    }
    public static Vector3 ProjectOntoPlane(Vector3 v,Vector3 normal) {
        return v - Vector3.Project(v,normal);
    }
    /** TO DO: #optimization This function uses way overkill with converting to vectors and back. There has GOT to be a simpler way with just using the angles. */
    public static float GetAngleReflection (float angleIn, float surfaceAngle) {
        return 180+GetAngleDeg (Vector2.Reflect (GetVectorFromDeg(-angleIn), GetVectorFromDeg(surfaceAngle)));
    }
    //*
    public static Vector3 SnapVector2ToGrid(Vector3 _vector, float gridSize) { return SnapVector2ToGrid(_vector.x, _vector.y, gridSize); }
    public static Vector2 SnapVector2ToGrid(float _x, float _y, float gridSize) {
        return new Vector2(
            Mathf.Round(_x / gridSize) * gridSize,
            Mathf.Round(_y / gridSize) * gridSize);
    }
    //	public static Vector3 SnapVector3ToGrid (Vector3 _vector) { return SnapVector3ToGrid (_vector.x, _vector.y, _vector.z); }
    static public Vector2 RoundVector2To4DPs(Vector2 _vector) { return new Vector2(RoundTo4DPs(_vector.x), RoundTo4DPs(_vector.y)); }
    static public Vector3 RoundVector3To4DPs(Vector3 _vector) { return new Vector3(RoundTo4DPs(_vector.x), RoundTo4DPs(_vector.y), RoundTo4DPs(_vector.z)); }
    public static Vector3 RoundVector3ToInts(Vector3 _vector) { return SnapVector3ToGrid(_vector, 1); }
    //public static Vector3 SnapVector3ToGrid(Vector3 _vector) { return SnapVector3ToGrid(_vector, gridSize); }
    public static Vector3 SnapVector3ToGrid(Vector3 _vector, float _gridSize) {
        return new Vector3(
            Mathf.Round(_vector.x / _gridSize) * _gridSize,
            Mathf.Round(_vector.y / _gridSize) * _gridSize,
            Mathf.Round(_vector.z / _gridSize) * _gridSize);
            //_vector.z);
    }
    //*/

    //TO-DO3D: Make this support 3D?
    public static Vector2 PointOnCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t) {
        Vector2 ret = new Vector2();

        float t2 = t * t;
        float t3 = t2 * t;

        ret.x = 0.5f * ((2.0f * p1.x) +
        (-p0.x + p2.x) * t +
        (2.0f * p0.x - 5.0f * p1.x + 4 * p2.x - p3.x) * t2 +
        (-p0.x + 3.0f * p1.x - 3.0f * p2.x + p3.x) * t3);

        ret.y = 0.5f * ((2.0f * p1.y) +
        (-p0.y + p2.y) * t +
        (2.0f * p0.y - 5.0f * p1.y + 4 * p2.y - p3.y) * t2 +
        (-p0.y + 3.0f * p1.y - 3.0f * p2.y + p3.y) * t3);

        return ret;
    }



    // ----------------------------------------------------------------
    //  Vector2Int, Sides
    // ----------------------------------------------------------------
    public static Vector2Int GetDir(int side) {
        switch (side) {
            case SidesOld.L: return Vector2Int.L;
            case SidesOld.R: return Vector2Int.R;
            case SidesOld.B: return Vector2Int.B;
            case SidesOld.T: return Vector2Int.T;
            default: throw new UnityException("Whoa, " + side + " is not a valid side. Try 0, 1, 2, or 3.");
        }
    }
    public static int GetSide(Vector2Int dir) {
        if (dir == Vector2Int.L) { return SidesOld.L; }
        if (dir == Vector2Int.R) { return SidesOld.R; }
        if (dir == Vector2Int.T) { return SidesOld.T; }
        if (dir == Vector2Int.B) { return SidesOld.B; }
        Debug.LogError("Whoa, dir not convertable to side. Dir: " + dir);
        return -1; // Whoops.
    }
    //public static int GetOppositeSide (Vector2Int dir) { return GetOppositeSide(GetSide(dir)); }
    //public static int GetOppositeSide (int side) {
    //    switch (side) {
    //        case 0: return 2;
    //        case 1: return 3;
    //        case 2: return 0;
    //        case 3: return 1;
    //        default: throw new UnityException ("Whoa, " + side + " is not a valid side. Try 0, 1, 2, or 3.");
    //    }
    //}
    public static Vector2Int GetOppositeDir(int side) { return GetDir(SidesOld.GetOpposite(side)); }
    /** Just returns the original value * -1. */
    public static Vector2Int GetOppositeDir(Vector2Int dir) { return new Vector2Int(-dir.x, -dir.y); }
    ///** corner: 0 top-left; 1 top-right; 2 bottom-right; 3 bottom-left. */
    //private static Vector2Int GetCornerDir (int corner) {
    //    switch (corner) {
    //        case 0: return new Vector2Int (-1,-1);
    //        case 1: return new Vector2Int ( 1,-1);
    //        case 2: return new Vector2Int ( 1, 1);
    //        case 3: return new Vector2Int (-1, 1);
    //        default: throw new UnityException ("Whoa, " + corner + " is not a valid corner. Try 0, 1, 2, or 3.");
    //    }
    //}
    
    



    // ----------------------------------------------------------------
    //  Rects
    // ----------------------------------------------------------------
    /// This much bloat is applied to EACH side of the rect. So bloat of 2 would make rect 4 units wider/taller total.
    public static Rect BloatRect(Rect r, float bloat) {
        r.xMin -= bloat;
        r.xMax += bloat;
        r.yMin -= bloat;
        r.yMax += bloat;
        return r;
    }
    /// Returns rect trimmed to fit within bounds. Like a haircut.
    public static Rect TrimRect(Rect r, Rect bounds) {
        if (r.xMin < bounds.xMin) { r.xMin = bounds.xMin; }
        if (r.xMax > bounds.xMax) { r.xMax = bounds.xMax; }
        if (r.yMin < bounds.yMin) { r.yMin = bounds.yMin; }
        if (r.yMax > bounds.yMax) { r.yMax = bounds.yMax; }
        return r;
    }
    //public static Line3 GetRectSideLine(Rect r, int side) {
    //    switch (side) {
    //        case Sides.L: return new Line(r.xMin,r.yMin, r.xMin,r.yMax);
    //        case Sides.R: return new Line(r.xMax,r.yMin, r.xMax,r.yMax);
    //        case Sides.B: return new Line(r.xMin,r.yMin, r.xMax,r.yMin);
    //        case Sides.T: return new Line(r.xMin,r.yMax, r.xMax,r.yMax);
    //        default: Debug.LogWarning("Side not recognized: " + side); return new Line3(); // Hmm.
    //    }
    //}
	/** easing: Higher is SLOWER. */
	public static void EaseRect (ref Rect rect, Rect rectTarget, float easing) {
		rect.xMin += (rectTarget.xMin-rect.xMin) / easing;
		rect.xMax += (rectTarget.xMax-rect.xMax) / easing;
		rect.yMin += (rectTarget.yMin-rect.yMin) / easing;
		rect.yMax += (rectTarget.yMax-rect.yMax) / easing;
	}
	public static Rect LerpRect (Rect rectA, Rect rectB, float t) {
		return new Rect (Vector2.Lerp (rectA.position,rectB.position, t), Vector2.Lerp (rectA.size,rectB.size, t));
	}
	public static float InverseLerpRect (Rect rectA, Rect rectB, Rect rectC) {
		float lerpPosX = Mathf.InverseLerp (rectA.position.x, rectB.position.x, rectC.position.x);
		float lerpPosY = Mathf.InverseLerp (rectA.position.y, rectB.position.y, rectC.position.y);
		float lerpSizeX = Mathf.InverseLerp (rectA.size.x, rectB.size.x, rectC.size.x);
		float lerpSizeY = Mathf.InverseLerp (rectA.size.y, rectB.size.y, rectC.size.y);
		// Return the average of all the sides' inverse lerps!
		float lerpAverage = (lerpPosX+lerpPosY+lerpSizeX+lerpSizeY) / 4f;
		return lerpAverage;
	}

	public static bool AreRectsAboutEqual (Rect rectA, Rect rectB, float threshold=0.1f) {
		return Mathf.Abs (rectA.center.x-rectB.center.x)<threshold
			&& Mathf.Abs (rectA.center.y-rectB.center.y)<threshold
			&& Mathf.Abs (rectA.size.x-rectB.size.x)<threshold
			&& Mathf.Abs (rectA.size.y-rectB.size.y)<threshold;
	}

	public static void UpdateRectFromPoint(ref Rect rect, Vector2 point) {
		if (rect.xMin > point.x) { // LEFT
			rect.xMin = point.x;
		}
		if (rect.xMax < point.x) { // RIGHT
			rect.xMax = point.x;
		}
		if (rect.yMin > point.y) { // TOP
			rect.yMin = point.y;
		}
		if (rect.yMax < point.y) { // BOTTOM
			rect.yMax = point.y;
		}
	}
	public static Rect GetCompoundRect (Rect rectA, Rect rectB) {
		// FIRST, check if either of these rectangles are total 0's. If one IS, we want to NOT include it in the return value, so simply return the OTHER rectangle. So we don't include the origin (0,0) accidentally.
		if (rectA == Rect.zero) {
			return rectB;
		}
		if (rectB == Rect.zero) {
			return rectA;
		}
		// Otherwise, make a compound rectangle of the two :)
		Rect returnRect = new Rect (rectA);
		UpdateRectFromPoint (ref returnRect, rectB.max);
		UpdateRectFromPoint (ref returnRect, rectB.min);
		return returnRect;
    }
    /** We can only rotate a Rect by 90, 180, or 270 with this function. (Rects don't rotate in the first place.) */
    public static Rect GetRotatedRect(Rect rect, float degrees) {
        Vector2 center = GetRotatedVector3Deg(rect.center, degrees);
        Vector2 size = rect.size;
        if (degrees < 0) { degrees += 360; } // keep it positive, Jane.
        if (degrees == 180) { } // 180 means leave w/h the same.
        else if (degrees == 90 || degrees == 270) { size = new Vector2(rect.height, rect.width); }
        else { Debug.LogWarning("Heya! We're trying to rotate a rect by NOT 90 degree increment. Degrees: " + degrees); }
        return new Rect(center - size * 0.5f, size);
    }
    /** Need to squeeze a big-ass rect into a smaller one (or vice-versa!), and wanna know how to scale it? Use this function. */
    public static float GetMinRectScale(Rect rectToScale, Rect rectToFitInto) {
        return Mathf.Min(rectToFitInto.width / rectToScale.width, rectToFitInto.height / rectToScale.height);
    }

    /** 0 top, 1 right, 2 bottom, 3 left. E.g. If the second point is mostly to the RIGHT of the first, this'll return 1. */
    public static int GetSideRectIsOn (Rect rectA, Rect rectB) {
        // Because rooms aren't always perfectly in line, determine WHICH direction they're more different by. Use that.
        // Whichever value of these is the GREATEST, that's the side rectB is on.
        float diffL = rectA.xMin - rectB.xMax;
        float diffR = rectB.xMin - rectA.xMax;
        float diffB = rectA.yMin - rectB.yMax;
        float diffT = rectB.yMin - rectA.yMax;
        // Sort 'em!
        float[] diffs = { diffL, diffR, diffB, diffT };
        System.Array.Sort (diffs);
        // WHICH is the LARGEST value??
        float largestValue = diffs [diffs.Length - 1];
        if (largestValue == diffL) { return SidesOld.L; }
        if (largestValue == diffR) { return SidesOld.R; }
        if (largestValue == diffB) { return SidesOld.B; }
        if (largestValue == diffT) { return SidesOld.T; }
        return -1; // impossibru!!
    }
    public static int GetSidePointIsOn (Rect rect, Vector2 point) {
        // Because rooms aren't always perfectly in line, determine WHICH direction they're more different by. Use that.
        // Whichever value of these is the GREATEST, that's the side rectB is on.
        float diffL = rect.xMin - point.x;
        float diffR = point.x - rect.xMax;
        float diffB = rect.yMin - point.y;
        float diffT = point.y - rect.yMax;
        // Sort 'em!
        float[] diffs = { diffL, diffR, diffB, diffT };
        System.Array.Sort (diffs);
        // WHICH is the LARGEST value??
        float largestValue = diffs [diffs.Length - 1];
        if (largestValue == diffL) { return SidesOld.L; }
        if (largestValue == diffR) { return SidesOld.R; }
        if (largestValue == diffB) { return SidesOld.B; }
        if (largestValue == diffT) { return SidesOld.T; }
        return -1; // impossibru!!
    }

    /** Rounds the x,y, and width,height of a rect to multiples of 4. So its center will DEFINITELY be even values. */
    public static void RoundRectValuesToEvenInts(ref Rect rect) {
        rect = new Rect(RoundToFarthestMultipleOf4(rect.x), RoundToFarthestMultipleOf4(rect.y), RoundToFarthestMultipleOf4(rect.width), RoundToFarthestMultipleOf4(rect.height));
    }
    /** Makes negative values MORE negative and positive values MORE positive. e.g. -2.5 will return -4, and 6.5 will return 8. */
    public static int RoundToFarthestMultipleOf4(float value) {
        if (value < 0) {
            return Mathf.FloorToInt((Mathf.FloorToInt(value) / 4f)) * 4;
        }
        return Mathf.CeilToInt((Mathf.CeilToInt(value) / 4f)) * 4;
    }
    //  /** Returns a rectangle at LEAST as big as our standard dimensions (1024x768).
    //sourceCenterPos: The center of the 1024x768 rectangle. So we can use this function for both locally and globally positioned rectangles. */
    //  public static Rect GetMinScreenRectangle(Rect sourceRect, Vector2 sourceCenterPos) {
    //      return GetCompoundRect(sourceRect, new Rect(sourceCenterPos - GameVisualProperties.OriginalScreenSize * 0.5f, GameVisualProperties.OriginalScreenSize));
    //  }

    public static bool IsInsideRectangle(float x, float z, float xCenter, float zCenter, float width, float height, float rotationDegrees) {
        // Step 1: Translate point so that rectangle's center is at the origin
        float xTranslated = x - xCenter;
        float zTranslated = z - zCenter;

        // Step 2: Rotate point by negative of rectangle's rotation angle
        float theta = -rotationDegrees * (float)Mathf.PI / 180.0f; // Convert to radians
        float xRotated = xTranslated * (float)Mathf.Cos(theta) - zTranslated * (float)Mathf.Sin(theta);
        float zRotated = xTranslated * (float)Mathf.Sin(theta) + zTranslated * (float)Mathf.Cos(theta);

        // Step 3: Check if point is inside the unrotated rectangle
        return Mathf.Abs(xRotated) <= width / 2.0f && Mathf.Abs(zRotated) <= height / 2.0f;
    }



    // ----------------------------------------------------------------
    //  Ease Functions
    // ----------------------------------------------------------------
    /** t: current time
	 * Default all values to going from 0 to 1.
	 */
    static public float EaseInQuad(float t) {
        return t * t * t;
    }
    /** t: current time
	 * Default all values to going from 0 to 1.
	 */
    static public float EaseInOutQuadInverse (float t) {
		t *= 2f;
		if (t < 1f) return Mathf.Pow (t, 1/3f) * 0.5f;
		t -= 2f;
		t *= -1f;
		return 1f - Mathf.Pow (t, 1/3f) * 0.5f;
	}
	
	/** t: current time
	 * Default all values to going from 0 to 1.
	 */
	static public float EaseInOutQuad (float t) {
		//		return EaseInOutQuad (t, 0,1,1);
		t *= 2f;
		if (t < 1f) return t*t*t * 0.5f;
		t -= 2f;
		return 1f + t*t*t * 0.5f;
	}
	/** t: current time
	 *  b: start value
	 *  c: change in value
	 *  d: duration
	 * */
	static public float EaseInOutQuad (float t, float b, float c, float d) {
		t /= d/2f;
		if (t < 1f) return c/2f*t*t*t + b;
		t -= 2f;
		return -c/2f * (t*t*t - 2f) + b;
	}
	
	/** t: current time
	 * Default all values to going from 0 to 1.
	 */
	static public float EaseInOutQuart (float t) {
		return EaseInOutQuart (t, 0,1,1);
	}
	/** t: current time
	 *  b: start value
	 *  c: change in value
	 *  d: duration
	 * */
	static public float EaseInOutQuart (float t, float b, float c, float d) {
		t /= d/2f;
		if (t < 1f) return c/2f*t*t*t*t + b;
		t -= 2f;
		return -c/2f * (t*t*t*t - 2f) + b;
	}



}




