using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace AsignmentWinUI.ViewModels
{
    public partial class ChatViewModel : ObservableRecipient
    {
        public ObservableCollection<string> OnlineUsers { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string _newMessage;
        public ChatViewModel()
        {
            // Khởi tạo danh sách người dùng online (có thể lấy từ SignalR)
            OnlineUsers.Add("User1");
            OnlineUsers.Add("User2");
            OnlineUsers.Add("User3");
        }

        [RelayCommand]
        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(NewMessage))
            {
                Debug.WriteLine("Ms: " + NewMessage);
                // Gửi tin nhắn qua SignalR
                // _signalRService.SendMessageAsync(CurrentUsername, message);
                NewMessage = string.Empty; // Xóa ô nhập
            }
        }
    }
}
