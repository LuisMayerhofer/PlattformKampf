using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero1 : MonoBehaviour
{
    public GameObject StatusIcon;
    public int Health;

    private Animator anim;
    private IEnumerator couroutine;
    private bool OverrrideAttackAllowed = false;
    private bool OverrrideBlockAllowed = true;

    //Attack variables
    public Transform attackPosition1;
    [HideInInspector] public LayerMask whatIsEnemie;
    public float attackRange;
    public float attackCD;
    public float BoxX, BoxY;

    public float wAttackDelay;
    public float sAttackDelay;
    public float aAttackDelay;
    public float dAttackDelay;

    private bool AttackAllowed = false;
    private bool DefenceAllowed = true;
    private bool gizFarbeÄndern = false;

    //Defence variables
    [HideInInspector] public Enum_Block blockDir;
    [HideInInspector] public Enum_Block attackDir;
    public float defenceCD;

    //Movement variables
    public float movementSpeed;
    [HideInInspector] public float stepCD;
    [HideInInspector] public float Wdelay;
    [HideInInspector] public float Sdelay;
    [HideInInspector] public float Ddelay;
    [HideInInspector] public float Adelay;

    private Rigidbody2D rb2D;
    private bool StepAllowed = true;
    

    
    void Awake()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        
    }

    private void Start()
    {
        StatusIcon.GetComponent<statusScr>().SetSprite(2);
    }

    // Update 
    void Update()
    {
        //Health
        if(Health <= 0)
        {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StatusIcon.GetComponent<statusScr>().SetSprite(5);
            AttackAllowed = true;
            OverrrideAttackAllowed = true;
            DefenceAllowed = false;
            attackDir = Enum_Block.nothing;
            blockDir = Enum_Block.nothing;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StatusIcon.GetComponent<statusScr>().SetSprite(2);
            AttackAllowed = false;
            OverrrideAttackAllowed = false;
            DefenceAllowed = true;
            attackDir = Enum_Block.nothing;
            blockDir = Enum_Block.nothing;
        }

        //Angriff
        if (Input.GetKeyDown("w") && AttackAllowed)
            {
                AttackAllowed = false;
                attackDir = Enum_Block.w;
                anim.SetTrigger("UpTrig");
                StatusIcon.GetComponent<statusScr>().SetSprite(4);

                couroutine = AttackStepDelay(Wdelay, 0);
                StartCoroutine(couroutine);
                StartCoroutine("OverlapDelay", wAttackDelay);
                StartCoroutine("AttackCD");
            }
        else if (Input.GetKeyDown("s") && AttackAllowed)
            {
                AttackAllowed = false;
                attackDir = Enum_Block.s;
                anim.SetTrigger("DownTrig");
                StatusIcon.GetComponent<statusScr>().SetSprite(4);

                couroutine = AttackStepDelay(Sdelay, 0);
                StartCoroutine(couroutine);
                StartCoroutine("OverlapDelay", sAttackDelay);
                StartCoroutine("AttackCD");
            }
        else if (Input.GetKeyDown("d") && AttackAllowed)
            {
                AttackAllowed = false;
                attackDir = Enum_Block.d;
                anim.SetTrigger("forwardTrig");
                StatusIcon.GetComponent<statusScr>().SetSprite(4);

                couroutine = AttackStepDelay(Ddelay, 20);
                StartCoroutine(couroutine);
                StartCoroutine("OverlapDelay", dAttackDelay);
                StartCoroutine("AttackCD");
            }
        else if (Input.GetKeyDown("a") && AttackAllowed)
            {
                AttackAllowed = false;
                attackDir = Enum_Block.a;
                anim.SetTrigger("backTrig");
                StatusIcon.GetComponent<statusScr>().SetSprite(4);

                couroutine = AttackStepDelay(Adelay, 0);
                StartCoroutine(couroutine);
                StartCoroutine("OverlapDelay", aAttackDelay);
                StartCoroutine("AttackCD");
            }

        //Defence
        if (Input.GetKeyDown("w") && DefenceAllowed)
        {
            DefenceAllowed = false;
            StatusIcon.GetComponent<statusScr>().SetSprite(1);
            blockDir = Enum_Block.w;

            couroutine = AttackStepDelay(0, -200);
            StartCoroutine("DefenceCD");
        }
        else if (Input.GetKeyDown("s") && DefenceAllowed)
        {
            DefenceAllowed = false;
            StatusIcon.GetComponent<statusScr>().SetSprite(1);
            blockDir = Enum_Block.s;

            couroutine = AttackStepDelay(0, -200);
            StartCoroutine("DefenceCD");
        }
        else if (Input.GetKeyDown("d") && DefenceAllowed)
        {
            DefenceAllowed = false;
            StatusIcon.GetComponent<statusScr>().SetSprite(1);
            blockDir = Enum_Block.d;

            couroutine = AttackStepDelay(0, -200);
            StartCoroutine("DefenceCD");
        }
        else if (Input.GetKeyDown("a") && DefenceAllowed)
        {
            DefenceAllowed = false;
            StatusIcon.GetComponent<statusScr>().SetSprite(1);
            blockDir = Enum_Block.a;

            couroutine = AttackStepDelay(0, -200);
            StartCoroutine("DefenceCD");
        }
    }

    private void FixedUpdate()
    {
        //Steps
        if ((Input.GetKeyDown("c")||Input.GetKeyDown("v")) && StepAllowed)
        {
            StepAllowed = false;
            rb2D.AddForce(new Vector2(Input.GetAxisRaw("HorizontalLeft"), 0)*movementSpeed);
            if (Input.GetAxisRaw("HorizontalLeft") < 0)
                anim.SetTrigger("StepVor");
            else if(Input.GetAxisRaw("HorizontalLeft") > 0)
                anim.SetTrigger("StepRück");
            StartCoroutine("StepCD");
        }      
    }

    public void Hit(bool hit, Enum_Block sendedAttackDir)
    {
        if (hit)
        {
            Debug.Log("Hit empfangen");
            if (sendedAttackDir == blockDir)
            {
                Debug.Log("Blocked");
            }
            else
            {
                Health -= 1;
                Debug.Log("Hit");
            }
        }
    }

    //Coroutines
    IEnumerator StepCD()
    {
        yield return new WaitForSeconds(stepCD);
        StepAllowed = true;
    }

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        attackDir = Enum_Block.nothing;
        
        if(OverrrideAttackAllowed)
        {
            StatusIcon.GetComponent<statusScr>().SetSprite(5);
            gizFarbeÄndern = false;
            AttackAllowed = true;
        }
        else
        {
            StatusIcon.GetComponent<statusScr>().SetSprite(2);
            gizFarbeÄndern = false;
            AttackAllowed = false;
        }
    }

    IEnumerator AttackStepDelay(float delay, float zusatzKraft)
    {
        yield return new WaitForSeconds(delay);
        rb2D.AddForce(new Vector2(1, 0) * (movementSpeed+zusatzKraft));
    }

    IEnumerator OverlapDelay(float delay)
    {
        bool hit = false;

        yield return new WaitForSeconds(delay);
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPosition1.position, new Vector2(BoxX, BoxY), whatIsEnemie);
        gizFarbeÄndern = true;
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            try
            {
                enemiesToDamage[i].GetComponent<Hero2>().Hit(hit = true, attackDir);
            }
            catch { Debug.LogError("Enemy hasn't got a Hero component"); }
        }
        Debug.Log("Attack!");
    }

    IEnumerator DefenceCD()
    {
        yield return new WaitForSeconds(defenceCD);
        blockDir = Enum_Block.nothing;

        if(OverrrideBlockAllowed)
        {
            StatusIcon.GetComponent<statusScr>().SetSprite(2);
            DefenceAllowed = true;
        }
        else
        {
            StatusIcon.GetComponent<statusScr>().SetSprite(5);
            DefenceAllowed = false;
        }
        
    }

    //for scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (gizFarbeÄndern)
            Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackPosition1.position, new Vector3(BoxX,BoxY));
    }
}
/* ToDo:
 * 
 * 
 */