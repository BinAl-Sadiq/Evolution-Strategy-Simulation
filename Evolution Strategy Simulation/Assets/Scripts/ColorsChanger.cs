using UnityEngine;

public class ColorsChanger : MonoBehaviour
{
    public SpriteRenderer[] mats;
    private float Timer = .05f;
    public static float r = 0f, g = 0f, b = 1f;
    private char color = 'r';
    private bool revers = false;
    private float value = 0.01f;

    void Update()
    {
        if (Timer <= 0f)
        {
            Timer = .05f;

            foreach (SpriteRenderer mat in mats)
            {
                if (mat)
                {
                    if (!revers)
                    {
                        if (color == 'b')
                        {
                            b -= value;

                            if (b <= 0f)
                                color = 'r';
                        }
                        else if (color == 'r')
                        {
                            r += value;

                            if (r >= 1f)
                                color = 'g';
                        }
                        else if (color == 'g')
                        {
                            g += value;

                            if (g >= 1f)
                            {
                                revers = true;
                                color = 'b';
                            }
                        }
                        
                    }
                    else
                    {
                        if (color == 'b')
                        {
                            b += value;

                            if (b >= 1f)
                                color = 'r';
                        }
                        else if (color == 'r')
                        {
                            r -= value;

                            if (r <= 0f)
                                color = 'g';
                        }
                        else if (color == 'g')
                        {
                            g -= value;

                            if (g <= 0f)
                            {
                                revers = false;
                                color = 'b';
                            }
                        }
                    }

                    mat.color = new Color(r, g, b);
                }
            }
        }
        else
            Timer -= Time.deltaTime;
    }
}
