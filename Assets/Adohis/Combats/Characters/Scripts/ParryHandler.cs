using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Roc.Combats.Characters
{
    public class ParryHandler : MonoBehaviour
    {
        [Header("Parry Settings")]
        [SerializeField] private float parryDuration = 0.3f;
        [SerializeField] private float parryCooldown = 0.5f;
        [SerializeField] private Color parryColor = Color.cyan;
        [SerializeField] private GameObject parryEffect; // 패리 시각 효과 (선택사항)

        [Header("References")]
        [SerializeField] private Renderer characterRenderer; // 인스펙터에서 직접 할당

        // 패리 관련 변수
        private bool isParrying = false;
        private bool canParry = true;
        private CancellationTokenSource parryCancellationTokenSource;
        private Material originalMaterial;
        private Color originalColor; // 원본 색상을 별도로 캐싱

        void Start()
        {
            // 패리 관련 초기화
            if (characterRenderer != null)
            {
                originalMaterial = characterRenderer.material;
                originalColor = originalMaterial.color; // 원본 색상 캐싱
            }
            else
            {
                Debug.LogWarning("ParryHandler: Character Renderer가 할당되지 않았습니다!");
            }
        }

        void Update()
        {
            // 패리 입력 처리
            if (Input.GetMouseButtonDown(1) && canParry && !isParrying) // 마우스 오른쪽 클릭
            {
                ParryAsync().Forget();
            }
        }

        private async UniTaskVoid ParryAsync()
        {
            if (isParrying || !canParry) return;

            // 패리 시작
            isParrying = true;
            canParry = false;

            // 이전 토큰 취소
            parryCancellationTokenSource?.Cancel();
            parryCancellationTokenSource = new CancellationTokenSource();

            // 패리 시각 효과 시작
            StartParryEffect();

            // 패리 지속 시간 대기
            await UniTask.Delay((int)(parryDuration * 1000), cancellationToken: parryCancellationTokenSource.Token);

            // 패리 종료
            isParrying = false;
            EndParryEffect();

            // 쿨타임 시작
            await UniTask.Delay((int)(parryCooldown * 1000), cancellationToken: parryCancellationTokenSource.Token);
            canParry = true;
        }

        private void StartParryEffect()
        {
            // 캐릭터 색상 변경
            if (characterRenderer != null)
            {
                characterRenderer.material.color = parryColor;
            }

            // 패리 효과 오브젝트 활성화
            if (parryEffect != null)
            {
                parryEffect.SetActive(true);
            }

            Debug.Log("패리 모드 활성화!");
        }

        private void EndParryEffect()
        {
            // 캐릭터 색상 복원
            if (characterRenderer != null && originalMaterial != null)
            {
                characterRenderer.material.color = originalColor;
            }

            // 패리 효과 오브젝트 비활성화
            if (parryEffect != null)
            {
                parryEffect.SetActive(false);
            }

            Debug.Log("패리 모드 종료!");
        }

        // 패리 상태를 외부에서 확인할 수 있도록 프로퍼티 제공
        public bool IsParrying => isParrying;
        public bool CanParry => canParry;

        void OnDestroy()
        {
            // 토큰 정리
            parryCancellationTokenSource?.Cancel();
            parryCancellationTokenSource?.Dispose();
        }
    }
}


