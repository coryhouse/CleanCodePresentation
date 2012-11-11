using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business_Layer
{
	public class WebBrowser
	{
		public BrowserName Name { get; set; }
		public int MajorVersion { get; set; }

		public WebBrowser()
		{
			Name = BrowserName.Unknown;
		}

		public enum BrowserName
		{
			Unknown,
			InternetExplorer,
			Firefox,
			Chrome,
			Opera,
			Safari,
			Dolphin,
			Konqueror,
			Linx
		}
	}
}
