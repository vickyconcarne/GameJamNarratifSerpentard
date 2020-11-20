using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName;
    private bool goToScene = false;
    public Animator animatorTransitionToScene;
    public string nameOfTrigger;
    public float waitTime;
    public AudioSource soundtrackPlayer;
    public void PlayGame()
    {
        if (goToScene == false)
        {
            goToScene = true;
            StartCoroutine("PlaySceneCoroutine");
        }
    }
 
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator PlaySceneCoroutine()
    {
        if (animatorTransitionToScene)
        {
            animatorTransitionToScene.SetTrigger(nameOfTrigger);
        }
        float volume = soundtrackPlayer.volume;
        while (volume > 0f)
        {
            volume -= 0.05f;
            soundtrackPlayer.volume = volume;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(waitTime);
        
        SceneManager.LoadScene(sceneName);
    }
}
