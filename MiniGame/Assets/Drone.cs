using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class Drone : MonoBehaviour 
{
	public float MoveSpeed = .5f;
	public GameObject DroneEye;
	public GameObject bulletPrefab;
	public Transform fireTip;
	public float bulletSpeed;
	public int bulletDamage = 25;
	public float timeBetweenShots;
	public AudioClip fireSound;
	public ParticleSystem fireEffect;
	
	private float lastShotTime;
	
	public StatusBar statusBar;
	public Vector2 barOffSet = Vector2.zero;

	[HideInInspector]
	public GameObject Owner = null;
	[HideInInspector]
	public GameObject Target = null;

	float EyeMaxDistanceMagnitude = .5f;
	Vector3 DroneEyeStartPosLocal;

	float closeRadius = 15;
	Rigidbody2D rig;
	
	// Use this for initialization
	void Start () 
	{
		DroneEyeStartPosLocal = DroneEye.transform.localPosition;
		rig = GetComponent<Rigidbody2D> ();
	}

	void ControlShooting()
	{
		if(PhotonNetwork.isMasterClient)
		{
			if (Time.time - lastShotTime >= timeBetweenShots)
			{
				lastShotTime = Time.time;

				Vector3 dir = Target.transform.position - DroneEye.transform.position;
				dir.Normalize();

				//for angle
				var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
				Quaternion bulletRot = Quaternion.AngleAxis(angle, Vector3.forward);

				/*FIX THIS SO THAT TURRETFIRES IN OPPOSITE DIRECTION AS WELL*/
				this.GetComponent<PhotonView>().RPC("FireDroneBullet", PhotonTargets.All, DroneEye.transform.position + dir * 2, bulletRot, dir);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(PhotonNetwork.isMasterClient)
		{
			Vector3 middleDrone = this.transform.position + DroneEyeStartPosLocal;

			if(Target != null)
			{
				//follow him with your eye
				Vector3 dir = Target.transform.position - DroneEye.transform.position;
				dir.Normalize();
				dir *= EyeMaxDistanceMagnitude;

				DroneEye.transform.position = Vector3.Lerp(DroneEye.transform.position, middleDrone + dir, .05f);

				ControlShooting();
				
				if(Target.GetComponent<PlayerStatus>().dead)
				{
					Target = null;
				}
			}
			else
			{
				DroneEye.transform.position = Vector3.Lerp(DroneEye.transform.position, middleDrone, .05f);
			}
		}

		if(statusBar!=null)
		{
			//Update position of bar
			statusBar.transform.position =
				new Vector3(this.transform.position.x + barOffSet.x,
				            this.transform.position.y + barOffSet.y,
				            this.transform.position.z);
		}
	}
	
	[RPC]
	void InstantiateHealthBar(float startHealth)
	{
		if(startHealth == null)
		{
			statusBar = (StatusBar)Instantiate(statusBar, Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity);
			statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
		}
		else
		{
			statusBar = (StatusBar)Instantiate(statusBar, this.transform.position, Quaternion.identity);
			statusBar.transform.SetParent(FindObjectOfType<Canvas>().transform);
			statusBar.GetComponent<StatusBar>().targetHealth = (float)startHealth;
			statusBar.GetComponent<StatusBar>().currentHealth = (float)startHealth;
		}
	}

	[RPC]
	void FireDroneBullet(Vector3 position, Quaternion rotation, Vector3 direction)
	{
		AudioSource.PlayClipAtPoint (fireSound, position);
		ParticleSystem fireEff = (ParticleSystem)Instantiate(fireEffect, fireTip.position, fireTip.rotation);
		fireEffect.Play();
		GameObject bull = (GameObject)Instantiate(bulletPrefab, position, rotation);
		bull.GetComponent<Bullet>().Damage = bulletDamage;
		bull.GetComponent<Bullet> ().ID = (int)AI_Constants.ID.Drone;//turret should have an ID that doesnt match any players ever.
		bull.GetComponent<Rigidbody2D>().AddForce(bulletSpeed * direction);
	}
	
	[RPC]
	void TakeDamage(int damage, int dealerID)
	{
		int deathID = (int)AI_Constants.ID.Drone;

		if (statusBar != null)
		{
			statusBar.GetComponent<StatusBar>().targetHealth -= damage;
			
			if (statusBar.GetComponent<StatusBar>().targetHealth <= 0)
			{
				//kill status bar
				GameObject.Destroy(statusBar.gameObject);
				
				if(PhotonNetwork.player.ID == dealerID)
				{
					//you are dope you killed him
					//PhotonHashTable pht = new PhotonHashTable();
					PhotonNetwork.player.customProperties["Kills"] = (int)PhotonNetwork.player.customProperties["Kills"] + 1;
					PhotonNetwork.player.customProperties["Deaths"] = (int)PhotonNetwork.player.customProperties["Deaths"];
					PhotonNetwork.player.customProperties["Assists"] = (int)PhotonNetwork.player.customProperties["Assists"];
					PhotonNetwork.player.SetCustomProperties(PhotonNetwork.player.customProperties);
				}
				else if(PhotonNetwork.player.ID == deathID)
				{
					//you suck you are dead
					//PhotonHashTable pht = new PhotonHashTable();
					PhotonNetwork.player.customProperties["Kills"] = (int)PhotonNetwork.player.customProperties["Kills"];
					PhotonNetwork.player.customProperties["Deaths"] = (int)PhotonNetwork.player.customProperties["Deaths"] + 1;
					PhotonNetwork.player.customProperties["Assists"] = (int)PhotonNetwork.player.customProperties["Assists"];
					PhotonNetwork.player.SetCustomProperties(PhotonNetwork.player.customProperties);
				}

				//message
				string msg = Helpers.GetPlayerName(deathID) + " was killed by " + Helpers.GetPlayerName(dealerID); 
				if(Helpers.GetPlayerName(deathID) == "You")
				{
					msg = Helpers.GetPlayerName(deathID) + " were killed by " + Helpers.GetPlayerName(dealerID); 
				}	
				GameObject.FindGameObjectWithTag("TextConsole").GetComponent<TextConsole>().AddMessage(msg);

				//have master client kill object
				if(PhotonNetwork.isMasterClient)
				{
					PhotonNetwork.Destroy (this.gameObject);
				}
			}
		}
	}
	
	void FixedUpdate()
	{
		if(PhotonNetwork.isMasterClient && Target != null)
		{
			Vector3 dir = Target.transform.position - DroneEye.transform.position;
			float d = dir.magnitude;
			
			if(d < closeRadius)
			{
				//strafe
				Strafe();
			}
			else if(d > closeRadius + 1)
			{
				Follow();
			}
			else
			{
				//do nothing
				rig.velocity = Vector2.zero;
			}
		}
	}
	
	void Follow()
	{
		Vector3 dirNorm = Target.transform.position - this.transform.position;
		dirNorm.Normalize ();
		dirNorm *= MoveSpeed;
		Vector2 moveVec = new Vector2 (dirNorm.x, dirNorm.y);
		rig.velocity = moveVec;
	}

	Vector2 randDir;
	void Strafe()
	{
		float rX = Random.Range (.001f, MoveSpeed);
		float rY = Random.Range (.001f, MoveSpeed);
		randDir = Vector2.Lerp (randDir, new Vector2 (rX, rY), .1f);
		rig.velocity = randDir;
	}

	void Trigger(Collider2D c)
	{
		if(PhotonNetwork.isMasterClient)
		{
			if (Target != null)
				return;
			
			if(c.gameObject.tag.Equals("Player"))
			{
				Target = c.gameObject;
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D c)
	{
		Trigger (c);
	}

	void OnTriggerStay2D(Collider2D c)
	{
		Trigger (c);
	}
	
	void OnTriggerExit2D(Collider2D c)
	{
		if(PhotonNetwork.isMasterClient)
		{
			if(Target != null &&
			   c.gameObject == Target)
			{
				//lose target
				Target = null;
			}
		}
	}
}
