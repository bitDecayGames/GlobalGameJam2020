using FMOD.Studio;
using UnityEngine;

public partial class FMODMusicPlayer
{
	public void PlaySongIfNoneAreCurrentlyPlaying(Songs.SongName songName)
	{
		PLAYBACK_STATE playbackState = GetAndUpdatePlaybackStateOfSong();
		if (playbackState == PLAYBACK_STATE.STOPPED)
		{
			Debug.Log($"Playback state is {playbackState}, Setting song");
			SetSong(songName);
			SetPlaybackState(FMODSongState.Play);	
		}
	}
}
