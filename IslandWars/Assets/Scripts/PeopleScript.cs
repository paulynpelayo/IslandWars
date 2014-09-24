using UnityEngine;
using System.Collections;

public class PeopleScript : MonoBehaviour {

	#region playerstate

	public enum Playerstate
	{
		Idle,
		Shooting, 
		Moving,
		Reserve
	}
	private Playerstate _playerstate;
	public Playerstate playerstate
	{
		get { return _playerstate; }
		set 
		{
			_playerstate = value;
			ChangeState();
		}
	}

	#endregion

	#region tk2d
	 
	private tk2dSprite PlayerSprite;
	private tk2dSpriteAnimator PlayerAnimator;

	#endregion

	private string Attack = "Nearest";
	public string AttackState
	{
		get { return Attack; }
		set { Attack = value; }
	}

	public int Level = 1;
	public int CurrentLevel
	{
		get { return Level; }
		set { Level = value; }
	}

	public TargetList ListOfTarget;
	private GameObject Target;
	private TrajectoryHelper Helper;
	private bool CanShoot = true;
	private Transform WeaponPoint;
	public Transform WeaponPrefab;
	public int Cost;
	public int UpgradeDamage;
	public Material Outline;

	public AudioClip AttackSound;

	// Use this for initialization
	void Start () {

		WeaponPoint = transform.FindChild("WeaponPoint");
		PlayerSprite = gameObject.GetComponent<tk2dSprite>();
		PlayerAnimator = gameObject.GetComponent<tk2dSpriteAnimator>();
		Helper = gameObject.GetComponent<TrajectoryHelper>();

		ChangeTargetSide();
		playerstate = Playerstate.Idle;

	}
	
	// Update is called once per frame
	void Update () {
		ChangeState();
	}

	void ShootComplete (tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{	
		CanShoot = false;
	}

	void ChangeState()
	{
		switch (_playerstate)
		{
			case Playerstate.Idle: 
				
				CanShoot = true;

				PlayerAnimator.Stop();
				PlayerSprite.SetSprite(PlayerSprite.Collection.Count);
							          
				if (transform.position.x < -0.3 || transform.position.x > 0.3)
				{	
					Target = ListOfTarget.GetEnemy(Attack);
					
					if (Target != null)
						playerstate = Playerstate.Shooting;			
				}
				else playerstate = Playerstate.Reserve;		

			break;

			case Playerstate.Shooting: 
				
				Target = ListOfTarget.GetEnemy(Attack);

				
				if (Target == null)
					playerstate = Playerstate.Idle;
				else
				{
					if (CanShoot)
					{	
						PlayerAnimator.Play("Attack");					
						PlayerAnimator.AnimationCompleted = ShootComplete;

					}
					else
				  	{	
						//play SFX
						audio.PlayOneShot(AttackSound);	

						float EnemySpeed = Target.GetComponent<EnemyScript>().Speed;
						
						Transform Weapon = Instantiate(WeaponPrefab) as Transform;					
						
						Weapon.position = WeaponPoint.position;
						Weapon.localScale = transform.localScale;

						Vector3 Rotation = new Vector3(Weapon.eulerAngles.x, Weapon.eulerAngles.y, (Weapon.eulerAngles.z + transform.eulerAngles.z) * transform.localScale.x);
						Weapon.eulerAngles = Rotation;
											
						Vector3 Velocity = Helper.GetVelocityWithAngleAndTarget(WeaponPoint, Target.transform.position,  EnemySpeed, Weapon.eulerAngles.z) * transform.localScale.x;

						

						//Weapon.rigidbody.velocity = Helper.GetVelocityWithAngleAndTarget(WeaponPoint, Target.transform.position,  EnemySpeed, Weapon.eulerAngles.z) * transform.localScale.x;

						if (!float.IsNaN(Velocity.x) && !float.IsNaN(Velocity.y) && !float.IsNaN(Velocity.z))
						{
							Weapon.rigidbody.velocity = Velocity;

							Weapon.GetComponent<WeaponScript>().setTarget(Target);
							Weapon.GetComponent<WeaponScript>().setUpgradeDamage(UpgradeDamage);
							
							CanShoot = true;


						}						
					
					}
				}

				break;	

			case Playerstate.Moving: 
				
				CanShoot = true;
				
				PlayerAnimator.Stop();
				PlayerSprite.SetSprite(PlayerSprite.Collection.Count);							          
				
				ChangeTargetSide();				
				
			break;

			case Playerstate.Reserve:

			break;

		}
	}

	public void ChangeTargetSide()
	{
		string targetName = gameObject.name;
		Vector2 Scale;

		if (transform.position.x > 0.3)
		{
			if (targetName.IndexOf("BlowGun") > -1)
				ListOfTarget = GameObject.Find("BlowGun_RBox").GetComponent<TargetList>();
			else if (targetName.IndexOf("Slingshot") > -1)
				ListOfTarget = GameObject.Find("Slingshot_RBox").GetComponent<TargetList>();
			else if (targetName.IndexOf("Archer") > -1)
				ListOfTarget = GameObject.Find("Archer_RBox").GetComponent<TargetList>();
			else if (targetName.IndexOf("Cannon") > -1)
				ListOfTarget = GameObject.Find("Cannon_RBox").GetComponent<TargetList>();

			Scale = new Vector2 (1, 1);
			transform.localScale = Scale;
		}
		else if (transform.position.x < 0.3)
		{
			if (targetName.IndexOf("BlowGun") > -1)
				ListOfTarget = GameObject.Find("BlowGun_LBox").GetComponent<TargetList>();
			else if (targetName.IndexOf("Slingshot") > -1)
				ListOfTarget = GameObject.Find("Slingshot_LBox").GetComponent<TargetList>();
			else if (targetName.IndexOf("Archer") > -1)
				ListOfTarget = GameObject.Find("Archer_LBox").GetComponent<TargetList>();
			else if (targetName.IndexOf("Cannon") > -1)
				ListOfTarget = GameObject.Find("Cannon_LBox").GetComponent<TargetList>();

			Scale = new Vector2 (-1, 1);
			transform.localScale = Scale;
		}
		//Debug.Log (GameObject.Find("Slingshot_RBox"));
		//ListOfTarget = GameObject.Find(targetName).GetComponent<TargetList>();
	}

}
