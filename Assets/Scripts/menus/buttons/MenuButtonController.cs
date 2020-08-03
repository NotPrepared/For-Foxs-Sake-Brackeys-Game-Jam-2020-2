using UnityEngine;

// ReSharper disable once CheckNamespace
public class MenuButtonController : MonoBehaviour
{
    public int index;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (!keyDown)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    if (index < maxIndex)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") > 0)
                {
                    if (index > 0)
                    {
                        index--;
                    }
                    else
                    {
                        index = maxIndex;
                    }
                }

                keyDown = true;
            }
        }
        else
        {
            keyDown = false;
        }
    }
}