using UnityEngine;

public class ReplacedTileData
{
    public Sprite OldSprite { get; private set; }
    public Sprite NewSprite { get; private set; }
    public int OldStep { get; private set; }
    public int NewStep { get; private set; }

    public ReplacedTileData(Sprite oldSprite, Sprite newSprite, int oldStep, int newStep)
    {
        OldSprite = oldSprite;
        NewSprite = newSprite;
        OldStep = oldStep;
        NewStep = newStep;
    }
}