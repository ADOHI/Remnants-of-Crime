using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace RoC.Adohis.Combats.Characters
{
    public class TopdownCharacterController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Mouse Direction")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform directionIndicator; // 바닥 방향 표시용 UI

        [Header("Roll Settings")]
        [SerializeField] private float rollDuration = 0.5f;
        [SerializeField] private float rollSpeedMultiplier = 2f;
        [SerializeField] private float rollCooldown = 1f;
        [SerializeField] private Ease rollEase = Ease.OutQuad;

        private Rigidbody rb;
        private Vector3 moveDirection;
        private Vector3 mouseWorldPosition;
        private Vector3 mouseDirection;

        // 구르기 관련 변수
        private bool isRolling = false;
        private bool canRoll = true;
        private CancellationTokenSource rollCancellationTokenSource;

        // 마우스 방향을 외부에서 접근할 수 있도록 프로퍼티 제공
        public Vector3 MouseDirection => mouseDirection;
        public Vector3 MouseWorldPosition => mouseWorldPosition;

        void Start()
        {
            rb = GetComponent<Rigidbody>();

            // Rigidbody가 없다면 추가
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }

            // 물리 기반 움직임을 위해 설정
            rb.useGravity = false;
            rb.linearDamping = 0f;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            // 카메라가 설정되지 않았다면 메인 카메라 사용
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }

        void Update()
        {
            HandleInput();
            HandleMouseDirection();
        }

        void FixedUpdate()
        {
            if (!isRolling)
            {
                Move();
            }
        }

        private void HandleInput()
        {
            // 구르기 입력 처리
            if (Input.GetKeyDown(KeyCode.Space) && canRoll && !isRolling)
            {
                RollAsync().Forget();
            }

            // WASD 입력 처리 (구르기 중에는 무시)
            if (!isRolling)
            {
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                float verticalInput = Input.GetAxisRaw("Vertical");

                // 8방향 정규화된 벡터 생성 (XZ 평면에서 움직임)
                moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
            }
        }

        private async UniTaskVoid RollAsync()
        {
            if (isRolling || !canRoll) return;

            // 구르기 시작
            isRolling = true;
            canRoll = false;

            // 이전 토큰 취소
            rollCancellationTokenSource?.Cancel();
            rollCancellationTokenSource = new CancellationTokenSource();

            Vector3 rollDirection;

            // 현재 이동 방향을 우선으로 사용
            if (moveDirection != Vector3.zero)
            {
                rollDirection = moveDirection;
            }
            // 멈춰있으면 마우스 방향 사용
            else if (mouseDirection != Vector3.zero)
            {
                rollDirection = mouseDirection;
            }
            // 둘 다 없으면 기본값
            else
            {
                rollDirection = transform.forward;
            }

            // 구르기 애니메이션 (회전)
            transform.DORotate(new Vector3(0, 360f, 0), rollDuration, RotateMode.FastBeyond360)
                .SetEase(rollEase)
                .SetRelative(true);

            // 구르기 이동
            Vector3 targetPosition = transform.position + (rollDirection * moveSpeed * rollSpeedMultiplier * rollDuration);

            await transform.DOMove(targetPosition, rollDuration)
                .SetEase(rollEase)
                .WithCancellation(rollCancellationTokenSource.Token);

            // 구르기 종료
            isRolling = false;

            // 쿨타임 시작
            await UniTask.Delay((int)(rollCooldown * 1000), cancellationToken: rollCancellationTokenSource.Token);
            canRoll = true;
        }

        private void HandleMouseDirection()
        {
            // 쿼터뷰 카메라용 마우스 위치 감지 - 평면과의 레이캐스트 사용
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // Y=0 평면

            if (groundPlane.Raycast(ray, out float distance))
            {
                mouseWorldPosition = ray.GetPoint(distance);

                // 캐릭터에서 마우스 방향 계산 (XZ 평면에서만)
                mouseDirection = new Vector3(mouseWorldPosition.x - transform.position.x, 0f, mouseWorldPosition.z - transform.position.z).normalized;

                // 디버깅 로그
                Debug.Log($"Mouse Screen Pos: {Input.mousePosition}");
                Debug.Log($"Mouse World Pos: {mouseWorldPosition}");
                Debug.Log($"Character Pos: {transform.position}");
                Debug.Log($"Mouse Direction: {mouseDirection}");
            }

            // 방향 표시 UI 업데이트
            UpdateDirectionIndicator();
        }

        private void UpdateDirectionIndicator()
        {
            if (directionIndicator != null)
            {
                // 방향 표시 UI를 캐릭터 바닥에 위치시키고 마우스 방향으로 회전
                directionIndicator.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

                if (mouseDirection != Vector3.zero)
                {
                    directionIndicator.rotation = Quaternion.LookRotation(mouseDirection);
                }
            }
        }

        private void Move()
        {
            // Rigidbody를 사용한 움직임
            rb.linearVelocity = moveDirection * moveSpeed;
        }

        void OnDestroy()
        {
            // 토큰 정리
            rollCancellationTokenSource?.Cancel();
            rollCancellationTokenSource?.Dispose();
        }
    }
}
