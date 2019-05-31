using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerPartBIT
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//MyAppContext myContext = new MyAppContext();
			using (NotifyIcon icon = new NotifyIcon())
			{
				icon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
				icon.ContextMenu = new ContextMenu(
					new[]
					{
						new MenuItem("Stop it!", (s,e) =>
						{
							Application.Exit();
						})
					}
				);
				icon.Visible = true;
				Application.Run(new MyAppContext());
				icon.Visible = false;
			}
		}
	}
}
