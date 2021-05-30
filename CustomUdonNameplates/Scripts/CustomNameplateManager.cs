using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class CustomNameplateManager : UdonSharpBehaviour
{
    public GameObject[] Nameplates;
    VRCPlayerApi[] currentPlayers;
    [Header("General Settings")]
    public bool HideNamePlates = false;
    [Header("Font/Text Options")]
    public Vector3 NamePlateOffset = new Vector3(0, 0.35f, 0);
    public int NameplateFontSize = 96;
    public Color NameplateFontColor = new Color(1, 1, 1);
    public Font NameplateFont;
    [Header("Text Effects")]
    public bool UseTextShadow;
    public Vector2 ShadowOffset;
    public Color ShadowColor = new Color(0,0,0,1);

    [Header("If enabled, Text of your own UdonNameplate will render")]
    [Header("This will introduce jank")]
    public bool RenderLocally = false;


    public string NameLookup(string Playername)
    {
        //Define your custom Nametag options here
        //For non programmers: the format is:
        /*
         *   
         *  case "Username":
         *      return "Text that you want do display";
         * 
         * (Be sure to not copy the asterisk)
         */

        switch (Playername)
        {
            case "M․O․O․N":
                return "Prefab Creator";
            case "Fusl™":
                return "VRChat Team";
            case "tupper":
                return "VRChat Team";
            case "Varneon":
                return "Cool Dude";
            case "N․O․O․M":
                return "Cool person";
        }

        //If nothing else is defined, return nothing.
        return "";
    }


    public void ToggleNameplates(bool val)
    {
        if (val)
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SetNameTagsVisible));
        }
        else
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SetNameTagsInvisible));
        }
    }

    [VRC.SDKBase.RPC]
    public void SetNameTagsVisible()
    {
        HideNamePlates = false;
    }

    [VRC.SDKBase.RPC]
    public void SetNameTagsInvisible()
    {
        HideNamePlates = true;
    }

    public void Start()
    {
        //Setup nameplate settings on start.
        foreach(GameObject n in Nameplates)
        {
            n.GetComponent<CustomNameplateEntity>().SetSize(NameplateFontSize);
            n.GetComponent<CustomNameplateEntity>().SetFont(NameplateFont);
            n.GetComponent<CustomNameplateEntity>().SetColor(NameplateFontColor);

            if (UseTextShadow)
            {
                n.GetComponent<CustomNameplateEntity>().SetTextShadow(UseTextShadow);
                n.GetComponent<CustomNameplateEntity>().SetTextShadowOffset(ShadowOffset);
                n.GetComponent<CustomNameplateEntity>().SetTextShadowColor(ShadowColor);
            }
        }
    }

    public void WipeAll()
    {
        foreach(GameObject x in Nameplates)
        {
            x.SetActive(false);
        }
    }

    public void LateUpdate()
    {

        if (HideNamePlates)
        {
            WipeAll();
            return;
        }

        //Refresh Playerdatabase Array first
        RefreshPlayerDB();
        if (currentPlayers.Length > 0)
        {
            for(int i=0;i<=currentPlayers.Length-1; i++)
            {
                if (currentPlayers[i] != null)
                {
                    Nameplates[i].SetActive(true);
                    GameObject x = Nameplates[i];
                    CustomNameplateEntity np = x.GetComponent<CustomNameplateEntity>();

                    if (currentPlayers[i].displayName != Networking.LocalPlayer.displayName)
                    {
                        np.SetText(NameLookup(currentPlayers[i].displayName));
                    } else
                    {
                        //Don't render for the Local player, unless RenderLocally is set to true.
                        if (RenderLocally == true)
                        {
                            np.SetText(NameLookup(currentPlayers[i].displayName));
                        } else
                        {
                            np.SetText("");
                        } 
                    }

                    Nameplates[i].transform.position = currentPlayers[i].GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position + NamePlateOffset;
                }
            }

            //Disable other unused custom nameplate entities.
            for (int x = currentPlayers.Length; x <= Nameplates.Length-1; x++)
            {
                Nameplates[x].SetActive(false);
            }
        }
    }

    public void RefreshPlayerDB()
    {
        currentPlayers = new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()];
        VRCPlayerApi.GetPlayers(currentPlayers);
    }
}
