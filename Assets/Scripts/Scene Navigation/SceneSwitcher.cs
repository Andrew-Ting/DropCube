using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SceneSwitcher : MonoBehaviour
{
    private enum SceneType {
        MainMenu,
        LoadLevel,
        GameplayScene,
        GameOver
    };

    [SerializeField] 
    private SceneType destinationScene;

    public Animator screenTransition;
    public void Awake() {
        Button thisButton = GetComponent<Button>();
       thisButton.onClick.AddListener(() => {
            StartCoroutine(loadScene());
       });
    }
    private IEnumerator loadScene()
    {
        screenTransition.SetTrigger("start");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(destinationScene.ToString());
        yield return false;
    }
}
