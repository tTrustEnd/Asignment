using System.Diagnostics;
using AsignmentWinUI.Core.Entities;
using AsignmentWinUI.Core.UseCases.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;

namespace AsignmentWinUI.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
  
    private HubConnection _hubConnection;
    public MainViewModel()
    {
        Debug.WriteLine("SDsdawdaw");
        _hubConnection = new HubConnectionBuilder()
           .WithUrl("http://localhost:5001/chatHub")
           .Build();
        ConnectAsync();
    }
    [RelayCommand]
    public async Task<IEnumerable<Message>> GetMessages()
    {
        try
        {
             await _hubConnection.InvokeAsync("GetMessages");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving messages: {ex.Message}");
            throw;
        }
        return null;
        //try
        //{
        //    var Mes = await _messageService.GetMessagesAsync();
        //    foreach (var m in Mes)
        //    {
        //        Debug.WriteLine(m.MessageID + " "
        //            + m.User?.UserName + " "
        //            + m.Group?.GroupName + " "
        //            + m.Content);
        //    }
        //    return Mes;
        //}
        //catch (Exception ex)
        //{
        //    Debug.WriteLine($"Error retrieving messages: {ex.Message}");
        //    throw;
        //}
    }
    private async Task ConnectAsync()
    {
        try
        {
            await _hubConnection.StartAsync();
            Debug.WriteLine("Connected to the hub.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error connecting to the hub: {ex.Message}");
        }
    }
}
