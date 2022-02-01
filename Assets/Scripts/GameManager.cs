using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Algine.Insects
{
    public class GameManager : MonoBehaviour
    {
        [Header("Bug")]
        [SerializeField]
        private int bugCount = 7;
        [SerializeField]
        private GameObject bugObject;


        [Header("Knife")]
        [SerializeField]
        private int knifeCount = 7;
        [SerializeField]
        private float readyKinfeTime = 0.5f;
        [SerializeField]
        private GameObject knifeObject;
        [SerializeField]
        private GameObject knifeUIObject;
        [SerializeField]
        private Transform knifeHideTransform;
        [SerializeField]
        private Transform knifeSpawnTransform;

        [Header("Sound FX")]
        [SerializeField]
        private AudioClip throwSound;
        [SerializeField]
        private AudioClip bugHitSound;
        [SerializeField]
        private AudioClip knifeHitSound;
        [SerializeField]
        private AudioClip platformHitSound;

        [Header("UI")]
        [SerializeField]
        private TextMeshProUGUI bugHitScoreUIText;

        [Header("Effects FX")]
        [SerializeField]
        private ParticleSystem bloodParticle;

        [Header("References")]
        [SerializeField]
        private Transform canvasTransForm;
        [SerializeField]
        private GameObject cameraObj;


        private TouchControls m_inputActions;
        private List<GameObject> m_spawnKnifeObjects;
        private List<GameObject> m_spawnKnifeUIObjects;
        private List<GameObject> m_spawnBugObjects;
        private ParticleSystem m_spawnBlood;
        private AudioSource m_audioSource;


        // Game States
        private int m_indexKnife = 0;
        private int m_indexBug = 0;
        private int m_bugHitCount = 0;

        private bool m_isAbleToThrow = true;



        private void Awake()
        {
            m_inputActions = new TouchControls();
        }
        private void OnEnable()
        {
            m_inputActions.Enable();
        }

        private void OnDisable()
        {
            m_inputActions.Disable();
        }
        private void Start()
        {
            m_spawnKnifeObjects = new List<GameObject>();
            m_spawnKnifeUIObjects = new List<GameObject>();
            m_spawnBugObjects = new List<GameObject>();
            // spawnKnife
            for (int i = 0; i < knifeCount; i++)
            {
                GameObject temp = Instantiate(knifeObject,
                knifeSpawnTransform.position, knifeSpawnTransform.rotation);
                temp.SetActive(false);

                m_spawnKnifeObjects.Add(temp);
            }
            //spawn Knife UI objects
            for (int i = 0; i < knifeCount; i++)
            {
                GameObject temp = Instantiate(knifeUIObject, canvasTransForm);
                RectTransform rectTransform = temp.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
                    rectTransform.anchoredPosition.y+(i)*rectTransform.rect.height+ rectTransform.rect.height/4);
                m_spawnKnifeUIObjects.Add(temp);
            }
            // spawnBug
            for (int i = 0; i < bugCount; i++)
            {
                GameObject temp = Instantiate(bugObject);
                temp.SetActive(false);

                m_spawnBugObjects.Add(temp);
            }
            m_spawnBugObjects[m_indexBug].SetActive(true);
            m_spawnKnifeObjects[m_indexKnife].SetActive(true);

            m_audioSource = GetComponent<AudioSource>();

            bloodParticle.gameObject.SetActive(false);
            m_spawnBlood = Instantiate(bloodParticle);

            m_bugHitCount = bugCount;
            bugHitScoreUIText.text = (m_bugHitCount).ToString();

            m_inputActions.Touch.Throw.performed += ctx => throwKnife();
        }

        private void throwKnife()
        {
            if (m_isAbleToThrow && m_indexKnife < knifeCount)
            {
                m_spawnKnifeObjects[m_indexKnife].GetComponent<KnifeBehaviour>().Throw()
                    .SetOnCollideWith((Transform t,string collideObjectTag)=> {
                        switch (collideObjectTag)
                        {
                            case "Bug":
                                m_spawnBlood.gameObject.SetActive(true);
                                m_spawnBlood.transform.position = t.position;
                                m_spawnBlood.transform.rotation = t.rotation;
                                m_spawnBlood.Play();
                                cameraObj.LeanMoveLocal(new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0), 0.06f).setOnComplete(() => {
                                    cameraObj.LeanMoveLocal(new Vector3(0, 0, 0), 0.06f);
                                });

                                m_bugHitCount--;
                                bugHitScoreUIText.text = (m_bugHitCount).ToString();

                                bugHitScoreUIText.gameObject.LeanRotateZ(-20f, 0.09f).setOnComplete(()=> {
                                    bugHitScoreUIText.gameObject.LeanRotateZ(0, 0.09f);
                                });

                                m_audioSource.PlayOneShot(bugHitSound);
                                readyBug();
                                break;
                            case "Knife":
                                m_audioSource.PlayOneShot(knifeHitSound);
                                break;
                            case "Deflector":
                                m_audioSource.PlayOneShot(platformHitSound);
                                break;
                        }

                });
                m_spawnKnifeUIObjects[m_indexKnife].GetComponent<Image>().color = Color.black;
                m_audioSource.PlayOneShot(throwSound);
                readyKnife();
               
            }
        }
        private void readyKnife()
        {
            m_indexKnife++;
            if (m_indexKnife < knifeCount)
            {
                m_spawnKnifeObjects[m_indexKnife].SetActive(true);

                m_isAbleToThrow = false;
                m_spawnKnifeObjects[m_indexKnife].transform.position = knifeHideTransform.position;

                m_spawnKnifeObjects[m_indexKnife].LeanMove(knifeSpawnTransform.position, readyKinfeTime)
                    .setOnComplete(() => {
                        m_isAbleToThrow = true;
                    });
            }
            else
            {
                gameOver();
            }
            
        }
        private void gameOver() {

            //Time.timeScale = 0;

            StartCoroutine(startScene());

        }
        IEnumerator startScene()
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(2);
            while (!op.isDone)
            {
                yield return null;
            }
        }
        private void readyBug()
        {
            m_indexBug++;
            if (m_indexBug < bugCount)
            {
                m_spawnBugObjects[m_indexBug].SetActive(true);
                
            }
        }
    }
}
