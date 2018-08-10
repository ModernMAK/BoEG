using UnityEngine;

namespace Old
{
    [ExecuteInEditMode]
    public class GameobjectArranger : MonoBehaviour
    {
        [SerializeField] private Vector2 _space = Vector2.one * 2f;

        [SerializeField] private bool _run;

        // Update is called once per frame
        private void Update()
        {
            if (!_run) return;
            _run = false;
            ApplyPositions();
        }

        private void ApplyPositions()
        {
            var square = Mathf.CeilToInt(Mathf.Sqrt(transform.childCount));
            var offset = square / 2;

            var pos = new Vector2Int(0, 0);

            foreach (Transform child in transform)
            {
                if (child.parent != transform)
                    continue;

                var shift = Vector2.Scale(pos, _space);
                shift -= Vector2.one * offset;

                pos.x++;
                if (pos.x >= square)
                {
                    pos.x = 0;
                    pos.y++;
                }

                var world = new Vector3(shift.x, 0f, shift.y);
                world.y = child.position.y;
                child.SetPositionAndRotation(world, Quaternion.identity);
            }
        }


        private void ApplySpiralPositions()
        {
            var dirs = new[] {Vector2.right, Vector2.up, Vector2.left, Vector2.down};
            var dirIndex = 0;
            var pos = Vector2.zero;
            int stepCounter = 0;
            int stepCounterMax = 1;
            int secondaryCounter = 0;
            const int secondaryCounterMax = 2;

            foreach (Transform child in transform)
            {
                if (child.parent != transform)
                    continue;

                var shift = Vector2.Scale(dirs[dirIndex], _space);
                pos += shift;
                stepCounter++;
                //Are we done walking?
                if (stepCounter >= stepCounterMax)
                {
                    //Turn
                    stepCounter = 0;
                    secondaryCounter++;
                    dirIndex++;
                    //Should we walk an additional step?
                    if (secondaryCounter >= secondaryCounterMax)
                    {
                        dirIndex %= 4;

                        secondaryCounter = 0;
                        stepCounterMax++;
                    }
                }

                var world = new Vector3(pos.x, 0f, pos.y);
                child.SetPositionAndRotation(world, Quaternion.identity);
            }
        }
    }
}