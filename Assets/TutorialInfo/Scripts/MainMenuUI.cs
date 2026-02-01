using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneLoad = "SampleScene";

    private void Awake(){
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        var playButton = root.Q<Button>("playButton");
        var quitButton = root.Q<Button>("quitButton");

        if(playButton == null) Debug.LogError("Play button not found, check name in Builder");
        else playButton.clicked += () => SceneManager.LoadScene(sceneLoad);

        if(quitButton == null) Debug.LogError("Quit button not found, check name in Builder");
        else quitButton.clicked += Quit;
    }

    private void Quit(){
        Application.Quit();
        Debug.Log("Quit clicked");
    }
}
