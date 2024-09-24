using System.Diagnostics;
using AsignmentWinUI.Contracts.Services;
using AsignmentWinUI.Core.Entities;
using AsignmentWinUI.Core.UseCases.Services;
using AsignmentWinUI.Services;
using AsignmentWinUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;

namespace AsignmentWinUI.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    private readonly INavigationService _navigationService;
    private readonly ISignalRService _signalRService;
    public MainViewModel(
        ISignalRService signalRService,
        INavigationService navigationService
        )
    {
        _navigationService = navigationService;
        _signalRService = signalRService;
       _signalRService.ConnectAsync("http://localhost:5192/chathub");
    }

    [RelayCommand]
    public void JoinChatPage()
    {
        _navigationService.NavigateTo(typeof(ChatViewModel).FullName, "JELL");

    }
    [RelayCommand]
    public async Task<IEnumerable<Message>> GetOnlineGroupMembers()
    {
        try
        {
            await _signalRService.GetOnlineGroupMembers(1);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"GetOnlineGroupMembers: {ex.Message}");
            throw;
        }
        return null;
    }

}
