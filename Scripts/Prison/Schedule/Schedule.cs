using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;

public class Schedule : MonoBehaviour
{
    [SerializeField] private List<ScheduleItem> _scheduleItemsOrder;
    [SerializeField] private float _itemChangeDelay;

    [Inject] private DayPhaseChanger _dayPhaseChanger;
    [Inject] private Timer _timer;
    
    private int _currentItemIndex = -1;
    private bool _night = false;
    
    private Timer.TimerDelay _actionDelay;
    private Timer.TimerDelay _breakDelay;

    public UnityAction<ScheduleItem> NewScheduleAction;

    public ScheduleItem CurrentItem
    {
        get
        {
            if (_night)
                return null;
            return _currentItemIndex > 0 ? _scheduleItemsOrder[_currentItemIndex] : _scheduleItemsOrder[0];
        }
    }

    

    public float CurrentActionTime => _actionDelay?.TimeLeft ?? 0;
    public float ItemChangeDelay => _itemChangeDelay;

    public bool IsPaused => _breakDelay != null && _breakDelay.Status == TimerStatus.Active;

    private void OnEnable()
    {
        _dayPhaseChanger.PhaseChanged += OnPhaseChanged;
    }
    
    private void OnDisable()
    {
        _dayPhaseChanger.PhaseChanged -= OnPhaseChanged;
    }
    
    private void OnPhaseChanged(DayPhase dayPhase)
    {
        if (dayPhase == DayPhase.Day)
        {
            _night = false;
            ResetSchedule();
            NextItem();
        }
        else if(dayPhase == DayPhase.Night)
        {
            ChangeScheduleItem(ScheduleItem.Default);
            ResetSchedule();
        }
    }

    private void ResetSchedule()
    {
        _currentItemIndex = -1;
        if (_actionDelay != null)
        {
            _actionDelay.Kill();
            _actionDelay = null;
        }

        if (_breakDelay != null)
        {
            _breakDelay.Kill();
            _breakDelay = null;
        }  
    }

    
    private void NextItem()
    {
        _currentItemIndex++;
        if (_currentItemIndex >= _scheduleItemsOrder.Count)
            _currentItemIndex = 0;
        
        ScheduleItem item = _scheduleItemsOrder[_currentItemIndex];
        var phaseToSearch = _dayPhaseChanger.CurrentPhase.Phase;
        bool haveEnoughTime = _dayPhaseChanger.TimeToNextChange >= item.Duration + _itemChangeDelay-1.0f;

        if (phaseToSearch == DayPhase.Night || haveEnoughTime == false)
        {
            _night = true;
            return;
        }
        
        ChangeScheduleItem(item);
    }

    private void ChangeScheduleItem(ScheduleItem item)
    {
        if (_actionDelay != null)
            _actionDelay.Kill();
        _actionDelay = _timer.ExecuteWithDelay(Break, item.Duration, TimeScale.Scaled);
        NewScheduleAction?.Invoke(item);
    }

    private void Break()
    {
        float breakDelay = _itemChangeDelay;
        float additiveDelay = GetNextPhaseDelay() + GetCurrentPhaseDelay();
        
        if (additiveDelay > 0.0f)
            breakDelay = additiveDelay;
        
        _breakDelay = _timer.ExecuteWithDelay(NextItem, breakDelay, TimeScale.Scaled);
    }

    private float GetCurrentPhaseDelay()
    {
        DayPhaseData currentPhase = _dayPhaseChanger.CurrentPhase;
        ScheduleItem currentItem = _scheduleItemsOrder.Find(x => x.TargetDayPhase == currentPhase.Phase);
        
        if (currentItem == null)
        {
            return _dayPhaseChanger.TimeToNextChange + 1.0f;
        }
        
        return 0.0f;
    }

    private float GetNextPhaseDelay()
    {
        DayPhaseData nextPhase = _dayPhaseChanger.GetNextPhase();
        ScheduleItem nextItem = _scheduleItemsOrder.Find(x => x.TargetDayPhase == nextPhase.Phase);
        
        if (nextItem == null && _dayPhaseChanger.TimeToNextChange < _itemChangeDelay)
        {
            return _dayPhaseChanger.TimeToNextChange + nextPhase.Duration + 1.0f; 
        }

        return 0.0f;
    }

}
