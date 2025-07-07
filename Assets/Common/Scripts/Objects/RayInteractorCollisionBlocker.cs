/* DISABLED UNTIL WE IMPORT OCULUS STUFF!
using Oculus.Interaction;
using UnityEngine;

/// <summary>
/// Put me on your RayInteractor GameObjects! I'll prevent them from laser-visioning through all 3D colliders.
/// I'm a MANUAL workaround for preventing RayInteractor from laser-visioning through all 3D colliders.
/// Naively, we change the MaxRayLength of the RayInteractor (as disabling it no longer updates its ray, which WE need), so this is NOT the best long-term solution. (I'm hoping Meta will have an improvement before I run into this being a problem.)
/// </summary>
[RequireComponent(typeof(RayInteractor))]
public class RayInteractorCollisionBlocker : MonoBehaviour {
    // Components
    private RayInteractor rayInteractor;
    // Properties
    public LayerMask blockingLayers; // Assign layers that should block the ray
    private float defaultMaxRayLength; // set in Awake, and NOT DESIGNED to change (sorry).


    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    private void Awake() {
        rayInteractor = GetComponent<RayInteractor>();
        defaultMaxRayLength = rayInteractor.MaxRayLength;
    }


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    void Update() {
        // CUT SHORT RayInteractor's max dist when there's something blocking us.
        if (Physics.Raycast(rayInteractor.Ray.origin, rayInteractor.Ray.direction, out RaycastHit hit, defaultMaxRayLength, blockingLayers)) {
            rayInteractor.MaxRayLength = 0.001f; // TEST
        }
        // RE-ENABLE RayInteractor if it's not blocked by anything.
        else {
            rayInteractor.MaxRayLength = defaultMaxRayLength;
        }
    }
}

*/