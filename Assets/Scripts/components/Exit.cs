using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public int nextScene;
    Animator anim;
    float tie;
    bool test=true;
    void Start(){
      anim=this.GetComponentInParent(typeof(Animator)) as Animator;
    }
    void OnTriggerStay(Collider other){
      if (other.GetComponent<Movement>()&&test) {
        tie+=Time.fixedDeltaTime;
        if (tie>=3) {
          StartCoroutine("Continue");
        }
      }
    }
    IEnumerator Continue(){
      test=false;
      anim.SetBool("finally",true);
      yield return new WaitForSeconds(12);
      int scindex=SceneManager.GetActiveScene().buildIndex+1;
      PlayerPrefs.SetInt("Nivel",scindex<SceneManager.sceneCountInBuildSettings?scindex:nextScene);
      PlayerPrefs.SetInt("Desbloqueado",Mathf.Max(PlayerPrefs.GetInt("Desbloqueado",1),PlayerPrefs.GetInt("Nivel")));
      AsyncOperation AsOp=SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("Nivel"));
      while (!AsOp.isDone) {
        Debug.Log(AsOp.progress);
        yield return null;
      }
    }
}
