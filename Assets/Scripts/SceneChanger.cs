using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public Animator faceAnim;

    public void Change(string sceneToChange)
    {
        faceAnim.Play("FadeToBlack");
        StartCoroutine(Delay(sceneToChange));
    } 

    IEnumerator Delay(string sceneToChange)
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(sceneToChange);
    }
}
