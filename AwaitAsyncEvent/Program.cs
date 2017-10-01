using System;
using System.Net;
using System.Threading.Tasks;

using static System.Console;

namespace AwaitAsyncEvent
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			var url = "http://google.com";

			WriteLine (url);

			// Event
			var workerEvent = new EventWebWorker ();

			workerEvent.HtmlStringReceived += (object sender, HtmlReceivedEventArgs e) => {
				WriteLine ($"event html:{ e.Html }");
			};

			workerEvent.DownloadHtmlStringWithEvent (url);


			// async, await
			var workerAsync = new AsyncWebWorker ();

			Task.Run (async () => { 
				var html = await workerAsync.DownloadHtmlStringAsync (url);
				WriteLine ($"async html:{ html }");
			} );

			ReadLine ();
		}
	}

	public class EventWebWorker
	{
		private WebClient MyWebClient { get; set; }

		public EventWebWorker ()
		{
			MyWebClient = new WebClient ();
		}

		public void DownloadHtmlStringWithEvent (string url)
		{
			var html = MyWebClient.DownloadString (url);

			EventHandler<HtmlReceivedEventArgs> handler = HtmlStringReceived;
			var args = new HtmlReceivedEventArgs { Html = html };
			if (null != handler) {
				handler (this, args);
			}
		}
		public event EventHandler<HtmlReceivedEventArgs> HtmlStringReceived;
	}
	public class HtmlReceivedEventArgs : EventArgs
	{
		public string Html { get; set; }
	}

	public class AsyncWebWorker
	{
		private WebClient MyWebClient { get; set; }

		public AsyncWebWorker ()
		{
			MyWebClient = new WebClient ();
		}

		public async Task<string> DownloadHtmlStringAsync (string url)
		{
			var task = MyWebClient.DownloadStringTaskAsync (url);
			var result = await task;
			return result;
		}
	}

}
