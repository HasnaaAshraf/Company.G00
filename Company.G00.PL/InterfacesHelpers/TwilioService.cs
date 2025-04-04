using Company.G00.DAL.Sms;
using Company.G00.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;

namespace Company.G00.PL.InterfacesHelpers
{
    //public class TwilioService(IOptions<TwilioSetting> _options) : ITwilioServices
    //{
    //    // IOptions<ITwilioServices> => Ioption (3l4an ygabli object mn TwilioSetting )

    //    public MessageResource SendSms(Sms sms)
    //    {


    //        // Initialization connection 
    //        // TwilioClient => Entry Point 
    //        TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);

    //        // Build 
    //        if (!sms.To.StartsWith("+"))
    //        {
    //            sms.To = $"+20{sms.To.TrimStart('0')}";  // Convert 0127699XXXX → +20127699XXXX
    //        }

    //        Console.WriteLine("Formatted To number: " + sms.To); // For debugging

    //        var message = MessageResource.Create(
    //             body : sms.Body,
    //             to: sms.To,
    //             from : new Twilio.Types.PhoneNumber( _options.Value.PhoneNumber)
    //         );

    //        // Return (MessageResource)

    //        return message;

    //    }
    //}

    public class TwilioService(Microsoft.Extensions.Options.IOptions<TwilioSetting> _options) : ITwilioServices
    {
        public MessageResource SendSms(Sms sms)
        {
            // Initialize Connetion
            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);



            // Build Message

            var message = MessageResource.Create(
                body: sms.Body,
                to: sms.To,
                from: _options.Value.PhoneNumber

            );


            // return Message

            return message;

        }
    }

}
