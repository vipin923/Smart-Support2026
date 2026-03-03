using Dapper;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Smart_Support2026.DataAccessLayer;
using Smart_Support2026.Models;
using System.Data;
using System.Data.Common;
using System.Security.Claims;


public class NotificationHub : Hub
{
    private readonly ApplicationDbContext _context;
    public NotificationHub(ApplicationDbContext context)
    {
        _context = context;
    }

public async Task GetNotification()
    {
        try
        {
            using (DbConnection connection = _context.Database.GetDbConnection())
            {
                string? userId = Context.UserIdentifier; // This is the value from ClaimTypes.NameIdentifier
                if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@emp_in", userId);
                var model = await connection.QueryAsync<NotificationModel>("sp_ssgetnotification", parameters, commandType: System.Data.CommandType.StoredProcedure);
                await Clients.User(userId).SendAsync("ReceiveNotify", model);


            }
        }
        catch (Exception ex)
        {
            throw new Exception($"SQL Error: {ex.Message}");

        }
    }
    public async Task MarkAsRead(int Id)
    {
        try
        {
            using (DbConnection connection = _context.Database.GetDbConnection())
            {
                string? userId = Context.UserIdentifier; // This is the value from ClaimTypes.NameIdentifier
                if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
                parameters.Add("@markasread", true);
                 await connection.ExecuteAsync("sp_ssmarkasread", parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"SQL Error: {ex.Message}");

        }
    }

    public async Task GetNotifyCount()
    {
        try
        {
            using (DbConnection connection = _context.Database.GetDbConnection())
            {
                string? userId = Context.UserIdentifier; // This is the value from ClaimTypes.NameIdentifier
                if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                var parameters = new DynamicParameters();
                parameters.Add("@emp_in", userId);
                int count = await connection.QueryFirstOrDefaultAsync<int>("sp_ssgetnotificationcount", parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (count > 0)
                {
                    await Clients.User(userId).SendAsync("ReceiveCount", count);

                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"SQL Error: {ex.Message}");

        }
    }
    public override async Task OnConnectedAsync()
    {
        string? userId = Context.UserIdentifier; // This is the value from ClaimTypes.NameIdentifier
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        // ... other logic using the userId

        await base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
    public async Task SendNotificationUser(string user, string message)
    {
        string? User = Context.UserIdentifier;
        await Clients.User(User).SendAsync("ReceiveNotification", message);
    }
}

