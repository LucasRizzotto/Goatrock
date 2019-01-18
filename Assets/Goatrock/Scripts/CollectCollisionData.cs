using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectCollisionData : MonoBehaviour {

    public LayerMask TargetLayers;
    [Serializable]
    public class CollisionEvent : UnityEvent { }
    [Space(10)]
    public CollisionEvent OnCollision;

    #region Unity APIs

    void OnCollisionEnter(Collision collision)
    {
        if(Helpers.IsInLayerMask(collision.gameObject.layer, TargetLayers))
        {
            CollisionHandler(collision);
        }
    }

    #endregion

    #region Main Methods

    public void CollisionHandler(Collision collision)
    {
        Debug.Log("Collision detected on: " + collision.contacts[0].point.ToString());
        OnCollision.Invoke();
    }

    #endregion

}
