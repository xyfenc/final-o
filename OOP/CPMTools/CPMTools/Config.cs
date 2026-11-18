namespace CPMTools;

public static class Env
{
    // ─── CPM1 CONFIG ─────────────────────────────────────────────
    public static string CPM1_API_KEY { get; } = "AIzaSyAe_aOVT1gSfmHKBrorFvX4fRwN5nODXVA";
    public static string CPM1_LOAD_URL { get; } = "https://europe-west1-cp-multiplayer.cloudfunctions.net/GetPlayerRecords3";
    public static string CPM1_SAVE_URL { get; } = "https://europe-west1-cp-multiplayer.cloudfunctions.net/SavePlayerRecordsPartially8";
    public static string CPM1_RANK_URL { get; } = "https://us-central1-cp-multiplayer.cloudfunctions.net/SetUserRating6";
    public static string CPM1_CARS_FETCH { get; } = "https://europe-west1-cp-multiplayer.cloudfunctions.net/GetAllCars2";
    public static string CPM1_CARS_SAVE { get; } = "https://europe-west1-cp-multiplayer.cloudfunctions.net/SaveCarsPartially8";

    // ─── CPM2 CONFIG ─────────────────────────────────────────────
    public static string CPM2_API_KEY { get; } = "AIzaSyCQDz9rgjgmvmFkvVfmvr2-7fT4tfrzRRQ";
    public static string CPM2_API_BASE { get; } = "https://europe-west1-cpm-2-7cea1.cloudfunctions.net";
    public static string CPM2_FB_VERSION { get; } = "1.3.2.3";
    public static string CPM2_FB_HASH { get; } = "F05A72840B40DC4FAADF539C5E38062527AE6422";
    public static string CPM2_FB_OG_BASE { get; } = "https://cpm-2.ogames.kz/api";
    public static string CPM2_FB_OG_KEY { get; } = "320b93f3e7f4410aa52ce24da363ad04";
    public static string CPM2_FB_BUNDLE { get; } = "com.olzhas.carparking.multyplayer2";

    // ─── LIMITS & CONSTANTS ──────────────────────────────────────
    public const int MAX_MONEY = 50_000_000;
    public const int MAX_COIN = 500_000;
    public const int CAR_IDS_RANGE = 274;

    // ─── SOURCE ACCOUNT (For Unlock All / Clone) ─────────────────
    // You can change these here, or load them from a .env file
    public static string CPM1_SOURCE_EMAIL { get; set; } = "yarasaClone_182b78761d3349a6@gmail.com";
    public static string CPM1_SOURCE_PASSWORD { get; set; } = "NEfFOwnfC8mZ";

    public static void Load()
    {
        // Optional: If you want to load from a .env file later, 
        // you can read the file here and override the default properties.
        // Example:
        // var lines = File.ReadAllLines(".env");
        // foreach(var line in lines) { ... set CPM1_SOURCE_EMAIL ... }
    }
}