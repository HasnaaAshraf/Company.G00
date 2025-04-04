using Company.G00.DAL.Sms;
using Twilio.Rest.Api.V2010.Account;

namespace Company.G00.PL.InterfacesHelpers
{
    public interface ITwilioServices
    {
        public MessageResource SendSms(Sms sms);

    }
}
