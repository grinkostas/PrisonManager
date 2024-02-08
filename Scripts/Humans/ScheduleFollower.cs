using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StaserSDK.Utilities;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

public class ScheduleFollower : MonoBehaviour
{
    [SerializeField] private Prisoner _prisoner;
    [SerializeField] private float _firstActionDelay;
    [SerializeField] private Vector2 _reactTimeRange;

    public List<string> _debugActionLog = new List<string>();

    [Inject] private DayPhaseChanger _dayPhaseChanger;
    [Inject] private Schedule _schedule;
    [Inject] private Timer _timer;

    private bool _populated = false;
    private ScheduleItem _currentScheduleItem;
    private bool _usedCurrentItem = false;

    public FollowData _followData = new FollowData();
    
    private void OnEnable()
    {
        _prisoner.Populated += OnPopulated;
        _prisoner.Kicked += OnKicked;
        _prisoner.OnRelease += OnRelease;
        _schedule.NewScheduleAction += OnNewScheduleAction;
        
    }
    
    private void OnDisable()
    {
        _prisoner.Populated -= OnPopulated;
        _prisoner.Kicked -= OnKicked;
        
        _prisoner.OnRelease -= OnRelease;
        _schedule.NewScheduleAction -= OnNewScheduleAction;
    }


    private void OnKicked()
    {
        NextAction();
    }

    private void OnRelease()
    {
        enabled = false;
        _populated = false;
    }
    
    private void OnPopulated(Bed bed)
    {
        _populated = true;
        Log("Populated");
        _timer.ExecuteWithDelay(_prisoner.NextAction, _firstActionDelay);
    }

    private void Log(string log)
    {
        _debugActionLog.Add($"{_prisoner.TimeInPrison}| {log}");
    }

    private void NextAction()
    {
        if(_populated == false)
            return;
        
        Log("NextAction");
        
        if(_prisoner.Bed == null)
            return;
        
        if (_schedule.IsPaused || _schedule.CurrentItem == null)
        {
            _followData.Reset();
            _followData.Result = UsePlaceResult.RoomAction;
            _followData.LastActualizeTime = _prisoner.TimeInPrison;
            Log("Room Action");
            _prisoner.NextAction();
            return;
        }
        FollowSchedule(_schedule.CurrentItem);
    }

    private void OnNewScheduleAction(ScheduleItem scheduleItem)
    {
        if(_populated == false)
            return;
        _usedCurrentItem = false;
        Log("New Schedule Action");
        if(_dayPhaseChanger.CurrentPhase.Phase == DayPhase.Night && scheduleItem != ScheduleItem.Default)
            return;
        float timeDelay = Random.Range(_reactTimeRange.Min(), _reactTimeRange.Max());
        
        if (scheduleItem == ScheduleItem.Default)
            timeDelay = 0.0f;
        
        _timer.ExecuteWithDelay(() => FollowSchedule(scheduleItem), timeDelay, TimeScale.Scaled);
        
    }

    private void FollowSchedule(ScheduleItem scheduleItem)
    {
        if(_populated == false)
            return;
        Log($"Try Follow {scheduleItem.Zone}");
        if (_prisoner.Enabled == false)
        {
            return;
        }
        
        if(_prisoner.Bed == null)
            return;

        if(scheduleItem != _followData.ScheduleItem)
            _followData.Reset();
        
        if (scheduleItem == _followData.ScheduleItem && _followData.Result == UsePlaceResult.Available &&
            _prisoner.TimeInPrison - _followData.LastActualizeTime < 1.0f)
        {
            return;
        }
        

        if (scheduleItem == _followData.ScheduleItem)
        {
            Log($"Try Room Action 2");
            _usedCurrentItem = true;
            if (_prisoner.Destination == null)
            {
                if(_followData.Result == UsePlaceResult.RoomAction &&
                   _prisoner.TimeInPrison - _followData.LastActualizeTime < 1.0f)
                    return;
                Log($"Room Action 2");
                
                _followData.Result = UsePlaceResult.RoomAction;
                _followData.LastActualizeTime = _prisoner.TimeInPrison;
                _prisoner.NextAction();
            }

            return;
        }
        
        if (scheduleItem == ScheduleItem.Default || _dayPhaseChanger.CurrentPhase.Phase == DayPhase.Night)
        {
            _currentScheduleItem = scheduleItem;
            Log($"Try Bed");
            _followData.Place = _prisoner._currentBed;
            _followData.Result = UsePlaceResult.RoomAction;
            _followData.LastActualizeTime = _prisoner.TimeInPrison;
            _followData.ScheduleItem = scheduleItem;
            _usedCurrentItem = true;
            
            if(_prisoner.CurrentPlace == _prisoner.Bed || _prisoner.Destination == _prisoner.Bed)
                return;
            Log($"Go Bed");
            _prisoner.ForceKickToPlaceBed();
            _prisoner.GoBed();
            return;
        }

        _followData.ScheduleItem = scheduleItem;

        var useSchedulePlace = TryToUseSchedulePlace(scheduleItem.Zone);

        if (useSchedulePlace == UsePlaceResult.Available)
        {
            return;
        }

        if (useSchedulePlace == UsePlaceResult.NoAvailablePlaces &&
            TryToUseSchedulePlace(scheduleItem.Queue) == UsePlaceResult.Available)
        {
            _usedCurrentItem = true;
            return;
        }

        Log($"Room Action 3");
        
        _followData.LastActualizeTime = _prisoner.TimeInPrison;
        _usedCurrentItem = true;
        _prisoner.NextAction();
        
    }

    private UsePlaceResult TryToUseSchedulePlace(ActionZoneBase zone)
    {
        Log($"Try use zone {zone}");
        if (zone.TryGetFreePlace(out ActionPlaceBase place))
        {
            Log($"Fetched place {place}");
            if (_prisoner.PreviousAction != null && _prisoner.PreviousAction.Type == place.Type)
            {
                Log($"already use {place}");
                return UsePlaceResult.AlreadyUsed;
            }

            if (HaveEnoughTime(place) == false)
            {
                Log($"Not Enough Time {place}");
                return UsePlaceResult.NotEnoughTime;
            }
            Log($"Use Place {place}");
            _followData.Zone = zone;
            _followData.Place = place;
            _followData.Result = UsePlaceResult.Available;
            _followData.LastActualizeTime = _prisoner.TimeInPrison; 

            if(place != _prisoner.CurrentPlace)
                _prisoner.ForceKick(place);
            _prisoner.IntentVisit(place);
            return UsePlaceResult.Available;
        }
        return UsePlaceResult.NoAvailablePlaces;
    }

    [System.Serializable]
    public class FollowData
    {
        public ScheduleItem ScheduleItem;
        public ActionPlaceBase Place;
        public UsePlaceResult Result;
        public ActionZoneBase Zone;
        public float LastActualizeTime;

        public void Reset()
        {
            Place = null;
            Zone = null;
            Result = UsePlaceResult.None;
        }
    }

    private bool HaveEnoughTime(ActionPlaceBase place)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(_prisoner.Human.transform.position, place.InteractPoint.position, NavMesh.AllAreas,
            path);
        float pathLength = path.GetDistance();
        float timeToMoveAcross = pathLength / _prisoner.Agent.speed;
    
        float availableTime = _schedule.CurrentActionTime + _schedule.ItemChangeDelay;

        return timeToMoveAcross + place.InteractTime <= availableTime;
    }

    public enum UsePlaceResult
    {
        AlreadyUsed, 
        NotEnoughTime, 
        NoAvailablePlaces, 
        Available,
        None, 
        RoomAction
    }
}
