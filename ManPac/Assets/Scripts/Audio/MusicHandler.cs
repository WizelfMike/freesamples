using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class MusicHandler : MonoBehaviour
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

    private void StopAllMusic()
    {
        _defaultMusicPath.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void OnManPacStateChange(ManPacStates currentState)
    {
        SwitchTrack(currentState == ManPacStates.Aggressive);
    }
}
