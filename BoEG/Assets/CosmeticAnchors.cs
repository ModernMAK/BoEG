using System.Collections;
using UnityEngine;

namespace Assets
{
	public class CosmeticAnchors : MonoBehaviour
	{
		[SerializeField]
		private Transform _hatAnchor;
		public Transform HatAnchor => _hatAnchor;


		public void SetHat(Transform transform) => transform.SetParent(_hatAnchor);

	}
}