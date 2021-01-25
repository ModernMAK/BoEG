using UnityEngine;

namespace Assets
{
	public class CosmeticHelper : MonoBehaviour
	{
		[SerializeField]
		private CosmeticAnchors _anchors;

		[SerializeField]
		private GameObject _hatCosmetic;

		private void Awake()
		{
			if(_anchors==null)
				_anchors = GetComponent<CosmeticAnchors>();
			if(_anchors==null)
			{
				Debug.Log($"'{name}' is missing '{nameof(CosmeticAnchors)}'");
				return;
			}
			if (_hatCosmetic != null)
			{
				var hat = Instantiate(_hatCosmetic);
				_anchors.SetHat(hat.transform);
			} 
		}
	}
}