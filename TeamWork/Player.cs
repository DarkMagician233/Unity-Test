using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    //[SerializeField] bool       m_noBlood = false;
    //[SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;                 //地面检测
    /*
     * private Sensor_HeroKnight   m_wallSensorR1;
     * private Sensor_HeroKnight   m_wallSensorR2;
     * private Sensor_HeroKnight   m_wallSensorL1;
     * private Sensor_HeroKnight   m_wallSensorL2;
     */
    private Attack PingA;
    private bool move = true;
    private bool                m_isWallSliding = false;                    //滑墙判断
    private bool                m_grounded = false;                        //着地判断
    private bool                m_rolling = false;                             //翻滚
    private bool                m_isTouchingPlatform = false;         //平台跳跃
    private int                 m_facingDirection = 1;                      //朝向
    private int                 m_currentAttack = 0;                        //攻击段数
    private float               m_timeSinceAttack = 0.0f;               //attack计时器
    private float               m_rollDuration = 8.0f / 14.0f;          //roll持续时间
    private float               m_rollCurrentTime = 0.0f;                //roll计时器
    private float               m_blockStartTime = 0.1f;                 //防御开始时间
    private float               m_blockTime = 0.0f;                        //防御持续时间

    //Animator Parameters注解：
    /*
     * AnimState：控制run与idle切换；
     * Attach1/2/3：攻击段数
     * Block：架盾
     * IdleBlock：站立持盾
     * Hurt：受击
     * Death：死亡
     * noBlood： 出血效果
     * AirSpeedY：Y轴fall下落
     * Grounded：触地
     * Jump：跳跃
     * Roll：翻滚
     * WallSlide：滑墙
    */

    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();                  //地面检测
        /* ------滑墙-------
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        */
        PingA = GameObject.Find("Attack").GetComponent<Attack>();               //用于传递攻击按键输入到Attack脚本中
    }

    // Update is called once per frame
    void Update ()
    {

        //attack计时器
        m_timeSinceAttack += Time.deltaTime;

        //block计时器
        m_blockTime += Time.deltaTime;

        //roll计时器
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;


        //之前这里计时器出了问题：计时器m_rollCurrentTime未重置导致roll没几秒就终止。
        //已解决。
        //计时器注意要重置
        if (m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
            m_rollCurrentTime = 0.0f;
        }
            
        //检测角色接触地面
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //检测角色离开地面
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- 输入以及运动 --
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        //移动方向与角色朝向
        if (inputX > 0)
        {
            GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
            m_facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
            m_facingDirection = -1;
        }


        //---- Move ----
        if (!m_rolling )
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //AirSpeed 空中速度
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // ---- 动画器控制 ----
        {
            //---- Wall Slide ----
            /*
            m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
            m_animator.SetBool("WallSlide", m_isWallSliding);
            */

            // ---- Death ----
            /* 
            if (Input.GetKeyDown("e") && !m_rolling)
            {
                m_animator.SetBool("noBlood", m_noBlood);
                m_animator.SetTrigger("Death");
            }
            */
            //---- Hurt ----
            /*
            if (Input.GetKeyDown("q") && !m_rolling)
                m_animator.SetTrigger("Hurt");
            */

            //---- Attack ----
            if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
            {
                //限制攻击时不可移动
                move = false;
                MoveSpeed();
                //多段攻击
                /*
                    m_currentAttack++;

                    //循环普攻段数
                    if (m_currentAttack > 3)
                        m_currentAttack = 1;

                    //连击过时则回复1段普攻
                    if (m_timeSinceAttack > 1.0f)
                        m_currentAttack = 1;

                    //触发攻击1/2/3
                    m_animator.SetTrigger("Attack" + m_currentAttack);

                   
                */
                //单次攻击
                m_animator.SetTrigger("Attack" + 1);
                //计时器重置以控制攻击间隔
                m_timeSinceAttack = 0.0f;
                //监听按键已输入传递给Attack
                PingA.KeyDown(1);
                
            }
            if (Input.GetMouseButtonUp(0))          //松开攻击按键则触发
            {
                //监听按键未输入传递给Attack
                PingA.KeyDown(0);
                //攻击结束后恢复移动速度
                move = true;
                Invoke("MoveSpeed", 0.5f);
            }

            //---- Block ----
            if (Input.GetMouseButtonDown(1) && !m_rolling && m_blockTime - m_blockStartTime > 0.7f)
            {
                m_animator.SetTrigger("Block");
                m_animator.SetBool("IdleBlock", true);
                //限制防御时不可移动
                move = false;
                MoveSpeed();
                //防御计时器重置
                m_blockTime = 0.0f;
            }

            if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
            {
                m_animator.SetBool("IdleBlock", false);
                //防御结束后恢复移动速度
                move = true;
                Invoke("MoveSpeed", 0.4f);
            }


            //---- Roll ----
            if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
            {
                m_rolling = true;
                m_animator.SetTrigger("Roll");
                m_body2d.velocity = new Vector2(m_rollForce * m_facingDirection, m_body2d.velocity.y);
            }


            //---- Jump ----（接触地面且不在翻滚中按下空格）
            if (Input.GetKeyDown("space") && m_grounded && !m_rolling && inputY >= 0)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
            //---- Fall ----
            else if (inputY < 0 && m_isTouchingPlatform && Input.GetKeyDown("space"))
            {
                GetComponent<BoxCollider2D>().isTrigger = true;
                m_isTouchingPlatform = false;
                Invoke("ReColl", 0.5f);
            }

            //---- Run ----
            if (Mathf.Abs(inputX) > Mathf.Epsilon)
            {
                m_animator.SetInteger("AnimState", 1);
            }

            //---- Idle ----
            else
            {
                m_animator.SetInteger("AnimState", 0);
            }
        }
    }

    //跳下平台后还原碰撞体
    void ReColl()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    //用于在进行攻击防御时限制移动
    void MoveSpeed()
    {
        if (move)
            m_speed = 4.0f;
        else
            m_speed = 0.0f;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            m_isTouchingPlatform = true;
        }
    }

    // Animation Events
    // 贴墙滑下
    /*
    void AE_SlideDust()
    {
        Vector3 spawnPosition;
        //读取滑墙时手扶位置
        spawnPosition = m_wallSensorR2.transform.position;

        if (m_slideDust != null)
        {
            //设定滑墙特效（烟雾）位置
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            //让滑墙特效（烟雾）方向正常
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
    */
    
}
    
