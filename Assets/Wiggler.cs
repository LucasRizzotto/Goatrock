using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wiggler : MonoBehaviour {

  private int size = 1;
  private int buoyancy = -1;
  private int motion = 1;

	//the rigidbody object
	private Rigidbody my_rb;

	// the force of bounce
	public float bounceForce = 200.0f;

  // Key event triggers for the shader relationships
  // public UnityEvent onWallBounce;
  // public UnityEvent onWormsCollide;
  // public UnityEvent onFloorHit;

	public bool useTraitDefaults = true;

	// force with which to launch the projectile
	public int launchSpeed = 1;

	// the hit effect mod for the contacts
	public Transform wallHitEffects;

	//the special gameObject to assess on collisions
	public GameObject hitObject;

	public UnityEvent onBirth;
	public UnityEvent onContact;
  public UnityEvent onDeath;
	public UnityEvent onSecondaryCollision;

	// Use this for initialization
	void Start ()
  {
		if (!useTraitDefaults)
		{
			GetTraits();
		}

		GeneratePhysics();
	}

	// Update is called once per frame
	void Update ()
  {

	}

	public void	GeneratePhysics()
	{
		my_rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;

		//size

		//impulse force to push particle
		my_rb.AddForce((float)launchSpeed * Vector3.forward, ForceMode.Impulse);

		if (buoyancy > 0)
		{
			my_rb.useGravity = false;
		}
		else
		{
			my_rb.useGravity = true;
		}

	}

	void OnCollisionEnter(Collision collision)
	{
		ContactPoint hitArea = collision.contacts[0];
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hitArea.normal);
		Vector3 position = hitArea.point;
		Transform eff = Instantiate(wallHitEffects, position, rotation);
		eff.gameObject.AddComponent<die>();
		onContact.Invoke();
		//Debug.Log("Bouncer");
		Bounce();

		//detects a special contact
		if (specialContact(hitObject, collision.gameObject))
		{
			AlternateCollide();
		}
		else
		{

		}
	}

  public void GetTraits()
  {
    //sizeTrait = GetDuration();
    //buoyancyTrait = GetPitch();
  	//  motionTrait = GetAmplitude();
  }

	public void Bounce()
	{
				//...by adding a force
		my_rb.AddForce(Vector3.up * bounceForce);
	}

			//Bool function to determine if two object references are the same
			// (to decide if this is a predetermined type of contact)
	bool specialContact(GameObject obj1, GameObject obj2)
	{
			//determines if two game objects are the same
	 if (GameObject.ReferenceEquals(obj1, obj2))
		{
			 Debug.Log("Key object hit");
			 return true;
		}
		else
		{
			Debug.Log("Other object hit");
			return false;
		}
	}

  public void WormDeath()
  {
    onDeath.Invoke();
  }

  public void AlternateCollide()
  {
		onSecondaryCollision.Invoke();
  }
}
