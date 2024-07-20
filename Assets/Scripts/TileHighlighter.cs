using UnityEngine;
using AudioHelm;
using System.Collections;

public class TileHighlighter : MonoBehaviour
{
    private SampleSequencer sequencer; // Reference to your SampleSequencer instance
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component of the tile

    private int previousIndex = -1; // Variable to store the previous sequencer index

    void Start()
    {
        sequencer = GameObject.Find("DrumSequencer").GetComponent<SampleSequencer>(); // Example: Find the SampleSequencer instance
        
        if (sequencer == null)
        {
            Debug.LogError("SampleSequencer instance not found!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        // Start a coroutine to poll for sequencer updates
        StartCoroutine(PollSequencer());
    }

    IEnumerator PollSequencer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // Adjust polling interval as needed

            if (sequencer != null)
            {
                int currentIndex = sequencer.currentIndex; // Get current sequencer index

                if (currentIndex != previousIndex)
                {
                    OnSequencerUpdated(currentIndex); // Trigger update if index has changed
                    previousIndex = currentIndex; // Update previous index
                }
            }
        }
    }

    private void OnSequencerUpdated(int currentIndex)
    {
        // Logic to handle sequencer update event
        Debug.Log($"Sequencer Updated: Current Index {currentIndex}");

        // Check each tile to see if its step matches the current sequencer index
        foreach (var kvp in BoardManager.Instance.GetTiles())
        {
            Tile tile = kvp.Value; // Get the Tile object from the KeyValuePair

            if (tile.Step == currentIndex)
            {
                tile.Highlight(); // Highlight the tile if its step matches the sequencer index
            }
            else
            {
                tile.Unhighlight(); // Unhighlight the tile if its step does not match the sequencer index
            }
        }
    }

    private void OnDestroy()
    {
        // Stop coroutine and clean up
        StopAllCoroutines();
    }
}
