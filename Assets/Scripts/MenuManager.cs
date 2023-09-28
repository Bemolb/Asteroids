using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private string _sceneName;

    private DataContainer _data;
    private SceneLoader _loader;

    [Inject]
    private void Construct(DataContainer dataContainer, SceneLoader sceneLoader)
    {
        _data = dataContainer;
        _loader = sceneLoader;
    }
    public void Start()
    {
        _scoreText.text = $"Best score {_data.BestScore}";
    }
    public void Transient()
    {
        _loader.LoadScene(_sceneName);

    }
}
