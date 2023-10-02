using UnityEngine;
using Zenject;

public class VFXManager
{
    private SignalBus _signalBus;
    private ParticleSystem _effect;
    public VFXManager(SignalBus signalBus, ParticleSystem effect) 
    {
        _signalBus = signalBus;
        _effect = effect;
        _signalBus.Subscribe<PlayerDeathSignal>(Play);
        _signalBus.Subscribe<EnemyDestroyedSignal>(Play);
    }

    private void Play(PlayerDeathSignal args) => Play(args.GameObject.transform.position);
    private void Play(EnemyDestroyedSignal args) => Play(args.Transform?.position);

    private void Play(Vector3? position)
    {
        if(position.HasValue)
        {
            _effect.transform.position = position.Value;
            _effect.Play();
        }
    }
}