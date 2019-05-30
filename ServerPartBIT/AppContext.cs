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
		}
	}
}
