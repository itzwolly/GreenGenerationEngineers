using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
    public static Transform FindChildWithTag(GameObject pParent, string pTag) {
        Transform parentTransform = pParent.transform;
        foreach (Transform transform in parentTransform) {
            if (transform.tag == pTag) {
                return transform;
            }
        }
        return null;
    }
}
