using System.Collections.Generic;
using UnityEngine;

namespace FidgetSnakeNamespace {
    public class SnakeVisual : MonoBehaviour {
        // Components
        private List<Transform> segments;


        // ----------------------------------------------------------------
        //  Awake
        // ----------------------------------------------------------------
        private void Awake() {
            segments = new();
            for (int i = 0; i<FidgetSnakeSimulator.NumSegments; i++) {
                Transform segment = Instantiate(ResourcesHandler.Instance.FidgetSnakeVisualSegment).GetComponent<Transform>();
                segment.SetParent(this.transform);
                segment.localPosition = Vector3.zero;
                segment.localRotation = Quaternion.identity;
                segments.Add(segment);
            }
        }


        //public void UpdateVisuals(Shape shape) {
        //    Vector3[] positions = FidgetSnakeSimulator.GetPositionsFromAngles(shape.angles);

        //    for (int i=0; i<positions.Length-1; i++) {
        //        Transform segment = segments[i];
        //        segment.localPosition = positions[i];
        //        segment.localEulerAngles = ...
        //    }
        //}
        public void UpdateVisuals(Shape shape) {
            Vector3 pos = Vector3.zero;
            Vector3 dir = Vector3.forward;
            Quaternion rot = Quaternion.identity;

            for (int i=0; i<shape.angles.Length; i++) {
                Quaternion deltaRot = FidgetSnakeSimulator.GetRotation(i, shape.angles[i]);
                rot *= deltaRot;

                Transform segment = segments[i];
                segment.localPosition = pos;
                segment.localRotation = rot;

                dir = rot * Vector3.forward;
                pos += dir;
            }
        }


    }
}