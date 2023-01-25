using deliverySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace deliverySystem.Controllers
{
    public class messagesController : ApiController
    {
        private OfficeDeliveryContext _context;

        public messagesController()
        {
            _context = new OfficeDeliveryContext();
        }
        // GET api/<controller>
        public IEnumerable<Message> Getmessages()
        {
            return _context.Messages.ToList();
        }

        // GET api/<controller>/5
        public Message Getmessages(int id)
        {
            var message = _context.Messages.SingleOrDefault(c=> c.Id == id);
            if (message == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return message;
        }

        // POST api/<controller>
        [HttpPost]
        public Message CreateMessage(Message message)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.Messages.Add(message);
            _context.SaveChanges();

            return message;
        }
    }
}