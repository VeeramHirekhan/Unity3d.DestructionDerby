using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    

public class AudioSelection : MonoBehaviour {

    private AudioSource musicSource;
    public AudioClip[] songs;
    private AudioClip chosenSong;

	// Use this for initialization
	void Awake () {
        musicSource = GetComponent<AudioSource>();


	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (!musicSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }

    void PlayRandomMusic() {
        int randSelc = Random.Range(0, songs.Length);
        musicSource.PlayOneShot(songs[randSelc],.8F);

    }

}
