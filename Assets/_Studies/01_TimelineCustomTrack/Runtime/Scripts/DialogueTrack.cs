using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ParkHaro.Studies.TimelineCustomTrack
{
    /// <summary>
    /// 대사 출력을 담당하는 커스텀 Timeline Track.
    ///
    /// TrackBindingType: 씬의 DialogueUI 컴포넌트를 바인딩 슬롯에 드래그 앤 드롭.
    /// TrackClipType: 이 Track에는 DialogueClip만 올릴 수 있음.
    ///
    /// Track 레벨에 defaultTypingSpeed를 보유하고,
    /// CreateTrackMixer에서 Clip을 순회하며 각 Clip의 최종 타이핑 속도를 계산·주입한다.
    /// </summary>
    [TrackColor(0.3f, 0.7f, 1.0f)]
    [TrackBindingType(typeof(DialogueUI))]
    [TrackClipType(typeof(DialogueClip))]
    public class DialogueTrack : TrackAsset
    {
        [Tooltip("Clip에 override가 없을 때 사용할 기본 타이핑 속도 (글자당 지연 시간, 초)")]
        public float defaultTypingSpeed = 0.04f;

        /// <summary>
        /// Timeline이 이 Track의 Mixer Playable을 요구할 때 호출.
        /// 본 Study는 단순 케이스라 별도 MixerBehaviour를 만들지 않고,
        /// 이 시점에 자식 Clip들을 순회하며 최종 타이핑 속도를 계산해 주입하는 용도로만 사용.
        /// </summary>
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            foreach (var clip in GetClips())
            {
                if (clip.asset is DialogueClip dialogueClip)
                {
                    dialogueClip.resolvedTypingSpeed = dialogueClip.typingSpeedOverride > 0f
                        ? dialogueClip.typingSpeedOverride
                        : defaultTypingSpeed;
                }
            }

            return Playable.Create(graph, inputCount);
        }
    }
}
