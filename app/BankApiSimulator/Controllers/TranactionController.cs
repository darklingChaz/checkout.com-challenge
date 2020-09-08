using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BankingProxy.Models;
using BankApiSimulator;

namespace BankApiSimulator.Controllers
{
    [ApiController]
    [Route("/")]
    public class TranactionController : ControllerBase
    {

        [HttpPost("actionpayment")]
        public TransactionResponse Get([FromBody] DetailsWrapper detailsWrapper)
        {

            Console.Clear();
            Console.WriteLine("------------- BANKING SIMULATOR -------------");
            Console.WriteLine();
            Console.WriteLine($"RECEIVED TRANSACTION REQUEST...");
            Console.WriteLine($"CARD NO.   = {detailsWrapper.CardDetails.CardNumber[^4..].PadLeft(detailsWrapper.CardDetails.CardNumber.Length - 4, '#')}");
            Console.WriteLine($"EXPIRY     = {detailsWrapper.CardDetails.Month}/{detailsWrapper.CardDetails.Year}  |  CVV    = {detailsWrapper.CardDetails.CVV}");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"AMOUNT     = {detailsWrapper.TransactionDetails.Amount:#,#.##} ({detailsWrapper.TransactionDetails.Currency})");
            Console.WriteLine();
            Console.WriteLine();

            ConsoleKey key = ConsoleKey.Spacebar;
            while(key != ConsoleKey.Y && key != ConsoleKey.N){
                Console.WriteLine("Accept?  Y / N");
                key = Console.ReadKey().Key;
            }

            if(key == ConsoleKey.Y)  
                return new TransactionResponse{ TransactionId = Guid.NewGuid().ToString(), StatusCode = TransactionStatusCodes.Success };

            return new TransactionResponse{ TransactionId = Guid.NewGuid().ToString(), StatusCode = TransactionStatusCodes.Failed };


        }


        [HttpGet("")]
        public string Get() {
            return "Welcome to the Bank Api Simulator";
        }
    }
}
