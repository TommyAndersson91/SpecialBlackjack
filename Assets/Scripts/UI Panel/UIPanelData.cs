public static class UIPanelData
{
    public delegate void NameChanged(string text);
    public static NameChanged onNameChanged;

    public delegate void AvatarChanged(string color);
    public static AvatarChanged onAvatarChanged;

    public delegate void TrinketChanged(string color);
    public static TrinketChanged onTrinketChanged;

    private static string playerNameEntered;
    public static string PlayerNameEntered
    {
        get { return playerNameEntered; }
        set 
        {
             onNameChanged?.Invoke(value);
             playerNameEntered = value; 
        }
    }

    private static string playerAvatarEntered;
    public static string PlayerAvatarEntered
    {
        get { return playerAvatarEntered; }
        set 
        {
            onAvatarChanged?.Invoke(value); 
            playerAvatarEntered = value; 
        }
    }

    private static string playerTrinketEntered;
    public static string PlayerTrinketEntered
    {
        get { return playerTrinketEntered; }
        set 
        {
            onTrinketChanged?.Invoke(value);
            playerTrinketEntered = value; 
        }
    }
}
