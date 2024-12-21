using System.Collections;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsMenu: MonoBehaviour {
    public GameObject last, category, general, sound, graphics, controls, anterior, gen, idioma, sonido, calidad, raton;
    public Slider mainVolume, voices, ojbects, amient, mouse;
    public Translator font, quality, keyboard;
    void Start() {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Calidad", QualitySettings.GetQualityLevel()), true);
    }
    public void Show() {
        EventSystem.current.SetSelectedGameObject(null);
        category.SetActive(true);
        if(last) {
            last.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(gen);
    }
    public void Hide() {
        EventSystem.current.SetSelectedGameObject(null);
        if(last) {
            last.SetActive(true);
            EventSystem.current.SetSelectedGameObject(anterior);
        }
        category.SetActive(false);
    }
    public void Back() {
        EventSystem.current.SetSelectedGameObject(null);
        category.SetActive(true);
        general.SetActive(false);
        sound.SetActive(false);
        graphics.SetActive(false);
        controls.SetActive(false);
        EventSystem.current.SetSelectedGameObject(gen);
    }
    public void General() {
        EventSystem.current.SetSelectedGameObject(null);
        general.SetActive(true);
        category.SetActive(false);
        EventSystem.current.SetSelectedGameObject(idioma);
    }
    public void Sound() {
        EventSystem.current.SetSelectedGameObject(null);
        sound.SetActive(true);
        category.SetActive(false);
        mainVolume.value = PlayerPrefs.GetFloat("Volumen", 1);
        Volumes();
        EventSystem.current.SetSelectedGameObject(sonido);
    }
    public void Graphics() {
        EventSystem.current.SetSelectedGameObject(null);
        graphics.SetActive(true);
        category.SetActive(false);
        quality.value = PlayerPrefs.GetInt("Calidad", QualitySettings.GetQualityLevel()) + 19;
        font.value = PlayerPrefs.GetInt("Fuente", 0) == 0 ? 26 : 27;
        quality.Translate();
        font.Translate();
        EventSystem.current.SetSelectedGameObject(calidad);
    }
    public void Controls() {
        EventSystem.current.SetSelectedGameObject(null);
        controls.SetActive(true);
        category.SetActive(false);
        mouse.value = PlayerPrefs.GetFloat("Raton", 0.32768f);
        keyboard.value = PlayerPrefs.GetInt("Teclado", 0) + 31;
        keyboard.Translate();
        EventSystem.current.SetSelectedGameObject(raton);
    }
    public void LeftLanguaje() {
        PlayerPrefs.SetInt("Idioma", PlayerPrefs.GetInt("Idioma", 0) + (PlayerPrefs.GetInt("Idioma", 0) <= 0 ? Translator.languajes - 1 : -1));
        Translator.TranslateAll();
    }
    public void RightLanguaje() {
        PlayerPrefs.SetInt("Idioma", PlayerPrefs.GetInt("Idioma", 0) + (PlayerPrefs.GetInt("Idioma", 0) >= Translator.languajes - 1 ? 1 - Translator.languajes : 1));
        Translator.TranslateAll();
    }
    public void Delete() {
        PlayerPrefs.DeleteAll();
        if(Application.isEditor) {
            EditorApplication.isPlaying = false;
        } else {
            Application.Quit();
        }
    }
    public void RightVolume() {
        float vol = Mathf.Max(PlayerPrefs.GetFloat("Volumen", 1), Mathf.Pow(0.8f, 30)) * 1.25f;
        mainVolume.value = Mathf.Min(vol, 1);
        PlayerPrefs.SetFloat("Volumen", mainVolume.value);
    }
    public void LeftVolume() {
        float vol = Mathf.Min(PlayerPrefs.GetFloat("Volumen", 1), 1) * 0.8f;
        mainVolume.value = vol;
        PlayerPrefs.SetFloat("Volumen", vol);
    }
    void Volumes() {
        voices.value = PlayerPrefs.GetFloat("Volumen" + 1, 1);
        ojbects.value = PlayerPrefs.GetFloat("Volumen" + 2, 1);
        amient.value = PlayerPrefs.GetFloat("Volumen" + 3, 1);
    }
    public void RightSubVolume(int i) {
        if(i < 1 || i > 3) {
            throw new Exception("Indice inválido");
        }
        float val = PlayerPrefs.GetFloat("Volumen" + i, 1);
        if(val >= 1) {
            PlayerPrefs.SetFloat("Volumen" + i, 1);
            for(int j = 1; j < 4; j++) {
                if(j != i) {
                    val = PlayerPrefs.GetFloat("Volumen" + j, 1);
                    PlayerPrefs.SetFloat("Volumen" + j, Mathf.Max(0, val - 0.05f));
                }
            }
        } else {
            PlayerPrefs.SetFloat("Volumen" + i, Mathf.Min(1, val + 0.05f));
        }
        Volumes();
    }
    public void LeftSubVolume(int i) {
        if(i < 1 || i > 3) {
            throw new Exception("Indice inválido");
        }
        float val = PlayerPrefs.GetFloat("Volumen" + i, 1);
        if(val <= 0) {
            PlayerPrefs.SetFloat("Volumen" + i, 0);
            for(int j = 1; j < 4; j++) {
                if(j != i) {
                    val = PlayerPrefs.GetFloat("Volumen" + j, 1);
                    PlayerPrefs.SetFloat("Volumen" + j, Mathf.Min(1, val + 0.05f));
                }
            }
        } else {
            PlayerPrefs.SetFloat("Volumen" + i, Mathf.Max(0, val - 0.05f));
        }
        Volumes();
    }
    public void SwapFont() {
        bool i = PlayerPrefs.GetInt("Fuente", 0)==0;
        PlayerPrefs.SetInt("Fuente", i ? 1 : 0);
        font.value = i ? 27 : 26;
        foreach(Translator t in Translator.translators) {
            t.label.fontSize += i ? -15 : 15;
        }
        Translator.TranslateAll();
    }
    public void LeftQuality() {
        QualitySettings.DecreaseLevel(true);
        int i = QualitySettings.GetQualityLevel();
        PlayerPrefs.SetInt("Calidad", i);
        quality.value = i + 19;
        quality.Translate();
        PostProcessing.quality = i;
    }
    public void RightQuality() {
        QualitySettings.IncreaseLevel(true);
        int i = QualitySettings.GetQualityLevel();
        PlayerPrefs.SetInt("Calidad", i);
        quality.value = i + 19;
        quality.Translate();
        PostProcessing.quality = i;
    }
    public void RightMouse() {
        float rata = Mathf.Max(PlayerPrefs.GetFloat("Raton", 0.32768f), Mathf.Pow(0.8f, 30)) * 1.25f;
        mouse.value = Mathf.Min(rata, 1);
        PlayerPrefs.SetFloat("Raton", mouse.value);
        Movement.look = PlayerPrefs.GetFloat("Raton", 0.32768f) * 2000;
    }
    public void LeftMouse() {
        float rata = Mathf.Min(PlayerPrefs.GetFloat("Raton", 0.32768f), 1) * 0.8f;
        mouse.value = rata;
        PlayerPrefs.SetFloat("Raton", rata);
        Movement.look = PlayerPrefs.GetFloat("Raton", 0.32768f) * 2000;
    }
    public void RightKeyboard() {
        int tecla = PlayerPrefs.GetInt("Teclado", 0) + 1;
        if(tecla >= 4) {
            tecla -= 4;
        }
        keyboard.value = tecla + 31;
        keyboard.Translate();
        PlayerPrefs.SetInt("Teclado", tecla);
    }
    public void LeftKeyboard() {
        int tecla = PlayerPrefs.GetInt("Teclado", 0) - 1;
        if(tecla < 0) {
            tecla += 4;
        }
        keyboard.value = tecla + 31;
        keyboard.Translate();
        PlayerPrefs.SetInt("Teclado", tecla);
    }
}
