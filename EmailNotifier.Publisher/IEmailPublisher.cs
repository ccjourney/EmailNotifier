using System.Threading.Tasks;

namespace EmailNotifier.Publisher
{
    public interface IEmailPublisher
    {
        Task SendEmail(string email);
    }
}
