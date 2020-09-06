using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    //Reference to timer script
    Timer timerRef;
    public AudioSource woodcack;
    public AudioSource upstream;
    // public vars
    public Text displayText;
    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float walkSpeed = 6;
    public float jumpForce = 220;
    public LayerMask groundedMask;
    public Animator cameraAnim;

    public GameObject[] layers;


    public static float DistanceFromTarget;  //float variable to keep distance from the target 
    public float ToTarget;

    static bool Ans1;
    static bool Ans2;
    static bool Ans3;

    bool grounded; 
    bool madeCrack;
    bool madeHole;
    static bool isCollided = false;
    static bool hasDeactivated = false;

    public int upcount = 0;
    public int downcount = 0;
    public string holeNum;
    public GameObject[] toDeactivate;
  
    bool madeCrackParticle;
    bool madeHoleparticle;

    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;
    int jumpCount = 0;
    int flyCount = 0;

    public int presentLayer = 0;
    Transform cameraTransform;
    public Rigidbody rb;
    int wincount = 0;  
    void Start()
    {
        timerRef = GetComponent<Timer>();
    }
    void Update()
    {
      
        CalculateMovement();
    
        // Jump
        if (isGrounded())
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpCount++;
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                if (jumpCount == 1)
                {                   
                    madeCrack = true;;
                    Invoke("MoveAgain", 2.5f);
                }
                else if (jumpCount == 2)
                {
                    madeHole = true;
                    woodcack.Play();
                    Invoke("MoveAgain", 2.5f);
                    jumpCount = 0;
                }

            }

            if (Input.GetKeyDown(KeyCode.F))
            {

                flyCount++;
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

                if (flyCount == 1)
                {                  
                    madeCrackParticle = true;
                    Invoke("MoveAgainParticle", 2.5f);
                }
                else if (flyCount == 2)
                {
                    madeHoleparticle = true;
                    upstream.Play();
                    Invoke("MoveAgainParticle", 2.5f);
                    flyCount = 0;
                }

            }

        }
        if (isCollided)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Key b is pressed");
                hasDeactivated = true;
            }
        }
        if (hasDeactivated)
        {
            Debug.Log("Has won");
            StopAllCoroutines();
            Reset();
            SceneManager.LoadScene("GameCompleteMenu");
        }
        if (timerRef.EndCheck())
        {
            Debug.Log("GameOver");
            StopAllCoroutines();
            Reset();
            SceneManager.LoadScene("GameOverMenu");
        }
        //if (upcount > 3 || downcount >= 6)
        //{
        //    Debug.Log("GameOver");
        //    StopAllCoroutines();
        //    Reset();
        //    SceneManager.LoadScene("GameOverMenu");
        //}
    }

    void FixedUpdate()
    {
        // Apply movement to rigidbody

        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + localMove);       
    }

    void CalculateMovement()
    {

        // Calculate movement:
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
        Vector3 targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
    }

    bool isGrounded()
    {
        // Grounded check
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
        {
            Debug.DrawRay(transform.position, -transform.up);        
            return true;
        }
        else
        {

            return false;
        }
    }

    void MoveAgain()
    {
        if (madeCrack)
            madeCrack = false;

        if (madeHole)
            madeHole = false;


    }
    void MoveAgainParticle()
    {
        if (madeCrackParticle)
            madeCrackParticle = false;

        if (madeHoleparticle)
            madeHoleparticle = false;


    }
   

    void deactivateObjForWrongPath(bool val,int changeVal,int trueVal)
    {
       
    if (changeVal == 0 && trueVal == 1/* && val == true*/)//it goes downward and layer 1 and correct answer
    {
        toDeactivate[0].SetActive(val);//object activated
        toDeactivate[1].SetActive(val);
    }
   
   
    if (changeVal == 0 && trueVal == 2/* && val == true*/)//it goes downward and layer 2 and correct answer
    {
        toDeactivate[2].SetActive(val);
        toDeactivate[3].SetActive(val);
    }
  

    if (changeVal == 1 && trueVal == 3)//it goes upward and Layer 3 
    {
        toDeactivate[0].SetActive(val);
        toDeactivate[1].SetActive(val);
    }

    if (changeVal == 0 && trueVal == 3 /*&& val == true*/)//it goes downward and layer 3 and correct answer
    {
        toDeactivate[4].SetActive(val);
    }
   
    if (changeVal == 1 && trueVal == 4)
    {
        toDeactivate[2].SetActive(val);
        toDeactivate[3].SetActive(val);
    }
            
    }
    IEnumerator GameStatus(int upDownVal)
    {
        if (presentLayer == 1 && upcount <= 3)
        {
            if(holeNum == "Hole 1")
            {
                Debug.Log("Layer 1");
                Ans1 = true;
                ++wincount;
                //Debug.Log("Wincount1" + wincount);
                deactivateObjForWrongPath(true, upDownVal, 1);
            }else
            {
                deactivateObjForWrongPath(false, upDownVal, 1);
            }
            
        }
       
        else if (presentLayer == 2 && upcount <= 3 )
        {
            if(holeNum == "Hole 3" && Ans1 == true)
            {
                Debug.Log("Layer 2");
                Ans2 = true;
                ++wincount;
                //Debug.Log("Wincount2" + wincount);
                deactivateObjForWrongPath(true, upDownVal, 2);
            }else
            {
                deactivateObjForWrongPath(false, upDownVal, 2);
            }        
          
        }
        else if (presentLayer == 3 && upcount <= 3 )
        {
            if (holeNum == "Hole 4" && Ans1 == true && Ans2 == true)
            {
                Debug.Log("Layer 3");
                Ans3 = true;
                ++wincount;
                deactivateObjForWrongPath(true, upDownVal, 3);
            }else
            {
                deactivateObjForWrongPath(false, upDownVal, 3);
            }
           
        }
        else if (presentLayer == 4 && upcount <= 5)
        {
            if (Ans1 == true && Ans2 == true && Ans3 == true)
            {
                Debug.Log("Layer 4");
                //TimerText.SetActive(true);
                ++wincount;
                Debug.Log("Wincount4" + wincount);
                deactivateObjForWrongPath(true, upDownVal, 4);
            }
 
        }

      

       
        yield return new WaitForSeconds(0.1f);
    }

    public void invokePlanetDown()
    {

        layers[presentLayer].gameObject.SetActive(false);
        layers[++presentLayer].gameObject.SetActive(true);
        Debug.Log(layers[presentLayer].name);
        StartCoroutine(GameStatus(0));
    }
    public void invokePlanetUp()
    {
        layers[presentLayer].gameObject.SetActive(false);
        layers[--presentLayer].gameObject.SetActive(true);
        Debug.Log(layers[presentLayer].name);
        StartCoroutine(GameStatus(1));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hole")
        {
            displayText.text = "Press 'Space' Hole the Ground";
        }
        if (other.gameObject.tag == "UpStream")
        {
            displayText.text = "Press 'F' for the using upstream";
        }

        if (other.gameObject.tag == "Hole" && madeCrack)
        {
        
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            cameraAnim.SetTrigger("CameraShake");

        }      
        if (other.gameObject.tag == "Hole" && madeHole)
        {
          
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            holeNum = other.gameObject.name;
            ++downcount;
            Invoke("invokePlanetDown", 0.5f);
        }
        if (other.gameObject.tag == "UpStream" && madeCrackParticle)
        {
          
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            cameraAnim.SetTrigger("CameraShake");

        }
        if (other.gameObject.tag == "UpStream" && madeHoleparticle)
        {
 
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            ++upcount;
            --wincount;
            Invoke("invokePlanetUp", 0.5f);
            Debug.Log("upcount" + upcount);

        }

        if (other.gameObject.tag == "Bomb")
        {
          
            displayText.text = "Press 'B' To deactivate Bomb";
            presentLayer += 1;
            isCollided = true;
            StartCoroutine(GameStatus(-1));          
        }
    }
  
   
    void OnTriggerExit(Collider target)
    { 
        if (target.gameObject.tag == "Hole")
        {
            displayText.text = " ";
        }
        if (target.gameObject.tag == "UpStream")
        {
            displayText.text = " ";
        }
        if (target.gameObject.tag == "Bomb")
        {
            displayText.text = " ";
        }

    }
    public void Reset()
    {
        Ans1= false;
        Ans2=false;
        Ans3= false;
        grounded =false;
       
        isCollided = false;
        hasDeactivated = false;
    }   
}


