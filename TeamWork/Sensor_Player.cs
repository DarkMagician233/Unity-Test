using UnityEngine;
using System.Collections;

public class Sensor_HeroKnight : MonoBehaviour {

    private int m_ColCount = 0;

    private float m_DisableTimer;

    private void Start()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        if (m_DisableTimer > 0)                                 //跳离地面计时器
            return false;
        return m_ColCount > 0;                                //接触地面是Col Count值为1，则返回值为true，即落地
    }

    void OnTriggerEnter2D(Collider2D other)         //落地则Col Count值为1
    {
        m_ColCount++;
    }

    void OnTriggerExit2D(Collider2D other)          //离地则Col Count值为0
    {
        m_ColCount--;
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}
