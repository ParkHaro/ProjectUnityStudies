using UnityEngine;
using UnityEngine.Playables;

namespace ParkHaro.Studies.TimelineCustomTrack
{
    /// <summary>
    /// DialogueTrack에 올라가는 Clip. Timeline 에디터에서 사용자가 편집하는 데이터 단위.
    ///
    /// 데이터 필드는 Inspector에 노출되며, CreatePlayable에서 DialogueBehaviour 인스턴스에 주입된다.
    /// typingSpeedOverride는 0 이하일 때 Track의 defaultTypingSpeed를 사용하고, 양수일 때만 override.
    /// resolvedTypingSpeed는 DialogueTrack.CreateTrackMixer에서 계산 후 주입되는 최종값이며,
    /// 디버그 편의를 위해 Inspector에 노출한다.
    /// </summary>
    public class DialogueClip : PlayableAsset
    {
        [Tooltip("화자 이름")]
        public string speaker;

        [TextArea(2, 5)]
        [Tooltip("대사 내용")]
        public string text;

        [Tooltip("글자당 지연 시간(초). 0 이하면 Track의 defaultTypingSpeed 사용.")]
        public float typingSpeedOverride = -1f;

        [Tooltip("Track이 계산해 주입한 최종 타이핑 속도. 런타임 디버그용 (직접 편집하지 말 것).")]
        public float resolvedTypingSpeed;

        /// <summary>
        /// Timeline이 재생 가능한 Playable 인스턴스를 요구할 때 호출.
        /// DialogueBehaviour를 생성하고 Clip의 데이터를 복사해 주입한다.
        /// </summary>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();

            behaviour.speaker = speaker;
            behaviour.text = text;
            behaviour.typingSpeed = resolvedTypingSpeed;

            return playable;
        }
    }
}
