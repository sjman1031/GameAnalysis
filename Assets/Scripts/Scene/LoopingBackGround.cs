using UnityEngine;

public class LoopingBackGround : MonoBehaviour
{
    public Vector2 scrollSpeed = new Vector2(0.1f, 0f);
    public Renderer _rend;
    public Vector2 _offset;

    void Update()
    {
        _offset += scrollSpeed * Time.deltaTime;
        // Offset 값이 너무 커지면 wrap 방지
        _offset.x %= 1f;
        _offset.y %= 1f;

        _rend.material.mainTextureOffset = _offset;
    }
}
