using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteRender : MonoBehaviour
{
    public Sprite[] blueTeam;
    public Sprite[] redTeam;

    public bool isBlueTeam = true;

    private Image m_image;

    private int m_count = 0;
    private float m_timer;

    public void Start()
    {
        m_image = GetComponent<Image>();
    }

    void Update ()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= 1.0f * 0.0416)
        {
            if (m_count >= 100)
            {
                m_count = 0;
            }

            if (m_count >= 0 && m_count < blueTeam.Length && m_count < redTeam.Length)
            {
                if (isBlueTeam)
                {
                    m_image.sprite = blueTeam[m_count];
                }
                else
                {
                    m_image.sprite = redTeam[m_count];
                }
            }
            else
            {
                m_count = 0;
            }

            m_count++;
        }
		
	}
}
