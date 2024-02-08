using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StaserSDK;
using Zenject;

public class Funicular : MonoBehaviour
{
    [SerializeField] private Transform _playerParent;
    [SerializeField] private float _moveDuration;
    [SerializeField] private Transform _movable;
    [SerializeField] private ParticleSystem _particle;

    [Inject] private Player _player;
    [Inject] private InputHandler _inputHandler;

    private bool _isMoving = false;
    
    public void Move(FunicularEnter enterPoint)
    {
        if(_isMoving)
            return;
        if (_particle != null)
        {
            Instantiate(_particle, _player.transform.position, Quaternion.identity);
            if(_particle.isPlaying == false)
                _particle.Play();
        }
        _player.transform.SetParent(_playerParent);
        _inputHandler.DisableHandle(this);
        _isMoving = true;
        _movable.DOMove(enterPoint.FunicularDestination.position, _moveDuration)
            .OnComplete(() => OnReceiveDestination(enterPoint));
    }

    private void OnReceiveDestination(FunicularEnter enterPoint)
    {
        _inputHandler.EnableHandle(this);
        _isMoving = false;
        _player.transform.SetParent(null);
        _player.transform.position = enterPoint.PlayerExitPoint.position;

    }
}
