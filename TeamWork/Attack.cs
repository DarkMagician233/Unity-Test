using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private PolygonCollider2D attackArea;
    private GameObject newEnemy;
    private bool keyDown;
    private float tempTime = 0.0f;
    private float cd = 1.0f;
    private LayerMask Enemy;

    private void Start()
    {
        attackArea = GetComponent<PolygonCollider2D>();
        Enemy = LayerMask.GetMask("Enemy");
    }
    private void Update()
    {
        //在攻击范围内存在敌人且进行攻击动作则触发
        if(attackArea.IsTouchingLayers(Enemy) && keyDown)
        {
            //控制受击cd
            if (Time.time - tempTime > cd)
            {
                newEnemy.GetComponent<Enemy>().beAttacked(3);
                newEnemy.GetComponent<Rigidbody2D>().AddForce(newEnemy.transform.localScale * 200, 0);
                tempTime = Time.time;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //记录目前在攻击范围内的敌人
        //仅支持单目标
        if(collision.gameObject.tag =="Enemy")
        {
            newEnemy = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //反馈如今没有敌人在攻击范围内
        if (collision.gameObject.tag == "Enemy")
        {
            newEnemy = null;
        }
    }

    //检测人物时候按下攻击键
    public void KeyDown(int a)
    {
        if (a == 1)
            keyDown = true;
        else
            keyDown = false;
    }
}
