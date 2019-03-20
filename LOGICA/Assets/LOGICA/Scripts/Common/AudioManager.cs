using UnityEngine;

namespace LOGICA.Common
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips = default;
        [SerializeField] private AudioSource _audioSource = default;

        public void Play(Clip clip)
        {
            _audioSource.PlayOneShot(_audioClips?[(int) clip]);
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }
        
        public float GetVolume()
        {
            return _audioSource.volume;
        }
    }

    public enum Clip
    {
        Apply,
        Cancel,
        Clear,
        Swich,
        LightOff,
        LightOn,
        StageSelect,
    }
}
