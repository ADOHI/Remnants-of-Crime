using UnityEngine;

namespace Roc.Combats.Enemies.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [Header("Projectile Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private bool destroyOnHit = true;
        [SerializeField] private LayerMask hitLayers = -1; // 모든 레이어와 충돌

        [Header("Visual Effects")]
        [SerializeField] private GameObject hitEffect; // 충돌 시 효과 (선택사항)
        [SerializeField] private TrailRenderer trailRenderer; // 궤적 효과 (선택사항)

        // 투사체 상태
        private Vector3 direction;
        private bool isInitialized = false;
        private float currentLifetime = 0f;

        void Start()
        {
            // 궤적 효과 초기화
            if (trailRenderer != null)
            {
                trailRenderer.enabled = true;
            }
        }

        void Update()
        {
            if (!isInitialized) return;

            // 투사체 이동
            transform.position += direction * speed * Time.deltaTime;

            // 수명 체크
            currentLifetime += Time.deltaTime;
            if (currentLifetime >= lifetime)
            {
                DestroyProjectile();
            }
        }

        /// <summary>
        /// 투사체를 초기화하고 발사 방향을 설정합니다.
        /// </summary>
        /// <param name="direction">발사 방향 (정규화된 벡터)</param>
        public void Initialize(Vector3 direction)
        {
            this.direction = direction.normalized;
            isInitialized = true;
            currentLifetime = 0f;

            // 투사체가 이동 방향을 바라보도록 회전
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        /// <summary>
        /// 투사체를 초기화하고 발사 방향과 속도를 설정합니다.
        /// </summary>
        /// <param name="direction">발사 방향 (정규화된 벡터)</param>
        /// <param name="speed">발사 속도</param>
        public void Initialize(Vector3 direction, float speed)
        {
            this.speed = speed;
            Initialize(direction);
        }

        void OnTriggerEnter(Collider other)
        {
            // 레이어 체크
            if (((1 << other.gameObject.layer) & hitLayers) == 0) return;

            // 충돌 처리
            HandleHit(other);
        }

        void OnCollisionEnter(Collision collision)
        {
            // 레이어 체크
            if (((1 << collision.gameObject.layer) & hitLayers) == 0) return;

            // 충돌 처리
            HandleHit(collision.collider);
        }

        private void HandleHit(Collider hitCollider)
        {
            // 충돌 효과 생성
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // 충돌한 오브젝트에 데미지나 효과를 줄 수 있는 부분
            // 예: hitCollider.GetComponent<Health>()?.TakeDamage(damage);

            Debug.Log($"Projectile hit: {hitCollider.name}");

            // 충돌 시 파괴
            if (destroyOnHit)
            {
                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            // 궤적 효과 정리
            if (trailRenderer != null)
            {
                trailRenderer.enabled = false;
                trailRenderer.transform.SetParent(null);
                Destroy(trailRenderer.gameObject, trailRenderer.time);
            }

            Destroy(gameObject);
        }

        // 외부에서 접근 가능한 프로퍼티들
        public bool IsInitialized => isInitialized;
        public Vector3 Direction => direction;
        public float Speed => speed;
    }
}

