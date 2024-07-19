using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class Highlight : MonoBehaviour
{
    public GameObject drumSequencer;
    public GameObject boardManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update() {
        SampleSequencer sequencer = drumSequencer.GetComponent<SampleSequencer>();
        
        if (sequencer == null) {
            Debug.LogWarning("SampleSequencer not found on drumSequencer GameObject.");
            return;
        }

        for (int i = 0; i < 64; i++) {
            GameObject cellObject = GameObject.Find(i.ToString());
            if (cellObject != null) {
                SpriteRenderer spriteRenderer = cellObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null) {
                    if (sequencer.currentIndex == i) {
                        spriteRenderer.color = Color.grey;
                    }
                    else {
                        spriteRenderer.color = Color.white;
                    }
                }
                else {
                    Debug.LogWarning("SpriteRenderer not found on GameObject with name " + i.ToString());
                }
            }
            else {
                Debug.LogWarning("GameObject with name " + i.ToString() + " not found.");
            }
        }
    }

}
