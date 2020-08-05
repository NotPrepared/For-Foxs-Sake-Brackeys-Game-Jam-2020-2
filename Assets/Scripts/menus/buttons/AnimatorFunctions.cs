using UnityEngine;

// ReSharper disable once CheckNamespace
public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    public bool disableOnce;

    void PlaySound(AudioClip whichSound){
        if(!disableOnce){
            //AudioController.instance.PlayAudio(whichSound);
        }else{
            disableOnce = false;
        }
    }
}	