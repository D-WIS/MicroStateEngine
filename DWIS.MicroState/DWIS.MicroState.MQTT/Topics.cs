namespace DWIS.MicroState.MQTT
{
    public class Topics
    {
        public static readonly string CurrentMicroStates = "dwis/microstates/current";

        public static readonly string SignalsInputs = "dwis/microstates/signals/inputs";

        public static readonly string ThresholdsRequests = "dwis/microstates/thresholds/requests";
        public static readonly string Thresholds = "dwis/microstates/thresholds/current";

        public static readonly string SignalSourceScalars = "dwis/signals/source-scalars";
        public static readonly string SignalSourceBooleans = "dwis/signals/source-booleans";

    }
}