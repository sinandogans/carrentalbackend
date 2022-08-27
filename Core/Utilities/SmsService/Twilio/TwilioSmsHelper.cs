using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Core.Utilities.SmsService.Twilio
{
    public class TwilioSmsHelper : ISmsHelper
    {
        private readonly SmsSettings _smsSettings;
        public TwilioSmsHelper(IConfiguration configuration)
        {
            _smsSettings = configuration.GetSection("SmsSettings").Get<SmsSettings>();
        }

        public void SendSms(SmsMessage sms)
        {
            TwilioClient.Init(_smsSettings.AccountSid, _smsSettings.AuthToken);

            MessageResource.Create(
                to: new PhoneNumber(sms.Reciever),
                from: new PhoneNumber(sms.Sender),
                body: sms.Message
            );
        }
    }
}
