using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    //id of the player that owns this unit
    public int playerID = 0;

    //attributes
    public int movementRange = 0;
    public int attackRange = 0;
    public int armour = 0;
    public int baseArmour = 0;

    public float damage = 0.0f;
    public float maxHealth = 1000.0f;
    public float health = 1000.0f;
    public float AOV = 1.0f;

    //flags indicating what the unit has done in a particular turn
    public int movementPoints = 0;
    public bool hasAttacked = false;

    //visual movement speed
    public float movementSpeed = 3.0f;

    //ratio to multiply the base damage by
    public float attackMultiplier = 1.0f;

    //time it takes the unit to apply damage after "attacking"
    public float attackTime = 1.0f;

    //time it takes the unit to be completely deleted after "dying"
    public float deathTime = 1.0f;

    //container of commands to execute
    public List<UnitCommand> commands = new List<UnitCommand>();

    //holds all the GameObjects that makes up the units area of vision
    public List<GameObject> aovOBJ = new List<GameObject>();

    //the game prefab that is used to make up the units area of sight
    public GameObject sightPrefab;

    //refrence to the sight prefab
    public GameObject sightHolder;

    //David
    //health bar transform
    public RectTransform HpTransform = null;

    //the Health bar image reference
    public Image hpBar = null;

    //the health bar number reference
    public Text hpNum = null;

    //image of the king buff
    public SpriteRenderer kingBuff = null;

    //reference to the UI armour bar
    public SpriteRenderer armourBar = null;
    
    //Animator 
    public Animator ArtLink;

    //check if the unit is within sight
    public bool inSight;

    [HideInInspector]
    public SkinnedMeshRenderer MeshLink;


    /*
    * Initialise 
    * 
    * called once at the start of the object's existence
    * called by the game manager
    * 
    * @returns void
    */
    public void Initialise()
    {
        MeshLink = ArtLink.GetComponentInChildren<SkinnedMeshRenderer>();
        transform.position = new Vector3((int)(transform.position.x + 0.5f), 0.15f, (int)(transform.position.z + 0.5f));
    }


    void Start()
    {
        MeshLink = ArtLink.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public void TurnOffRender()
    {
        inSight = false;
        MeshLink.enabled = false;
        hpBar.transform.parent.gameObject.SetActive(false);
    }
    public void TurnOnRender()
    {
        inSight = true;
        MeshLink.enabled = true;
        hpBar.transform.parent.gameObject.SetActive(true);
    }

    public void UnitInit()
	{
		//get the tile that the unit is standing on
		Tiles currentTile = GameObject.FindObjectOfType<Map>().GetTileAtPos(transform.position);

		//set the unit space to this
		currentTile.unit = this;

		//create the gameobjs that make up the units area of vision
		CreateAOVOBJ();

		baseArmour = armour;

		//reset the real-time turn tracking
		movementPoints = movementRange;
		hasAttacked = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (commands.Count > 0)
        {
            commands[0].Update();
        }
        //find a anim sync to trigger this
        HealthUpdate();

        if (Input.GetKey(KeyCode.F3) && Input.GetKey(KeyCode.S))
        {
            bool holder = sightHolder.activeSelf;
            DestroySight();
            CreateAOVOBJ();
            Debug.Log("Reloaded Sight");
            sightHolder.SetActive(holder);
        }

    }

    //called when the gameobject it is attached to is destroyed
    public void OnDestroy()
    {
        DestroySight();
    }

    /*
    * DestroySight 
    * void function
    * 
    * destorys the units sight prefabs that lets the player see in the
    * Fog of war
    * 
    * @returns void
    * @Author Callum Dunstone
    */
    private void DestroySight()
    {
        if (sightHolder != null)
        {
            foreach (Transform tran in sightHolder.transform)
            {
                GameObject.Destroy(tran.gameObject);
            }

            GameObject.Destroy(sightHolder.gameObject);
        }
    }

    /*
    * CreateAOVOBJ 
    * void function
    * 
    * creates the cubes used as the units area of sight in game
    * 
    * @returns void
    * @Author Callum Dunstone
    */
    private void CreateAOVOBJ()
    {
        GameObject holder = new GameObject();
        holder.transform.parent = transform;
        holder.transform.localPosition = new Vector3(0, 0, 0);
        holder.transform.localScale = new Vector3(1, 1, 1);
        holder.AddComponent<SightFollow>();

        for (int i = 0; i <= AOV; i++)
        {
            GameObject obj = Instantiate(sightPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            obj.transform.parent = holder.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3((i * 2) + 1, 1, (AOV * 2) + 1 - (i * 2));
            aovOBJ.Add(obj);
            obj.GetComponent<Sight>().myUnit = this;
        }

        sightHolder = holder;
    }

    /*
    * Attack 
    * virtual function
    * 
    * calculates the damage done to a target
    * 
    * @param Unit target - the unit to apply the damage to
    * @param float multiplier - multiplys the damage
    * @returns void
    */
    public virtual void Attack(Unit target, float multiplier = 1.0f)
    {
        target.Defend(damage * multiplier * attackMultiplier);
    }



    /*
    * Defend 
    * virtual function
    * 
    * recieves damage and modifies the base value
    * 
    * @param float damage - the amount of damage recieved
    * @returns void
    */
    public virtual void Defend(float damage)
    {

        //get the tile that the unit is standing on
        Tiles currentTile = GameObject.FindObjectOfType<Map>().GetTileAtPos(transform.position);

        //calculate the damage reduction
        float armourScalar = 1 - GameManagment.stats.armourCurve.Evaluate(armour) * 0.01f;

        if (ArtLink != null)
        {
            ArtLink.SetTrigger("TakeDamage");
        }

        //the armour scalar affects the damage output
        health -= damage * armourScalar;

        Debug.Log(gameObject.name + " was attacked for " + (damage * armourScalar).ToString() + " damage.");

        //has the unit died from the hit
        if (health <= 0.0f)
        {
            health = 0.0f;
            Execute(GameManagment.eActionType.DEATH, currentTile, null, null, false);
        }
    }



    /*
    * Heal 
    * virtual function
    * 
    * recieves healing
    * 
    * @param float points - the amount of health healed
    * @return void
    */
    public virtual void Heal(float points)
    {
        //add the points
        health += points;
        
        //clamp the max health
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        Debug.Log(gameObject.name + " was healed for " + points.ToString() + " health.");
    }
    

    /*
    * Execute 
    * virtual function
    * 
    * base function for adding commands to the unit
    * 
    * @param GameManagement.eActionType actionType - the type of action to execute
    * @param Tiles st - the first tile selected
    * @param Tiles et - the last tile selected
    * @param UnitCommand.VoidFunc callback - function reference to invoke if the command completes
    * @param safeMove - Determins if we are doing a safe move or not
    * @returns void
    */
    public virtual void Execute(GameManagment.eActionType actionType, Tiles st, Tiles et, UnitCommand.VoidFunc callback, bool safeMove)
    {

    }


    /*
    * isDead 
    *
    * checks if the unit has positive health
    * 
    * @returns bool - the result of the check (positive if dead)
    */
    public bool isDead()
    {
        return health > 0.0f;
    }


    /*
    * IsBusy 
    * 
    * checks if the unit is still executing UnitCommands
    * 
    * @returns bool - the result of the check (positive if still running)
    */
    public bool IsBusy()
    {
        return commands.Count > 0 && health > 0.0f;
    }


    /*
    * OnCommandFinish
    * virtual function
    * 
    * called when the unit's latest command finishes
    * 
    * @returns void
    */
    public virtual void OnCommandFinish()
    {
        //remove the latest command
        commands.RemoveAt(0);
    }



    /*
    * OnCommandFailed
    * 
    * called when the unit's latest command fails
    * 
    * @returns void
    */
    public void OnCommandFailed()
    {
        //remove all commands as they may depend on the one that just failed being successful
        commands.Clear();
    }
    /* 
     * Billboard
     * called at the end of each turn
     * rotates all health bars to face the camera
     * 
     * 
     */
    public void Billboard(Camera cam)
    {
        HpTransform.rotation = cam.transform.rotation;
    }


   
    /*  
    *   HealthUpdate
    *   
    *    TODO change this to update in sync with the anims
    *   
    *   runs each update to show the health of the unit in a bar & text
    *   manages armour points for the HP bar as well
    *   @returns void
    */  
    public virtual void HealthUpdate()
    {
        if (armour > 5)
        {
            armour = 5;
        }

        Vector2 shield1 = new Vector2(2.5f, 2.5f);
        Vector2 shield2 = new Vector2(5.0f, 2.5f);
        Vector2 shield3 = new Vector2(7.5f, 2.5f);
        Vector2 shield4 = new Vector2(10.0f, 2.5f);
        Vector2 shield5 = new Vector2(12.8f, 2.5f);

        switch(armour)
        {
            case 0:
                armourBar.size = new Vector2(0f, 2.5f);
                break;

            case 1:
                armourBar.size = shield1;
                break;

            case 2:
                armourBar.size = shield2;
                break;

            case 3:
                armourBar.size = shield3;
                break;

            case 4:
                armourBar.size = shield4;
                break;

            case 5:
                armourBar.size = shield5;
                break;
        }

        float hpPercent = 0;
        
        if (attackMultiplier > 1.0f)
        {
            kingBuff.gameObject.SetActive(true);
        }
        else
        {
            kingBuff.gameObject.SetActive(false);
        }

        hpPercent = health / maxHealth;

        hpBar.fillAmount = hpPercent;

        hpNum.text = (int)health + " / " + (int)maxHealth;

    }
}
