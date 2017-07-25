using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {


	//// Adapted from Mike Geig's 'Object Pooling' from Unity Live Training.
	//// It runs on an array of Lists for each object pool. 


	public static ObjectPooler current;
	//public GameObject pooledobject;
	public int pooledamount = 5;
	public bool willgrow = true;

	List<GameObject>[] pooledobjects;
	public List<GameObject> objecttypes = new List<GameObject>();

	public int poolLimit = 100;

	int nextEmpty = 1;

	void Awake()
	{
		current = this;
		InitializePool ();
	}

	void OnLevelWasLoaded(int level)
	{
		InitializePool ();
	}

	void Start () 
	{

	}

	public void ResetPool()
	{
		foreach (List<GameObject> l in pooledobjects)
		{
			if(l != null)
			{
				foreach(GameObject o in l)
				{
					if(o != null)
						Destroy (o);
				}
			}
		}
		InitializePool ();
	}

	void InitializePool () 
	{
		objecttypes = new List<GameObject>();
		//pooledobjects = new List<GameObject>[objecttypes.Count]; //original
		pooledobjects = new List<GameObject>[poolLimit];

		for (int i = 0; i < pooledobjects.Length; i++) 
		{

			if(objecttypes.Count > i)
			{
				pooledobjects[i] = new List<GameObject>();
				for (int o = 0; o < pooledamount; o++) 
				{
						if(objecttypes[i] != null)
						{
							GameObject obj = (GameObject)Instantiate(objecttypes[i]);
							obj.SetActive(false);
							pooledobjects[i].Add(obj);
						}
				}
			}
			else
			{
				nextEmpty = i;
				//print ("next empty pool spot is at " + nextEmpty);
				break;
			}
		}


	}

	public void AddObjectToPool(GameObject newObject)
	{
		objecttypes.Add (newObject);

		int i = nextEmpty;
		pooledobjects[i] = new List<GameObject>();
		//print (newObject.name+": "+ i);
		for (int o = 0; o < pooledamount; o++) 
		{
			GameObject obj = (GameObject)Instantiate(newObject);
			obj.SetActive(false);
			pooledobjects[i].Add(obj);
		}
		nextEmpty++;

	}

	public GameObject GetPooledObject(GameObject itemget)
	{
		int pool;
		//Get the list that matches the gameobject called for
		if(objecttypes.Contains(itemget))
		{
			//objecttypes.
			pool = objecttypes.IndexOf(itemget);
		}
		else //if the object isn't in the list, add it! So no need to set up pooled objects at start anymore... the pool is created dynamically
		{
			AddObjectToPool(itemget);
			//print (itemget.name + " does not exist, adding to pool");
			pool = objecttypes.IndexOf(itemget);
		}
		//print (pool);

		//Find an inactive object in that list // Set it active and return it
		//*
		for (int i = 0; i < pooledobjects[pool].Count; i++) 
		{
			if(!pooledobjects[pool][i].activeInHierarchy)
			{
				pooledobjects[pool][i].SetActive(true);
				return pooledobjects[pool][i];
			}				
		}
		//If none to choose from, grow the list
		if (willgrow) 
		{
			//print ("growing pool for " + itemget);
			GameObject obj = (GameObject)Instantiate(itemget);
			pooledobjects[pool].Add(obj);
			obj.SetActive(true);
			return obj;
		}

		return null;

		//*/

	}

}
