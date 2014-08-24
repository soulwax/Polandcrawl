using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	#region Variables
	public float xp, yp; //actual position
    public float lerpRate = 0.1f;
    public GameObject damageText;

    public float maxHealth;
    public float maxMana;

    private bool processMove;

    private float health;
    private float mana;
    private float experience;
    private float damage;

    protected GameView view;
	#endregion

	void Awake()
	{
		xp = transform.position.x; //Store location
		yp = transform.position.y;

        //temporary stats
        Damage = 10;
        health = maxHealth;
        mana = maxMana;
        Experience = 0;

        processMove = false;

        view = GameObject.FindGameObjectWithTag("GameView").GetComponent<GameView>();
	}

    void LateUpdate()
    {
        if (processMove)
        {
            ProcessMovement(lerpRate);
        }        
    }

    /*public virtual void setPosition(float x, float y) 
    {
        xp = x;
        yp = y;
        processMove = true;
    }*/

    //I think the NPC map should contain the player aswell, NPC map rename to EntityMap or something similar?
    public virtual void setPosition(float xn, float yn)
    {
        NPCController.npcMap[(int)xp, (int)yp] = null; //Empty old location
        NPCController.npcMap[(int)xn, (int)yn] = this; //Store enemy in the new location
        xp = xn;
        yp = yn;
        processMove = true;
    }

    public virtual void setPositionDirectly(float x, float y)
    {
        xp = x;
        yp = y;

        transform.position = new Vector3(xp, yp, transform.position.z);
    }

    //Lerping is a temporary solution, I'm pretty sure there are fancier approaches
    protected void ProcessMovement(float rate)
    {
        float x0 = transform.position.x;
        float y0 = transform.position.y;

        float x1 = Mathf.Lerp(x0, xp, rate);
        float y1 = Mathf.Lerp(y0, yp, rate);

        transform.position = new Vector3(x1, y1, transform.position.z); //Move to location

        if (x1 == xp && y1 == yp) processMove = false;
    }

	public Vector2 GetPosition()
	{
		return new Vector2(xp, yp);
	}

	public virtual void OnAttack(int x, int y, float dmg)
	{	
		Actor enemyTemp = NPCController.npcMap[x, y];
        enemyTemp.OnDamage(dmg);
        
	}

	public void OnDamage(float dmg)
	{
        AdjustHealth(dmg);
        if (health <= 0) Death();
	}

    protected void SpawnDamageText(float damage, Color col)
    {
        DamageText txt = damageText.GetComponent<DamageText>();
        if (txt != null)
        {
            TextMesh txtmesh = txt.GetComponent<TextMesh>();
            if (txtmesh == null) return;
            txtmesh.text = ((int)damage).ToString();
            txtmesh.color = col;
            Instantiate(damageText, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }

    public void AdjustHealth(float damage)
    {
        this.health -= damage;
        if (this.health > maxHealth) health = maxHealth;
        if (this.health <= 0) this.health = 0;
    }

    public float Damage
    {
        get { return damage; }
        set 
        {
            if (value >= 0) this.damage = value;
        }
    }

    public float Experience
    {
        get { return experience; }
        set
        {
            this.experience = value;
            if (experience < 0) experience = 0;
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
