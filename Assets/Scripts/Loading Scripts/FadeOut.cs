using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour {
    public CanvasGroup cg;
    public float totalTime = 5;

    float m_perDt;
    // Start is called before the first frame update
    void Start() {
        m_perDt = 1 / totalTime;
        cg.alpha = 0;
    }

    // Update is called once per frame
    void Update() {
    }

    IEnumerator LoadNextLevel() {
        while(cg.alpha > 0) {
            cg.alpha -= m_perDt * Time.deltaTime;
            yield return null;
        }

        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(2);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == Layers.Player) {
            StartCoroutine(LoadNextLevel());
        }
    }
}
