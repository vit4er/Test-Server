using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerPartBIT
{
	public class ListenerState
	{
		public Socket LstSocket = null;
		public const int BufferSize = 1024;
		public byte[] Buff = new byte[BufferSize];
		public StringBuilder Str = new StringBuilder();
	}
	public class Listener
	{
		public static ManualResetEvent receiveDone = new ManualResetEvent(false);

		public Listener() { }

		private static void SendCallback(IAsyncResult res)
		{
			Socket handler = (Socket)res.AsyncState;
			/// что-то сделать с этими байтами (напр. в лог...)
			int bytes = handler.EndSend(res);
			handler.Shutdown(SocketShutdown.Both);
			handler.Close();
		}

		private static void Send(Socket handler, string text)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(text);
			handler.BeginSend(bytes, 0, bytes.Length, 0,
				new AsyncCallback(SendCallback), handler);
		}

		public static void ReadCallback(IAsyncResult res)
		{
			try
			{
				string text = String.Empty;
				ListenerState state = (ListenerState)res.AsyncState;
				Socket handler = state.LstSocket;
				int bytesReceived = handler.EndReceive(res);

				if (bytesReceived > 0)
				{
					state.Str.Append(Encoding.Unicode.GetString(state.Buff, 0, bytesReceived));

					text = state.Str.ToString();

					/// тут пробуем запихнуть это все в клипборд...
					/// 
					PushToClipboard(text);

					/// обработать на стороне клиента (почистить клипборд?)
					Send(handler, "Ok");
				} /*else
				{
					Send(handler, "Шеф, все пропало!");
				}*/
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public static void AcceptCallback(IAsyncResult res)
		{
			receiveDone.Set();
			Socket listener = (Socket)res.AsyncState;
			Socket handler = listener.EndAccept(res);
			ListenerState state = new ListenerState();
			state.LstSocket = handler;
			handler.BeginReceive(
				state.Buff,
				0,
				ListenerState.BufferSize,
				0,
				new AsyncCallback(ReadCallback),
				state
			);
		}

		public static void RunListener()
		{
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress _address = null;
			try
			{
				foreach (IPAddress ip in host.AddressList)
				{
					if (ip.AddressFamily == AddressFamily.InterNetwork)
					{
						_address = ip;
						break;
					}
				}
				if (_address == null) throw new Exception(@"Адрес IPv4 не найден!");

				IPEndPoint localPoin = new IPEndPoint(_address, 11111);
				Socket srv = new Socket(
					_address.AddressFamily,
					SocketType.Stream,
					ProtocolType.Tcp);

				srv.Bind(localPoin);
				srv.Listen(10);

				while (true)
				{
					receiveDone.Reset();
					srv.BeginAccept(new AsyncCallback(AcceptCallback), srv);
					receiveDone.WaitOne();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static void PushToClipboard(string text)
		{
			Clipboard.SetData(DataFormats.StringFormat, text);
		}
	}
}

