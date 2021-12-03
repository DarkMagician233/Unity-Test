using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//该脚本负责受击判定
public class Enemy : MonoBehaviour
{
    private int HP = 11;             //HP初始化
    // Start is called before the first frame update
    void Start()
    {
    }
    public void beAttacked(int Dmg)
    {
        //受击后的伤害判定
        HP -= Dmg;
    }

    private void Update()
    {
        //HP清零则销毁目标
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
