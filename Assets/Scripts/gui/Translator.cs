using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Translator: MonoBehaviour {
    public TextMeshProUGUI label;
    public int value;
    public TextAsset source;
    public TMP_FontAsset weird, common;
    static string[] sources = null;
    public static int texts;
    public static int languajes;
    public static readonly HashSet<Translator> translators = new HashSet<Translator>();
    private void OnEnable() {
        if(sources==null) {
            sources = source.text.Split('\n');
            texts = sources.Length;
            languajes = sources[0].Split(';').Length;
        }
        translators.Add(this);
        Translate();
        if(PlayerPrefs.GetInt("Fuente", 0) != 0) {
            label.fontSize -= 15;
        }
    }
    private void OnDisable() {
        translators.Remove(this);
        if(PlayerPrefs.GetInt("Fuente", 0) != 0) {
            label.fontSize += 15;
        }
    }
    public void Translate() {
        label.text = sources[value].Split(';')[PlayerPrefs.GetInt("Idioma", 0)];
        label.font = (PlayerPrefs.GetInt("Fuente", 0) == 0)?weird:common;
    }
    public static void TranslateAll() {
        foreach(Translator a in translators) {
            a.Translate();
        }
    }
}
