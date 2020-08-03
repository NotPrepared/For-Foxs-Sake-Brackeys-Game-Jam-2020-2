using UnityEngine;
using UnityEngine.UI;


// ReSharper disable once CheckNamespace
public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    //[SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;

    [SerializeField] private Button.ButtonClickedEvent onClickEvent;

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
                onClickEvent.Invoke();
            }
        }else{
            animator.SetBool ("selected", false);
        }
    }
}