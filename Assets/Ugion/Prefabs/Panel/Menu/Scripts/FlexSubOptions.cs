using UnityEngine;
using UnityEngine.UI;

public class FlexSubOptions : MonoBehaviour
{
    [SerializeField] private LayoutElement layout;

    [SerializeField] private float speed = 3f;

    public float preferredHeight;

    private float process;
    private bool narrow = false;
    private bool expand = false;

    public void Spread(bool expand)
    {
        this.expand = expand;
        narrow = !expand;
        process = Mathf.InverseLerp(0, preferredHeight, layout.preferredHeight);
    }

    public void SpreadImmediate(bool expand)
    {
        process = expand ? 1 : 0;
        layout.preferredHeight = process * preferredHeight;
    }

    private void Update()
    {
        if (!expand && !narrow) return;

        if (expand)
        {
            process = Mathf.Clamp01(process + Time.deltaTime * speed);
            layout.preferredHeight = process * preferredHeight;
            if (process >= 1)
            {
                expand = false;
            }
        }
        else if (narrow)
        {
            process = Mathf.Clamp01(process - Time.deltaTime * speed);
            layout.preferredHeight = process * preferredHeight;
            if (process <= 0)
            {
                narrow = false;
            }
        }
    }
}
