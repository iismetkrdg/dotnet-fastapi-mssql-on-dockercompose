using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taslak.Services
{
    public interface IEmailService
    {
        Task<string> SendEmail(string name,string email, string subject, string message);
    }
}