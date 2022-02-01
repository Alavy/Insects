using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectorBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speedValue = 1f;

    private Rigidbody2D m_rigidbody2D;
    private Vector2 m_moveDir = new Vector2(1, 0);
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        //m_rigidbody2D.AddForce(new Vector2(1, 0) * ForceValue);
    }
    private void FixedUpdate()
    {
        /*
        if (transform.position.x > 2.0f)
        {
            m_rigidbody2D.velocity = Vector2.zero;
            m_rigidbody2D.AddForce(new Vector2(-1, 0) * ForceValue);
        }
        else if (transform.position.x < -2.0f)
        {
            m_rigidbody2D.velocity = Vector2.zero;

            m_rigidbody2D.AddForce(new Vector2(1, 0) * ForceValue);

        }
        */
        if (transform.position.x > 2.0f)
        {
            m_moveDir = new Vector2(-1, 0);
        }
        else if (transform.position.x < -2.0f)
        {
            m_moveDir = new Vector2(1, 0);

        }
        m_rigidbody2D.MovePosition(m_rigidbody2D.position + m_moveDir * Time.fixedDeltaTime * speedValue);
    }
}
