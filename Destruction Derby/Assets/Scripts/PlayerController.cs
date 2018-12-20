using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour {

    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;
    public Transform WheelFLtrans;
    public Transform WheelFRtrans;
    public Transform WheelRLtrans;
    public Transform WheelRRtrans;
    public Vector3 eulertest;
    float maxFwdSpeed = -3000;
    float maxBwdSpeed = 1000f;
    //float gravity = 9.8f;
    private bool braked = false;
    //private float maxBrakeTorque = 500;
    private Rigidbody rb;
    public Transform centreofmass;
    private float maxTorque = 1000;
    public Slider healthBar;
    public Slider e1hp;
    public Slider e2hp;
    public Slider e3hp;

    public float totalHealth = 100;
    public float currentHealth = 100;
    private bool isGameOver = false;
    private bool isDead = false;
    public GameObject healthBoost;
    public ParticleSystem ps;
    public Button quitBtn;
    public Button restartBtn;
    public Button nextBtn;
    public Text playerTalk;
    public GameObject playerTalkCont;
    public float currentSpeed = 0.0f;
    private Vector3 prevPosition;

    public Scene currentScene;
		public string sceneName;

    private AudioSource theSource;
    public AudioClip engine;
    public AudioClip[] audioHitClips;
    public AudioClip[] healthClips;
    public AudioClip[] explosions;
    public AudioClip[] minorCollision;
    private float lowPitchRange = .75F;
    private float highPitchRange = 1.5F;
    private AudioClip chosenClip;
    private float volVariant = .1F;

    void Start ()
    {
        prevPosition = transform.position;
        ps = GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.enabled = false;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centreofmass.transform.localPosition;
        healthBar.value = PercentHealth();
        restartBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        currentScene = SceneManager.GetActiveScene ();
        sceneName = currentScene.name;
        if (sceneName == "Level1")
            StartCoroutine(Talk("Ah, The Destruction Derby! Finally a chance to prove I'm the best driver around!"));
        else if (sceneName == "Level2")
            StartCoroutine(Talk("Just one more round and I'll prove I'm the best driver around!"));
        theSource = GetComponent<AudioSource>();
    }

    void FixedUpdate () {
        if(!isGameOver)
        {
            if(!isDead)
            {
                if(!braked){
                    WheelFL.brakeTorque = 0;
                    WheelFR.brakeTorque = 0;
                    WheelRL.brakeTorque = 0;
                    WheelRR.brakeTorque = 0;
                }
                //speed of car, Car will move as you will provide the input to it.

                WheelRR.motorTorque = maxTorque * Input.GetAxis("Vertical");
                WheelFR.motorTorque = maxTorque * Input.GetAxis("Vertical");
                WheelFL.motorTorque = maxTorque * Input.GetAxis("Vertical");
                WheelRL.motorTorque = maxTorque * Input.GetAxis("Vertical");

                //changing car direction Here we are changing the steer angle of the front tires of the car so that we can change the car direction.
                WheelFL.steerAngle = 15 * (Input.GetAxis("Horizontal"));
                WheelFR.steerAngle = 15 * Input.GetAxis("Horizontal");
            }
            if(e1hp.value <= 0 && e2hp.value <= 0 && e3hp.value <= 0 && currentHealth > 0)
            {
                isGameOver = true;

            }
        }
    }
    void Update()
    {
        Vector3 curMov = transform.position - prevPosition;
        currentSpeed = curMov.magnitude / Time.deltaTime;
        prevPosition = transform.position;

        HandBrake();

        //for tire rotate
        WheelFLtrans.Rotate(WheelFL.rpm/60*360*Time.deltaTime ,0,0);
        WheelFRtrans.Rotate(WheelFR.rpm/60*360*Time.deltaTime ,0,0);
        WheelRLtrans.Rotate(WheelRL.rpm/60*360*Time.deltaTime ,0,0);
        WheelRRtrans.Rotate(WheelRL.rpm/60*360*Time.deltaTime ,0,0);
        //changing tire direction
        Vector3 temp = WheelFLtrans.localEulerAngles;
        Vector3 temp1 = WheelFRtrans.localEulerAngles;
        temp.y = WheelFL.steerAngle - (WheelFLtrans.localEulerAngles.z);
        WheelFLtrans.localEulerAngles = temp;
        temp1.y = WheelFR.steerAngle - WheelFRtrans.localEulerAngles.z;
        WheelFRtrans.localEulerAngles = temp1;
        eulertest = WheelFLtrans.localEulerAngles;
    }
    void HandBrake()
    {
        if(Input.GetButton("Jump"))
        {
            braked = true;
        }
        else
        {
            braked = false;
        }
        if(braked){

            WheelRL.brakeTorque = 10000;//0000;
            WheelRR.brakeTorque = 10000;//0000;
            WheelFL.brakeTorque = 10000;
            WheelFR.brakeTorque = 10000;
            WheelRL.motorTorque = 0;
            WheelRR.motorTorque = 0;
            WheelFR.motorTorque = 0;
            WheelFL.motorTorque = 0;
        }
    }
    float PercentHealth(){
        return (currentHealth / totalHealth);
    }

    void OnCollisionEnter(Collision other)
    {
        theSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        int randINT = Random.Range(0,audioHitClips.Length);
        chosenClip = audioHitClips[randINT];
        float volVAR = other.relativeVelocity.magnitude * volVariant - .5F;

        if (other.transform.tag == "Enemy")
        {
            // Then lose health because you got hit
            if(Mathf.Abs(other.gameObject.GetComponent<EnemyHPController>().currentSpeed) > Mathf.Abs(this.currentSpeed))
			{
                theSource.PlayOneShot(chosenClip, volVAR);
                currentHealth -= Mathf.Abs(other.gameObject.GetComponent<EnemyHPController>().currentSpeed * .9f);
                StartCoroutine(Talk("I got hit and it wasn't my fault!"));
            }
            else
            {
                theSource.PlayOneShot(chosenClip, volVAR);
                StartCoroutine(Talk("Yeah take that!"));
            }
        }
        if (other.transform.tag == "DeadZone")
        {
            // Game Over you fell in the water
            currentHealth -= totalHealth;
        }
        if(other.gameObject.tag == "Post")
        {
            int randINT2 = Random.Range(0, minorCollision.Length);
            chosenClip = minorCollision[randINT2];
            theSource.PlayOneShot(chosenClip, 1.5F);
            StartCoroutine(DestoryObject(other.gameObject));
        }
        // For testing
        //currentHealth -= 10;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
        }
        healthBar.value = PercentHealth();

    }

    void OnTriggerEnter(Collider other)
    {
        theSource.pitch = Random.Range(lowPitchRange, highPitchRange);
        int randINT = Random.Range(0, healthClips.Length);
        chosenClip = healthClips[randINT];

        if (other.transform.tag == "HealthUp")
        {
            // Then gain health because you got the health
            currentHealth += 20;
            if(currentHealth > totalHealth)
            {
                currentHealth = totalHealth;
            }
            theSource.PlayOneShot(chosenClip,.7F);
            healthBar.value = PercentHealth();
            Vector3 location = new Vector3(other.gameObject.transform.position.x, 2.5f, other.gameObject.transform.position.z);
            StartCoroutine(Respawn(healthBoost, location));
            StartCoroutine(Talk("Little bit of tape and it's all good!"));
            Destroy(other.gameObject);
        }
    }
    IEnumerator Respawn(GameObject item, Vector3 location)
    {
        // Wait 5secs then respawn the item at it's original location
        yield return new WaitForSeconds(5);
        Instantiate(item, location, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }
    IEnumerator DestoryObject(GameObject item)
    {
        // Wait 5secs then remove the object from play
        yield return new WaitForSeconds(5);
        Destroy(item);
    }
    IEnumerator Talk(string quote)
    {
        playerTalk.text = quote;
        playerTalkCont.SetActive(true);
        yield return new WaitForSeconds(7);
        playerTalkCont.SetActive(false);
    }

    void OnGUI(){
        GUIStyle deadLabelStyle = new GUIStyle(GUI.skin.GetStyle("label"));

        deadLabelStyle.fontSize = 40;
        deadLabelStyle.fontStyle = FontStyle.Bold;
        deadLabelStyle.normal.textColor = Color.red;
        deadLabelStyle.hover.textColor = Color.red;
        // You died
        if(isDead)
        {
            int randINT = Random.Range(0, explosions.Length);
            chosenClip = explosions[randINT];
            theSource.PlayOneShot(chosenClip, 1F);

            restartBtn.gameObject.SetActive(true);

            playerTalk.text = "I lost... Don't tell my friends..";
            playerTalkCont.SetActive(true);
            WheelFL.brakeTorque = 10000;
            WheelFR.brakeTorque = 10000;
            WheelRL.brakeTorque = 10000;
            WheelRR.brakeTorque = 10000;
            var emission = ps.emission;
            emission.enabled = true;
        }
        // You Won
        if(isGameOver)
        {

            restartBtn.gameObject.SetActive(true);
            nextBtn.gameObject.SetActive(true);
            if (sceneName == "Level1")
            {
                playerTalk.text = "I won! One step closer to proving I'm the best!";
                nextBtn.gameObject.SetActive(true);
            }
            else if (sceneName == "Level2")

            playerTalk.text = "I won! I finally proved I'm the best!";
            playerTalkCont.SetActive(true);
            WheelFL.brakeTorque = 10000;
            WheelFR.brakeTorque = 10000;
            WheelRL.brakeTorque = 10000;
            WheelRR.brakeTorque = 10000;

           
        }
    }
}
