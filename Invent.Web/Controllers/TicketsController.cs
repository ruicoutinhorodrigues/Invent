using Invent.Web.Data.Entities;
using Invent.Web.Data.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Invent.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketRepository ticketRepository;
        private readonly IProductRepository productRepository;
        private readonly IEmailSender emailSender;

        public TicketsController(ITicketRepository ticketRepository, IProductRepository productRepository, IEmailSender emailSender)
        {
            this.ticketRepository = ticketRepository;
            this.productRepository = productRepository;
            this.emailSender = emailSender;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(int inventoryId)
        {
            var allTickets = await ticketRepository.GetAll().ToListAsync();

            ViewBag.InventoryId = inventoryId;

            return View(allTickets.FindAll(t => t.InventoryId == inventoryId));
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GuyName, GuyContact,ProdReferenceCode,Trouble,Remedy,OpenDate,CloseDate")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                Product product = await this.productRepository.GetAll().FirstOrDefaultAsync(p => p.ReferenceCode == ticket.ProdReferenceCode);

                if (product != null)
                {
                    ticket.InventoryId = product.InventoryId;
                    ticket.OpenDate = DateTime.Now;
                    ticket.ClosedIssue = false;

                    await this.ticketRepository.CreateAsync(ticket);

                    await this.emailSender.SendEmailAsync(ticket.GuyContact, "Open Ticket",
                        $"Hello {ticket.GuyName.Split(" ")[0]}!!!<br/><br/>" +
                        "It's nice to hear from you.<br/><br/>" +
                        "Your ticket was successfully open and you'll be hearing from us over the next couple of days.<br/><br/>" +
                        "Please remember: <br/><br/>\"Four wheels move the body,<br/>Two wheels move the soul.\" ;)<br/><br/>" +
                        "Best regards, AMS");

                    return RedirectToAction(nameof(Index), "Home");
                }

            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await this.ticketRepository.GetByIdAsync(id.Value);

            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, GuyName, GuyContact, ProdReferenceCode, Trouble, Remedy, OpenDate, CloseDate,ClosedIssue, InventoryId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ticket.ClosedIssue == true)
                    {
                        ticket.CloseDate = DateTime.Now;
                    }
                    else
                    {
                        ticket.CloseDate = null;
                    }

                    await this.ticketRepository.UpdateAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.ticketRepository.ExistsAsync(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { inventoryId = ticket.InventoryId });
            }
            return View(ticket);
        }
    }
}
