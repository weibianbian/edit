using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TestAtlas : MonoBehaviour
{
    public Image img;

    public SpriteAtlas atlas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private int idx = 0;
    // Update is called once per frame
    void Update()
    {
        const string SpawnPointName = "SpawnPoint";
        if (img != null && atlas != null)
        {
            idx = ++idx % atlas.spriteCount;
            var clone = atlas.GetSprite($"{SpawnPointName}{idx}");
            img.sprite = clone;
        }
    }
}
