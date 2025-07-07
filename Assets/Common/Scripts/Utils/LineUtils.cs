using UnityEngine;
using System.Collections;

public static class LineUtils {
	/*
	// assume Coord has members x(), y() and z() and supports arithmetic operations
	// that is Coord u + Coord v = u.x() + v.x(), u.y() + v.y(), u.z() + v.z()

	inline Point
dot(const Vector3 u, const Vector3 v) 
{
return u.x* v.x + u.y* v.y + u.z* v.z;
}

inline Point
float norm2(Vector3 v)
{
	return v.x * v.x + v.y * v.y + v.z * v.z;
}

inline Point
norm( const Vector3& v ) 
{
	return sqrt(norm2(v));
}

inline
Vector3
cross( const Vector3& b, const Vector3& c) // cross product
{
	return Vector3(b.y() * c.z() - c.y() * b.z(), b.z() * c.x() - c.z() * b.x(), b.x() *  c.y() - c.x() * b.y());
}

bool
intersection(const Line& a, const Line& b, Vector3& ip)
// http://mathworld.wolfram.com/Line-LineIntersection.html
// in 3d; will also work in 2d if z components are 0
{
	Vector3 da = a.second - a.first;
	Vector3 db = b.second - b.first;
	Vector3 dc = b.first - a.first;

	if (dot(dc, cross(da, db)) != 0.0) // lines are not coplanar
		return false;

	Point s = dot(cross(dc, db), cross(da, db)) / norm2(cross(da, db));
	if (s >= 0.0 && s <= 1.0) {
		ip = a.first + da * Vector3(s, s, s);
		return true;
	}

	return false;
}
*/


	// ================================================================
	//	Bools
	// ================================================================
	static public bool Are3PointsInALine(Vector3 point1, Vector3 point2, Vector3 point3, float threshold = 1) {
		float distance12 = Vector3.Distance(point1, point2);
		float distance23 = Vector3.Distance(point2, point3);
		float distance13 = Vector3.Distance(point1, point3);
		return distance12+distance23-threshold <= distance13; // -threshold for a little wiggle room, in case the values aren't *perfect*.
															  //		// Is the area of the triangle these form basically 0?
															  //		float area = point1.x * (point2.y-point3.y) + point2.x * (point3.y-point1.y) + point3.x * (point1.y-point2.y); // NOTE: technically, it's also /2 for actual area, but we don't need to do that.
															  //		return area < 1; // 1 for a little wiggle room, in case the values aren't *perfect*.
	}


	// ----------------------------------------------------------------
	//  Basic Getters
	// ----------------------------------------------------------------
	static public Vector3 ForwardBetweenPoints(Vector3 startPos, Vector3 endPos) {
		return (endPos - startPos).normalized;
	}
	//static public Vector3 EulerAnglesBetweenPoints(Vector3 startPos, Vector3 endPos) {
	//	Vector3 forward = (endPos - startPos).normalized;
	//	return Quaternion.LookRotation(forward).eulerAngles;
	//}
	static public float GetAngle_Degrees(Vector2 pointA, Vector2 pointB) {
		return Mathf.Rad2Deg * Mathf.Atan2(pointB.y-pointA.y, pointB.x-pointA.x);
	}
	public static float GetLength(Vector3 lineStart, Vector3 lineEnd) {
		return Vector3.Distance(lineStart, lineEnd);
	}

	///** Returns Rect's line in CLOCKwise direction (e.g. Side T would return from TL to TR of Rect). */
	//public static Line3 GetLineCW(Rect rect, int side) {
	//	switch (side) {
	//		case Sides.T: return new Line(rect.xMin, rect.yMax, rect.xMax, rect.yMax);
	//		case Sides.R: return new Line(rect.xMax, rect.yMax, rect.xMax, rect.yMin);
	//		case Sides.B: return new Line(rect.xMax, rect.yMin, rect.xMin, rect.yMin);
	//		case Sides.L: return new Line(rect.xMin, rect.yMin, rect.xMin, rect.yMax);
	//		default: Debug.LogError("Whoa, " + side + " is not a valid side. Try 0-3."); return new Line3();
	//	}
	//}
	///** Returns Rect's line in COUNTER-clockwise direction (e.g. Side T would return from TR to TL of Rect). */
	//public static Line3 GetLineCCW(Rect rect, int side) {
	//	switch (side) {
	//		case Sides.T: return new Line(rect.xMax, rect.yMax, rect.xMin, rect.yMax);
	//		case Sides.R: return new Line(rect.xMax, rect.yMin, rect.xMax, rect.yMax);
	//		case Sides.B: return new Line(rect.xMin, rect.yMin, rect.xMax, rect.yMin);
	//		case Sides.L: return new Line(rect.xMin, rect.yMax, rect.xMin, rect.yMin);
	//		default: Debug.LogError("Whoa, " + side + " is not a valid side. Try 0-3."); return new Line3();
	//	}
	//}
	public static float DistancePointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
		// Vector along the line
		Vector3 lineVec = lineEnd - lineStart;
		// Calculate the position of the orthogonal projection of the point onto the line
		float t = Vector3.Dot(point - lineStart, lineVec) / Vector3.Dot(lineVec, lineVec);
		// Limit t to the range [0,1] to restrict the projected point to the finite line segment
		t = Mathf.Clamp01(t);
		// The closest point on the line to the input point
		Vector3 projection = lineStart + t * lineVec;
		// Return the distance between the point and its projection
		return Vector3.Distance(point, projection);
	}





	// ================================================================
	//	Pos and Loc
	// ================================================================
	public static Vector3 GetCenterPos(Line3 line) { return GetCenterPos(line.start, line.end); }
	public static Vector3 GetCenterPos(Vector3 lineStart, Vector3 lineEnd) {
		return Vector3.Lerp(lineStart, lineEnd, 0.5f);
	}

	public static float PosToLoc(Line3 line, Vector3 pos) {
		return line.PosToLoc(pos);
	}
	static public float GetLocFromPos(Line3 line, Vector3 pos) {
		return GetLocFromPos(line.start, line.end, pos);
	}
	static public float GetLocFromPos(Vector3 lineStart, Vector3 lineEnd, Vector3 pos) {
		float distFromStart = Vector3.Distance(pos, lineStart);
		float distFromEnd = Vector3.Distance(pos, lineEnd);
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

	public static Vector3 GetPosFromLoc(Line3 line, float loc) {
		return GetPosFromLoc(line.start, line.end, loc);
	}
	public static Vector3 GetPosFromLoc(Vector3 lineStart, Vector3 lineEnd, float loc) {
		return Vector3.LerpUnclamped(lineStart, lineEnd, loc);
		//float posX, posY;
		//posX = lineStart.x + (lineEnd.x - lineStart.x) * loc;
		//posY = lineStart.y + (lineEnd.y - lineStart.y) * loc;
		//return new Vector3(posX, posY);
	}




	// ----------------------------------------------------------------
	//  Intersections!
	// ----------------------------------------------------------------
	static public bool GetIntersectionLineToLine(out Vector3 intPos, Line3 lineA, Line3 lineB) {
		return GetIntersectionLineToLine(out intPos, lineA.start, lineA.end, lineB.start, lineB.end);
	}
	static public bool GetIntersectionLineToLine(out Vector3 intPos, Vector3 lineAStart, Vector3 lineAEnd, Vector3 lineBStart, Vector3 lineBEnd) {
		return GetIntersectionLineToLine(out intPos,
			lineAStart.x, lineAStart.y, lineAStart.z,
			lineAEnd.x, lineAEnd.y, lineAEnd.z,
			lineBStart.x, lineBStart.y, lineBStart.z,
			lineBEnd.x, lineBEnd.y, lineBEnd.z);
	}
	static public bool GetIntersectionLineToLine(out Vector3 intPos,
		float x1, float y1, float z1,
		float x2, float y2, float z2,
		float x3, float y3, float z3,
		float x4, float y4, float z4
	) {
		// FIRST, check if these lines end/begin at each other. If so, return THAT point.
		const float samePosThresh = 0.1f; // if any of the poses of these lines are closer than this to each other, SNAP the intersection to EXACTLY that point!
		if ((Mathf.Abs(x1-x3)<samePosThresh && Mathf.Abs(y1-y3)<samePosThresh && Mathf.Abs(z1-z3)<samePosThresh) // A's START is B's START...
		 || (Mathf.Abs(x1-x4)<samePosThresh && Mathf.Abs(y1-y4)<samePosThresh && Mathf.Abs(z1-z4)<samePosThresh) // A's START is B's END...
			) {
			intPos = new Vector3(x1, y1, z1); // NOTE: Maybe try rounding these values? Sometimes, they're not perfect...
			return true;
		}
		if ((Mathf.Abs(x2-x3)<samePosThresh && Mathf.Abs(y2-y3)<samePosThresh && Mathf.Abs(z2-z3)<samePosThresh) // A's END is B's START...
		 || (Mathf.Abs(x2-x4)<samePosThresh && Mathf.Abs(y2-y4)<samePosThresh && Mathf.Abs(z2-z4)<samePosThresh) // A's END is B's END...
			) {
			intPos = new Vector3(x2, y2, z2);
			return true;
		}

		// TO-DO3D: This, baby.

		double x, y; // intersection location
		double a1, a2, b1, b2, c1, c2;
		double r1, r2, r3, r4;
		double denom, offset, num;

		// Compute a1, b1, c1, where line joining points 1 and 2
		// is "a1 x + b1 y + c1 = 0".
		a1 = y2 - y1;
		b1 = x1 - x2;
		c1 = (x2 * y1) - (x1 * y2);

		// Compute r3 and r4.
		r3 = ((a1 * x3) + (b1 * y3) + c1);
		r4 = ((a1 * x4) + (b1 * y4) + c1);

		// Check signs of r3 and r4. If both point 3 and point 4 lie on
		// same side of line 1, the line segments do not intersect.
		if ((r3 != 0) && (r4 != 0) && MathUtils.IsSameSign(r3, r4)) {
			intPos = MathUtils.Vector3NaN;
			return false;
		}

		// Compute a2, b2, c2
		a2 = y4 - y3;
		b2 = x3 - x4;
		c2 = (x4 * y3) - (x3 * y4);

		// Compute r1 and r2
		r1 = (a2 * x1) + (b2 * y1) + c2;
		r2 = (a2 * x2) + (b2 * y2) + c2;

		// Check signs of r1 and r2. If both point 1 and point 2 lie
		// on same side of second line segment, the line segments do
		// not intersect.
		if ((r1 != 0) && (r2 != 0) && (MathUtils.IsSameSign(r1, r2))) {
			intPos = MathUtils.Vector2NaN;
			return false;
		}

		//Line segments intersect: compute intersection point.
		denom = (a1 * b2) - (a2 * b1);

		if (denom == 0) {
			intPos = MathUtils.Vector2NaN;
			return false; // colinear
		}

		if (denom < 0) {
			offset = -denom / 2;
		}
		else {
			offset = denom / 2;
		}

		// The denom/2 is to get rounding instead of truncating. It
		// is added or subtracted to the numerator, depending upon the
		// sign of the numerator.
		num = (b1 * c2) - (b2 * c1);
		if (num < 0) {
			x = (num - offset) / denom;
		}
		else {
			x = (num + offset) / denom;
		}

		num = (a2 * c1) - (a1 * c2);
		if (num < 0) {
			y = (num - offset) / denom;
		}
		else {
			y = (num + offset) / denom;
		}

		// Lines intersect!
		intPos = new Vector3(Mathf.Round((float)x), Mathf.Round((float)y)); // Round these values... NOTE: I've found that rounding them PREVENTS bugs. No known bugs caused from rounding.
																			//intPos = new Vector2((float)x,(float)y);
		return true;
	}

	/*
	static public bool GetNonEndIntersectionLineToLine(out Vector2 intPos, Line3 lineA, Line3 lineB) {
		return GetNonEndIntersectionLineToLine(out intPos, lineA.start, lineA.end, lineB.start, lineB.end);
	}
	static public bool GetNonEndIntersectionLineToLine(out Vector2 intPos, Vector2 lineAStart, Vector2 lineAEnd, Vector2 lineBStart, Vector2 lineBEnd) {
		return GetNonEndIntersectionLineToLine(out intPos, lineAStart.x, lineAStart.y, lineAEnd.x, lineAEnd.y, lineBStart.x, lineBStart.y, lineBEnd.x, lineBEnd.y);
	}
	/** IGNORES intersections at the very tippy ends of lines! Only counts the middle area. * /
	static public bool GetNonEndIntersectionLineToLine(out Vector3 intPos, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) {
		// FIRST, check if these lines end/begin at each other. If so, say they DON'T intersect!
		const float samePosThresh = 1f; // if any of the poses of these lines are closer than this to each other, SNAP the intersection to EXACTLY that point!
		if ((Mathf.Abs(x1-x3)<samePosThresh && Mathf.Abs(y1-y3)<samePosThresh) // A's START is B's START...
		 || (Mathf.Abs(x1-x4)<samePosThresh && Mathf.Abs(y1-y4)<samePosThresh) // A's START is B's END...
			) {
			intPos = MathUtils.Vector2NaN;
			return false;
		}
		if ((Mathf.Abs(x2-x3)<samePosThresh && Mathf.Abs(y2-y3)<samePosThresh) // A's END is B's START...
		 || (Mathf.Abs(x2-x4)<samePosThresh && Mathf.Abs(y2-y4)<samePosThresh) // A's END is B's END...
		   ) {
			intPos = MathUtils.Vector2NaN;
			return false;
		}
		// Ok, we can just return whatever the usual line-intersection result is.
		return GetIntersectionLineToLine(out intPos, x1, y1, x2, y2, x3, y3, x4, y4);
	}
	*/

	public static Vector2 ClosestIntersectionCircleToLine(Vector2 circlePos, float radius, Vector2 lineStart, Vector2 lineEnd, Vector2 pointClosestTo) {
		Vector2 nanVector = new Vector2(float.NaN, float.NaN);
		Vector2 intersection1;
		Vector2 intersection2;
		int numIntersections = GetIntersectionsCircleToLine(circlePos, radius, lineStart, lineEnd, out intersection1, out intersection2);

		if (numIntersections == 1) {
			return intersection1; // one intersection
		}
		if (numIntersections == 2) {
			double dist1 = Vector2.Distance(intersection1, pointClosestTo);
			double dist2 = Vector2.Distance(intersection2, pointClosestTo);

			if (dist1 < dist2) {// && !float.IsNaN(intersection1.x)) {
				return intersection1;
			}
			else {
				return intersection2;
			}
		}

		return nanVector; // no intersections at all
	}

	// Get the points of intersection.
	public static int GetIntersectionsCircleToLine(
		Vector2 circlePos, float radius,
		Vector2 point1, Vector2 point2,
		out Vector2 intersection1, out Vector2 intersection2
	) {
		float dx, dy, A, B, C, det, t;
		float cx = circlePos.x;
		float cy = circlePos.y;

		dx = point2.x - point1.x;
		dy = point2.y - point1.y;

		A = dx * dx + dy * dy;
		B = 2 * (dx * (point1.x - cx) + dy * (point1.y - cy));
		C = (point1.x - cx) * (point1.x - cx) + (point1.y - cy) * (point1.y - cy) - radius * radius;

		det = B * B - 4 * A * C;
		if ((A <= 0.0000001) || (det < 0)) {
			// No real solutions.
			intersection1 = new Vector2(float.NaN, float.NaN);
			intersection2 = new Vector2(float.NaN, float.NaN);
			return 0;
		}
		else if (det == 0) {
			// One solution.
			t = -B / (2 * A);
			intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
			intersection2 = new Vector2(float.NaN, float.NaN);
			return 1;
		}
		else {
			// Two solutions.
			t = (float)((-B + Mathf.Sqrt(det)) / (2 * A));
			intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
			t = (float)((-B - Mathf.Sqrt(det)) / (2 * A));
			intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
			return 2;
		}
	}

	public static bool RayPlaneIntersection(Ray ray, Vector3 planePos, Vector3 planeUp, out Vector3 hitPos) {
		//A plane can be defined as:
		//a point representing how far the plane is from the world origin
		Vector3 p_0 = planePos;
		//a normal (defining the orientation of the plane), should be negative if we are firing the ray from above
		Vector3 n = -planeUp;
		//We are intrerested in calculating a point in this plane called p
		//The vector between p and p0 and the normal is always perpendicular: (p - p_0) . n = 0

		//A ray to point p can be defined as: l_0 + l * t = p, where:
		//the origin of the ray
		Vector3 l_0 = ray.origin;
		//l is the direction of the ray
		Vector3 l = ray.direction;
		//t is the length of the ray, which we can get by combining the above equations:
		//t = ((p_0 - l_0) . n) / (l . n)

		//But there's a chance that the line doesn't intersect with the plane, and we can check this by first
		//calculating the denominator and see if it's not small. 
		//We are also checking that the denominator is positive or we are looking in the opposite direction
		float denominator = Vector3.Dot(l, n);

		if (denominator > 0.00001f) {
			//The distance to the plane
			float t = Vector3.Dot(p_0 - l_0, n) / denominator;

			//Where the ray intersects with a plane
			hitPos = l_0 + l * t;
			return true;
		}
		else {
			hitPos = Vector3.zero;
			return false;
		}
	}


}




