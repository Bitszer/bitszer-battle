using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingeltonBase<SoundManager>
{
	public AudioSource EffectAudioSource;

	[Header("SFX")]
	public AudioClip ButtonClickSound;
	public AudioClip SpawnSound;
	public AudioClip ArrowDamageSound;
	public AudioClip SwordDamageSound;
	public AudioClip SuperPowerSound;

	[Header("Background Music")]
	public AudioClip GameplayBgTrack;
	public AudioClip MainMenuBgTrack;

	public enum BackGroundState {MainMenu, Gameplay};
	public enum SoundState {Click, Spawn, ArrowDamage, SwordDamage, SuperPower};
	public enum Fade {In, Out};

	private SoundState stateSFX;

	private AudioSource BgAudioSource;

	private float Volume = 1;
	//private bool stopFading = false;

	public override void Awake()
	{
		base.Awake();
		BgAudioSource = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Plays the background music.
	/// </summary>
	/// <param name="pPlay">If true PLAY else PAUSE</param>
	/// <param name="state">Bg Music</param>
	public void PlayBgMusic(bool pPlay, BackGroundState state)
	{
		if (CentralVariables.IS_BG_MUSIC_ON) {
			if (BgAudioSource != null) {
				switch (state) {
				case BackGroundState.MainMenu:
					BgAudioSource.clip = MainMenuBgTrack;
					break;
				case BackGroundState.Gameplay:
					BgAudioSource.clip = GameplayBgTrack;
					break;
				}
				BgAudioSource.Play ();
			}
		} else {
			BgAudioSource.Pause ();
		}
	}

	/// <summary>
	/// Plays the one shot SFX.
	/// </summary>
	/// <param name="state">Effect Sound</param>
	public void PlayOneShot(SoundState state, float _volume)
	{
		if (CentralVariables.IS_SFX_ON) {
			if (EffectAudioSource != null) {
				EffectAudioSource.volume = _volume;
				switch (state) {
				case SoundState.Click:
					EffectAudioSource.PlayOneShot (ButtonClickSound);
					break;
				case SoundState.Spawn:
					EffectAudioSource.PlayOneShot (SpawnSound);
					break;
				case SoundState.SwordDamage:
					EffectAudioSource.PlayOneShot (SwordDamageSound);
					break;
				case SoundState.ArrowDamage:
					EffectAudioSource.PlayOneShot (ArrowDamageSound);
					break;
				case SoundState.SuperPower:
					EffectAudioSource.PlayOneShot (SuperPowerSound);
					break;
				}
			}
		}
	}

	/// <summary>
	/// Adjusts the volume.
	/// </summary>
	/// <param name="_volume">Volume.</param>
	public void AdjustVolume(float _volume)
	{
		Volume = _volume;
	}

	/// <summary>
	/// Fade Musics.
	/// </summary>
	/// <param name="fadeTime">Fade time</param>
	/// <param name="pFade">Fade IN or OUT</param>
	public void musicFader (float fadeTime, Fade pFade ) {
		StartCoroutine(FadeAudio(fadeTime, pFade));
	}

	// Coroutine to Fade Music.
	private IEnumerator FadeAudio (float timer, Fade fadeType) {
		float start = fadeType == Fade.In? 0.0F : Volume;
		float end = fadeType == Fade.In? Volume : 0.0f;
		float vol = 0.0f;
		float step = 1.0f/timer;

		while (vol <= 1.0f) {
			vol += step * Time.deltaTime;
			this.GetComponent<AudioSource>().volume = Mathf.Lerp(start, end, vol);
			yield return new WaitForSeconds(step * Time.deltaTime);
		}
	}
}
