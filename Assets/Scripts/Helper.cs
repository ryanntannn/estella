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
    public static GameObject FindChildWithTag(this GameObject parent, string tag) {
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
    /// Same as the Gameobject version, but with Transform instead
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static Transform FindChildWithTag(this Transform parent, string tag) {
        foreach (Transform child in parent) {  //search parent's childs
            if (child.CompareTag(tag)) {
                return child;    //when found    
            } else if (child.childCount > 0) {    //if not found, we search the childs children
                Transform temp = child.FindChildWithTag(tag);
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

    /// <summary>
    /// Changes all the shaders of a transform
    /// </summary>
    /// <param name="T"></param>
    /// <param name="shd"></param>
    public static void ChangeShader(this Transform T, Shader shd) {
        foreach (Transform child in T) {
            Renderer rd = child.GetComponent<Renderer>();
            if (rd) {
                foreach (Material mat in rd.materials) {
                    mat.shader = shd;
                }
            }
            child.ChangeShader(shd);
        }
    }

    public static IEnumerator KillSelf(this GameObject gameObject, float ttl) {
        yield return new WaitForSeconds(ttl);
        MonoBehaviour.Destroy(gameObject);   
    }
}

public static class Layers {
    //contains all the layer numbers DO NOT CHANGE UNLESS LAYERS CHANGE TOO
    public static readonly int
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Water = 4,
        UI = 5,
        PostProcessing = 8,
        Enemy = 9,
        Player = 10,
        Terrain = 11;
}

//I got mad at key value pairs so here is my own implementation
public class KeyAndValue<TKey, TValue> {
    public List<TKey> Keys { get; set; }
    public List<TValue> Values { get; set; }

    public KeyAndValue() {
        Keys = new List<TKey>();
        Values = new List<TValue>();
    }

    public TValue SearchForValue(TKey key) {
        for (int count = 0; count <= Keys.Count - 1; count++) {
            if (key.Equals(Keys[count])) {
                return Values[count];
            }
        }
        Keys.Add(key);
        Values.Add(default(TValue));
        return Values[Values.Count - 1];
    }

    public void ForEach(Action<TValue> a) {
        for (int count = 0; count <= Keys.Count - 1; count++) {
            a(Values[count]);
        }
    }

    public int LocationOfValue(TKey key) {
        for (int count = 0; count <= Keys.Count - 1; count++) {
            if (key.Equals(Keys[count])) {
                return count;
            }
        }
        Keys.Add(key);
        Values.Add(default(TValue));
        return Keys.Count - 1;
    }
}

//For aStar path finding
public class PiorityQueue<T> where T : IComparable {
    //member function
    private List<T> m_queue = new List<T>();

    //properties
    public int Count { get { return m_queue.Count; } }
    public T Head {
        get {
            if (m_queue.Count > 0) {
                return m_queue[0];
            }
            return default(T);
        }
    }

    public PiorityQueue() { } //default 

    /// <summary>
    /// Push new object in
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    public void Enqueue(T _input) {
        m_queue.Add(_input);
        m_queue.Sort();
    }

    /// <summary>
    /// Remove head of m_queue
    /// </summary>
    public T Dequeue() {
        if (m_queue.Count > 0) {
            T temp = m_queue[0];
            m_queue.RemoveAt(0);
            return temp;
        }
        return default(T);
    }
}
