using UnityEngine;


[CreateAssetMenu(fileName = "Named.asset", menuName = "Entity/Named")]
public class NamedEntityData : ScriptableObject
{
    [SerializeField] private string _name;

    public string Name
    {
        get { return _name; }
    }
}