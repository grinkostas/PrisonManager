using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleType _type;
    public ObstacleType Type => _type;
}
