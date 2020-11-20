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
    public void PlayGame()
    {
        if (!goToScene)
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
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneName);
    }
}
