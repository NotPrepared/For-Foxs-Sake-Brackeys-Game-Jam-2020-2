using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public bool isDisabled;

    private static readonly int Pressed = Animator.StringToHash("pressed");
    private static readonly int Selected = Animator.StringToHash("selected");

    private void Start()
    {
        var eventTrigger = gameObject.GetComponent<EventTrigger>();
        var enterEvent = new EventTrigger.Entry {eventID = EventTriggerType.PointerEnter};
        enterEvent.callback.AddListener(_ => menuButtonController.index = thisIndex);

        var clickEvent = new EventTrigger.Entry {eventID = EventTriggerType.PointerClick};
        clickEvent.callback.AddListener(_ =>
        {
            if (isDisabled) return;
            animator.SetBool(Pressed, true);
        });
        
        eventTrigger.triggers.Add(enterEvent);
        eventTrigger.triggers.Add(clickEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisabled) GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        if(menuButtonController.index == thisIndex)
        {
            animator.SetBool (Selected, true);
            if(Input.GetAxisRaw("Submit") == 1){
                if (isDisabled) return;
                animator.SetBool (Pressed, true);
            }else if (animator.GetBool (Pressed)){
                animator.SetBool (Pressed, false);
                //animatorFunctions.disableOnce = true;
                onClick.Invoke();
            }
        }else{
            animator.SetBool (Selected, false);
        }
    }
    
}