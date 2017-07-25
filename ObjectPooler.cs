using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public interface IManager
{
    void init();
    void remove();
}

public class ObjectPooler : MonoBehaviour, IManager
{
	//// Adapted from Mike Geig's 'Object Pooling' from Unity Live Training.
	//// It runs on an array of Lists for each object pool. 

	//public static ObjectPooler current; //only the factory is static now
	//public GameObject pooledobject;
	public int pooledamount = 3;
	public bool willgrow = true;

	List<GameObject>[] pooledobjects;
	public List<GameObject> objecttypes = new List<GameObject>();

	public int poolLimit = 100;

	int nextEmpty = 1;

	public delegate void Spawned(GameObject obj);
	public Spawned ObjectSpawned;

	public void init()
	{
	    InitializePool();

	    print("[Core] Object pooler is set up");
	}

	public void ResetPool()
	{
	    foreach (List<GameObject> l in pooledobjects)
	    {
		if (l != null)
		{
		    foreach (GameObject o in l)
		    {
			if (o != null)
			    Destroy(o);
		    }
		}
	    }
	    InitializePool();
	}

	void InitializePool()
	{
	    objecttypes = new List<GameObject>();
	    pooledobjects = new List<GameObject>[poolLimit];

	    for (int i = 0; i < pooledobjects.Length; i++)
	    {
		if (objecttypes.Count > i)
		{
		    pooledobjects[i] = new List<GameObject>();
		    for (int o = 0; o < pooledamount; o++)
		    {
			if (objecttypes[i] != null)
			{
			    GameObject obj = (GameObject)Instantiate(objecttypes[i]);
			    obj.SetActive(false);
			    pooledobjects[i].Add(obj);
			    ObjectSpawned(obj);
			}
		    }
		}
		else
		{
		    nextEmpty = i;
		    break;
		}
	    }


	}

	public void AddObjectToPool(GameObject newObject)
	{
	    objecttypes.Add(newObject);

	    int i = nextEmpty;
	    pooledobjects[i] = new List<GameObject>();

	    for (int o = 0; o < pooledamount; o++)
	    {
		GameObject obj = (GameObject)Instantiate(newObject);
		obj.SetActive(false);
		pooledobjects[i].Add(obj);
		ObjectSpawned(obj);
	    }
	    nextEmpty++;

	}

	public GameObject GetPooledObject(GameObject itemget)
	{
	    int pool;
	    //Get the list that matches the gameobject called for
	    if (objecttypes.Contains(itemget))
	    {
		pool = objecttypes.IndexOf(itemget);
	    }
	    else //if the object isn't in the list, add it! So no need to set up pooled objects at start anymore... the pool is created dynamically
	    {
		AddObjectToPool(itemget);
		pool = objecttypes.IndexOf(itemget);
	    }

	    //Find an inactive object in that list // Set it active and return it            
	    for (int i = 0; i < pooledobjects[pool].Count; i++)
	    {
		if (!pooledobjects[pool][i].activeInHierarchy)
		{
		    pooledobjects[pool][i].SetActive(true);
		    return pooledobjects[pool][i];
		}
	    }

	    //If none to choose from, grow the list
	    if (willgrow)
	    {                
		GameObject obj = CreateNewObject(itemget);
		pooledobjects[pool].Add(obj);
		obj.SetActive(true);	
		ObjectSpawned(obj);
		return obj;
	    }

	    return null;            
	}
	
	public void remove()
	{

	}
}

