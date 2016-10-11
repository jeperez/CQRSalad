﻿using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Samples.Domain.Events.User;

namespace Samples.View.SingleUseHandlers
{
    [DispatcherHandler]
    public sealed class UserEmailSingleUseHandler
    {
        private readonly IEmailSender _emailSender;
        private const string EMAIL_GREETING = "Thanks for registering!";

        public UserEmailSingleUseHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Apply(UserCreatedEvent evnt)
        {
            await _emailSender.SendEmail(evnt.Email, EMAIL_GREETING);
        }
    }

    public interface IEmailSender
    {
        Task SendEmail(string email, string text);
    }
}