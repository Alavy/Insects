using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandlerStartScene : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField]
    private GameObject quitUI;
    [SerializeField]
    private GameObject playUI;
    [SerializeField]
    private GameObject knifeStartUI;
    [SerializeField]
    private GameObject knifeTextMiddleUI;
    [SerializeField]
    private GameObject insectsTextUI;
    [SerializeField]
    private GameObject killTextUI;

    [Header("Event Time")]
    [SerializeField]
    private float knifeStartTime = 1.0f;
    [SerializeField]
    private float insectsTextTime = 1.0f;
    [SerializeField]
    private float knifeTextMiddleUiTime = 1.0f;

    [Header("Sound FX")]
    [SerializeField]
    private AudioClip knifeHooshSound;
    [SerializeField]
    private AudioClip knifeHitSound;
    [SerializeField]
    private AudioClip buttonClickSound;

    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();


        m_audioSource.PlayOneShot(knifeHooshSound);
        knifeStartUI.LeanMoveLocalY(0, knifeStartTime).setOnComplete(()=> {

            playUI.LeanMoveLocalY(-500, knifeStartTime).setEaseOutBounce();
            quitUI.LeanMoveLocalY(-500, knifeStartTime).setEaseOutBounce();


            insectsTextUI.LeanMoveLocalX(0, insectsTextTime);

            killTextUI.LeanMoveLocalX(0, insectsTextTime).setOnComplete(()=> {

                knifeTextMiddleUI.LeanMoveLocalX(0, knifeTextMiddleUiTime).setOnComplete(()=> {
                    m_audioSource.PlayOneShot(knifeHitSound);
                    
                    killTextUI.LeanMoveLocalY(400, 0.1f);
                    insectsTextUI.LeanMoveLocalY(580, 0.1f).setOnComplete(() => {

                        killTextUI.LeanScaleX(0.8f, 0.05f);
                        insectsTextUI.LeanScaleX(0.8f, 0.05f).setOnComplete(() => {

                            killTextUI.LeanScaleY(0.8f, 0.05f);
                            insectsTextUI.LeanScaleY(0.8f, 0.05f).setOnComplete(() => {

                                killTextUI.LeanScale(new Vector3(1, 1, 1), 0.08f);
                                insectsTextUI.LeanScale(new Vector3(1, 1, 1), 0.08f).setOnComplete(()=> {

                                    insectsTextUI.LeanMoveLocalX(35, 0.7f).setLoopPingPong();
                                    killTextUI.LeanMoveLocalX(-35, 0.7f).setLoopPingPong();

                                    knifeTextMiddleUI.LeanScaleX(0.8f, 0.5f).setLoopPingPong();
                                });
                            });
                        });
                    });


                }).setEaseInBack();
            });
        }).setEaseInBack();

    }

    public void QuitPointerDown()
    {
        quitUI.LeanScale(new Vector3(0.8f, 0.8f, 1.0f), 0.2f);
    }
    public void QuitPointerUp()
    {
        quitUI.LeanScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f);

    }

    public void PlayPointerDown()
    {
        playUI.LeanScale(new Vector3(0.8f, 0.8f, 1.0f), 0.2f);

    }
    public void PlayPointerUp()
    {
        playUI.LeanScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f);

        StartCoroutine(startScene());
    }

    IEnumerator startScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(1);
        while (!op.isDone)
        {

            yield return null;
        }
    }

}
