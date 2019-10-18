using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Helper {
    /// <summary>
    /// Finds a Gameobject with the tag in a parent gameobject
    /// </summary>
    /// <param name="parent">Parent to search for tag</param>
    /// <param name="tag">Tag to search for</param>
    /// <returns>Child object with the tag</returns>
    public static GameObject FindChildWithTag(this GameObject parent, string tag){
        foreach (Transform child in parent.transform) {  //search parent's childs
            if (child.CompareTag(tag)) {
                return child.gameObject;    //when found    
            } else if (child.childCount > 0) {    //if not found, we search the childs children
                GameObject temp = child.gameObject.FindChildWithTag(tag);
                if (temp) {
                    return temp;    //if childs children have it
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the component T in the child with tag
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="parent">Parent object</param>
    /// <param name="tag">Tag to search for</param>
    /// <returns>Component in a child of parent</returns>
    public static T FindComponentOfChildWithTag<T>(this GameObject parent, string tag) where T : Component {
        GameObject childWithTag = parent.FindChildWithTag(tag);
        if (childWithTag) {
            return childWithTag.GetComponent<T>();
        }
        return default(T);
    }
}
