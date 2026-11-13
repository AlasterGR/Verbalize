using System.Net.NetworkInformation;

namespace _Verbalize
{
    /// <summary> Tells whether the computer can reach the internet, using Windows' own check (the same one behind the network icon's "No internet"). </summary>
    internal static class Handler_Connectivity
    {
        /// <summary> Windows' network list service, which knows whether the internet is reachable. </summary>
        private static readonly Type? networkListManagerType = Type.GetTypeFromCLSID(new Guid("DCB00C01-570F-4A9B-8D69-199FDBA5723B"));
        /// <summary> The network list service, created once when first needed, or null if it is not available. </summary>
        private static dynamic? networkListManager;

        /// <summary> Returns true if the computer is connected to the internet. If Windows cannot tell, it assumes online, so actions are never blocked by mistake. </summary>
        /// <returns>True if online.</returns>
        public static bool IsConnectedToInternet()
        {
            //  Ask Windows' network list service.
            try
            {
                networkListManager ??= Activator.CreateInstance(networkListManagerType!);
                return (bool)networkListManager!.IsConnectedToInternet;
            }
            //  Fall back to checking for any working network connection if the service cannot be asked.
            catch (Exception)
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
        }
    }
}
