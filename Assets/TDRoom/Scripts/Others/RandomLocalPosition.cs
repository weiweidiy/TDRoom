
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public class RandomLocalPosition : MonoBehaviour
    {
        [SerializeField] private Vector3 bound;

        private void OnEnable()
        {
            // var sr = this.GetComponent<SpriteRenderer>();
            // if (sr == null)
            //     return;
            // if (listRandomSprite.IsNullOrEmpty())
            //     return;
            var trans = this.transform;
            trans.localPosition = new Vector3(Random.Range(-bound.x, bound.x),
                Random.Range(-bound.y, bound.y), Random.Range(-bound.z, bound.z));
            // var targetSprite = listRandomSprite[Random.Range(0, listRandomSprite.Count)];
            // sr.sprite = targetSprite;
        }

        private void OnDrawGizmosSelected()
        {
            var size = new Vector3(bound.x, bound.y, bound.y);
            size *= 2;
            Gizmos.DrawWireCube(this.transform.position, size);
        }
    }
}