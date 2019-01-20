using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMagicCollision : CollectCollisionData {

    // private bool MagicPerformed = false;
    // private bool OnlyPerformMagicOnce = false;
    // private int hitCount = 3;

    // public override void CollisionHandler(Collision collision)
    // {
    //     if(hitCount >= 3)
    //     {
    //         Destroy(gameObject);
    //     }

    //     if(OnlyPerformMagicOnce)
    //     {
    //         if (MagicPerformed)
    //         {
    //             return;
    //         }
    //     }

    //     base.CollisionHandler(collision);

    //     hitCount++;

    //     if (true)
    //     {
    //         RoomMagic.OnCollision(collision.contacts[0].point);
    //         MagicPerformed = true;
    //     }
    //     else
    //     {
    //         Debug.LogWarning("RoomMagic Singleton Not Found");
    //     }
    // }
}
