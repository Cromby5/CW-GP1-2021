using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        // If the player stays in the collision they are put in the slowed state
        PlayerMoveRB player = collision.gameObject.GetComponent<PlayerMoveRB>();
        if (player != null)
        {
            player.myState = PlayerMoveRB.CharacterState.Slowed;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // If the player exits the collision they are returned to the walking state
        PlayerMoveRB player = collision.gameObject.GetComponent<PlayerMoveRB>();
        if (player != null)
        {
            player.myState = PlayerMoveRB.CharacterState.Walking;
        }
    }
}
