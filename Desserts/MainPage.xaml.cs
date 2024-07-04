using System.Net;
using System.Net.Sockets;

namespace Desserts;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
        CounterBtn.Text = NetworkHelper.GetLocalIPAddress();
    }
}
public static class NetworkHelper
{
    public static string GetLocalIPAddress()
    {
        var hostName = Dns.GetHostName();
        var addresses = Dns.GetHostAddresses(hostName);
        var ipv4Address = addresses.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);

        if (ipv4Address != null)
        {
            return ipv4Address.ToString();
        }

        return "No IP Address Found";
    }
}

