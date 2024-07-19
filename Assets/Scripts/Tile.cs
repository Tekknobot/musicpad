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

    private bool rotating = false;
    private float rotationSpeed = 300f; // Adjust speed as needed  
    private float totalRotation = 0f;  
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rotating) {
            float degreesToRotate = rotationSpeed * Time.deltaTime;
            RotateSpriteY(degreesToRotate); // Rotate continuously
            totalRotation += Mathf.Abs(degreesToRotate);

            if (totalRotation >= 360f) {
                StopRotation();
            }
        }     
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
                    StartRotation();
                }
            }
            else if (spriteRenderer.sprite == red_cell) {
                spriteRenderer.sprite = default_cell;
                SampleSequencer sequencer = GameObject.Find("DrumSquencer").GetComponent<SampleSequencer>();
                if (sequencer != null) {
                    float noteStart = step;
                    sequencer.RemoveNotesContainedInRange(48, noteStart, noteStart + duration);
                    StartRotation();
                }
            }
        }
    }

    void StartRotation() {
        rotating = true;
        totalRotation = 0f; // Reset total rotation
    }

    void StopRotation() {
        rotating = false;
    }

    void RotateSpriteY(float degreesPerSecond) {
        transform.Rotate(Vector3.up * degreesPerSecond);
    }
}
