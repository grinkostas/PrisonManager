using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using NepixCore.Game.API;
using StaserSDK;
using Unity.VisualScripting;
using Zenject;
using Timer = StaserSDK.Utilities.Timer;

public class ZeusShoot : MonoBehaviour
{
    [SerializeField] private StackGrabber _stackGrabber;
    [SerializeField] private RadarDisabler _radarDisabler;
    [SerializeField] private float _radarEnableDelay;
    [Header("Lightning")] 
    [SerializeField] private GameObject _lightning;
    [SerializeField] private Vector3 _lightningSpawnDelta;
    [Header("Zeus")]
    [SerializeField] private Transform _zeusSpawnPoint;
    [SerializeField] private GameObject _zeusPrefab;
    [SerializeField] private float _zeusSpawnTime;
    [SerializeField] private Vector3 _rotateAxis;
    [Header("Delays")]
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _shootTime;
    [Header("Animator")]
    [SerializeField] private Animator _playerAnimator;

    [SerializeField, AnimatorParam(nameof(_playerAnimator))]
    private string _shootPoseTrigger;

    [SerializeField, AnimatorParam(nameof(_playerAnimator))]
    private string _shootTrigger;

    [SerializeField, AnimatorParam(nameof(_playerAnimator))]
    private string _idleTrigger;

    [Header("Camera")] 
    [SerializeField] private CinemachineVirtualCamera _shootCamera;
    [SerializeField] private Transform _cameraFollowTarget;
    [SerializeField] private float _slideTime;
    [SerializeField] private float _showTime;

    [Space]
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private float _shootSoundDelay;
    
    [Inject] private Player _player;
    [Inject] private InputHandler _inputHandler;
    [Inject] private Timer _timer;
    [Inject] private IHapticService _hapticService;
    
    
    private Vector3 _previousRotation;

    private bool _shootEnd = true;

    private void OnEnable()
    {
        Criminal.Detected += OnDetectCriminal;
    }


    private void OnDisable()
    {
        Criminal.Detected -= OnDetectCriminal;
    }

    private void OnDetectCriminal(Human human)
    {
        ShootPrepare(human);
        var zeus = SpawnZeus();

        _timer.ExecuteWithDelay(() => Shoot(zeus, human), _zeusSpawnTime + _shootDelay);
        _timer.ExecuteWithDelay(() => ShootEnd(zeus), _zeusSpawnTime + _shootDelay + _shootTime);
    }

    private GameObject SpawnZeus()
    {
        var zeus = Instantiate(_zeusPrefab, _zeusSpawnPoint);
        zeus.transform.localScale = Vector3.zero;
        zeus.transform.DOScale(Vector3.one, _zeusSpawnTime);
        var finalRotation = zeus.transform.localRotation.eulerAngles + _rotateAxis * 360;
        zeus.transform.DOLocalRotate(finalRotation, _zeusSpawnTime);
        return zeus;
    }

    private void ShootPrepare(Human human)
    {
        _shootEnd = false;
        StartCoroutine(LookAt(human));
        _radarDisabler.Disable(this);
        _stackGrabber.enabled = false;
        _inputHandler.DisableHandle(this);
        
        _playerAnimator.SetTrigger(_shootPoseTrigger);
        _player.Body.LookAt(human.transform);
        var rotationAngles = _player.Body.transform.rotation.eulerAngles;
        rotationAngles.x = 0;
        rotationAngles.z = 0;
        _player.Body.rotation = Quaternion.Euler(rotationAngles);
        
        _cameraFollowTarget.localPosition = Vector3.zero;
        
        _shootCamera.gameObject.SetActive(true);
        _timer.ExecuteWithDelay(() => _shootCamera.gameObject.SetActive(false), _showTime);
        Vector3 destination = human.transform.position;
        destination.y = _cameraFollowTarget.transform.position.y;
        _cameraFollowTarget.transform.position = destination;
        PlaySound();

    }

    private void PlaySound()
    {
        _timer.ExecuteWithDelay(() =>
            {
                if (_shootSound != null && _shootSound.enabled)
                    _shootSound.Play();
            }
            , _shootSoundDelay);
    }

    private IEnumerator LookAt(Human human)
    {
        while (_shootEnd == false)
        {
            _player.Body.LookAt(human.transform);
            yield return null;
        }
    }

    private void Shoot(GameObject zeus, Human human)
    {
        _playerAnimator.SetTrigger(_shootTrigger);
        SpawnLightning(zeus, human);
        _hapticService.Selection();
    }
    
    private void ShootEnd(GameObject zeus)
    {
        _shootEnd = true;
        _playerAnimator.SetTrigger(_idleTrigger);
        zeus.transform.DOScale(Vector3.zero, _zeusSpawnTime);
        _inputHandler.EnableHandle(this);
        Destroy(zeus, _zeusSpawnTime*1.5f);
        _inputHandler.EnableHandle(this);
        
        _stackGrabber.enabled = true;
        _timer.ExecuteWithDelay(()=>_radarDisabler.Enable(this), _radarEnableDelay);
    }

    private void SpawnLightning(GameObject zeus, Human target)
    {
        var lightning = Instantiate(_lightning, zeus.transform.position, Quaternion.identity);
        lightning.transform.LookAt(target.transform);
        Vector3 destination = target.transform.position + _lightningSpawnDelta;
        lightning.transform.DOMove(destination, 0.1f);
    }
}
