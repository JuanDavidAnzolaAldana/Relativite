using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CrossCursor: MonoBehaviour {
    public Texture2D cruz, empty, nompty, pur, ple, gre, een, blu, lue, ora, nge, selector;
    Texture2D purple, green, blue, orange;
    public bool select, p1, p2, g1, g2, b1, b2, o1, o2;
    public int px;
    public MeshRenderer render;
    public Material purp, gree, none;
    public Material[] mat;
    public GameObject pause, button;
    public static void Lock() {
        Cursor.lockState = CursorLockMode.Locked;
        Movement.play = true;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    static void Unlock() {
        Cursor.lockState = CursorLockMode.None;
        Movement.play = false;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    public void Continue() {
        Lock();
        pause.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void Return() {
        StartCoroutine(Exit());
    }
    IEnumerator Exit() {
        Time.timeScale = 1;
        AsyncOperation AsOp = SceneManager.LoadSceneAsync(0);
        while(!AsOp.isDone) {
            Debug.Log(AsOp.progress);
            yield return null;
        }
    }
    void Start() {
        Lock();
        if(render) {
            mat = render.materials;
        }
    }
    void Update() {
        if(Movement.play) {
            if(Input.GetButton("Cancel")) {
                Unlock();
                pause.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(button);
            }
        }
    }
    void OnGUI() {
        if(Movement.play) {
            if(select) {
                GUI.DrawTexture(new Rect((Screen.width - px) / 2, (Screen.height - px) / 2, px, px), selector);
            } else {
                if(p1) {
                    if(p2) {
                        purple = ple;
                    } else {
                        purple = pur;
                    }
                } else {
                    if(Input.GetButton("Stabilize") || (Input.GetButton("Fire2") && !Input.GetButton("Swap"))) {
                        purple = nompty;
                    } else {
                        purple = empty;
                    }
                }
                if(g1) {
                    if(g2) {
                        green = een;
                    } else {
                        green = gre;
                    }
                } else {
                    if(Input.GetButton("Fire1") && !Input.GetButton("Swap")) {
                        green = nompty;
                    } else {
                        green = empty;
                    }
                }
                if(b1) {
                    if(b2) {
                        blue = lue;
                    } else {
                        blue = blu;
                    }
                } else {
                    if(Input.GetButton("Fire1") && Input.GetButton("Swap")) {
                        blue = nompty;
                    } else {
                        blue = empty;
                    }
                }
                if(o1) {
                    if(o2) {
                        orange = nge;
                    } else {
                        orange = ora;
                    }
                } else {
                    if(Input.GetButton("Fire2") && Input.GetButton("Swap")) {
                        orange = nompty;
                    } else {
                        orange = empty;
                    }
                }
                GUI.DrawTexture(new Rect((Screen.width - px) / 2, (Screen.height - px) / 2, px, px), cruz);
                GUI.DrawTexture(new Rect((Screen.width - (5 * px / 6)) / 2, (Screen.height - (5 * px / 6)) / 2, px / 3, px / 3), purple);
                GUI.DrawTexture(new Rect((Screen.width + (px / 6)) / 2, (Screen.height + (px / 6)) / 2, px / 3, px / 3), green);
                GUI.DrawTexture(new Rect((Screen.width - (5 * px / 6)) / 2, (Screen.height + (px / 6)) / 2, px / 3, px / 3), blue);
                GUI.DrawTexture(new Rect((Screen.width + (px / 6)) / 2, (Screen.height - (5 * px / 6)) / 2, px / 3, px / 3), orange);
            }
            if(render) {
                if(g1 && g2) {
                    mat[3] = gree;
                } else if(p1 && p2) {
                    mat[3] = purp;
                } else {
                    mat[3] = none;
                }
                render.materials = mat;
            }
        }
    }
}
