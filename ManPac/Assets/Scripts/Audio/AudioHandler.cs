using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private FMOD.Studio.EventInstance _defaultMusicPath;

    private FMOD.Studio.EventInstance _angryMusicPath;

    private void Start()
    {
        _defaultMusicPath = FMODUnity.RuntimeManager.CreateInstance("event:/LevelMusic");
        _angryMusicPath = FMODUnity.RuntimeManager.CreateInstance("event:/AngryMusic");

        _defaultMusicPath.start();
    }

    private void SwitchTrack(bool isAngry)
    {
        StopAllMusic();
        
        if (isAngry)
        {
            _angryMusicPath.start();
        }
        else
        {
            _defaultMusicPath.start();
        }
    }

    public void StopAllMusic()
    {
        _defaultMusicPath.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _angryMusicPath.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void OnManPacStateChange(ManPacStates currentState)
    {
        SwitchTrack(currentState == ManPacStates.Aggressive);
    }

    public void PlayDivaDeathSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/DivaDeath");
    }

    public void PlayGhostDeathSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/GhostDeath");
    }

    public void PlayPelletPickupNoise()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/PickUpBasicPellet");
    }
}
