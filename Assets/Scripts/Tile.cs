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
    private float rotationSpeed = 250f; // Adjust speed as needed  
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

            if (totalRotation >= 180f) {
                StopRotation();
            }
        }     
    }

    void OnMouseDown() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            SampleSequencer sequencer = GameObject.Find("DrumSquencer").GetComponent<SampleSequencer>();
            if (sequencer != null) {
                float noteStart = step;
                float noteEnd = noteStart + duration;

                if (spriteRenderer.sprite != red_cell) {
                    spriteRenderer.sprite = red_cell;
                    sequencer.AddNote(48, noteStart, noteEnd);
                    StartRotation();
                }
                else if (spriteRenderer.sprite == red_cell) {
                    spriteRenderer.sprite = default_cell;
                    sequencer.RemoveNotesContainedInRange(48, noteStart, noteEnd);
                    StartRotation();
                }

                Debug.Log($"Adding note: Pitch = 48, Start = {noteStart}, End = {noteEnd}");
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
