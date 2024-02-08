using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.Events;

public class LevelSystem : MonoBehaviour
{
    private const string SaveId = "PlayerLevel";
    private int _currentLevel = int.MinValue;

    public UnityAction OnLevelUp;
    
    public int CurrentLevel
    {
        get
        {
            if (_currentLevel == int.MinValue)
                _currentLevel = ES3.Load(SaveId, 1);
            return _currentLevel;
        }
        private set
        {
            _currentLevel = value;
            ES3.Save(SaveId, _currentLevel);
        }
    }

    [Button("LevelUp")]
    private void LevelUp()
    {
        LevelUp(3);
    }
    
    public void LevelUp(int targetLevel)
    {
        if (targetLevel < CurrentLevel) return;
        CurrentLevel = targetLevel;
        OnLevelUp?.Invoke();
    }
}

