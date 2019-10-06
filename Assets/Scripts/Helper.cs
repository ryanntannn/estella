using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper {
    /// <summary>
    /// <para>Finds the child with the given tag</para>
    /// <para>Returns null if not found</para>
    /// </summary>
    public static GameObject findChildWithTag(GameObject parent, string tag) {
        foreach(Transform child in parent.transform) {  //search parent's childs
            if (child.CompareTag(tag)) {
                return child.gameObject;    //when found    
            }else if(child.childCount > 0) {    //if not found, we search the childs children
                GameObject temp = findChildWithTag(child.gameObject, tag);
                if (temp){
                    return temp;    //if childs children have it
                }
            }
        }

        return null;
    }
}
