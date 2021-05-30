using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class CustomNameplateEntity : UdonSharpBehaviour
{
    public Text npText;

    public void SetText(string textInput)
    {
        npText.text = ""+textInput; //AA
    }

    public void SetSize(int FontSize)
    {
        npText.fontSize = FontSize;
    }

    public void SetColor(Color c)
    {
        npText.color = c;
    }

    public void SetFont(Font f)
    {
        npText.font = f;
    }

    public void SetTextShadow(bool isActive)
    {
        npText.GetComponent<Shadow>().enabled = isActive;
    }

    public void SetTextShadowOffset(Vector2 effectDistance)
    {
        npText.GetComponent<Shadow>().effectDistance = effectDistance;
    }

    public void SetTextShadowColor(Color textShadowColor)
    {
        npText.GetComponent<Shadow>().effectColor = textShadowColor;
    }

    public void Update()
    {
        transform.LookAt(Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position);
    }
}
