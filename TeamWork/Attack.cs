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
        //�ڹ�����Χ�ڴ��ڵ����ҽ��й��������򴥷�
        if(attackArea.IsTouchingLayers(Enemy) && keyDown)
        {
            //�����ܻ�cd
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
        //��¼Ŀǰ�ڹ�����Χ�ڵĵ���
        //��֧�ֵ�Ŀ��
        if(collision.gameObject.tag =="Enemy")
        {
            newEnemy = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�������û�е����ڹ�����Χ��
        if (collision.gameObject.tag == "Enemy")
        {
            newEnemy = null;
        }
    }

    //�������ʱ���¹�����
    public void KeyDown(int a)
    {
        if (a == 1)
            keyDown = true;
        else
            keyDown = false;
    }
}
