using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerPartBIT
{
	class MyAppContext: ApplicationContext
	{
		public static MyForm form;
		public MyAppContext()
		{
			form = new MyForm();
			//MyForm form = new MyForm();
			//Listener.RunListener();
			Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
			
		}

		private void OnApplicationExit(object s, EventArgs e)
		{
			Console.WriteLine("Application exit...");
		}
	}
}
