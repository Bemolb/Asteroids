using NTC.Pool;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score { get; private set; }
    public int BestScore { get => _data.BestScore; }
    public int lives { get; private set; }

    private float _respawnDelay;

    private Player _player;
    private DataContainer _data;
    private SceneLoader _sceneLoader;

    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    [Inject]
    private void Construct(GameConfig gameConfig, Player player, DataContainer dataContainer, SceneLoader sceneLoader)
    {
        _player = player;
        _respawnDelay = gameConfig.RespawnDelay;
        _data = dataContainer;
        _sceneLoader = sceneLoader;
    }
    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return)) {
            NewGame();
        }
    }

    private void NewGame()
    {
        DespawnAll();
        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        Respawn();
    }

    private void DespawnAll()
    {
        NightPool.ForEachPool(p => p.DespawnAllClones());
    }

    private void SetScore(int score)
    {
        _data.BestScore = score;
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }

    private void Respawn()
    {
        _player.transform.position = Vector3.zero;
        _player.gameObject.SetActive(true);
    }

    public void OnAsteroidDestroyed(Enemy enemy)
    {
        explosionEffect.transform.position = enemy.transform.position;
        explosionEffect.Play();
        int addScore = enemy.GetScore();
        SetScore(score + addScore);
    }

    public void OnPlayerDeath(Player player)
    {
        player.gameObject.SetActive(false);

        explosionEffect.transform.position = player.transform.position;
        explosionEffect.Play();

        SetLives(lives - 1);

        if (lives <= 0) {
            gameOverUI.SetActive(true);
        } else {
            Invoke(nameof(Respawn), _respawnDelay);
        }
    }

}
