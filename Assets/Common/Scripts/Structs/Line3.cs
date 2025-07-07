using System.Collections;
using UnityEngine;

public struct Line3 {
    // Properties
	public Vector3 start,end;

    // Getters
    //public float GetAngleDeg () {
    //    return MathUtils.GetAngleDeg(start-end);
    //}
    public Line3 RotateZ (float radians) {
        return new Line3(MathUtils.GetRotatedVector2Rad(start, radians), MathUtils.GetRotatedVector2Rad(end, radians));
    }
    public Vector3 LocToPos(float loc) {
        return Vector3.LerpUnclamped(start,end, loc);
    }
    public float PosToLoc(Vector3 pos) {
        float distFromStart = Vector3.Distance(pos, start);
        float distFromEnd = Vector3.Distance(pos, end);
        // Normalize the distances to be between 0 and 1.
        float totalDist = distFromStart + distFromEnd;
        distFromStart /= totalDist;
        distFromEnd /= totalDist;
        // Correct for strange float-point errors. "Why is this three lines? Make it one." For reasons unknown, as one line, passing in certain values (e.g. 0.45) actually returns a messed up value (e.g. 0.449).
        distFromStart *= 10000f;
        distFromStart = (int)distFromStart;
        distFromStart /= 10000f;
        return distFromStart; // The distance from the START is what we care about with loc.
    }
    
    
    // Constructor
	public Line3 (Vector3 _start, Vector3 _end) {
		start = _start;
		end = _end;
	}
	public Line3 (float startX,float startY,float startZ, float endX,float endY,float endZ) {
		start = new Vector3(startX, startY, startZ);
		end = new Vector3(endX, endY, endZ);
	}

}



public struct Line2 {
    // Properties
    public Vector2 start, end;

    // Getters
    public float GetAngleDeg() {
        return MathUtils.GetAngleDeg(start-end);
    }
    public Line2 Rotate(float radians) {
        return new Line2(MathUtils.GetRotatedVector2Rad(start, radians), MathUtils.GetRotatedVector2Rad(end, radians));
    }
    public Vector2 LocToPos(float loc) {
        return Vector2.LerpUnclamped(start, end, loc);
    }
    public float PosToLoc(Vector2 pos) {
        float distFromStart = Vector2.Distance(pos, start);
        float distFromEnd = Vector2.Distance(pos, end);
        // Normalize the distances to be between 0 and 1.
        float totalDist = distFromStart + distFromEnd;
        distFromStart /= totalDist;
        distFromEnd /= totalDist;
        // Correct for strange float-point errors. "Why is this three lines? Make it one." For reasons unknown, as one line, passing in certain values (e.g. 0.45) actually returns a messed up value (e.g. 0.449).
        distFromStart *= 10000f;
        distFromStart = (int)distFromStart;
        distFromStart /= 10000f;
        return distFromStart; // The distance from the START is what we care about with loc.
    }


    // Constructor
    public Line2(Vector2 _start, Vector2 _end) {
        start = _start;
        end = _end;
    }
    public Line2(float startX, float startY, float endX, float endY) {
        start = new Vector2(startX, startY);
        end = new Vector2(endX, endY);
    }

}
