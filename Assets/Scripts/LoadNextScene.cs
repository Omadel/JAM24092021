using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour {
    [SerializeField] private bool autoLoad = false;

    private void Start() {
        if(autoLoad) {
            DoLoadNextScene();
        }
    }

    public void DoLoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
