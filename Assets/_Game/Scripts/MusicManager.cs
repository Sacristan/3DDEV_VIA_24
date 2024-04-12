using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public enum MusicState
    {
        None,
        OutsideAmbient,
        Shelter
    }

    [System.Serializable]
    class Track
    {
        public MusicState playAtState;
        public AudioClip clip;
    }
    [SerializeField] Track[] backgroundTracks;
    AudioSource _audioSource;
    MusicState _currentMusicState = MusicState.None; //c# variable
    public MusicState CurrentMusicState //c# property
    {
        get => _currentMusicState;
        private set
        {
            if (_currentMusicState != value)
            {
                _currentMusicState = value;
                StartCoroutine(RequestChangeMusicState(value));
            }
        }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        CurrentMusicState = MusicState.OutsideAmbient;
    }

    public void PlayMusicState(MusicState musicState)
    {
        CurrentMusicState = musicState;
    }

    IEnumerator RequestChangeMusicState(MusicState musicState)
    {
        Track track = FindTrackBy(musicState);

        if (track != null)
        {
            if (_audioSource.isPlaying)
            {
                yield return FadeVolume(fadeIn: false);
            }

            _audioSource.clip = track.clip;
            _audioSource.loop = true;

            StartCoroutine(FadeVolume(fadeIn: true));
            _audioSource.Play();
        }
    }

    IEnumerator FadeVolume(bool fadeIn)
    {
        float t = 0;

        float start = fadeIn ? 0f : 1f;
        float end = fadeIn ? 1f : 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(start, end, t);
            yield return new WaitForEndOfFrame();
        }

    }

    Track FindTrackBy(MusicState musicState)
    {
        for (int i = 0; i < backgroundTracks.Length; i++)
        {
            Track track = backgroundTracks[i];
            if (track.playAtState == musicState) return track;
        }

        return null;
    }

}