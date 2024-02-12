using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{

    public Gamemodes GameMode;
    public Speeds Speed;
    public bool Gravity;
    public int State;

public void InitiatePortal(Movement _movement)
    {
        _movement.ChangeThroughPortal(GameMode, Speed, Gravity ? 1 : -1, State, transform.position.y);
    }
}
