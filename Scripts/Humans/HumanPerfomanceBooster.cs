using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using Zenject;

public class HumanPerfomanceBooster : MonoBehaviour
{
    [SerializeField] private Human _human;
    [SerializeField] private float _distanceToPlayer;

    [Inject] private InputHandler _inputHandler;
    [Inject] private Player _player;

    private void OnEnable()
    {
        _inputHandler.OnMove.AddListener(OnMove);
    }

    private void OnDisable()
    {
        _inputHandler.OnMove.RemoveListener(OnMove);
        _human.Animator.enabled = true;
    }

    private void OnMove(Vector3 input)
    {
        var playerPosition = _player.transform.position;
        var humanPosition = _human.transform.position;
        float distance = Mathf.Abs(playerPosition.x - humanPosition.x) + Mathf.Abs(playerPosition.z - humanPosition.z);
        if (distance > _distanceToPlayer)
        {
            
        }
    }


}
