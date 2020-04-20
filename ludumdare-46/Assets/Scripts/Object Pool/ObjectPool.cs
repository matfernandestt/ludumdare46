using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public PoolableObject BaseObject;
    public List<PoolableObject> poolableObjects = new List<PoolableObject>();
    
    public PoolableObject RequestObject(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        PoolableObject newObject;
        if (poolableObjects.Count > 0)
        {
            newObject = poolableObjects[0];
            poolableObjects.RemoveAt(0);
        }
        else
        {
            newObject = Instantiate(BaseObject);
        }
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;
        newObject.transform.parent = parent;
        newObject.ActivateObject();
        return newObject;
    }

    public void ReturnObject(PoolableObject returnedObject, float timeToReturn)
    {
        StartCoroutine(RestoringObject(returnedObject, timeToReturn));
    }

    private IEnumerator RestoringObject(PoolableObject returnedObject, float timeToReturn)
    {
        yield return new WaitForSeconds(timeToReturn);
        returnedObject.transform.parent = transform;
        returnedObject.transform.localPosition = Vector3.zero;
        poolableObjects.Add(returnedObject);
        returnedObject.DeactivateObject();
    }
}
