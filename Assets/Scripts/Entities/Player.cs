using Assets.Scripts.Signals;
using NTC.Pool;
using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IMovable, IAttackable
{
    public new Rigidbody2D rigidbody { get; private set; }
    public bool Thrusting { set => _thrusting = value; }
    public float TurnDirection { set => _turnDirection = value; }


    private float _thrustSpeed;
    private bool _thrusting;
    private float _turnDirection = 0f;

    private float _rotationSpeed;
    private float _respawnInvulnerability;

    private bool _screenWrapping;
    private Bounds screenBounds;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(GameConfig gameConfig, SignalBus signalBus)
    {
        _respawnInvulnerability = gameConfig.RespawnInvulnerability;
        _rotationSpeed = gameConfig.RotationSpeed;
        _thrustSpeed = gameConfig.ThrustSpeed;
        _screenWrapping = gameConfig.ScreenWrapping;
        _signalBus = signalBus;
        _signalBus.Subscribe<RespawnSignal>(Respawn);
    }


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");
        for (int i = 0; i < boundaries.Length; i++) {
            boundaries[i].SetActive(!_screenWrapping);
        }

        screenBounds = new Bounds();
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(Vector3.zero));
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)));
    }

    private void OnEnable()
    {
        TurnOffCollisions();
        Invoke(nameof(TurnOnCollisions), _respawnInvulnerability);
    }

    private void FixedUpdate()
    {
        if (_thrusting)
            rigidbody.AddForce(transform.up * _thrustSpeed);

        if (_turnDirection != 0f)
            rigidbody.AddTorque(_rotationSpeed * _turnDirection);

        if (_screenWrapping)
            ScreenWrap();
    }

    private void ScreenWrap()
    {
        if (rigidbody.position.x > screenBounds.max.x + 0.5f) {
            rigidbody.position = new Vector2(screenBounds.min.x - 0.5f, rigidbody.position.y);
        }
        else if (rigidbody.position.x < screenBounds.min.x - 0.5f) {
            rigidbody.position = new Vector2(screenBounds.max.x + 0.5f, rigidbody.position.y);
        }
        else if (rigidbody.position.y > screenBounds.max.y + 0.5f) {
            rigidbody.position = new Vector2(rigidbody.position.x, screenBounds.min.y - 0.5f);
        }
        else if (rigidbody.position.y < screenBounds.min.y - 0.5f) {
            rigidbody.position = new Vector2(rigidbody.position.x, screenBounds.max.y + 0.5f);
        }
    }
    private void Shoot()
    {
        if(!gameObject.activeSelf) 
            return;
        _signalBus.Fire(new PlayerAttackSignal() { Position = transform.position, Rotation = transform.rotation, Direction = transform.up });
    }

    private void TurnOffCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
    }

    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = 0f;
            gameObject.SetActive(false);
            PlayerDeathSignal playerDeathSignal = new PlayerDeathSignal()
            {
                GameObject = gameObject
            };
            _signalBus.Fire(playerDeathSignal);
        }
    }
    private void Respawn()
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.SetActive(true);
    }
    public void Attack() => Shoot();
}
