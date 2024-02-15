namespace UnityEngine.XR.OpenXR.Features.Meta.Tests
{
    public class MetaQuestDisplayUtilitiesSample
    {
        [UnityTest]
        public void RequestDisplayRefreshRate()
        {
            #region request_display_refreshRate
            // Omitted null checks for brevity. You should check each line for null.
            var displaySubsystem = XRGeneralSettings.Instance
                .Manager
                .activeLoader
                .GetLoadedSubsystem<XRDisplaySubsystem>();

            // Get the supported refresh rates.
            // If you will save the refresh rate values for longer than this frame, pass
            // Allocator.Persistent and remember to Dispose the array when you are done with it.
            if (displaySubsystem.TryGetSupportedDisplayRefreshRates(
                    Allocator.Temp,
                    out var refreshRates))
            {
                // Request a refresh rate.
                // Returns false if you request a value that is not in the refreshRates array.
                bool success = displaySubsystem.TryRequestDisplayRefreshRate(refreshRates[0]);
            }
            #endregion
        }
    }
}
