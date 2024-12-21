using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScaler: MonoBehaviour {
    void Update() {
        transform.localScale = Mathf.Min(Screen.width / 704f, Screen.height / 396f) * Vector3.one;
    }
}
