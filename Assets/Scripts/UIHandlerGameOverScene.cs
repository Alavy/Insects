using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandlerGameOverScene : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private GameObject restartUI;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.LeanMoveLocalY(450, 0.4f);
        restartUI.LeanMoveLocalY(-450f, 0.4f).setOnComplete(()=> {
            restartUI.LeanScale(new Vector3(0.9f, 0.9f, 1), 0.5f).setLoopPingPong();
            gameOverText.LeanScale(new Vector3(0.9f, 0.9f, 1), 0.4f).setLoopPingPong();

        });
    }
    public void RestartPointerDown()
    {
        restartUI.LeanScale(new Vector3(0.8f, 0.8f, 1.0f), 0.2f);

    }
    public void RestartPointerUp()
    {
        restartUI.LeanScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f);

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
