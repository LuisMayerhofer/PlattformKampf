using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero2 : MonoBehaviour
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

    public float upAttackDelay;
    public float downttackDelay;
    public float backAttackDelay;
    public float frontAttackDelay;

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
    [HideInInspector] public float Updelay;
    [HideInInspector] public float Downdelay;
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
        if (Health <= 0)
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
}
