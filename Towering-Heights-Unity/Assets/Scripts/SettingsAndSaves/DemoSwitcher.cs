using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSwitcher : MonoBehaviour {
    void Awake()
    {
#if DEMO
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }
}
