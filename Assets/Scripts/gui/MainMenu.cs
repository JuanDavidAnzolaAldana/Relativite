using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu: MonoBehaviour {
    bool nostar = true;
    public bool follow = true;
    public GameObject text, main, play, lev, comenzar, jugar, atras;
    public Animator anim;
    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        anim.SetInteger("state", 0);
        StartCoroutine(Loop());
        if(PlayerPrefs.GetInt("Nivel", 1) < 1) {
            PlayerPrefs.SetInt("Nivel", 1);
        }
    }
    IEnumerator Loop() {
        while(nostar) {
            text.SetActive(true);
            yield return new WaitForSeconds(1);
            text.SetActive(false);
            yield return new WaitForSeconds(1);
        }
        anim.SetInteger("state", 1);
        EventSystem.current.SetSelectedGameObject(jugar);
    }
    void Update() {
        if(Input.anyKey) {
            nostar = false;
        }
    }
    public void Close() {
        EventSystem.current.SetSelectedGameObject(null);
        main.SetActive(true);
        EventSystem.current.SetSelectedGameObject(jugar);
        play.SetActive(false);
    }
    public void Begin() {
        if(PlayerPrefs.GetInt("Nivel", 1) == 1 && PlayerPrefs.GetInt("Desbloqueado", 1) == 1) {
            Run(1);
        } else {
            EventSystem.current.SetSelectedGameObject(null);
            play.SetActive(true);
            EventSystem.current.SetSelectedGameObject(comenzar);
            main.SetActive(false);
        }
    }
    public void Run(int a) {
        play.SetActive(false);
        main.SetActive(false);
        lev.SetActive(false);
        a = (a < 1 ? PlayerPrefs.GetInt("Nivel", 1) : a);
        PlayerPrefs.SetInt("Nivel", a);
        anim.SetInteger("state", 2);
        StartCoroutine(Wait(a));
    }
    public void Select() {

        EventSystem.current.SetSelectedGameObject(null);
        lev.SetActive(true);
        EventSystem.current.SetSelectedGameObject(atras);
    }
    public void Back() {
        EventSystem.current.SetSelectedGameObject(null);
        lev.SetActive(false);
        EventSystem.current.SetSelectedGameObject(comenzar);
    }
    public void Quit() {
        if(Application.isEditor) {
            EditorApplication.isPlaying = false;
        } else {
            Application.Quit();
        }
    }
    IEnumerator Wait(int b) {
        while(follow) {
            yield return null;
        }
        AsyncOperation AsOp = SceneManager.LoadSceneAsync(b);
        while(!AsOp.isDone) {
            Debug.Log(AsOp.progress);
            yield return null;
        }
    }
}
