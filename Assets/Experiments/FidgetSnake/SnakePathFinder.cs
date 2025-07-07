using System;
using System.Collections.Generic;
using UnityEngine;


namespace FidgetSnakeNamespace {
    public struct Shape {
        public byte[] angles; // Index into Angles array (e.g. 0 for -90, 1 for -45, etc.)

        public Shape(byte[] angles) {
            this.angles = new byte[angles.Length];
            Array.Copy(angles, this.angles, angles.Length);
        }

        // Optional: implement Equals/GetHashCode if deduping shapes
    }




    /// <summary>
    /// This ONLY does the search to find all the legal shapes.
    /// </summary>
    public class SnakePathFinder {
        // Properties
        private int totalCount = 0;
        private HashSet<Vector3> occupied = new();
        private byte[] currentAngles;
        private Vector3[] currentPath;
        public List<Shape> shapes { get; private set; } = new();

        private int NumSegments => FidgetSnakeSimulator.NumSegments;
        private float[] AngleOptions => FidgetSnakeSimulator.AngleOptions;



        public SnakePathFinder() {
            currentAngles = new byte[NumSegments];
            currentPath = new Vector3[NumSegments + 1];

            Vector3 startPos = Vector3.zero;
            Vector3 startDir = Vector3.forward;

            occupied.Add(startPos);
            currentPath[0] = startPos;

            Explore(startPos, startDir, 0);
            Debug.Log($"Total legal shapes: {totalCount}");
        }

        void Explore(Vector3 pos, Vector3 dir, int depth) {
            // Final segment?
            if (depth == NumSegments) {
                if (pos.magnitude < 0.001f) {
                    string anglesStr = string.Join(", ", currentAngles);
                    Debug.Log("pos: " + pos + "  pos.magnitude: " + pos.magnitude + "   " + anglesStr);
                    shapes.Add(new Shape(currentAngles));
                    totalCount++;
                }
                return;
            }

            for (byte i=0; i<AngleOptions.Length; i++) {
                float angle = AngleOptions[i];
                Quaternion rotation = FidgetSnakeSimulator.GetRotation(depth, i);
                Vector3 newDir = rotation * dir;
                Vector3 nextPos = pos + newDir;

                if (depth<NumSegments-1 && occupied.Contains(nextPos)) continue;
                if (depth == NumSegments-1) {//QQQ
                    if (nextPos.magnitude < 0.001) Debug.Log("pos: " + pos + "  nextPos: " + nextPos);
                }

                currentAngles[depth] = i;
                occupied.Add(nextPos);
                currentPath[depth + 1] = nextPos;

                Explore(nextPos, newDir, depth + 1);

                occupied.Remove(nextPos);
            }
        }

        //Vector3 RotateDirection(Vector3 dir, float angle, bool xPlane) {
        //    Quaternion rot = xPlane ? Quaternion.AngleAxis(angle, Vector3.right)
        //                            : Quaternion.AngleAxis(angle, Vector3.up);
        //    return rot * dir;
        //}
    }

}