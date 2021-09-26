using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadNextScene : MonoBehaviour {
    [SerializeField] private bool autoLoad = false;
    [SerializeField] private Sprite quitGameSprite;

    private void Start() {
        if(autoLoad) {
            DoLoadNextScene();
        }

        string uuu = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
        if(string.IsNullOrEmpty(uuu)) {
            transform.GetChild(0).GetComponent<Image>().sprite = quitGameSprite;
        }
    }

    public void DoLoadNextScene() {
        string uuu = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
        if(!string.IsNullOrEmpty(uuu)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }
}
