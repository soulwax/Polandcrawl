using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{
	#region Variables
	public float xp, yp; //actual position

	public int health;
	public int mana;
	public int damage;

    public float lerpRate = 0.1f;
    public GameObject damageText;

    private bool processMove;
	#endregion

	void Awake()
	{
		xp = transform.position.x; //Store location
		yp = transform.position.y;

        processMove = false;
	}

    void LateUpdate()
    {
        if (processMove)
        {
            ProcessMovement(lerpRate);
        }
    }

	public virtual void setPosition(float x, float y) 
	{
		xp = x;
		yp = y;
        processMove = true;
	}

    public virtual void setPositionDirectly(float x, float y)
    {
        xp = x;
        yp = y;

        transform.position = new Vector3(xp, yp, transform.position.z);
    }

    protected void ProcessMovement(float rate)
    {
        float x0 = transform.position.x;
        float y0 = transform.position.y;

        float x1 = Mathf.Lerp(x0, xp, rate);
        float y1 = Mathf.Lerp(y0, yp, rate);

        transform.position = new Vector3(x1, y1, transform.position.z); //Move to location

        if (x1 == xp && y1 == yp) processMove = false;
    }

	public Vector2 getPosition()
	{
		return new Vector2(xp, yp);
	}

	public virtual void OnAttack(int x, int y)
	{	
		Actor enemyTemp = NPCController.npcMap[x, y];
		enemyTemp.OnDamage(damage);
        
	}

	public void OnDamage(int damage)
	{
		health -= damage;

		if(health <= 0) {
			Destroy(this.gameObject);
		}
	}

    public void SpawnDamageText(int damage, Color col)
    {
        DamageText txt = damageText.GetComponent<DamageText>();
        TextMesh txtmesh = txt.GetComponent<TextMesh>();
        txtmesh.text = damage.ToString();
        txtmesh.color = col;
        Instantiate(damageText, transform.position, Quaternion.Euler(0, 0, 0));
    }
}
