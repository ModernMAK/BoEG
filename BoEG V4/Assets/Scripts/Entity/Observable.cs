public class Observable : UnetBehaviour
{
    public void AddObserver(InvisVisionTriggerStrategy obs)
    {
    }
}

public struct VisionData
{
    //Not revealed
    public bool Visible;

    public bool Spotted;
}

/*
Vision has several states

    Fogged
    Invisible
        Hidden
        Unspotted
        Spotted
    Visible
        
        
        


 
*/