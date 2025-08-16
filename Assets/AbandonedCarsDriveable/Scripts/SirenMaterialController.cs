using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class SirenMaterialController : MonoBehaviour
    {
        [SerializeField] private Renderer[] lodRenderers;
        [SerializeField] private Material burntMaterial;

        private Material[][] _originalMaterials;

        private void Awake()
        {
            CacheOriginalMaterials();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                CacheOriginalMaterials();
            }
        }
#endif

        private void CacheOriginalMaterials()
        {
            if (lodRenderers == null || lodRenderers.Length == 0)
                return;

            _originalMaterials = new Material[lodRenderers.Length][];
            for (int i = 0; i < lodRenderers.Length; i++)
            {
                var mats = lodRenderers[i]?.sharedMaterials;
                if (mats == null) continue;

                _originalMaterials[i] = new Material[mats.Length];
                for (int j = 0; j < mats.Length; j++)
                {
                    _originalMaterials[i][j] = mats[j];
                }
            }
        }

        public void SetBurnt()
        {
            if (lodRenderers == null || lodRenderers.Length == 0) return;

            for (int i = 0; i < lodRenderers.Length; i++)
            {
                var renderer = lodRenderers[i];
                if (renderer == null) continue;

                var original = renderer.sharedMaterials;
                if (original == null || original.Length == 0) continue;

                var updated = new Material[original.Length];

                for (int j = 0; j < original.Length; j++)
                {
                    var instance = new Material(burntMaterial);
                        instance.name = burntMaterial.name ;
                        updated[j] = instance;
                }

                renderer.sharedMaterials = updated;
            }
        }

        public void SetDefault()
        {
            if (_originalMaterials == null || lodRenderers == null)
                return;

            for (int i = 0; i < lodRenderers.Length; i++)
            {
                var renderer = lodRenderers[i];
                if (renderer == null || _originalMaterials.Length <= i || _originalMaterials[i] == null)
                    continue;

                renderer.sharedMaterials = _originalMaterials[i];
            }
        }
    }
}
