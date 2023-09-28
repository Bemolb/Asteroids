using UnityEngine.SceneManagement;

public class SceneLoader
{
    private string _previousScene = string.Empty;

    public void LoadScene(string sceneName)
    {
        _previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public void ReturnToPreviousScene()
    {
        if (!string.IsNullOrEmpty(_previousScene))
        {
            SceneManager.LoadScene(_previousScene);
        }
    }
}
