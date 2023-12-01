using System.Diagnostics;
using BepInEx.Logging;

namespace ReduceRecycler {

	internal static class Log {
		private static ManualLogSource Logger { get; set; }

		internal static void Init(ManualLogSource logger) {
			Logger = logger;
		}

		[Conditional("DEBUG")]
		internal static void Debug(object data) => Logger.LogDebug(data);

		internal static void Info(object data) => Logger.LogInfo(data);

		internal static void Message(object data) => Logger.LogMessage(data);

		internal static void Warning(object data) => Logger.LogWarning(data);

		internal static void Error(object data) => Logger.LogError(data);

		internal static void Fatal(object data) => Logger.LogFatal(data);
	}

}
