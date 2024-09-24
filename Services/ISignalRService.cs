using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignmentWinUI.Services;
public interface ISignalRService
{
    Task ConnectAsync(string hubUrl);
    Task GetOnlineGroupMembers(int groupId);
    Task DisconnectAsync();
}
