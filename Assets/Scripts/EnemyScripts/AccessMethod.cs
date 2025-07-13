using UnityEngine;
using UnityEngine.U2D.Animation;

public class AccessMethod : MonoBehaviour
{
    private SpriteResolver resolver;

    void Awake()
    {
        resolver = GetComponent<SpriteResolver>();
    }


    public void SetSpriteCategoryLabel(string categoryAndLabel)
    {
        if (resolver != null)
        {
            var split = categoryAndLabel.Split(':');
            if (split.Length == 2)
            {
                resolver.SetCategoryAndLabel(split[0], split[1]);
            }
        }
    }

}
