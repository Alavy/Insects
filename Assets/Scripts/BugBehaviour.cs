using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Algine.Insects
{
    public class BugBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float minDistance = 0.2f;
        [SerializeField]
        private float speedFactor = 1.0f;
        [SerializeField]
        private float yAxisMagnitude = 0.5f;
        [SerializeField]
        private float sinSpeed = 4.0f;
        [SerializeField]
        private Vector2 axisOffsetInScreenCord = new Vector2(100,400);

        private Animator m_animator;
        private Rigidbody2D m_rgbody;

        private Vector2 m_startPos;
        private Vector2 m_endPos;

        private Vector2 m_currentTarget;


        private void Start()
        {
            m_animator = GetComponent<Animator>();
            m_rgbody = GetComponent<Rigidbody2D>();
            m_startPos = Camera.main.ScreenToWorldPoint(
                new Vector3(-axisOffsetInScreenCord.x, Screen.height -axisOffsetInScreenCord.y, transform.position.z)
                );
            m_endPos = Camera.main.ScreenToWorldPoint(
                new Vector3(Screen.width+ axisOffsetInScreenCord.x, Screen.height-axisOffsetInScreenCord.y, transform.position.z)
                );

            transform.position = new Vector3 (m_startPos.x,
                m_startPos.y,transform.
                position.z);

            m_currentTarget = m_endPos;
        }
        private void FixedUpdate()
        {
            if(Vector2.Distance(transform.position,m_endPos) < minDistance)
            {
                m_currentTarget = m_startPos;
            }
            else if(Vector2.Distance(transform.position, m_startPos) < minDistance)
            {
                m_currentTarget = m_endPos;
            }
            m_rgbody.MovePosition(Vector2.Lerp(transform.position, 
                new Vector2(m_currentTarget.x, m_currentTarget.y + Mathf.Sin(Time.time* sinSpeed) * yAxisMagnitude)
                , Time.deltaTime/ speedFactor));
        }
        public void Dead()
        {
            m_animator.SetBool("isDead",true);
            StartCoroutine(DestroyObjectAfter(1.0f));
        }
        IEnumerator DestroyObjectAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }
    }
}

