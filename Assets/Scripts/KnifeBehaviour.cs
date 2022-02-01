using System.Collections;
using UnityEngine;
using System;

namespace Algine.Insects
{
    public class KnifeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float forceValue = 20;

        private Rigidbody2D m_rgBody;
        private BoxCollider2D m_collider2D;

        private Action<Transform,string> m_OnCollideWith;

        private Coroutine m_coroutine;

        private void Start()
        {
            m_rgBody = GetComponent<Rigidbody2D>();
            m_collider2D = GetComponent<BoxCollider2D>();
        }
        public KnifeBehaviour Throw()
        {
            m_rgBody.AddForce(Vector2.up * forceValue);
            m_coroutine = StartCoroutine(DestroyObjectAfter(3.0f));
            return this;
        }
        public void SetOnCollideWith(Action<Transform,string> onCollide)
        {
            m_OnCollideWith = onCollide;
        }
       
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Knife"))
            {
                Destroy(m_collider2D);
                m_rgBody.AddForce(other.GetContact(0).normal * 3000f);
                m_OnCollideWith?.Invoke(other.transform, "Knife");
                

            }
            else if (other.collider.CompareTag("Deflector"))
            {
                if (Mathf.Abs(transform.position.x - other.collider.transform.position.x)<0.8f)
                {
                    StopCoroutine(m_coroutine);
                    transform.SetParent(other.transform);
                    //rgBody.isKinematic = true;
                    //rgBody.velocity = Vector2.zero;
                    m_OnCollideWith?.Invoke(other.transform, "Deflector");
                    Destroy(m_rgBody);
                    Destroy(this);
                }
                

            }
            
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.gameObject.GetComponent<BugBehaviour>().Dead();
            m_OnCollideWith?.Invoke(other.transform,"Bug");
            Destroy(gameObject);
        }
        IEnumerator DestroyObjectAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }

    }

}
