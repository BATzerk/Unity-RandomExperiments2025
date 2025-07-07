using UnityEngine;

/** Any GameObject with this script will destroy itself on Awake() if the current platform isn't supported. */
public class SelfDestructingPlatformSpecificGO : MonoBehaviour {
    [SerializeField] private bool isEditorOnly;
    [SerializeField] private bool isBuildOnly;
    //[SerializeField] private bool isOnPC = true;
    //[SerializeField] private bool isOnMobile;
    //[SerializeField] private bool isOnXbox;
    //[SerializeField] private bool isOnPlayStation;
    //[SerializeField] private bool isNotOnAndroid;
    //[SerializeField] private bool isNotOniOS;

    public bool IsAvailableOnCurrentPlatform() {
        if (!Application.isEditor && isEditorOnly) return false;
        if (Application.isEditor && isBuildOnly) return false;
        //if (isNotOnAndroid && PlatformManager.IsAndroid()) { return false; }
        //if (isNotOniOS && PlatformManager.IsiOS()) { return false; }
        //return isOnPC && PlatformManager.IsPC()
        //    || isOnMobile && PlatformManager.IsMobile
        //    || isOnXbox && PlatformManager.IsXbox()
        //    || isOnPlayStation && PlatformManager.IsPlayStation();
        return true;
    }

    void Awake() {
        if (!IsAvailableOnCurrentPlatform()) {
            Destroy(this.gameObject);
        }
    }

}
