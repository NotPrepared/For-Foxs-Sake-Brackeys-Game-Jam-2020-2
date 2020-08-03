using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


// ReSharper disable once CheckNamespace
public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    //[SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] public int thisIndex;

    [FormerlySerializedAs("onClickEvent")] [SerializeField]
    public Button.ButtonClickedEvent onClick;

    // Update is called once per frame
    void Update()
    {
        if(menuButtonController.index == thisIndex)
        {
            animator.SetBool ("selected", true);
            if(Input.GetAxisRaw("Submit") == 1){
                animator.SetBool ("pressed", true);
            }else if (animator.GetBool ("pressed")){
                animator.SetBool ("pressed", false);
                //animatorFunctions.disableOnce = true;
                onClick.Invoke();
            }
        }else{
            animator.SetBool ("selected", false);
        }
    }
}