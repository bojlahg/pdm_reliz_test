using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get { return m_Instance; } }

    private static SoundManager m_Instance;

    [System.Serializable]
    public class Sound
    {
        public string m_Name;
        public AudioClip[] m_Variants;
    }

    public AudioMixerGroup m_AudioMixerGroup;
    public Sound[] m_Sounds;

    private Dictionary<string, Sound> m_SoundsDict = new Dictionary<string, Sound>();

    private List<AudioSource> m_Pool = new List<AudioSource>();
    private List<AudioSource> m_PlayedSounds = new List<AudioSource>();
    private Dictionary<string, AudioSource> m_PlayedLoopSounds = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        m_Instance = this;

        for (int i = 0; i < m_Sounds.Length; ++i)
        {
            if(!m_SoundsDict.ContainsKey(m_Sounds[i].m_Name))
            {
                m_SoundsDict.Add(m_Sounds[i].m_Name, m_Sounds[i]);
            }
            else 
            {
                Debug.LogErrorFormat("Duplicate sound name found: {0}", m_Sounds[i].m_Name);
            }
        }
    }

    public void PlayOnce(string n)
    {
        Sound snd;
        if(m_SoundsDict.TryGetValue(n, out snd))
        {
            Play(snd, false);
        }
        else
        {
            Debug.LogErrorFormat("PlayOnce: unknown sound name: {0}", n);
        }
    }

    public void PlayLooped(string n)
    {
        Sound snd;
        if (m_SoundsDict.TryGetValue(n, out snd))
        {
            if(!m_PlayedLoopSounds.ContainsKey(n))
            {
                Play(snd, true);
            }
            else
            {
                // Already playing
            }
        }
        else
        {
            Debug.LogErrorFormat("PlayOnce: unknown sound name: {0}", n);
        }
    }

    public void StopLooped(string n)
    {
        AudioSource audsrc;
        if (m_PlayedLoopSounds.TryGetValue(n, out audsrc))
        {
            Stop(audsrc);
            m_Pool.Add(audsrc);
            m_PlayedLoopSounds.Remove(n);
        }
    }

    private void Play(Sound snd, bool loop)
    {
        AudioSource audsrc = null;
        string objname = string.Format("Snd: {0} {1}", snd.m_Name, loop ? "(loop)" : "");
        if (m_Pool.Count > 0)
        {
            // Reuse old
            audsrc = m_Pool[m_Pool.Count - 1];
            m_Pool.RemoveAt(m_Pool.Count - 1);
            audsrc.gameObject.name = objname;
            audsrc.gameObject.SetActive(true);
        }
        else
        {
            // Create new
            GameObject newgo = new GameObject(objname, typeof(AudioSource));
            newgo.transform.SetParent(transform);
            audsrc = newgo.GetComponent<AudioSource>();
        }

        audsrc.clip = snd.m_Variants.GetRandom<AudioClip>();
        audsrc.loop = loop;
        audsrc.outputAudioMixerGroup = m_AudioMixerGroup;

        if (loop)
        {
            m_PlayedLoopSounds.Add(snd.m_Name, audsrc);
        }
        else
        {
            m_PlayedSounds.Add(audsrc);
        }

        audsrc.Play();

    }

    private void Stop(AudioSource audsrc)
    {
        audsrc.Stop();
        audsrc.gameObject.name = "Pooled";
        audsrc.gameObject.SetActive(false);
    }

    private void Update()
    {
        int removeCnt = 0;
        for(int i = 0; i < m_PlayedSounds.Count; ++i)
        {
            if(!m_PlayedSounds[i].isPlaying)
            {
                Stop(m_PlayedSounds[i]);
                m_Pool.Add(m_PlayedSounds[i]);
                m_PlayedSounds[i] = null;
                ++removeCnt;
            }
        }
        if (removeCnt > 0)
        {
            m_PlayedSounds.RemoveAll((o) => (o == null));
        }
    }
}
