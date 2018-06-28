using UnityEngine;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour
{

    [SerializeField] private Text _text;
    [SerializeField] private NamedEntityData _entity;

    public void SetData(NamedEntityData entity)
    {
        _entity = entity;
    }


    private void Update()
    {
        _text.text = _entity.Name;
    }
}