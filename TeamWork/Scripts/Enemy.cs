using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ýű������ܻ��ж�
public class Enemy : MonoBehaviour
{
    private int HP = 11;             //HP��ʼ��
    // Start is called before the first frame update
    void Start()
    {
    }
    public void beAttacked(int Dmg)
    {
        //�ܻ�����˺��ж�
        HP -= Dmg;
    }

    private void Update()
    {
        //HP����������Ŀ��
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
