using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScheduleItem
{
    public DayPhase TargetDayPhase;
    public float Duration;
    public ActionZoneBase Zone;
    public ActionZoneBase Queue;

    public static ScheduleItem Default = new ScheduleItem
        { TargetDayPhase = DayPhase.Any, Duration = 10f, Zone = null };
}
