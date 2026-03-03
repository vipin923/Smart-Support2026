using AspNetCoreGeneratedDocument;
using Dapper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.ML;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NToastNotify;
using Smart_Support2026.DataAccessLayer;
using Smart_Support2026.Models;
using System.Data;

using System.Data.Common;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;
using static Smart_Support2026.Auto_Tag_Model;
using static Smart_Support2026.ModelUser;

namespace Smart_Support2026.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly PredictionEnginePool<TicketInput, TicketPrediction> _predictionEnginePool;
        private readonly PredictionEnginePool<TicketsAI.TicketData, TicketsAI.TicketPrediction> _ticketPredictionPool;
        private readonly IMemoryCache _cache;
        private readonly IHubContext<NotificationHub> _hubContext;

        public HomeController(ApplicationDbContext context, IToastNotification toastNotificat, PredictionEnginePool<TicketInput, TicketPrediction> predictionEnginePool,PredictionEnginePool<TicketsAI.TicketData,TicketsAI.TicketPrediction> predictionEngine, IMemoryCache memoryCache,IHubContext<NotificationHub> hubContext) {
            _context = context;
            _toastNotification = toastNotificat;
            _predictionEnginePool = predictionEnginePool;
            _ticketPredictionPool = predictionEngine;
            _cache = memoryCache;
            _hubContext = hubContext;


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectAnswer(int ticket_id)
        {
            
            _cache.Set("ticket_id", ticket_id, TimeSpan.FromMinutes(30));

            return RedirectToAction("AnswerTickets");
        }

        [HttpGet]
        public async Task<IActionResult> AnswerTickets()
        {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    if (!_cache.TryGetValue("ticket_id", out int ticket_id))
                    {
                        // Data not in cache; fetch from source and save it
                    }
                        parameters.Add("@ticket_id", ticket_id);


                        if (userid == "admin150")
                        {
                            TempData["Role"] = "Admin";
                        }
                        else
                        {
                            TempData["Role"] = "Emp";
                        }
                        var tickets = await connection.QueryAsync<RaiseTickets>("sp_ssgetticketbyid", parameters, commandType: System.Data.CommandType.StoredProcedure);
                        return View(tickets);
                    
                    
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return null;

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerQuery(RaiseTickets tickets,int ticket_id,string sendername)
        {
            try
            {

                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                   
                    if (userid == "admin150")
                    {
                        TempData["Role"] = "Admin";
                    }
                    else
                    {
                        TempData["Role"] = "Emp";
                    }
                   
                    parameters.Add("@Id", ticket_id);
                    parameters.Add("@answer", tickets.Answer);
                    parameters.Add("@ans_data", DateTime.Now);
                    parameters.Add("@status", tickets.Status);
                    await connection.ExecuteAsync("sp_answertickets", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    _toastNotification.AddSuccessToastMessage("Answer  Sucsessfully");
                    await _hubContext.Clients.User(sendername).SendAsync("ReceiveNotification", "Admin Response on Your Tickets");
                    var notification = new NotificationModel { emp_Id = sendername, IsRead = false, message = userid + "Admin Response on Your Tickets", Notify_On = DateTime.Now };
                    await SaveNotification(notification);
                    return View("Index");

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return null;

            }
        }

        
        public IActionResult Index()
        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == "admin150")
            {
                TempData["Role"] = "Admin";
            }
            else
            {
                TempData["Role"] = "Emp";
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Reply()
        {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    if (userid == "admin150")
                    {
                        TempData["Role"] = "Admin";
                    }
                    else
                    {
                        TempData["Role"] = "Emp";
                    }
                    var ticket_id = TempData["ticket_id"];

                    parameters.Add("@ticketid", ticket_id);
                    var replyticket = await connection.QueryAsync<ReplyModel>("sp_getssticketreply", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return View(replyticket);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return null;

            }
        }
        public async Task<IActionResult> SendReply(int ticket_id)
        {
            TempData["ticket_id"] = ticket_id;
            return RedirectToAction("Reply");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveReply(string message,int ticket_id,string replyid)
        {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    if (userid == "admin150")
                    {
                        TempData["Role"] = "Admin";
                    }
                    else
                    {
                        TempData["Role"] = "Emp";
                    }
                    parameters.Add("@ticket_id", ticket_id);
                    parameters.Add("@reply_by", userid);
                    parameters.Add("@reply", message);
                    parameters.Add("@answer_date", DateTime.Now);
                    await connection.ExecuteAsync("sp_sssavereply",parameters,commandType: System.Data.CommandType.StoredProcedure);
                    _toastNotification.AddSuccessToastMessage("Reply Successfully");
                    if (userid == "admin150")
                    {
                        await _hubContext.Clients.User(replyid).SendAsync("ReceiveNotification", "You Have Reply");
                        var notification = new NotificationModel { emp_Id = replyid ,IsRead = false,message= userid +" Give Reply",Notify_On = DateTime.Now};
                        await SaveNotification(notification);
                    }
                    else
                    {
                        await _hubContext.Clients.User("admin150").SendAsync("ReceiveNotification", "You Have Reply");
                        var notification = new NotificationModel { emp_Id = "admin150", IsRead = false, message = userid + " Give Reply", Notify_On = DateTime.Now };
                        await SaveNotification(notification);
                    }

                    return View("Index");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return null;

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AIAgentQuery(TicketsAI.TicketData ticketsAI,string description)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var allRecords = System.IO.File.ReadAllLines("modelcsv/it_ticketsol.csv")
     .Skip(1)
     .Select(line => Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)")) // Handles commas in quotes
     .Where(cols => cols.Length >= 4) // Safety check: skip lines that are too short
     .Select(cols => new {
         ErrorType = cols[1].Replace("\"", "").Trim(),
         Solution = cols[3].Replace("\"", "").Trim()
     })
     .ToList();

                // 2. Identify the most frequent solution for each ErrorType
                var bestSolutionMap = allRecords
                    .GroupBy(r => r.ErrorType)
                    .ToDictionary(
                        group => group.Key,
                        group => group.GroupBy(s => s.Solution)
                                      .OrderByDescending(g => g.Count())
                                      .First().Key // Selects the solution that appears most often
                    );
                var prediction = _ticketPredictionPool.Predict(modelName: "AI-TicketSolution-Model", new TicketsAI.TicketData { TicketDescription = description });
                if (userid == "admin150")
                {
                    TempData["Role"] = "Admin";
                }
                else
                {
                    TempData["Role"] = "Emp";
                }
                if (bestSolutionMap.TryGetValue(prediction.ErrorType, out string bestFix))
                {

                   
                }
                ViewData["Solution"] = bestFix;
                return PartialView("_AIQuery");

            }
            catch
            (Exception ex)
            {

                throw new Exception($" Error: {ex.Message}");
                return null;
            }

        }

        public IActionResult AskAI()
        {
            return View();
        }

        public async Task<IActionResult> AskQuery(string description)
        {
            try
            {
                var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var allRecords = System.IO.File.ReadAllLines("modelcsv/it_ticketsol.csv")
     .Skip(1)
     .Select(line => Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)")) // Handles commas in quotes
     .Where(cols => cols.Length >= 4) // Safety check: skip lines that are too short
     .Select(cols => new {
         ErrorType = cols[1].Replace("\"", "").Trim(),
         Solution = cols[3].Replace("\"", "").Trim()
     })
     .ToList();

                // 2. Identify the most frequent solution for each ErrorType
                var bestSolutionMap = allRecords
                    .GroupBy(r => r.ErrorType)
                    .ToDictionary(
                        group => group.Key,
                        group => group.GroupBy(s => s.Solution)
                                      .OrderByDescending(g => g.Count())
                                      .First().Key // Selects the solution that appears most often
                    );
                var prediction = _ticketPredictionPool.Predict(modelName: "AI-TicketSolution-Model", new TicketsAI.TicketData { TicketDescription = description });
                if (userid == "admin150")
                {
                    TempData["Role"] = "Admin";
                }
                else
                {
                    TempData["Role"] = "Emp";
                }
                if (bestSolutionMap.TryGetValue(prediction.ErrorType, out string bestFix))
                {


                }
                ViewData["Solution"] = bestFix;
                return View("AskAI");

            }
            catch
            (Exception ex)
            {

                throw new Exception($" Error: {ex.Message}");
                return null;
            }

        }

        public async Task SaveNotification(NotificationModel notification)
        {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@emp_in", notification.emp_Id);
                    parameters.Add("@message", notification.message);
                    parameters.Add("@isread", notification.IsRead);
                    parameters.Add("@notifyon", notification.Notify_On);
                    await connection.ExecuteAsync("sp_sssavnotification", parameters, commandType: System.Data.CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");

            }
        }
        
        public IActionResult RaiseTickets()

        {
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userid == "admin150")
            {
                TempData["Role"] = "Admin";
            }
            else
            {
                TempData["Role"] = "Emp";
            }


                return View();
        }
        
       
        [HttpGet]
        public async Task<IActionResult> EmpViewTicket()
            {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@empid", userid);

                   
                    if (userid == "admin150")
                    {
                        TempData["Role"] = "Admin";
                    }
                    else
                    {
                        TempData["Role"] = "Emp";
                    }
                    var tickets = await connection.QueryAsync<RaiseTickets>("sp_getticketsview", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    return View(tickets);

                }
                
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return null;

            }

        }
        [HttpGet]
        public async Task<IActionResult> ViewTickets()
        {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                  
                   


                    if (userid == "admin150")
                    {
                        TempData["Role"] = "Admin";
                    }
                    else
                    {
                        TempData["Role"] = "Emp";
                    }
                    var tickets = await connection.QueryAsync<RaiseTickets>("sp_ssgetadmintickets", commandType: System.Data.CommandType.StoredProcedure);
                    return View(tickets);

                }

            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return null;

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUnAnswered()
        {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();



                    if (userid == "admin150")
                    {
                        TempData["Role"] = "Admin";
                    }
                    else
                    {
                        TempData["Role"] = "Emp";
                    }
                    var tickets = await connection.QueryAsync<RaiseTickets>("sp_ssgetunanswer", commandType: System.Data.CommandType.StoredProcedure);
                    if (tickets != null)
                    {
                        return PartialView("_UnAnswerDisplay", tickets);
                    }
                    return PartialView("_UnAnswerDisplay", "No Tickets");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return View("Error");

            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RaiseTickets(RaiseTickets tickets)
        {
            try
            {
                using (DbConnection connection = _context.Database.GetDbConnection())
                {
                    var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var input = new TicketInput();
                    var prediction = _predictionEnginePool.Predict(modelName: "Auto-Tag-Model", new ModelUser.TicketInput { Description = tickets.Query });

                    if (connection.State != ConnectionState.Open) await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@employeeid", userid);
                    parameters.Add("@createdon", DateTime.Now);
                    parameters.Add("@query", tickets.Query);
                    parameters.Add("@type", prediction.SelectedTag);
                    parameters.Add("@answer", "UnAnswered");
                    parameters.Add("@ansdate", DateTime.Now);
                    parameters.Add("@status", "Pending");
                       
                    int output = await connection.ExecuteAsync("sp_ssraisetickets", parameters, commandType: System.Data.CommandType.StoredProcedure);
                   
                     _toastNotification.AddSuccessToastMessage("Raise Add Sucsessfully");
                    await _hubContext.Clients.User("admin150").SendAsync("ReceiveNotification", "Employee Have Raise Tickets");
                    var notification = new NotificationModel { emp_Id = "admin150", IsRead = false, message = userid + "Employee Have Raise Tickets", Notify_On = DateTime.Now };
                    await SaveNotification(notification);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
                return View("Error");

            }
        }
      
       
    }
}
