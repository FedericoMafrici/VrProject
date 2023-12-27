using System;
using UnityEngine;

public class Clean : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private Texture2D _dirtMaskBase;
    [SerializeField] private Texture2D _brush;

    [SerializeField] private Material _material;

    private Texture2D _templateDirtMask;

    private void Start()
    {
        CreateTexture();
    }

    private void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                Vector2 textureCoord = hit.textureCoord;

                int pixelX = (int)(textureCoord.x * _templateDirtMask.width);
                int pixelY = (int)(textureCoord.y * _templateDirtMask.height);
                Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
                Debug.Log("UV: " + textureCoord + "; Pixels: " + paintPixelPosition);
                int pixelXOffset = pixelX - (_brush.width / 2);
                int pixelYOffset = pixelY - (_brush.height / 2);
                for (int x = 0; x < (_brush.width+500); x++)
                {
                    for (int y = 0; y < _brush.height; y++)
                    {
                        Color pixelDirt = _brush.GetPixel(x, y);
                        Color pixelDirtMask = _templateDirtMask.GetPixel(pixelXOffset + x, pixelYOffset + y);

                        _templateDirtMask.SetPixel(pixelX + x,
                            pixelY + y,
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0));
                    }
                }

                _templateDirtMask.Apply(); // modifichiamo la texture dipingendola di nero 
            }
        }
    }

    private void CreateTexture()
    {
        _templateDirtMask = new Texture2D(_dirtMaskBase.width, _dirtMaskBase.height);
        _templateDirtMask.SetPixels(_dirtMaskBase.GetPixels());
        _templateDirtMask.Apply();

        _material.SetTexture("DirtTexture", _templateDirtMask); // settiamo la texture del nostro materiale con quella che andremo a modificare 
    }
}
