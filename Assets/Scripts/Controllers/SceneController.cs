using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
    public static SceneController instance;

    public void SwitchScene(string scene) {
        Application.LoadLevel(scene);
    }

    protected void Awake() {
        instance = this;
    }

    protected void OnDestroy() {
        if (instance != null) {
            instance = null;
        }
    }
}
