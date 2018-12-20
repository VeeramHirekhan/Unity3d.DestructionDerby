using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHPController : MonoBehaviour
{

	public Slider healthBar;
	public float totalHealth = 100;
	public float currentHealth = 100;
    public float userDamage = 10;
	private bool isGameOver = false;
	public GameObject healthBoost;
    public GameObject Enemy;
	public ParticleSystem ps;
    private moveAgent ag;
	private Rigidbody rb;
	public float currentSpeed = 0.0f;
    private Vector3 prevPosition;

    private AudioSource  cpuSource;
    private AudioClip chosenClip;
    public AudioClip[] explosions;

    void Start ()
	{
		healthBar.value = PercentHealth();
		ps = GetComponent<ParticleSystem>();
        ag = GetComponent<moveAgent>();
		rb = GetComponent<Rigidbody>();
		var emission = ps.emission;
		emission.enabled = false;
		prevPosition = transform.position;

        cpuSource = GetComponent<AudioSource>();
    }

	void Update()
    {
        Vector3 curMov = transform.position - prevPosition;
        currentSpeed = curMov.magnitude / Time.deltaTime;
        prevPosition = transform.position;
    }
	void FixedUpdate ()
    {
		if(isGameOver)
		{
            int randINT = Random.Range(0, explosions.Length);
            chosenClip = explosions[randINT];
            cpuSource.PlayOneShot(chosenClip, .8F);

            var emission = ps.emission;
			emission.enabled = true;
            ag.enabled = false;
            Enemy.GetComponent<EnemyHPController>().enabled = false;
            transform.gameObject.tag = "DeadEnemy";
		}
	}
	float PercentHealth()
	{
		return (currentHealth / totalHealth);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag == "Enemy")
		{
            // Then lose health because you got hit
            //currentHealth -= 10;
			if(Mathf.Abs(other.gameObject.GetComponent<EnemyHPController>().currentSpeed) > Mathf.Abs(this.currentSpeed))
			{
				currentHealth -= Mathf.Abs(other.gameObject.GetComponent<EnemyHPController>().currentSpeed * .9f);
			}
        }

		if (other.transform.tag == "Player")
		{
			// Then lose health because you got hit
			//currentHealth -= 15;
			if(Mathf.Abs(other.gameObject.GetComponent<PlayerController>().currentSpeed) > Mathf.Abs(this.currentSpeed))
			{
				currentHealth -= Mathf.Abs(other.gameObject.GetComponent<PlayerController>().currentSpeed * .9f);
			}
		}
		if (other.transform.tag == "DeadZone")
		{
			// Game Over you fell in the water
			currentHealth -= totalHealth;
		}
		if (other.transform.tag == "HealthUp")
		{
			if (currentHealth < totalHealth)// Then gain health because you got the health
			{
				currentHealth += 20;
				Vector3 location = new Vector3(other.gameObject.transform.position.x, 2.5f, other.gameObject.transform.position.z);
				StartCoroutine(Respawn(healthBoost, location));
				Destroy(other.gameObject);
			}

			if (currentHealth > totalHealth)
			{
				currentHealth = totalHealth;
			}
		}
		// For testing
		//currentHealth -= 10;
		if(currentHealth <= 0)
		{
			currentHealth = 0;
			isGameOver = true;
		}
		healthBar.value = PercentHealth();

	}

	IEnumerator Respawn(GameObject item, Vector3 location)
	{
        GameObject HealthKit;
        // Wait 5secs then respawn the item at it's original location
		yield return new WaitForSeconds(5);
        HealthKit = Instantiate(item, location, Quaternion.Euler(new Vector3(-90, 0, 0)));

	}
}
