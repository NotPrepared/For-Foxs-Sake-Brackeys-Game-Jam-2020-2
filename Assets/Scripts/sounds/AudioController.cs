using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


// ReSharper disable once CheckNamespace
public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    private const float MAX_VOLUME = 1f;
    private const float MIN_VOLUME = 0f;

    private float baseVolume;

    public float BaseVolume
    {
        get => baseVolume;
        set
        {
            baseVolume = value;
            onVolumeChanged?.Invoke(baseVolume);
            PersistenceHandler.updateBaseVolume(baseVolume);
        }
    }

    private bool isMuted;

    public bool IsMuted
    {
        get => isMuted;
        set
        {
            isMuted = value;
            onMutedChanged?.Invoke(isMuted);
        }
    }

    public VolumeEvent onVolumeChanged;
    public MuteEvent onMutedChanged;

    [Serializable]
    public class VolumeEvent : UnityEvent<float>
    {
    }

    [Serializable]
    public class MuteEvent : UnityEvent<bool>
    {
    }

    [SerializeField] [Range(MIN_VOLUME, MAX_VOLUME)]
    private float musicVolume;


    public AudioTrack[] tracks;


    private Hashtable audioTable; // relationship of audio types (key) and tracks (value)
    private Hashtable jobTable; // relationship between audio types (key) and jobs (value)

    private void Awake()
    {
        if (instance) return;
        // else configure
        instance = this;
        if (onVolumeChanged != null)
        {
            onVolumeChanged = new VolumeEvent();
        }
        if (onMutedChanged != null)
        {
            onMutedChanged = new MuteEvent();
        }
        BaseVolume = PersistenceHandler.getBaseVolume(MAX_VOLUME);
        musicVolume = PersistenceHandler.getMusicVolume(MAX_VOLUME);
        audioTable = new Hashtable();
        jobTable = new Hashtable();
        GenerateAudioTable();
    }

    private void OnDisable()
    {
        foreach (var job in from DictionaryEntry entry in jobTable select (IEnumerator) entry.Value)
        {
            StopCoroutine(job);
        }
    }

    public void PlayAudio(GameAudioType type, bool fade = false, float delay = 0.0F)
    {
        AddJob(new AudioJob(AudioAction.START, type, fade, delay));
    }

    public void StopAudio(GameAudioType type, bool fade = false, float delay = 0.0F)
    {
        AddJob(new AudioJob(AudioAction.STOP, type, fade, delay));
    }

    public void RestartAudio(GameAudioType type, bool fade = false, float delay = 0.0F)
    {
        AddJob(new AudioJob(AudioAction.RESTART, type, fade, delay));
    }

    private void AddJob(AudioJob job)
    {
        // cancel any job that might be using this job's audio source
        RemoveConflictingJobs(job.type);

        var jobRunner = RunAudioJob(job);
        jobTable.Add(job.type, jobRunner);
        StartCoroutine(jobRunner);
        Log("Starting job on [" + job.type + "] with operation: " + job.action);
    }

    private void RemoveJob(GameAudioType type)
    {
        if (!jobTable.ContainsKey(type))
        {
            Log("Trying to stop a job [" + type + "] that is not running.");
            return;
        }

        var runningJob = (IEnumerator) jobTable[type];
        StopCoroutine(runningJob);
        jobTable.Remove(type);
    }

    private void RemoveConflictingJobs(GameAudioType type)
    {
        // cancel the job if one exists with the same type
        if (jobTable.ContainsKey(type))
        {
            RemoveJob(type);
        }

        // cancel jobs that share the same audio track
        var conflictAudio = GameAudioType.NONE;
        foreach (DictionaryEntry entry in jobTable)
        {
            var audioType = (GameAudioType) entry.Key;
            var audioTrackInUse = GetAudioTrack(audioType, "Get Audio Track In Use");
            var audioTrackNeeded = GetAudioTrack(type, "Get Audio Track Needed");
            if (audioTrackInUse.source == audioTrackNeeded.source)
            {
                conflictAudio = audioType;
            }
        }

        if (conflictAudio != GameAudioType.NONE)
        {
            RemoveJob(conflictAudio);
        }
    }

    private IEnumerator RunAudioJob(AudioJob job)
    {
        yield return new WaitForSeconds(job.delay);

        var track = GetAudioTrack(job.type); // track existence should be verified by now
        track.source.clip = GetAudioClipFromAudioTrack(job.type, track);


        track.source.volume = BaseVolume;
        track.source.mute = isMuted;

        onVolumeChanged.AddListener(vol => track.source.volume = vol);
        onMutedChanged.AddListener(muted => track.source.mute = muted);


        switch (job.action)
        {
            case AudioAction.START:
                track.source.Play();
                break;
            case AudioAction.STOP:
                if (!job.fade)
                {
                    track.source.Stop();
                }

                break;
            case AudioAction.RESTART:
                track.source.Stop();
                track.source.Play();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // fade volume
        if (job.fade)
        {
            float initial = job.action == AudioAction.START || job.action == AudioAction.RESTART ? 0 : 1;
            float target = initial == 0 ? 1 : 0;
            const float duration = 1.0f;
            var timer = 0.0f;

            while (timer < duration)
            {
                track.source.volume = Mathf.Lerp(initial, target, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            if (job.action == AudioAction.STOP)
            {
                track.source.Stop();
            }
        }

        jobTable.Remove(job.type);
        Log("Job count: " + jobTable.Count);
    }

    private void GenerateAudioTable()
    {
        foreach (var track in tracks)
        {
            foreach (var audioObj in track.audio)
            {
                // do not duplicate keys
                if (audioTable.ContainsKey(audioObj.type))
                {
                    LogWarning("You are trying to register audio [" + audioObj.type +
                               "] that has already been registered.");
                }
                else
                {
                    audioTable.Add(audioObj.type, track);
                    Log("Registering audio [" + audioObj.type + "]");
                }
            }
        }
    }

    private AudioTrack GetAudioTrack(GameAudioType type, string job = "")
    {
        if (audioTable.ContainsKey(type)) return (AudioTrack) audioTable[type];

        LogWarning("You are trying to <color=#fff>" + job + "</color> for [" + type +
                   "] but no track was found supporting this audio type.");
        return null;
    }

    private AudioClip GetAudioClipFromAudioTrack(GameAudioType type, AudioTrack track) =>
        (from audioObj in track.audio where audioObj.type == type select audioObj.clip).FirstOrDefault();

    private bool debug = true;

    private void Log(string msg)
    {
        if (!debug) return;
        Debug.Log("[Audio Controller]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!debug) return;
        Debug.LogWarning("[Audio Controller]: " + msg);
    }


    private enum AudioAction
    {
        START,
        STOP,
        RESTART
    }

    private class AudioJob
    {
        public AudioAction action;
        public GameAudioType type;
        public bool fade;
        public float delay;

        public AudioJob(AudioAction action, GameAudioType type, bool fade, float delay)
        {
            this.action = action;
            this.type = type;
            this.fade = fade;
            this.delay = delay;
        }
    }
}