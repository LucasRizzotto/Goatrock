using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoatRock;

public class trackPitch : MonoBehaviour {

	public GameObject cloner;
	public colorPitch pitcher;
    public bool keepInstantiating = false;


    private Vector3 position;
	//private int increment = 1;
	private GameObject[] equalizers;
	private int elements = 40;
	private int count = 0;

	private Renderer rend;

	// Use this for initialization
	void Start () {
		position = new Vector3(0,0,0);
		InvokeRepeating("GenerateEqual", 0.2f,0.2f);
		equalizers = new GameObject[elements];

	}

	// Update is called once per frame
	void Update () {
        if (AudioReactMicSourceBehavior.Instance.userIsTalking)
            toggleBLOOP();
	}

    public void toggleBLOOP()
    {
        keepInstantiating = !keepInstantiating;
    }


	public void GenerateEqual()
	{
        if (keepInstantiating)
        {
            GameObject newObj = Instantiate(cloner, position, Quaternion.identity, gameObject.transform);

            if (count > elements)
                Destroy(equalizers[count % elements]);

            equalizers[count % elements] = newObj;
            //position = new Vector3(position.x + increment, 0, 0);
            count++;

            rend = newObj.GetComponent<Renderer>();

            rend.material.color = pitcher.col;
        }
	}
}
