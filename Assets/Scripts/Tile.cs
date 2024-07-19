using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;

public class Tile : MonoBehaviour
{
    public Sprite default_cell;
    public Sprite red_cell;

    public float step;
    public float duration;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            if (spriteRenderer.sprite != red_cell) {
                spriteRenderer.sprite = red_cell;
                SampleSequencer sequencer = GameObject.Find("DrumSquencer").GetComponent<SampleSequencer>();
                if (sequencer != null) {
                    float noteStart = step;
                    sequencer.AddNote(48, noteStart, noteStart + duration);
                }
            }
            else if (spriteRenderer.sprite == red_cell) {
                spriteRenderer.sprite = default_cell;
                SampleSequencer sequencer = GameObject.Find("DrumSquencer").GetComponent<SampleSequencer>();
                if (sequencer != null) {
                    float noteStart = step;
                    sequencer.RemoveNotesContainedInRange(48, noteStart, noteStart + duration);
                }
            }
        }
    }
}
