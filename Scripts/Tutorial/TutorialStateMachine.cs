using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TutorialStateMachine : StateMachine
{
    [SerializeField] private List<GameObject> _gameObjectsToHide;
    [SerializeField] private List<GameObject> _gameObjectsToSkipAtReturn;
    [SerializeField] private string _tutorialId;

    private bool _tutorialPassed => ES3.Load(_tutorialId, false);

    private void OnEnable()
    {
        if(_tutorialPassed)
            enabled = false;
    }

    private void OnDisable()
    {
        ShowObjects();
    }

    protected override void OnStart()
    {
        if (_tutorialPassed)
        {
            ShowObjects();
            enabled = false;
            return;
        }

        foreach (var objectToHide in _gameObjectsToHide)
        {
            objectToHide.SetActive(false);
        }
    }

    private void ShowObjects()
    {
        var objectsToShow = _gameObjectsToHide.Except(_gameObjectsToSkipAtReturn).ToList();
        foreach (var objectToShow in objectsToShow)
        {
            if(objectToShow == null)
                continue;
            objectToShow.SetActive(true);
        }
    }
    
    public void EndTutorial()
    {
        ES3.Save(_tutorialId, true);
        gameObject.SetActive(false);
    }
    
}
