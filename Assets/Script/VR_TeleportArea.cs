using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using UnityEngine;

public class VR_TeleportArea : TeleportMarkerBase
{
    public override void Highlight(bool highlight)
    {
        
    }

    public override void SetAlpha(float tintAlpha, float alphaPercent)
    {
        
    }

    public override bool ShouldActivate(Vector3 playerPosition)
    {
        return true;
    }

    public override bool ShouldMovePlayer()
    {
        return true;
    }

    public override void UpdateVisuals()
    {
        
    }


}
