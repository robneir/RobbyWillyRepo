using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
    #region Visible In Inspector
    /// <summary>
    /// Weather or not the item is being used by a player.
    /// </summary>
    public bool HasUser = false;
	/// <summary>
	/// The name of the item.
	/// </summary>
	public string Name = "";
	/// <summary>
	/// The damage, if any, of the item
	/// </summary>
	public int Damage = 0;
	/// <summary>
	/// The defense, if any, of the item
	/// </summary>
	public int addHealth = 0;
	/// <summary>
	/// The ItemType.
	/// </summary>
	public ItemType Type;
	/// <summary>
	/// The transform used to fire out of the tip of an ammo based weapon.
	/// </summary>
	public Transform FireTip;
	/// <summary>
	/// The muzzle flash (usually particle system) to use on an ammo based weapon. Fired out of the FireTip.
	/// </summary>
	public GameObject MuzzleFlash;
    /// <summary>
    /// The sound that is made when you use the item (audioclip)
    /// </summary>
    public AudioClip useItemSoundClip;
    /// <summary>
    /// The fire speed, if any, of a projectile out of this item.
    /// </summary>
    public float FireSpeed = 50;
    /// <summary>
    /// bullet casing prefab to be instantiate once bullets are shot
    /// </summary>
    public GameObject bulletCasingPrefab;
    /// <summary>
    /// This is where the bullet casing will instantiate once shot
    /// </summary>
    public Transform ejectTip;
    /// <summary>
    /// The ejection force of the bullet casing
    /// </summary>
    public float ejectForce;

    #endregion

    #region Hidden From Inspector Variables

    [HideInInspector]
	public UseItem UseFunc;
	[HideInInspector]
	public Vector3 posOffset;
    [HideInInspector]
    public bool isAutomatic;
    [HideInInspector]
    public float lastShotTime;
    [HideInInspector]
    public float timeBetweenShots;

    //to be put into a Quaternion later, using Quaternion.Euler(Vector3 rotationVector) (we can rotation take values from inspector this way.)
    [HideInInspector]
    public Vector3 rotOffset;
    [HideInInspector]
	public Vector3 OriginalScale;
	[HideInInspector]
    public bool isBeingUsed;

	#endregion

	public enum ItemType
	{
		Melee,
		Ammo,
		OneShot,
		Special
	}

	// Use this for initialization
	void Start () 
	{
		OriginalScale = transform.localScale;

		switch(Type)
		{
			case ItemType.Ammo:
				switch(Name)
				{
					case "Shotgun":
						posOffset = new Vector3(0.07998937f,0-1.710141f,0);
						rotOffset = new Vector3(0,0,276.1943f);
				        UseFunc = FireSemiAuto;
                        isAutomatic = false;
						break;
					case "Colt45":
						posOffset = new Vector3(0.6901398f,-2.161226f,0);
						rotOffset = new Vector3(0,0, 267.1234f);
						UseFunc = FireSemiAuto;
                        isAutomatic = false;
						break;
                    case "AR-15":
                        posOffset = new Vector3(1.48f, -1.5f, 0);
                        rotOffset = new Vector3(0, 0, 276.1943f);
                        UseFunc = FireSemiAuto;
                        isAutomatic = false;
                        break;
                    case "SMG":
                        posOffset = new Vector3(1.48f, -1.5f, 0);
                        rotOffset = new Vector3(0, 0, 276.1943f);
                        UseFunc = FireSemiAuto;
                        isAutomatic = true;
                        timeBetweenShots = .1f;
                        break;
                }
			break;
			case ItemType.Melee:
				switch(Name)
				{
					case "Katana":
                        posOffset = new Vector3(1.83f, -2.46f, 0);
						rotOffset = new Vector3(0, 0, 217.8387f);
		                UseFunc = SwingSword;
						break;
				}
				break;
            case ItemType.OneShot:
                switch (Name)
                {
                    case "HealthPotion":
                        UseFunc = AddHealth;
                        break;
                }
                break;
        }
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if(HasUser && !GetComponent<Rigidbody2D>().fixedAngle)
		{
			GetComponent<Rigidbody2D>().fixedAngle = true;
			GetComponent<Transform>().rotation = SetTran.rotation;
			GetComponent<Transform>().localPosition = Offset;
		}
		else
		{
			GetComponent<Rigidbody2D>().fixedAngle = false;
		}*/
	}

	#region UseItem methods
	//Here will lie all the methods for using unique items. Some may be shared. 
	//EG---> SwingSword() may be applicable to both a baseball bat and a katana
	//EG---> FireRocket() will probably be different than FireHandgun()

	private void FireSemiAuto(GameObject owner)
	{
		InstantiateBullet ();
		DoRecoil ();
        this.GetComponent<AudioSource>().clip = useItemSoundClip;
        this.GetComponent<AudioSource>().Play();
        InstantiateBulletCasing();
	}

	void DoRecoil ()
	{
		GameObject rotateArm = this.transform.parent.parent.parent.gameObject;
		rotateArm.transform.GetComponent<PointTowardMouse> ().recoilOffset += 5f;
	}

    private void SwingSword(GameObject owner)
    {
        isBeingUsed = true;
    }

    private void AddHealth(GameObject owner)
    {
        owner.GetComponent<PhotonView>().RPC("AddHealth", PhotonTargets.AllBuffered, addHealth);
        Debug.Log("Added health");
    }

	void InstantiateBullet()
	{
		GameObject muz = (GameObject)GameObject.Instantiate (MuzzleFlash, this.FireTip.position, Quaternion.identity);
		GameObject.Destroy(muz, 1f);

		PhotonView pv = this.transform.root.gameObject.GetComponent<PhotonView>();
		if(pv.isMine)
		{
			Vector3 sendV = new Vector3 (FireTip.right.x * this.transform.root.localScale.x, FireTip.right.y, FireTip.right.z) * FireSpeed;
			float lScale = this.transform.root.localScale.x;
            //ROB'S METHOD//pv.RPC("FireBullet2", PhotonTargets.All, PhotonNetwork.player.ID, FireTip.position, FireTip.rotation, FireTip.right, this.Damage, FireSpeed);
            pv.RPC ("FireBullet", PhotonTargets.All, PhotonNetwork.player.ID, FireTip.position, FireTip.rotation, sendV, this.Damage, lScale); 
        }
	}

    void InstantiateBulletCasing()
    {
        GameObject bulletCasing = (GameObject)Instantiate(bulletCasingPrefab, ejectTip.position, ejectTip.rotation);
        //Add force somwhat randomly to the bullet casings
        bulletCasing.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-10,10)*.1f,1)* ejectForce);
        bulletCasing.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-10, 10)*ejectForce, ForceMode2D.Impulse);
    }

    public delegate void UseItem(GameObject owner);

	#endregion
}
