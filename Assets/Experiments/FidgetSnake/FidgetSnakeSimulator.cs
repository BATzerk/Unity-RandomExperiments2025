using TMPro;
using UnityEngine;


namespace FidgetSnakeNamespace {
    public class FidgetSnakeSimulator : MonoBehaviour {
        // Parameters
        //public static readonly float[] Angles = { -90f, -45f, 0f, 45f, 90f };
        public static readonly float[] AngleOptions = { -90f, 0f, 90f };
        public const int NumSegments = 8;
        // Components and Stuff
        [SerializeField] private TextMeshProUGUI t_currShapeIndex;
        private SnakePathFinder pathFinder;
        public SnakeVisual snakeVisual;
        private int currShapeIndex = 0;


        // Getters (Public)
        public static Quaternion GetRotation(int segmentIndex, int angleByte) {
            bool xPlane = segmentIndex % 2 == 0;
            float angle = AngleOptions[angleByte];
            return Quaternion.AngleAxis(angle, xPlane ? Vector3.right : Vector3.up);
        }
        public static Vector3[] GetPositionsFromAngles(byte[] angles) {
            Vector3[] positions = new Vector3[angles.Length + 1];
            positions[0] = Vector3.zero;
            Vector3 dir = Vector3.forward;

            for (int i=0; i<angles.Length; i++) {
                Quaternion rot = GetRotation(i, angles[i]);
                dir = rot * Vector3.forward;
                positions[i + 1] = positions[i] + dir;
            }

            return positions;
        }
        // Getters (Private)
        private int NumShapes => pathFinder.shapes.Count;



        // ----------------------------------------------------------------
        //  Start
        // ----------------------------------------------------------------
        void Start() {
            pathFinder = new SnakePathFinder();
            SetCurrShapeIndex(0);
        }



        // ----------------------------------------------------------------
        //  SetCurrShapeIndex
        // ----------------------------------------------------------------
        private void ChangeCurrShapeIndex(int delta) { SetCurrShapeIndex(currShapeIndex + delta); }
        private void SetCurrShapeIndex(int newIndex) {
            currShapeIndex = Mathf.Clamp(newIndex, 0, NumShapes-1);
            string str = $"currShapeIndex: {currShapeIndex} / {NumShapes - 1}";
            str += "\n";
            for (int i=0; i<pathFinder.shapes[currShapeIndex].angles.Length; i++) {
                str += $"{pathFinder.shapes[currShapeIndex].angles[i]} ";
            }
            t_currShapeIndex.text = str;
            // Update the visual representation of the snake
            snakeVisual.UpdateVisuals(pathFinder.shapes[currShapeIndex]);
        }



        // ----------------------------------------------------------------
        //  Update
        // ----------------------------------------------------------------
        void Update() {
            // Keyboard input
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                ChangeCurrShapeIndex(-1);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                ChangeCurrShapeIndex(1);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                ChangeCurrShapeIndex(-100);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                ChangeCurrShapeIndex(100);

            // Mouse-wheel input
            float scroll = Input.mouseScrollDelta.y;
            if (scroll != 0) {
                ChangeCurrShapeIndex((int)scroll);
            }
        }




    }
}