using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsignmentWinUI.Services
{
    public class SignalRService: ISignalRService
    {
        private HubConnection _hubConnection;

        public event Action<string, string> MessageReceived;

        public async Task ConnectAsync(string hubUrl)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect() 
                .Build();
            try
            {
                await _hubConnection.StartAsync();
                Debug.WriteLine("SignalR connected.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Connection failed: {ex.Message}");
            }
        }

        public async Task GetOnlineGroupMembers(int groupId)
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("GetOnlineGroupMembers", groupId);
            }
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                Console.WriteLine("SignalR disconnected.");
            }
        }
    }
}
