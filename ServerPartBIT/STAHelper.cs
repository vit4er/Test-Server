using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerPartBIT
{
	abstract class STAHelper
	{
		readonly ManualResetEvent _complete = new ManualResetEvent(false);
		public void Go()
		{
			var thread = new Thread(new ThreadStart(JustDoIt));
			thread.IsBackground = true;
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
		}

		//thread.
		private void JustDoIt()
		{
			try
			{
				_complete.Reset();
				Work();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw ex;
			}
			finally
			{
				_complete.Set();
			}
		}

		protected abstract void Work();
	}
}
