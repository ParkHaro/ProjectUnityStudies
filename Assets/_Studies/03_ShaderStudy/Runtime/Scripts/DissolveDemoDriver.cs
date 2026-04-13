using UnityEngine;

namespace ParkHaro.Studies.ShaderStudy
{
    /// <summary>
    /// Dissolve 머티리얼의 _DissolveAmount 값을 시간에 따라 자동으로 왕복시키는 드라이버.
    /// 씬 열고 Play하면 녹았다 복원되는 애니메이션이 반복된다.
    ///
    /// MaterialPropertyBlock을 사용해 머티리얼 원본을 수정하지 않고 인스턴스 값만 변경.
    /// (여러 오브젝트가 같은 머티리얼을 공유해도 각자 다른 값을 가질 수 있게 함)
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class DissolveDemoDriver : MonoBehaviour
    {
        [Tooltip("왕복 한 사이클의 시간(초)")]
        [SerializeField] private float cycleDuration = 4f;

        [Tooltip("_DissolveAmount 최소값")]
        [SerializeField, Range(0f, 1f)] private float minAmount = 0f;

        [Tooltip("_DissolveAmount 최대값")]
        [SerializeField, Range(0f, 1f)] private float maxAmount = 1f;

        [Tooltip("셰이더 프로퍼티 이름 (머티리얼 설정과 일치해야 함)")]
        [SerializeField] private string propertyName = "_DissolveAmount";

        private Renderer _renderer;
        private MaterialPropertyBlock _mpb;
        private int _propertyId;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _mpb = new MaterialPropertyBlock();
            _propertyId = Shader.PropertyToID(propertyName);
        }

        private void Update()
        {
            // 0~1 왕복 (PingPong 효과)
            float t = Mathf.PingPong(Time.time / cycleDuration * 2f, 1f);
            float amount = Mathf.Lerp(minAmount, maxAmount, t);

            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetFloat(_propertyId, amount);
            _renderer.SetPropertyBlock(_mpb);
        }
    }
}
