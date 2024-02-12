using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{

    public GameModes GameMode;
    public Speeds Speed;
    public Gravity Gravity;
    public int State;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            Movement movement = collision.gameObject.GetComponent<Movement>();

            movement.ChangeThroughPortal(GameMode, Speed, Gravity, State);
        }
        catch
        {
            print("No Script");
        }
    }
}
