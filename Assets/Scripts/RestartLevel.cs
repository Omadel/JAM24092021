using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour {
    public void Restart() {
        Bomber.ResetInstance();
        //Etienne.AudioManager.ResetInstance();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
